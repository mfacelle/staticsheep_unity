using UnityEngine;
using UnityEngine.Pool;

public class ChargedParticle : ChargedObject
{
	private Rigidbody2D body;

    private IObjectPool<ChargedParticle> objectPool;

	// eventually this should just be a callback for DeleteParticle
	private PhysicsManager physicsManager;

	// essentially the constructor, since Unity doesn't construct objects like regular code
	public void Init(PhysicsManager manager, IObjectPool<ChargedParticle> pool)
	{
		physicsManager = manager;
		objectPool = pool;
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
		DestroyParticle();

		// TODO - add checking for if "goal" was hit (could even do in that class instead?)

		// TODO - add animations
	}
	private void OnBecameInvisible()
    {
        DestroyParticle();
    }

	// don't love this, because it means particles need to know who manages them...
	// but is there a better way? need to detect collision in this class.
	// maybe having a callback function to PhysicsManager (instead of the object ref) 
	// to delete the particle could also release it from the pool there?
	private void DestroyParticle()
	{
		// remove this particle from list in the physics manager
		physicsManager.DeleteParticle(this);

		// release to object pool instead of destroying.
		// verify the object is still active before releasing to avoid errors
        if (gameObject.activeSelf && objectPool != null)
        {
            objectPool.Release(this);
        }
	}
}