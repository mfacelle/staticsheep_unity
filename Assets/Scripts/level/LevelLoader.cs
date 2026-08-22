using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MbSingleton<LevelLoader>
{
    [field: SerializeField] private string LevelSelectSceneName = "level_select";

    public LevelInfo CurrentLevel {get; private set;}

    private int currentStageIndex;

    public override void Awake()
    {
        base.Awake();

        // initialize to dummy level in case stages are loaded via editor manually
        var tmpLevelGameObject = new GameObject("DebugLevelInfo");
        tmpLevelGameObject.AddComponent<LevelInfo>();
        CurrentLevel = tmpLevelGameObject.GetComponent<LevelInfo>();
    }

    // load level in a coroutine
    public void LoadLevel(LevelInfo level)
    {
        Debug.Log("Loading level " + level.Name);
        CurrentLevel = level;
        // also need to set things like player's initial num particles, etc
        currentStageIndex = 0;
        PlayerInfo.Instance.SetNumParticles(level.InitialNumParticles);
        LoadScene(level.Stages[currentStageIndex]);
    }

    public void FailLevel()
    {
        // for now, just log msg and return to level select.
        // eventually, add some kind of animation or screen display, then load
        Debug.Log("Level " + CurrentLevel.Name + " FAILED");
        LoadScene(LevelSelectSceneName);
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadAsynchronously(sceneName));
    }

    public void LoadNextStage()
    {
        currentStageIndex++;
        if (currentStageIndex >= CurrentLevel.Stages.Length)
        {
            Debug.Log("level cleared!");
            // mark level as cleared in player info
            PlayerInfo.Instance.LevelCompleted(CurrentLevel.LevelIndex);

            // for now, just return to level select.
            // eventually, want to hanlde things like player progresison, unlocking more levels,
            // displaying score, etc
            LoadScene(LevelSelectSceneName);
        }
        else
        {
            LoadScene(CurrentLevel.Stages[currentStageIndex]);
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

    public string GetCurrentStageName()
    {
        return "Stage " + (currentStageIndex + 1);
    }
}
