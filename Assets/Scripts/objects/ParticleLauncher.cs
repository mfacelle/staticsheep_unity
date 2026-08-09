using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;

public class ParticleLauncher : MonoBehaviour
{    
    [SerializeField] private float initialSpeed = 0.1f;
    [SerializeField] private InputActionReference launchAction;

    // physics manager, for adding particles when launched
    [SerializeField] public PhysicsManager PhysicsManager;

    // player object, for getting position to calculate particle trajectory
    [SerializeField] private GameObject PlayerObject;

    // making public static to easily change this during gameplay (for now, this is not a great method)
    public GameObject ParticlePrefab;


    private IObjectPool<ChargedParticle> particlePool;
    private int particlePoolDefaultCapacity = 20;
    private int particlePoolMaxSize = 100;


    private void Awake()
    {
        // initialize object pool for particles
        particlePool = new ObjectPool<ChargedParticle>(
            CreateParticle,       // Function to create new item if pool is empty
            OnTakeFromPool,         // Function called when taking item from pool
            OnReturnedToPool,       // Function called when returning item to pool
            OnDestroyPoolObject,    // Function called if maxPoolSize is exceeded
            true,                   // Collection check (throws error if releasing an item already in pool)
            particlePoolDefaultCapacity, 
            particlePoolMaxSize
        );
    }

    // ---
    // object pool callbacks
    private ChargedParticle CreateParticle()
    {
        GameObject newParticle = Instantiate(ParticlePrefab);
        if (newParticle.TryGetComponent<ChargedParticle>(out var particle))
        {
            return particle;
        }
        else
        {
            Debug.Log($"ParticlePrefab has a null ChargedParticle component!");
            return null;
        }
    }

    private void OnTakeFromPool(ChargedParticle particle)
    {
        particle.gameObject.SetActive(true);
    }

    private void OnReturnedToPool(ChargedParticle particle)
    {
        particle.gameObject.SetActive(false);
    }

    private void OnDestroyPoolObject(ChargedParticle particle)
    {
        Destroy(particle.gameObject);
    }

    // ---

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
        Debug.Log("Mouse Click Detected!");
        
        Vector2 screenMousePos = Mouse.current.position.ReadValue();

        // convert to world position, and flatten to 2d vector
        Vector3 worldMousePos3D = Camera.main.ScreenToWorldPoint(new Vector3(screenMousePos.x, screenMousePos.y, Camera.main.nearClipPlane));        
        Vector2 mouseWorldPos = (Vector2)worldMousePos3D; 

        Vector2 playerPos = PlayerObject.transform.position;

        Vector2 launchVector = (mouseWorldPos - playerPos);
        launchVector.Normalize();

        Debug.Log($"Clicked at screen coordinates: {screenMousePos}");
        Debug.Log($"Clicked at world coordinates: {mouseWorldPos}");
        Debug.Log($"Launch vector: {launchVector}");


        // create particle at player location (using object pool), then give it velocity
        ChargedParticle newParticle = particlePool.Get();
        if (newParticle != null)
        {
            newParticle.Init(PhysicsManager, particlePool);
            newParticle.gameObject.transform.position = PlayerObject.transform.position;
            newParticle.ApplyForce(launchVector * initialSpeed);
            PhysicsManager.AddParticle(newParticle);
        }
    }
}
