using System.ComponentModel;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(PanelRenderer))]
public class LevelLabelManager : MonoBehaviour
{
    [SerializeField] private string woolAmountLabelName = "WoolAmountLabel";
    [SerializeField] private string levelLabelName = "LevelLabel";
    [SerializeField] private string stageLabelName = "StageLabel";

    // how many particles to start triggering a more dramatic wool amount bounce
    [SerializeField] private int minParticlesAlertThreshold = 10;

    // data model for tracking wool amount changes
    [SerializeField] private PlayerProperties playerProperties;

    private Label woolAmountLabel;


    private PanelRenderer panelRenderer;

    void OnEnable()
    {
        panelRenderer = GetComponent<PanelRenderer>();
        panelRenderer.RegisterUIReloadCallback(OnUIReload);

        playerProperties.propertyChanged += WoolTextBounce;
    }

    void OnDisable()
    {
        if (playerProperties != null)
        {
            playerProperties.propertyChanged -= WoolTextBounce;
        }

        if (panelRenderer != null)
        {
            panelRenderer.UnregisterUIReloadCallback(OnUIReload);
        }
    }

    private void WoolTextBounce(object sender, BindablePropertyChangedEventArgs args)
    {
        if (args.propertyName == nameof(PlayerProperties.CurrentNumParticles))
        {
            // make wool amount appear to bounce with uss style
            if (woolAmountLabel != null)
            {        
                woolAmountLabel.RemoveFromClassList("text-label");
                // display larger bounce when num particles gets low
                if (playerProperties.CurrentNumParticles <= minParticlesAlertThreshold)
                {
                    Debug.Log("low wool style");
                    woolAmountLabel.AddToClassList("text-label-big-bounce");
                }
                else
                {
                    woolAmountLabel.AddToClassList("text-label-bounce");
                }

                // return to standard style after bounce happens
                woolAmountLabel.schedule.Execute(() =>
                {
                    woolAmountLabel.RemoveFromClassList("text-label-bounce");
                    woolAmountLabel.RemoveFromClassList("text-label-big-bounce");
                    woolAmountLabel.AddToClassList("text-label");
                }).StartingIn(400); // 300ms + 100ms
                // don't love that this has to be hardcoded and the same value as uss file...
                // but I guess it works for now?
            }
        }
    }

    private void OnUIReload(PanelRenderer renderer, VisualElement root)
    {
        woolAmountLabel = root.Q<Label>(woolAmountLabelName);
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
