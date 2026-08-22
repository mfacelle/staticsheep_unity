using UnityEngine;

// container to hold information about the player that should persist
// across scenes (levels, stages, etc)
public class PlayerInfo : MbSingleton<PlayerInfo>
{
    #region Variables
    public int CurrentNumParticles {get; private set;}

    public int HighestCompleteLevelIdx {get; private set;}

    [field: SerializeField] private int StartingCompleteLevelIdx;

    [SerializeField] private PlayerProperties playerProps;

    #endregion

    public override void Awake()
    {
        base.Awake();

        // set to default of 100, for when stage scenes are loaded via editor manually
        CurrentNumParticles = 100;
        HighestCompleteLevelIdx = StartingCompleteLevelIdx;
    }

    public void DecrementNumParticles()
    {
        SetNumParticles(CurrentNumParticles-1);
    }

    public void SetNumParticles(int numParticles)
    {
        CurrentNumParticles = numParticles;
        playerProps.CurrentNumParticles = numParticles;
    }

    public void LevelCompleted(int levelIndex)
    {
        Debug.Log("marking level " + levelIndex + " complete. current highestCompleteLevelIdx=" + HighestCompleteLevelIdx);
        if (levelIndex > HighestCompleteLevelIdx)
        {
            HighestCompleteLevelIdx = levelIndex;
        }
    }
}
