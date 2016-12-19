using UnityEngine;
using System.Collections;

public class FireBall : MonoBehaviour {

  	public ParticleSystem FireBallparticleSystem;
	public CharacterManager charManager;
	public GameObject character;
	public int FireBallDamage;
	public float FireBallSpeed;
	public Rigidbody FireBallRigid;

	int skillLv;
	AudioSource fireBallSound;
	AudioClip flyingBall;
    void Start()
    {
		character = GameObject.FindWithTag ("Player");
		FireBallRigid = GetComponent<Rigidbody> ();
		FireBallRigid.velocity = transform.forward* FireBallSpeed;
		fireBallSound =this.gameObject.GetComponent<AudioSource> ();
		FireBallSpeed = 15;

		flyingBall = Resources.Load<AudioClip> ("Sound/MageEffectSound/MeteorDropSound");

		fireBallSound.PlayOneShot (flyingBall);

		FireBallparticleSystem = GetComponent<ParticleSystem>();
		skillLv = charManager.CharacterStatus.SkillLevel [0];
		FireBallDamage =(int) ((SkillManager.instance.SkillData.GetSkill ((int)charManager.CharacterStatus.HClass, 1).GetSkillData (skillLv).SkillValue)* charManager.CharacterStatus.Attack);
		Destroy (this.gameObject, 1.45f);
	}

    void Update()
    {
		FireBallparticleSystem.Simulate(1.5f, true);
    }

	void OnCollisionEnter(Collision coll)
	{
		if(coll.gameObject.layer == LayerMask.NameToLayer("Map"))
		{
			Destroy(gameObject);
			Instantiate(Resources.Load<GameObject>("Effect/FireBallExplosion"), coll.contacts[0].point, Quaternion.identity);
		}

	}
	void OnTriggerEnter(Collider coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer ("Enermy"))
		{
			Debug.Log ("in monster");
			Monster monsterDamage = coll.gameObject.GetComponent<Monster> ();

			if (monsterDamage != null)
			{	
				monsterDamage.HitDamage (FireBallDamage);
				FireBallDamage = 0;
			}

			Destroy(gameObject);
			Instantiate(Resources.Load<GameObject>("Effect/FireBallExplosion"), this.transform.position, Quaternion.identity);

		}
	}

}
