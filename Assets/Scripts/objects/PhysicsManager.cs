using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// contains constants and functions for physics stuff
public class PhysicsManager : MonoBehaviour
{

	// electrostatic constant (to scale calculations, like gravitational constant G).
	// TODO should this be hard-coded and consistent across all levels? probably
	// real world: k=8.98E9
	[SerializeField] private float E = 0.01f;

	// softening to avoid massive forces if objects too close
	[SerializeField] private float SOFTENING = 0.1f;

	// max distance to apply force calculations for
	[SerializeField] private float MAX_DISTANCE = 12.0f;

    // charged objects in the scene.  Use array because these should be static (...for now).
	// eventually, want the ability to hit stuff in the scene and create new ChargedObject instances
	[SerializeField] private ChargedObject[] chargedObjects;

	// non-charged objects in the scene
	// TODO - may not need this here... just do colision detection
	// public Obstacle[] obstacles;

    // particles launched by the player.  Use HashSet for repeated random add/remove
	private HashSet<ChargedParticle> chargedParticles = new HashSet<ChargedParticle>();


	// apply electromagnetic force to all particles
	void FixedUpdate() 
	{
		// need to add logic for scene being paused
		foreach (ChargedParticle particle in chargedParticles) 
		{

			Vector2 particlePosition = particle.transform.position;
			float accelX = 0;
			float accelY = 0;

			// ignore any calculations if particle is neutral-charged
			// (note that a charge value of 0 can still be affected by omnitive/ablative charges)
			if (particle.Type == ChargedObject.ChargeType.Zero) 
			{
				continue;
			}

			// get total of all forces that affect the particle
			foreach (ChargedObject chargedObj in chargedObjects) 
			{
				// ignore if object is neutral-charged
				if (chargedObj.Type == ChargedObject.ChargeType.Zero) 
				{
					continue;
				}

				// calculate force and get angle
				Vector2 objectPosition = chargedObj.transform.position;
				float dx = particlePosition.x - objectPosition.x;
				float dy = particlePosition.y - objectPosition.y;
				float distanceSq = dx*dx + dy*dy + SOFTENING*SOFTENING;
				if (distanceSq < MAX_DISTANCE * MAX_DISTANCE) 
				{
					float chargeProduct = chargedObj.Charge * particle.Charge;
					float forceMag = E * chargeProduct / distanceSq;

					// make omnitive always attract
					if (chargedObj.Type == ChargedObject.ChargeType.Omnitive)
					{
						// need to also handle case where particle.Charge == 0
					}
					// make ablative always repel
					else if (chargedObj.Type == ChargedObject.ChargeType.Omnitive)
					{
						// need to also handle case where particle.Charge == 0
					}

					float distance = Mathf.Sqrt(distanceSq);
					accelX += forceMag * (dx/distance);
					accelY += forceMag * (dy/distance);
				}
			}

			// apply force to the particle
			// TODO - apply to rigidbody2D or something? or set for ChargedParticle.FixedUpdate to apply
			particle.ApplyForce(new Vector2(accelX, accelY));
		}
	}

	public void AddParticle(ChargedParticle particle)
	{
		chargedParticles.Add(particle);
	}


	public void DeleteParticle(ChargedParticle particle)
	{
		chargedParticles.Remove(particle);
	}

}
