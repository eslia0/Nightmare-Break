using UnityEngine;
using System.Collections;

public class SwordShadow : MonoBehaviour 
{
	public CharacterManager charManager;
	public GameObject character;
	int swordShadowDamage;
	int skillLv;


	// Use this for initialization
	void Start () 
	{
		character = GameObject.FindWithTag ("Player");
		charManager = character.GetComponent<CharacterManager> ();
		skillLv = GameManager.Instance.CharacterStatus.SkillLevel [1];
		swordShadowDamage =(int) ((SkillManager.Instance.SkillData.GetSkill ((int)GameManager.Instance.CharacterStatus.HClass, 2).GetSkillData (skillLv).SkillValue)* GameManager.Instance.CharacterStatus.Attack);
	}	
	// Update is called once per frame


	void OnTriggerEnter(Collider coll)
	{

		Debug.Log (swordShadowDamage);
		if (coll.gameObject.layer == LayerMask.NameToLayer ("Enermy"))
		{
			Monster monsterDamage = coll.gameObject.GetComponent<Monster> ();
			if (monsterDamage != null)
			{	
				monsterDamage.HitDamage (swordShadowDamage);
				swordShadowDamage = 0;
			}
		}
	}
}
