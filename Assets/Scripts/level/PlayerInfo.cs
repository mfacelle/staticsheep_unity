using UnityEngine;

// container to hold information about the player that should persist
// across scenes (levels, stages, etc)
public class PlayerInfo : MbSingleton<PlayerInfo>
{
    #region Variables

    // current number of particles player has to launch
    public int CurrentNumParticles {get; private set;}

    // highest completed level, for unlocking levels in level select scene
    public int HighestCompleteLevelIdx {get; private set;}

    // number of unlocked particles, for access/display on level toolbar
    // Consider making this a bool array? Or should it always be sequential?
    public int UnlockedParticles {get; private set;}

    // starting value for current number of particles. used for debug
    [field: SerializeField] private int StartingCurrentNumParticles;

    // starting value for levels complete. used for debug
    [field: SerializeField] private int StartingCompleteLevelIdx;

    // starting value for unlocked number of particles. used for debug
    [field: SerializeField] private int StartingUnlockedParticles;

    // player properties, used for updating UI
    [SerializeField] private PlayerProperties playerProps;

    #endregion

    public override void Awake()
    {
        base.Awake();

        // set to default values for debugging.
        CurrentNumParticles = StartingCurrentNumParticles;
        HighestCompleteLevelIdx = StartingCompleteLevelIdx;
        UnlockedParticles = StartingUnlockedParticles;
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

        // TODO also unlock particles after completing certain levels?
    }
}
