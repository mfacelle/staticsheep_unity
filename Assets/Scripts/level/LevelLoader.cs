using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    // singleton instance
    public static LevelLoader Instance { get; private set; }

    [field: SerializeField] public PlayerInfo Player {get; private set; }

    [field: SerializeField] private string LevelSelectSceneName = "level_select";

    private LevelInfo currentLevel;

    private int currentStageIndex;

    private void Awake()
    {
        // keep alive across scenes
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }

        currentLevel = null;
        currentStageIndex = 0;
    }

    // load level in a coroutine
    public void LoadLevel(LevelInfo level)
    {
        Debug.Log("Loading level " + level.Name);
        currentLevel = level;
        // also need to set things like player's initial num particles, etc
        currentStageIndex = 0;
        Player.SetNumParticles(level.InitialNumParticles);
        LoadScene(level.Stages[currentStageIndex]);
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadAsynchronously(sceneName));
    }

    public void LoadNextStage()
    {
        currentStageIndex++;
        if (currentStageIndex >= currentLevel.Stages.Length)
        {
            Debug.Log("level cleared!");
            // for now, just return to level select.
            // eventually, want to hanlde things like player progresison, unlocking more levels,
            // displaying score, etc
            LoadScene(LevelSelectSceneName);
        }
        else
        {
            LoadScene(currentLevel.Stages[currentStageIndex]);
        }
    }

    // asynchronously load a scene (in a coroutine), only displaying when complete
    private IEnumerator LoadAsynchronously(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        // prevent scene from showing until it's ready
        operation.allowSceneActivation = false; 

        while (!operation.isDone)
        {
            // convert internal progress (0 - 0.9) to a standard 0 - 1 percentage
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            
            // TODO loading progress bar? may not care about this
            // LoadingUI.Instance.UpdateProgressBar(progress);

            if (operation.progress >= 0.9f)
            {
                // display scene
                operation.allowSceneActivation = true; 
            }

            yield return null;
        }
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
