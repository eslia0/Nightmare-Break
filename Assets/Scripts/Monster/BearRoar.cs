using UnityEngine;
using System.Collections;

public class BearRoar : MonoBehaviour 
{
	public Monster monster;
	public int damage;
	public Rigidbody sphereRigid;
	public BoxCollider roarBox;
	public GameObject player;

	// Use this for initialization
	void Start () 
	{
		damage = 10;
		player = GameObject.FindGameObjectWithTag ("Player");
		roarBox = this.GetComponent<BoxCollider> ();
		roarBox.enabled = false;
		StartCoroutine (Roar());
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
