using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[RequireComponent(typeof(PanelRenderer))]
public class PauseManager : MonoBehaviour
{    
    #region Variables

    [SerializeField] private InputActionReference pauseAction;

    [SerializeField] private string returnMainMenuBtnName = "ReturnMainMenuBtn";

    [SerializeField] private string returnLevelSelectBtnName = "ReturnLevelSelectBtn";

    // [SerializeField] private string pauseButtonName = "PauseBtn";

    [SerializeField] private string pauseMenuName = "PauseMenu";

    private bool isPaused = false;

    private PanelRenderer panelRenderer;

    private VisualElement pauseMenu;

    #endregion

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

        // TODO no main menu yet, so this just returns to level select
        Button returnMainMenuBtn = root.Q<Button>(returnMainMenuBtnName);
        returnMainMenuBtn.clicked += () => LevelLoader.Instance.ReturnToLevelSelect();

        Button returnLevelSelectBtn = root.Q<Button>(returnLevelSelectBtnName);
        returnLevelSelectBtn.clicked += () => LevelLoader.Instance.ReturnToLevelSelect();

        // default to unpaused
        Resume();
    }

    // input action callback requires context arg
    public void TogglePause(InputAction.CallbackContext context)
    {
        // TODO need to disable this hotkey when stage/level clear/failure ui is present
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
        // pauseMenu.visible = false;
        pauseMenu.style.display = DisplayStyle.None;
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Pause()
    {
        Debug.Log("Pausing level");
        // pauseMenu.visible = true;
        pauseMenu.style.display = DisplayStyle.Flex;
        Time.timeScale = 0f;
        isPaused = true;
    }
}
