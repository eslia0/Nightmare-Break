using UnityEngine;
using System.Collections;

public class SwordDance : MonoBehaviour 
{
    
	public CharacterManager charManager;
	public GameObject character;
	public int bladeStormDamage=1;
	public float bladeStormSpeed = 20;
	public Rigidbody bladeStromRigid;
	public GameObject swordDanceEffect;
	int skillLv;

	// Use this for initialization
	void Start ()
	{
		character = GameObject.FindWithTag ("Player");
		charManager = character.GetComponent<CharacterManager> ();
		bladeStromRigid = GetComponent<Rigidbody> ();
		bladeStromRigid.velocity = transform.forward * bladeStormSpeed;
		swordDanceEffect = Resources.Load<GameObject> ("Effect/SwordShadow");
		skillLv = characterStatus.SkillLevel [2];
		bladeStormDamage =(int) ((SkillManager.instance.SkillData.GetSkill ((int)characterStatus.HClass, 3).GetSkillData (skillLv).SkillValue)* characterStatus.Attack);
	}
	void Update()
	{
		if (!swordDanceEffect)
		{
			Debug.Log ("In danceDes");
			Destroy (this.gameObject);
		}
	}
	void OnTriggerEnter(Collider coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer ("Enermy"))
		{
			Debug.Log (skillLv);
			Debug.Log (bladeStormDamage);

			Monster monsterDamage = coll.gameObject.GetComponent<Monster> ();

			if (monsterDamage != null)
			{	
				monsterDamage.HitDamage (bladeStormDamage,character );
				bladeStormDamage = 0;
			}
		}
	}
}
