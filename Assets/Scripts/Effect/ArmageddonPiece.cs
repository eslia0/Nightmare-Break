using UnityEngine;
using System.Collections;

public class ArmageddonPiece : MonoBehaviour 
{

	public CharacterStatus charStatus;
	public CharacterManager charManager;
	public GameObject character;
	public BoxCollider pieceBox;
	public Armageddon armageddon;
	public int armageddonPieceDamage;

	// Use this for initialization
	void Start () 
	{
		character = GameObject.FindWithTag ("Player");
		charManager = character.GetComponent<CharacterManager> ();
		charStatus = charManager.CharStatus;
		pieceBox = GetComponent<BoxCollider> ();
		armageddon = this.gameObject.GetComponentInParent<Armageddon> ();
	
		pieceBox.enabled = false;
		armageddonPieceDamage = (int)((float)armageddon.armageddonDamage * 0.1f);
	}


	public IEnumerator pieceAttackStart()
	{
		while (true)
		{
			if (pieceBox.enabled == false)
			{

				pieceBox.enabled = true;
			}

			yield return new WaitForSeconds (0.5f);		

			if (pieceBox.enabled == true)
			{
				
				pieceBox.enabled = false;
			}
		}
	}

	void OnTriggerEnter(Collider coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer ("Enermy"))
		{
			Debug.Log ("ArmaPioece");
			Monster monsterDamage = coll.gameObject.GetComponent<Monster> ();

			if (monsterDamage != null)
			{	
				//monsterDamage.HitDamage (armageddonDamage,character );
				//armageddonDamage = 0;

			}
		}

	}

	public void PieceAttack()
	{
		StartCoroutine (pieceAttackStart());
	}



}
