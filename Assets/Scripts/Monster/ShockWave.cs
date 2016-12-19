using UnityEngine;
using System;
using System.Collections;

public class ShockWave : MonoBehaviour
{
	public Monster AttackMonster;

	public int damage;
	[SerializeField] GameObject checkTempData;
	[SerializeField] CharacterManager tempData;

	public void SetDamage (int _damage, Monster _AttackMonster)
	{
		AttackMonster = _AttackMonster;
		damage = _damage;
	}

	void OnParticleCollision (GameObject objectData)
	{
		checkTempData = objectData.gameObject;
		try
		{
			tempData = checkTempData.GetComponent<CharacterManager> ();
			tempData.HitDamage (damage);
		} catch

		{
		}
	}
}
