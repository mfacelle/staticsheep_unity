using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[RequireComponent(typeof(PanelRenderer))]
public class PauseManager : MonoBehaviour
{    
    [SerializeField] private InputActionReference pauseAction;

    // [SerializeField] private string pauseButtonName = "PauseBtn";

    [SerializeField] private string pauseMenuName = "PauseMenu";

    private bool isPaused = false;

    private PanelRenderer panelRenderer;

    private VisualElement pauseMenu;


    private void OnEnable()
    {
        panelRenderer = GetComponent<PanelRenderer>();
        panelRenderer.RegisterUIReloadCallback(OnUIReload);

        pauseAction.action.Enable();

        // subscribe to perform and cancel events
        pauseAction.action.started += TogglePause;
    }

    private void OnDisable()
    {
        if (panelRenderer != null)
        {
            panelRenderer.UnregisterUIReloadCallback(OnUIReload);
        }

        // unsubscribe from events to prevent memory leaks
        pauseAction.action.started -= TogglePause;

        pauseAction.action.Disable();
    }

    private void OnUIReload(PanelRenderer renderer, VisualElement root)
    {
        pauseMenu = root.Q<VisualElement>(pauseMenuName);

        // TODO animations might still be running even when panel isn't display.
        // may want to look into better way to disable pause menu?

        // TODO implement return to level select / main menu buttons

        // default to off
        Resume();
    }

    // input action callback requires context arg
    public void TogglePause(InputAction.CallbackContext context)
    {
        TogglePause();
    }
    
    public void TogglePause()
    {
        if (isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Resume()
    {
        Debug.Log("Resuming from pause");
        pauseMenu.visible = false;
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Pause()
    {
        Debug.Log("Pausing level");
        pauseMenu.visible = true;
        Time.timeScale = 0f;
        isPaused = true;
    }
}
