using UnityEngine;
using System.Collections;

// an object that has a charge (attracts/repels particles)
public class ChargedObject : MonoBehaviour 
{
	[SerializeField] private float charge;

	public float GetCharge()
	{
		return charge;
	}
}
