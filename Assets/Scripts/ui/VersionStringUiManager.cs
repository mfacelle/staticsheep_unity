using UnityEngine;

public class VersionStringUiManager : MonoBehaviour
{
    // singleton instance
   private static VersionStringUiManager instance;

    // don't allow other instances to instantiate,
    // to prevent myself from dropping this into multiple scenes accidentally
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        
        // keep alive across scenes
        DontDestroyOnLoad(gameObject); 
    }
}
