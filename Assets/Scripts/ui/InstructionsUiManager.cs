using UnityEngine;
using UnityEngine.UIElements;

public class InstructionsUiManager : MonoBehaviour
{
    [SerializeField] private string startButtonName = "StartBtn";
    
    private PanelRenderer panelRenderer;

    private VisualElement instructionsPanelRoot;

    void Start()
    {
        panelRenderer = GetComponent<PanelRenderer>();
        panelRenderer.RegisterUIReloadCallback(OnUIReload);

        // start game state as paused, if this object is present
        GameStateManager.Instance.SetGameState(GameStateManager.GameState.Intro);
    }

    private void OnUIReload(PanelRenderer renderer, VisualElement root)
    {
        Debug.Log("resuming. State=" + GameStateManager.Instance.CurrentState);
        
        instructionsPanelRoot = root;

        Button startButton = root.Q<Button>(startButtonName);
        startButton.clicked += () => StartLevel();
        
        // set to intro state here to ensure state is set when UI displayed
        GameStateManager.Instance.SetGameState(GameStateManager.GameState.Intro);
    }

    private void StartLevel()
    {
        instructionsPanelRoot.style.display = DisplayStyle.None;
        GameStateManager.Instance.SetGameState(GameStateManager.GameState.Running);
    }

}
