using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimationController : MonoBehaviour
{
    private PlayerMovementController movementController;

    private Animator animator;

    void Start()
    {
        movementController = GetComponent<PlayerMovementController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {

        if (GameStateManager.Instance.CurrentState == GameStateManager.GameState.Running)
        {
            // get movement direction, and whether or not player is moving
            Vector2 moveDir = movementController.MoveDirection;

            // calculate look direction, based off mouse position

            Vector3 playerPos = gameObject.transform.position;
            Vector2 screenMousePos = Mouse.current.position.ReadValue();
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(screenMousePos.x, screenMousePos.y, Camera.main.nearClipPlane));        

            Vector2 lookDir = (mousePos - playerPos).normalized;

            Debug.Log("moveDir: " + moveDir + ", lookDir: " + lookDir);
            // if moving, set flag and move direction
            // TODO or do we always want to use look direction?
            if (moveDir != Vector2.zero)
            {
                animator.SetFloat("MoveX", lookDir.x);
                animator.SetFloat("MoveY", lookDir.y);
                animator.SetBool("IsMoving", true);
            }
            else // use idle blend tree
            {
                animator.SetBool("IsMoving", false);
                animator.SetFloat("LookDirX", lookDir.x);
                animator.SetFloat("LookDirY", lookDir.y);
            }
        }
    }
}
