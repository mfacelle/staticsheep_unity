using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(PanelRenderer))]
public class LevelLabelManager : MonoBehaviour
{
    [SerializeField] private string woolAmountLabelName = "WoolAmountLabel";
    [SerializeField] private string levelLabelName = "LevelLabel";
    [SerializeField] private string stageLabelName = "StageLabel";


    private PanelRenderer panelRenderer;

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

    private void OnUIReload(PanelRenderer renderer, VisualElement root)
    {
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
