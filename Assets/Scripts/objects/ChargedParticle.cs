using UnityEngine;

public class ChargedParticle : ChargedObject
{
	private Rigidbody2D body;

	// eventually this should just be a callback for DeleteParticle
	private PhysicsManager physicsManager;

	// essentially the constructor, since Unity doesn't construct objects like regular code
	public void Init(PhysicsManager manager)
	{
		physicsManager = manager;
	}

	void Awake() 
	{
		body = GetComponent<Rigidbody2D>();
	}


	public void ApplyForce(Vector2 force) 
	{
		body.AddForce(force, ForceMode2D.Impulse);
	}


	private void OnCollisionEnter2D(Collision2D collision)
	{
		// don't destroy if collision was with another particle
		if (!collision.gameObject.CompareTag("particle"))
		{
			DestroyParticle();
		}

		// TODO - add animations
	}
	private void OnBecameInvisible()
    {
        DestroyParticle();
    }

	private void DestroyParticle()
	{
		// remove this particle from list in the physics manager

		// don't love this, because it means particles need to know who manages them...
		// but is there a better way? need to detect collision in this class.
		// maybe having a callback function to PhysicsManager (instead of the object ref) 
		// to delete the particle could also release it from the pool there?

		physicsManager.DeleteParticle(this);

		Destroy(gameObject);
	}
}