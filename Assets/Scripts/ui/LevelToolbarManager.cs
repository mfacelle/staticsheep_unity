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
    [SerializeField] private string[] toolbarImgNames;

    [SerializeField] private Sprite[] toolbarSprites;

    [SerializeField] private Sprite toolbarLockedSprite;

    // instance of particle launcher, to set particle prefab on
    [SerializeField] private ParticleLauncher particleLauncher;

    private PanelRenderer panelRenderer;

    // ensure hotkeys, buttons, and prefabs have same sized arrays
    private void OnValidate()
    {
        // Check if sizes match
        int expectedSize = hotkeyActions.Length;
        if (hotkeyActions != null && 
            particlePrefabs != null && 
            toolbarImgNames != null &&
            toolbarSprites != null &&
           (hotkeyActions.Length != expectedSize ||
              hotkeyActions.Length != expectedSize ||
              particlePrefabs.Length != expectedSize ||
              toolbarSprites.Length != expectedSize ) )
        {
            Debug.LogError($"[Size Mismatch] Lengths need to match: hotkeyActions.Length={hotkeyActions.Length}" +
               $"; particlePrefabs.Length={particlePrefabs.Length}" + 
               $"; toolbarImgNames.Length={toolbarImgNames.Length}" +
               $"; toolbarSprites.Length={toolbarSprites.Length}", 
               this);
        }
    }

    void OnEnable()
    {
        panelRenderer = GetComponent<PanelRenderer>();
        panelRenderer.RegisterUIReloadCallback(OnUIReload);

        int numUnlockedParticles = PlayerInfo.Instance.UnlockedParticles;

        // set up hotkey callbacks
        for (int idx = 0; idx < hotkeyActions.Length; idx++)
        {
            // only allow callback if particle has been unlocked
            if (idx+1 > numUnlockedParticles)
            {
                hotkeyActions[idx].action.performed += ctx => SetParticlePrefab(idx);
            }
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
            // Debug.Log("setting callback for idx " + idx + ", hotkey: " + hotkeyActions[idx]);
            int prefabIdx = idx;
            hotkeyActions[idx].action.performed -= ctx => SetParticlePrefab(prefabIdx);
        }
    }

    private void OnUIReload(PanelRenderer renderer, VisualElement root)
    {
        int numUnlockedParticles = PlayerInfo.Instance.UnlockedParticles;

        for (int idx = 0; idx < toolbarImgNames.Length; idx++)
        {
            Image toolbarImg = root.Q<Image>(toolbarImgNames[idx]);
            if (toolbarImg != null)
            {
                if (idx+1 > numUnlockedParticles)
                {
                    toolbarImg.sprite = toolbarLockedSprite;
                }
                else
                {
                    toolbarImg.sprite = toolbarSprites[idx];
                }
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
