using UnityEngine;
using System.Collections;

public class MageNormalAttack : MonoBehaviour {
    
	public CharacterManager characterManager;
	public CharacterStatus characterStatus;

	public int MageBallDamage;
	public float MageBallSpeed;
	public Rigidbody MageBallRigid;

    public void InitializeMageNormalAttack(CharacterManager newCharacterManager)
    {
        characterManager = newCharacterManager;
        characterStatus = characterManager.CharacterStatus;
        MageBallSpeed = 15;
        MageBallRigid = GetComponent<Rigidbody>();
        MageBallRigid.velocity = transform.forward * MageBallSpeed;
        Destroy(this.gameObject, 1f);
    }

	void OnTriggerEnter(Collider coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer ("Enermy"))
		{
			Monster monsterDamage = coll.gameObject.GetComponent<Monster> ();

			if (monsterDamage != null)
			{	
				MageBallDamage = characterStatus.Attack;
				monsterDamage.HitDamage (MageBallDamage);
				Debug.Log (MageBallDamage);
				MageBallDamage = 0;
			}

			Destroy (gameObject);
			Instantiate (Resources.Load<GameObject> ("Effect/NormalAttackExplosion"), this.transform.position, Quaternion.identity);

		}
		else if (coll.gameObject.layer == LayerMask.NameToLayer ("Map"))
		{

			Destroy (gameObject);
			Instantiate (Resources.Load<GameObject> ("Effect/NormalAttackExplosion"), this.transform.position, Quaternion.identity);
			
		}
	}


}
