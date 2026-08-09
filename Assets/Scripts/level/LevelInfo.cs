using UnityEngine;


// holds info about a level, which consists of a set of sequential stages
public class LevelInfo : MonoBehaviour
{
    // level name, mainly for debugging
    [field: SerializeField] public string Name {get; private set; }

    // number of particles the player will start this level with
    [field: SerializeField] public int InitialNumParticles {get; private set; } = 100;

    // list of stage names, to use when loading stages
    [field: SerializeField] public string[] Stages {get; private set;}

}
