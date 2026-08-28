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
        // ...maybe just don't use animations for showing the controls? kinda overkill and inflexible

        // TODO no main menu yet, so this just returns to level select
        Button returnMainMenuBtn = root.Q<Button>(returnMainMenuBtnName);
        returnMainMenuBtn.clicked += () => LevelLoader.Instance.ReturnToLevelSelect();

        Button returnLevelSelectBtn = root.Q<Button>(returnLevelSelectBtnName);
        returnLevelSelectBtn.clicked += () => LevelLoader.Instance.ReturnToLevelSelect();
    }

    // input action callback requires context arg; unused here
    public void TogglePause(InputAction.CallbackContext context)
    {
        // only allow pausing the game under certain game states
        if (GameStateManager.Instance.CurrentState == GameStateManager.GameState.Paused)
        {
            Resume();
        }
        else if (GameStateManager.Instance.CurrentState == GameStateManager.GameState.Running)
        {
            Pause();
        }
    }

    public void Resume()
    {
        Debug.Log("Resuming from pause");
        pauseMenu.style.display = DisplayStyle.None;
        Time.timeScale = 1f;
        GameStateManager.Instance.SetGameState(GameStateManager.GameState.Running);
    }

    public void Pause()
    {
        Debug.Log("Pausing level");
        pauseMenu.style.display = DisplayStyle.Flex;
        Time.timeScale = 0f;
        GameStateManager.Instance.SetGameState(GameStateManager.GameState.Paused);
    }
}
