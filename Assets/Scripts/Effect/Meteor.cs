using UnityEngine;
using System.Collections;

public class Meteor : MonoBehaviour {
    
	public CharacterManager charManager;
	public GameObject character;
	public int MeteorDamage;
	int skillLv;
	AudioSource meteorSound;
	public AudioClip meteorDropSound;

	void Start()
	{
		character = GameObject.FindWithTag ("Player");
		charManager = character.GetComponent<CharacterManager> ();
		skillLv = charManager.CharacterStatus.SkillLevel [1];
		MeteorDamage =(int) ((SkillManager.Instance.SkillData.GetSkill ((int)GameManager.Instance.CharacterStatus.HClass, 2).GetSkillData (skillLv).SkillValue)* charManager.CharacterStatus.Attack);
		meteorSound = this.gameObject.GetComponent<AudioSource> ();
		meteorDropSound = Resources.Load<AudioClip> ("Sound/MageEffectSound/MeteorDropSound");

		meteorSound.PlayOneShot (meteorDropSound);
	}

    void Update()
    {
        transform.Translate(0, 0, 30 * Time.smoothDeltaTime, Space.Self);    
    }

    void OnCollisionEnter(Collision coll)
    {
        if(coll.gameObject.layer == LayerMask.NameToLayer("Map"))
        {
            Destroy(gameObject);
            Instantiate(Resources.Load<GameObject>("Effect/MeteorExplosion"), coll.contacts[0].point, Quaternion.identity);
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
				monsterDamage.HitDamage (MeteorDamage);
				MeteorDamage = 0;
			}

			Destroy (gameObject);
			Instantiate (Resources.Load<GameObject> ("Effect/MeteorExplosion"), this.transform.position, Quaternion.identity);
		}
		else if (coll.gameObject.layer == LayerMask.NameToLayer ("Map"))
		{
			Destroy (gameObject);
			Instantiate (Resources.Load<GameObject> ("Effect/MeteorExplosion"), this.transform.position, Quaternion.identity);
		
		}
	}
}
