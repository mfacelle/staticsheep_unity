using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(PanelRenderer))]
public class LevelLabelManager : MonoBehaviour
{
    [SerializeField] private string woolAmountLabelName = "WoolAmountLabel";
    [SerializeField] private string levelLabelName = "LevelLabel";
    [SerializeField] private string stageLabelName = "StageLabel";


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

    // TODO this doesn't get called like Update, so it only refreshes on scene load...
    // need to use property bindings or something (and learn about them) to actually
    // have the wool count update in real time
    private void OnUIReload(PanelRenderer renderer, VisualElement root)
    {
        Debug.Log("loading labels");
        // Prevent duplicate callback execution on live reloads
        // if (uiVersion == version) 
        // {
        //     return;
        // }
        // uiVersion = version;

        Label woolAmountLabel = root.Q<Label>(woolAmountLabelName);
        Label levelLabel = root.Q<Label>(levelLabelName);
        Label stageLabel = root.Q<Label>(stageLabelName);

        if (woolAmountLabel != null)
        {
            woolAmountLabel.text = "" + PlayerInfo.Instance.CurrentNumParticles;
        }
        else
        {
            Debug.Log("wool amount label is null");
        }

        if (levelLabel != null)
        {
            levelLabel.text = LevelLoader.Instance.CurrentLevel.Name;
        }

        if (stageLabel != null)
        {
            // kind of clunky.  maybe change later
            stageLabel.text = LevelLoader.Instance.GetCurrentStageName() + " / " + LevelLoader.Instance.CurrentLevel.Stages.Length;
        }
    }
}
