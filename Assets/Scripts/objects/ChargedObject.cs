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

}
