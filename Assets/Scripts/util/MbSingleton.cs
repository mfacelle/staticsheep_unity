using UnityEngine;
using System.Collections;

// singleton based on:
//	http://answers.unity3d.com/questions/408518/dontdestroyonload-duplicate-object-in-a-singleton.html
//
// monobehavior singleton.  
// Requires a gameobject to be in the first scene being loaded to work properly.
// so, not really a true singleton, but it'll work for now.
// Just going to copy/paste my prefab into every scene rather than figure
// out some fancy solution for now.  Probably needs some refactor/redesign, though
public class MbSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
	public static T Instance { get; private set; }

	[field: SerializeField] public bool IsPersistent { get; private set; } = true;


	public virtual void Awake() 
	{
		if(IsPersistent) 
        {	
			// is persistent: any other instances created 
			//	AFTER the first will be immediately destroyed
			if(!Instance) 
            {
				Instance = this as T;
			}
			else 
            {
				Destroy(gameObject);
			}
			DontDestroyOnLoad(gameObject);
		}
		else 
        {
			// not persistent: overwrite any previously-created instances.
			// will be destroyed when loading new scene
			Instance = this as T;
		}
	}
}