using UnityEngine;
using System.Collections;

public class BearRoar : MonoBehaviour 
{
	public GameObject Attacker;
	public int damage;
	public Rigidbody sphereRigid;
	public BoxCollider roarBox;
	public GameObject player;

	// Use this for initialization
	void Start () 
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		roarBox = this.GetComponent<BoxCollider> ();
		roarBox.enabled = false;
		StartCoroutine (Roar());
	}
	public void SetDamage(int _damage, GameObject _Attacker){
		damage = _damage;
		Attacker = _Attacker;
		
	}


	void Update ()
	{
		Destroy (this.gameObject , 3f);
	}
	
	// Update is called once per frame

	IEnumerator Roar()
	{
		while (true)
		{
			if (roarBox.enabled == false)
			{
				roarBox.enabled = true;
			}

			yield return new WaitForSeconds (0.1f);

			if (roarBox.enabled == true)
			{
				roarBox.enabled = false;
			}
			
		}
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
