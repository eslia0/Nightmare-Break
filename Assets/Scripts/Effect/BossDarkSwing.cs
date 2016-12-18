using UnityEngine;
using System.Collections;

public class BossDarkSwing : MonoBehaviour 
{

	public GameObject Attacker;
	public int damage;
	public Rigidbody sphereRigid;
	public CapsuleCollider swingBox;
	public GameObject player;

	public Rigidbody swingRigd;
	public float swingSpeed;


	// Use this for initialization
	void Start () 
	{
		
		swingSpeed = 10.0f;
		swingRigd = GetComponent<Rigidbody> ();
		player = GameObject.FindGameObjectWithTag ("Player");
		swingBox = this.GetComponent<CapsuleCollider> ();
		swingRigd.velocity = -transform.up * swingSpeed;
		Destroy (this.gameObject , 3f);	
	}

	public void SetDamage(int _damage ,GameObject _Attacker){
		damage = _damage;
		Attacker = _Attacker;
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
