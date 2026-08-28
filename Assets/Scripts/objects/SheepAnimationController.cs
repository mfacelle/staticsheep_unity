using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SheepAnimationController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        // start idle animation at a random normalized time to stagger multiple sheep
        // -1 means default/current layer
        animator.Play("Sheep_Idle", -1, Random.Range(0.0f, 1.0f));
    }

    // TODO include some logic to handle multiple animations, eventually

	private void OnCollisionEnter2D(Collision2D collision)
    {
        // if hit with a particle, trigger animation
		if (collision.gameObject.CompareTag("particle"))
        {
            animator.SetTrigger("SheepHit");
        }
    }

}
