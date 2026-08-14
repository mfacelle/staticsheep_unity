using UnityEngine;
using System.Collections;


// an object that has a charge (attracts/repels particles)
public class ChargedObject : MonoBehaviour 
{
	// the type of charge.  Some rules regarding this:
	// - Normal follows standard force calculations
	// - Omnitive/Albative will be applied even if the particle has a charge value of 0
	// - Zero overrides everything and will be totally ignored (just an obstacle). may not even need this
	public enum ChargeType
	{
		Normal, // standard positive/negative
		Omnitive, // attract all, regardless of charge
		Albative, // repel all, regardless of charge
		Zero // no interaction
	}

	[field: SerializeField] public float Charge {get; private set;}

	[field: SerializeField] public ChargeType Type {get; private set;}


	// -----
	// TEMP CODE until I make real assets with different colors.
	// color sheep (and, also particles due to derived class), based on charge
	private SpriteRenderer spriteRenderer;
	void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}
	void Update()
	{
		// don't color the omnitive "goal" sheep
		if (Type != ChargeType.Omnitive)
		{
			if (Charge > 0)
			{
				// red filter
				spriteRenderer.color = new Color(1f, 0f, 0f, 0.7f); 
			}
			else if (Charge < 0)
			{
				// blue filter
				spriteRenderer.color = new Color(0f, 0f, 1.0f, 0.7f); 
			}
		}
	}
}
