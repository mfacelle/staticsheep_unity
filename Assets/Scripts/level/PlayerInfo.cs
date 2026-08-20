using UnityEngine;

// container to hold information about the player that should persist
// across scenes (levels, stages, etc)
[CreateAssetMenu(fileName = "PlayerInfo", menuName = "Game/Player Info")]

public class PlayerInfo : ScriptableObject
{
    public int CurrentNumParticles {get; private set; } = 1;

    public int HighestCompleteLevelIdx {get; private set; } = -1;

    public void DecrementNumParticles()
    {
        CurrentNumParticles--;
    }

    public void SetNumParticles(int numParticles)
    {
        CurrentNumParticles = numParticles;
    }

    public void LevelCompleted(int levelIndex)
    {
        Debug.Log("marking level " + levelIndex + " complete. current HighestCompleteLevelIdx=" + HighestCompleteLevelIdx);
        if (levelIndex > HighestCompleteLevelIdx)
        {
            HighestCompleteLevelIdx = levelIndex;
        }
    }
}
