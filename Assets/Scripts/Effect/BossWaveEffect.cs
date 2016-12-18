using UnityEngine;
using System.Collections;

public class BossWaveEffect : MonoBehaviour 
{
	public Monster monster;
	public int damage;
	public Rigidbody sphereRigid;
	public BoxCollider waveBox;
	public GameObject player;

	public Rigidbody waveRigd;
	public float waveSpeed;
	// Use this for initialization
	void Start () 
	{
		damage = 10;
		waveSpeed = 10.0f;
		waveRigd = GetComponent<Rigidbody> ();
		player = GameObject.FindGameObjectWithTag ("Player");
		waveBox = this.GetComponent<BoxCollider> ();
		waveBox.enabled = false;
		//Destroy (this.gameObject , 1f);
		StartCoroutine (WaveDown());
	}

	IEnumerator WaveDown()
	{
		
		yield return new WaitForSeconds (0.3f);
		waveRigd.velocity = transform.up * 3f;
	}
	public void BoxColliderOn()
	{
		waveBox.enabled = true;
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
