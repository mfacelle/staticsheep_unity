using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[RequireComponent(typeof(PanelRenderer))]
public class StageClearManager : MonoBehaviour
{
    #region Varables

    [SerializeField] private string retryButtonName = "RetryBtn";
    [SerializeField] private string returnButtonName = "ReturnBtn";
    [SerializeField] private string stageClearMenuName = "StageClearPanel";
    [SerializeField] private string levelClearMenuName = "LevelClearPanel";
    [SerializeField] private string levelFailMenuName = "LevelFailPanel";

    // how long to display stage clear panel before loading next stage
    [SerializeField] private float stageClearTimeSec = 1.0f;

    // how long to display level clear panel before loading next stage
    [SerializeField] private float levelClearTimeSec = 2.0f;


    private PanelRenderer panelRenderer;

    private VisualElement stageClearMenu;

    private VisualElement levelClearMenu;

    private VisualElement levelFailMenu;

    #endregion
    

    void Start()
    {
        panelRenderer = GetComponent<PanelRenderer>();
        panelRenderer.RegisterUIReloadCallback(OnUIReload);
    }

    void OnDisable()
    {
        if (panelRenderer != null)
        {
            panelRenderer.UnregisterUIReloadCallback(OnUIReload);
        }
    }

    private void OnUIReload(PanelRenderer renderer, VisualElement root)
    {
        stageClearMenu = root.Q<VisualElement>(stageClearMenuName);
        levelClearMenu = root.Q<VisualElement>(levelClearMenuName);
        levelFailMenu = root.Q<VisualElement>(levelFailMenuName);

        // don't display these panels when UI reloaded
        // TODO could this cause a panel to disappear later though?
        stageClearMenu.style.display = DisplayStyle.None;
        levelClearMenu.style.display = DisplayStyle.None;
        levelFailMenu.style.display = DisplayStyle.None;

        Button retryButton = root.Q<Button>(retryButtonName);
        retryButton.clicked += () => LevelLoader.Instance.LoadLevel(LevelLoader.Instance.CurrentLevel);

        Button returnButton = root.Q<Button>(returnButtonName);
        returnButton.clicked += () => LevelLoader.Instance.ReturnToLevelSelect();
    }

    public void LoadNextStage()
    {
        // this is kind of clunky, because stage/level management and transitions are done
        // in multiple classes.  But, for now, this will get something working, so I'm keeping it

        if (LevelLoader.Instance.IsLastStage())
        {
            // display level clear menu and set timer for that
            levelClearMenu.style.display = DisplayStyle.Flex;
            StartCoroutine(WaitAndLoadNextStage(levelClearTimeSec));
        }
        else
        {
            // display stage clear menu and set timer for that
            stageClearMenu.style.display = DisplayStyle.Flex;
            StartCoroutine(WaitAndLoadNextStage(stageClearTimeSec));
        }
    }

    private IEnumerator WaitAndLoadNextStage(float waitTimeAmountSec)
    {
        
        GameStateManager.Instance.SetGameState(GameStateManager.GameState.StageEnd);

        yield return new WaitForSeconds(waitTimeAmountSec);

        LevelLoader.Instance.LoadNextStage();
    }

    // display UI that includes buttons for retry/return.
    // TODO pretty sure the player can still move when this is up...
    public void FailLevel()
    {
        levelFailMenu.style.display = DisplayStyle.Flex;
    }

}
