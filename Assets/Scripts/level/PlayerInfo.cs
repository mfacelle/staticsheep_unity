using UnityEngine;

// container to hold information about the player that should persist
// across scenes (levels, stages, etc)
[CreateAssetMenu(fileName = "PlayerInfo", menuName = "Game/Player Info")]

public class PlayerInfo : ScriptableObject
{
    public int CurrentNumParticles {get; private set; } = 1;

    public void DecrementNumParticles()
    {
        CurrentNumParticles--;
    }

    public void SetNumParticles(int numParticles)
    {
        CurrentNumParticles = numParticles;
    }
}
