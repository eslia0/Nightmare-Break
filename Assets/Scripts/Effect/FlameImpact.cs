using UnityEngine;
using System.Collections;

public class FlameImpact : MonoBehaviour 
{
	public ParticleSystem FireBallparticleSystem;
	public CharacterStatus charStatus;
	public CharacterManager charManager;
	public GameObject character;
	public int flameImpactDamage;
	AudioSource ringSound;
	public AudioClip ringClipSound;
	int skillLv;
	// Use this for initialization
	void Start () 
	{
		character = GameObject.FindWithTag ("Player");
		charManager = character.GetComponent<CharacterManager> ();
		charStatus = charManager.CharStatus;
		skillLv = charStatus.SkillLevel [0];
		ringSound = this.gameObject.GetComponent<AudioSource> ();
		flameImpactDamage =(int) ((SkillManager.instance.SkillData.GetSkill ((int)charStatus.HClass, 1).GetSkillData (skillLv).SkillValue)*  charStatus.Attack);

	}

	void OnTriggerEnter(Collider coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer ("Enermy"))
		{
			Debug.Log ("in monster");
			Monster monsterDamage = coll.gameObject.GetComponent<Monster> ();

			if (monsterDamage != null)
			{	
				monsterDamage.HitDamage (flameImpactDamage,character );
				flameImpactDamage = 0;
			}

			Destroy(gameObject);
			//Instantiate(Resources.Load<GameObject>("Effect/MeteorExplosion"), this.transform.position, Quaternion.identity);

		}

	}
}
