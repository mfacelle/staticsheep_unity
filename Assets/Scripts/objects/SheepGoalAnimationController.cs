using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SheepGoalAnimationController : SheepAnimationController
{
    private bool alreadyCollided = false;

	void Awake() 
	{
        alreadyCollided = false;
    }

	override protected void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("goal sheep hit! outside if. alreadyCollided=" + alreadyCollided);
        // only allow collisions to trigger animation for goal sheep once
        if (!alreadyCollided && collision.gameObject.CompareTag("particle"))
        {
            Debug.Log("goal sheep hit!");
            alreadyCollided = true;
            base.OnCollisionEnter2D(collision);
        }
    }

}
