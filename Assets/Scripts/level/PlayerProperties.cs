using UnityEngine;
using Unity.Properties;

// container for player runtime data, to be bound to UI
[CreateAssetMenu(fileName = "PlayerProperties", menuName="Game/PlayerProperties")]
public class PlayerProperties : ScriptableObject
{
    // just making this public for ease of use (for now - need to figure this stuff out)
    [CreateProperty] public int CurrentNumParticles;
}
