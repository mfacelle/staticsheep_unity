using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class ParticleLauncher : MonoBehaviour
{    
    [SerializeField] private float initialSpeed = 0.1f;
    [SerializeField] private InputActionReference launchAction;

    // physics manager, for adding particles when launched
    [SerializeField] public PhysicsManager PhysicsManager;

    // player object, for getting position to calculate particle trajectory
    [SerializeField] private GameObject PlayerObject;

    // making public to easily change this during gameplay (for now, this is not a great method)
    [SerializeField] private GameObject particlePrefab;


    private ChargedParticle CreateParticle()
    {
        // was originally trying an object pool for this, but it's likely unnecessary.
        // if re-adding, need to ensure we handle the different types of particles, or
        // the pool will incorrectly use the wrong type of particles
        
        GameObject newParticle = Instantiate(particlePrefab);
        if (newParticle.TryGetComponent<ChargedParticle>(out var particle))
        {
            return particle;
        }
        else
        {
            Debug.Log($"particlePrefab has a null ChargedParticle component!");
            return null;
        }
    }


    private void OnEnable()
    {
        launchAction.action.Enable();

        // subscribe to perform and cancel events
        launchAction.action.started += OnLaunch;
    }

    private void OnDisable()
    {
        // unsubscribe from events to prevent memory leaks
        launchAction.action.started -= OnLaunch;

        launchAction.action.Disable();
    }

    public void OnLaunch(InputAction.CallbackContext context)
    {
        // do nothing if a UI element was pressed
        // TODO - need to figure out a real workaround for this.
        // maybe just make it so buttons aren't real clickable buttons...? keyboard only?
        // if (EventSystem.current.IsPointerOverGameObject())
        // {
        //     Debug.Log("over UI");
        //     return;
        // }

        Debug.Log("Mouse Click Detected!");

        // TODO consider adding a pause after launch to avoid launching particles
        // if player somehow clicks every single frame.
        
        Vector2 screenMousePos = Mouse.current.position.ReadValue();

        // convert to world position, and flatten to 2d vector
        Vector3 worldMousePos3D = Camera.main.ScreenToWorldPoint(new Vector3(screenMousePos.x, screenMousePos.y, Camera.main.nearClipPlane));        
        Vector2 mouseWorldPos = (Vector2)worldMousePos3D; 

        Vector2 playerPos = PlayerObject.transform.position;

        Vector2 launchVector = (mouseWorldPos - playerPos);
        launchVector.Normalize();

        // create particle at player location (using object pool), then give it velocity
        ChargedParticle newParticle = CreateParticle();
        if (newParticle != null)
        {
            newParticle.Init(PhysicsManager);
            newParticle.gameObject.transform.position = PlayerObject.transform.position;
            newParticle.ApplyForce(launchVector * initialSpeed);
            PhysicsManager.AddParticle(newParticle);
        }
    }

    public void SetParticlePrefab(GameObject newParticlePrefab)
    {
        particlePrefab = newParticlePrefab;
    }
}
