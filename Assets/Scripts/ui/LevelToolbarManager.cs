using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


// class to manage the toolbar displayed in a level
[RequireComponent(typeof(PanelRenderer))]
public class LevelToolbarManager : MonoBehaviour
{
    // eventually add: 
    // - list of prefabs that each button will select
    // - a way to "lock" buttons that the player hasn't unlocked yet
    [SerializeField] private InputActionReference[] hotkeyActions;

    [SerializeField] private GameObject[] particlePrefabs;

    // names of buttons, for setting up callbacks
    [SerializeField] private string[] buttonNames;

    // instance of particle launcher, to set particle prefab on
    [SerializeField] private ParticleLauncher particleLauncher;

    private PanelRenderer panelRenderer;
    private int uiVersion = 0;

    // ensure hotkeys, buttons, and prefabs have same sized arrays
    private void OnValidate()
    {
        // Check if sizes match
        if (hotkeyActions != null && particlePrefabs != null && buttonNames != null &&
           (hotkeyActions.Length != particlePrefabs.Length ||
              hotkeyActions.Length != buttonNames.Length ||
              particlePrefabs.Length != buttonNames.Length) )
        {
            Debug.LogError($"[Size Mismatch] Lengths need to match: hotkeyActions.Length={hotkeyActions.Length}" +
               $"; particlePrefabs.Length={particlePrefabs.Length}" + 
               $"; buttonNames.Length={buttonNames.Length}", this);
        }
    }

    void OnEnable()
    {
        panelRenderer = GetComponent<PanelRenderer>();
        panelRenderer.RegisterUIReloadCallback(OnUIReload);

        // set up hotkey callbacks
        for (int idx = 0; idx < hotkeyActions.Length; idx++)
        {
            Debug.Log("setting callback for idx " + idx + ", hotkey: " + hotkeyActions[idx]);
            int prefabIdx = idx;
            hotkeyActions[idx].action.performed += ctx => SetParticlePrefab(prefabIdx);
        }
    }

    void OnDisable()
    {
        if (panelRenderer != null)
        {
            panelRenderer.UnregisterUIReloadCallback(OnUIReload);
        }

        // disable hotkey callbacks
        for (int idx = 0; idx < hotkeyActions.Length; idx++)
        {
            Debug.Log("setting callback for idx " + idx + ", hotkey: " + hotkeyActions[idx]);
            int prefabIdx = idx;
            hotkeyActions[idx].action.performed -= ctx => SetParticlePrefab(prefabIdx);
        }
    }

    private void OnUIReload(PanelRenderer renderer, VisualElement root)
    {
        Debug.Log("loading toolbar");
        // Prevent duplicate callback execution on live reloads
        // if (uiVersion == version) 
        // {
        //     return;
        // }
        // uiVersion = version;

        // query the buttons by the name used in UI builder
        for (int idx = 0; idx < buttonNames.Length; idx++)
        {
            Button button = root.Q<Button>(buttonNames[idx]);
            if (button != null) 
            {
                // register callback with lambda, providing the level to load
                // TODO consider providing LevelLoader as a variable, not a singleton?
                Debug.Log("setting button callback for idx " + idx + ", button: " + buttonNames[idx]);
                int prefabIdx = idx;
                button.clicked += () => SetParticlePrefab(prefabIdx);
            }
        }
    }

    // load a particle prefab, using the index from the array of bottons/hotkeys/prefabs
    void SetParticlePrefab(int prefabIdx)
    {
        Debug.Log("Loading particle prefab " + prefabIdx);
        particleLauncher.SetParticlePrefab(particlePrefabs[prefabIdx]);
    }
}
