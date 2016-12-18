using UnityEngine;
using System;
using System.Collections;

public class FrogBullet : MonoBehaviour {
	public GameObject AttackMonster;
	public int damage;
	public Vector3 moveVector;

	public void SetDamage(int _damage, GameObject _AttackMonster){
		AttackMonster = _AttackMonster;
		damage = _damage;
	}
	public void SetMoveVector(Vector3 _moveVector){
		moveVector = _moveVector;
		StartCoroutine(MoveStart ());
	
	}
	IEnumerator MoveStart(){
		while (true) {
			yield return null;
			transform.Translate (moveVector*5*Time.deltaTime);
		}
	}

	void OnTriggerEnter(Collider coll){
		if (coll.gameObject.layer == LayerMask.NameToLayer ("Player")) {
			coll.gameObject.GetComponent<CharacterManager> ().HitDamage (damage);
		}
		if (coll.gameObject.layer == LayerMask.NameToLayer ("Map")) {
			Destroy (this.gameObject);
		}
	}
}