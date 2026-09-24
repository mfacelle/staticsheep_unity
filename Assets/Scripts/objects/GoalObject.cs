using UnityEngine;

public class GoalObject : MonoBehaviour
{

	private Rigidbody2D body;

    [SerializeField] private StageClearManager stageClearManager;

    // need some kind of level manager reference

	void Awake() 
	{
		body = GetComponent<Rigidbody2D>();
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
        Debug.Log("goal object hit. outside if");
        // TODO this conflicts with SheepGoalAnimationController... game state gets set to stage end
        // before the actual animation collision stuff is triggered.
		if (collision.gameObject.CompareTag("particle"))
        {
            // add some kind of UI element saying stage cleared
            // (or "sheep restored" or something)
            Debug.Log("win!");

            // TODO what about subsequent collisions between the win screen showing up and actually loading
            // the next scene? maybe just need a simple bool for this
            stageClearManager.LoadNextStage();
        }
	}
}
