using UnityEngine;

public class GoalObject : MonoBehaviour
{

	private Rigidbody2D body;

    // need some kind of level manager reference

	void Awake() 
	{
		body = GetComponent<Rigidbody2D>();
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("particle"))
        {
            // add some kind of UI element saying stage cleared
            // (or "sheep restored" or something)
            Debug.Log("win!");

            // should instead pass in some kind of sequential stage manager or callback or something,
            // rather than reference as a singleton.  Works for now, though
            LevelLoader.Instance.LoadNextStage();
        }
	}
}
