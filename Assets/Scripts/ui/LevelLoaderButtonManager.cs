using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(PanelRenderer))]
public class LevelLoaderButtonManager : MonoBehaviour
{
    [SerializeField] private LevelInfo[] levels;

    [SerializeField] private string[] buttonNames;

    private PanelRenderer panelRenderer;
    private int uiVersion = 0;

    void OnEnable()
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

    private void OnUIReload(PanelRenderer renderer, VisualElement root, int version)
    {
        // Prevent duplicate callback execution on live reloads
        if (uiVersion == version) 
        {
            return;
        }
        uiVersion = version;

        // query the buttons by the name used in UI builder
        for (int idx = 0; idx < buttonNames.Length; idx++)
        {
            Button button = root.Q<Button>(buttonNames[idx]);
            if (button != null) 
            {
                Debug.Log("level button idx=" + idx + "; HighestCompleteLevelIdx=" + LevelLoader.Instance.Player.HighestCompleteLevelIdx);
                // enable button if player has completed at least up to the level before this
                if (LevelLoader.Instance.Player.HighestCompleteLevelIdx >= idx-1)
                {
                    // register callback with lambda, providing the level to load
                    // TODO consider providing LevelLoader as a variable, not a singleton?
                    Debug.Log("setting load callback for idx " + idx + ", level: " + levels[idx].Name);
                    LevelInfo level = levels[idx];
                    button.clicked += () => LevelLoader.Instance.LoadLevel(level);
                }
                else
                {
                    button.SetEnabled(false);
                }
            }
        }
    }
}
