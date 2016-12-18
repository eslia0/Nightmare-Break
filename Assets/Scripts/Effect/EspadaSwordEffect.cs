using UnityEngine;
using System.Collections;

public class EspadaSwordEffect : MonoBehaviour
{
	GameObject giganticSword;
	public CharacterManager charManager;
	float giganticSwordAliveTime;
	public GameObject character;
	public GameObject swordEffect;
	public int swordDamage;
	public AudioSource swordSound;
	public AudioClip swordSummonSound;
	public AudioClip swordFinishSound;

	public Rigidbody giganticSwordRigd;
	public float swordSpeed;
	public bool checkMap;

	public BoxCollider giganticSwordBox;
	public Material swordMaterial;
	public float swordAlpha;

    void Start()
    {
		character = GameObject.FindWithTag ("Player");
		swordSound = this.gameObject.GetComponent<AudioSource> ();
		swordSummonSound =  Resources.Load<AudioClip> ("Sound/WarriorEffectSound/GiganticSwordSummonEffectSound");
		swordFinishSound = Resources.Load<AudioClip> ("Sound/WarriorEffectSound/GiganticSwordFinishEffectSound");
		charManager = character.GetComponent<CharacterManager> ();
		giganticSwordRigd = GetComponent<Rigidbody> ();
		giganticSwordBox = GetComponent<BoxCollider> ();
		swordSpeed = 40;
		giganticSwordRigd.velocity = (transform.forward* swordSpeed);
		swordDamage = 10000;
		giganticSword = this.gameObject;
		swordSound.volume = 0.1f;
		swordSound.PlayOneShot (swordSummonSound);
		checkMap = true;
		Destroy (this.gameObject, 3.0f);

		swordMaterial.SetFloat ("_Alpha",1);
		swordAlpha = 1f;

	}



	IEnumerator SwordAlpha()
	{
		yield return new WaitForSeconds (0.35f);
		while (true)
		{
// 			swordDamage =(int) ((SkillManager.instance.SkillData.GetSkill ((int)charStatus.HClass, 4).GetSkillData (charStatus.skillLv).SkillValue)*  charStatus.Attack);
			swordAlpha -= 0.1f;
			swordMaterial.SetFloat ("_Alpha", swordAlpha);
			Debug.Log (swordAlpha);
			yield return new WaitForSeconds (0.1f);

		}
	}

	void OnTriggerEnter(Collider coll)
	{

		if (coll.gameObject.layer == LayerMask.NameToLayer ("Map") && checkMap)
		{
			checkMap = false;
			StartCoroutine (InStopSword());
		}
		if (coll.gameObject.layer == LayerMask.NameToLayer ("Enermy"))
		{
			Debug.Log ("in monster");
			Monster monsterDamage = coll.gameObject.GetComponent<Monster> ();

			if (monsterDamage != null)
			{	
				Debug.Log (character);

				monsterDamage.HitDamage (swordDamage,character );
				swordDamage = 0;

			}
		}
	}

	IEnumerator InStopSword()
	{
		yield return new WaitForSeconds (0.15f);
		StartCoroutine (SwordAlpha ());
		giganticSwordRigd.velocity = (transform.forward* 0);
		giganticSwordRigd.useGravity = false;
		swordEffect =Instantiate(Resources.Load<GameObject>("Effect/SwordExplosion"), new Vector3(this.transform.position.x,this.transform.position.y-3f,this.transform.position.z),new Quaternion(90,-90,0,0))as GameObject;
		
		Destroy (swordEffect, 2.5f);
		swordSound.PlayOneShot (swordFinishSound);
	}

}