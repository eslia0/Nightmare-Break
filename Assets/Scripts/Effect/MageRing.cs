using UnityEngine;
using System.Collections;

public class MageRing : MonoBehaviour 
{

	public ParticleSystem FireBallparticleSystem;
	public CharacterManager charManager;
	public GameObject character;
	public int ringDamage;
	AudioSource ringSound;
	public AudioClip ringClipSound;
	int skillLv;

	// Use this for initialization
	void Start () 
	{
		ringSound = this.gameObject.GetComponent<AudioSource> ();
		ringClipSound = Resources.Load<AudioClip> ("Sound/MageEffectSound/HowlingSound");
		ringSound.PlayOneShot (ringClipSound);
	
		character = GameObject.FindWithTag ("Player");
		charManager = character.GetComponent<CharacterManager> ();
	
		skillLv = CharacterStatus.Instance.SkillLevel [2];
		ringDamage =(int) ((SkillManager.instance.SkillData.GetSkill ((int)CharacterStatus.Instance.HClass, 3).GetSkillData (skillLv).SkillValue)* CharacterStatus.Instance.Attack);
	
	}
	
	// Update is called once per frame

	void OnTriggerEnter(Collider coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer ("Enermy"))
		{
			Debug.Log ("in monster");
			Monster monsterDamage = coll.gameObject.GetComponent<Monster> ();

			if (monsterDamage != null)
			{	
				monsterDamage.HitDamage (ringDamage,character );
				ringDamage = 0;
			}

		}
	}

}
