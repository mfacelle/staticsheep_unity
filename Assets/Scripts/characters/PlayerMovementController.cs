using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 4.0f;
    [SerializeField] private float acceleration = 20.0f;
    [SerializeField] private float deceleration = 40.0f;
    [SerializeField] private InputActionReference moveAction;


    private Rigidbody2D rb;
    private Vector2 moveInput;


    void Start()
    {
        // Get the Rigidbody2D component attached to the player
        rb = GetComponent<Rigidbody2D>();
    }


    private void OnEnable()
    {
        moveAction.action.Enable();

        // subscribe to perform and cancel events
        moveAction.action.performed += OnMovePerformed;
        moveAction.action.canceled += OnMoveCanceled;
    }

    private void OnDisable()
    {
        // unsubscribe from events to prevent memory leaks
        moveAction.action.performed -= OnMovePerformed;
        moveAction.action.canceled -= OnMoveCanceled;

        moveAction.action.Disable();
    }


    // handle continuous movement
    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    // handle movement stopping
    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
    }


    void FixedUpdate()
    {
        if (GameStateManager.Instance.CurrentState == GameStateManager.GameState.Running)
        {
            // Calculate what our target velocity should be based on current input
            Vector2 targetVelocity = moveInput * moveSpeed;

            // Determine if we are accelerating (input detected) or decelerating (no input)
            float currentSpeedChange = (moveInput.magnitude > 0) ? acceleration : deceleration;

            // Smoothly transition current velocity towards target velocity based on rate of change
            float newVelocityX = Mathf.MoveTowards(rb.linearVelocity.x, targetVelocity.x, currentSpeedChange * Time.fixedDeltaTime);
            float newVelocityY = Mathf.MoveTowards(rb.linearVelocity.y, targetVelocity.y, currentSpeedChange * Time.fixedDeltaTime);
            
            // Apply the calculated velocity directly to the Rigidbody2D
            rb.linearVelocity = new Vector2(newVelocityX, newVelocityY);
        }
        else
        {
            // don't let player move
            rb.linearVelocity = new Vector2(0.0f, 0.0f);
        }
    }
}
