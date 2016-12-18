using UnityEngine;
using System.Collections;

public class BossSphereExplosion : MonoBehaviour
{
	BoxCollider box;
	public int damage;

	// Use this for initialization
	void Start () 
	{
		box = this.GetComponent<BoxCollider> ();
		damage = 20;
		Destroy (this.gameObject, 0.3f);
	}
	
	void OnTriggerEnter(Collider coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer ("Player"))
		{

			CharacterManager CharObject = coll.gameObject.GetComponent<CharacterManager> ();
			if (damage != 0)
			{
				CharObject.HitDamage (damage);
				//damage = 0;
			}
		}

	}

}
