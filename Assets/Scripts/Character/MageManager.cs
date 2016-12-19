using UnityEngine;
using System.Collections;

public class MageManager : CharacterManager 
{
	public GameObject frameDestroy;
	public GameObject fireBall;
	public GameObject FireBallPos;
	public GameObject armageddon;
	public GameObject mageRing;
	public GameObject howlingObj;
	public Armageddon ArmageddonDamage;
	public bool howling;
	public bool poweroverwhelming;
	public GameObject superArmorEffect;
	public Armageddon armaDestroy;
	public AudioClip ArmageddonFinishSound;
	public AudioClip Meteor;
	public AudioClip fireBallCastSound;


	public override void ProcessSkill1 ()
	{
		
	}
	public override void ProcessSkill2 ()
	{
		
	}
	public override void ProcessSkill3 ()
	{
		FireHowling ();
	}
	public override void ProcessSkill4 ()
	{

	}

	public void StrikeBall()
	{
		if (transform.rotation.y == 0)
		{
			Instantiate (Resources.Load<GameObject> ("Effect/MageNormalAttack"), FireBallPos.transform.position, Quaternion.Euler (0, 0, 0));
		}
		else
		{
			Instantiate (Resources.Load<GameObject> ("Effect/MageNormalAttack"), FireBallPos.transform.position, Quaternion.Euler (0, 180, 0));
		}
	}

	public void SummonFireBall()
	{
		if(!fireBall)
		{
			if (transform.rotation.y == 0)
			{
				fireBall = Instantiate (Resources.Load<GameObject> ("Effect/FireBall"),FireBallPos.transform.position, Quaternion.Euler (0, 0, 0)) as GameObject;
			}
			else
			{
				fireBall = Instantiate (Resources.Load<GameObject> ("Effect/FireBall"), FireBallPos.transform.position,Quaternion.Euler (0, 180, 0)) as GameObject;
			}
			
		}
		
	}

	public void DropMeteo()
	{
		if (!frameDestroy)
		{
			if (transform.rotation.y == 0)
			{
				frameDestroy = Instantiate (Resources.Load<GameObject> ("Effect/FireMagic"), new Vector3 (transform.position.x, transform.position.y + 10.0f, transform.position.z -3.0f), Quaternion.Euler (-135, 0, 0)) as GameObject;
				
			}
			else
			{
				frameDestroy = Instantiate (Resources.Load<GameObject> ("Effect/FireMagic"), new Vector3 (transform.position.x, transform.position.y + 10.0f, transform.position.z + 3.0f),Quaternion.Euler (-45, 0, 0)) as GameObject;
			}
		}
		
	}
	public void FlameObj()
	{
		howlingObj = Instantiate (Resources.Load<GameObject> ("Effect/HowlingExplosion"), new Vector3 (transform.position.x, transform.position.y, transform.position.z), Quaternion.Euler (0, 0, 0))as GameObject;
		Destroy(howlingObj , 2f);
	}

	public void FireHowling()
	{
		if (howling)
		{
			if (!mageRing)
			{
				mageRing = Instantiate (Resources.Load<GameObject> ("Effect/FlameImpact"), new Vector3 (transform.position.x, transform.position.y, transform.position.z), Quaternion.Euler (0, 0, 0))as GameObject;

			}
			float howlingSpeed = 5f;
			float howlingDistance;
			skillTime += Time.deltaTime;

			if (enermy != null)
			{
				for (int i = 0; i < enermy.Length; i++)
				{
					if (enermy [i] != null)
					{
						howlingDistance = Vector3.Distance (this.transform.position, enermy [i].transform.position);
					

						if (howlingDistance < 10)
						{
							enermy [i].transform.Translate ((new Vector3(enermy [i].transform.position.x,0,enermy [i].transform.position.z) - new Vector3(this.transform.position.x,0,this.transform.position.z)) * howlingSpeed * Time.deltaTime, Space.World);
							//enermy [i].transform.Translate (new Vector3(enermy [i].transform.position.x,0,enermy [i].transform.position.z)  - new Vector3(this.transform.position.x,0,this.transform.position.z) * howlingSpeed * Time.deltaTime, Space.World);
						}
					}
				}
			}
			if (skillTime >= 0.23f)
			{
				skillTime = 0;
				howling = false;
				Destroy (mageRing, 0);
			}
		}
	}
	public void Armageddon()
	{
		if (transform.rotation.y == 0) 
		{
			armageddon = Instantiate (Resources.Load<GameObject> ("Effect/Armageddon"), new Vector3 (transform.position.x, transform.position.y+7.0f, transform.position.z), Quaternion.Euler (0, 0, 0)) as GameObject;
		}
		else 
		{
			armageddon = Instantiate (Resources.Load<GameObject> ("Effect/Armageddon"), new Vector3 (transform.position.x, transform.position.y+7.0f, transform.position.z), Quaternion.Euler (0, 180, 0)) as GameObject;
		}
			ArmageddonDamage = armageddon.GetComponent<Armageddon> ();

		ArmageddonDamage.armageddonDamage = 100;
	}
	public void HowlingForce()
	{
		howling = true;
	}
	public void Destroy()
	{
		armaDestroy = armageddon.GetComponent<Armageddon> ();
		armaDestroy.Destroy ();
	}

	public override void HitDamage (int _damage)
	{
		
		if (!poweroverwhelming)
		{
			Debug.Log (characterStatus.HealthPoint);
			if (characterStatus.SkillLevel [5] < 4)
			{
				if (charAlive)
				{
                    characterStatus.SkillLevel [4] = 3;
					int chance = (characterStatus.SkillLevel [4]) * 25;
					int superArmor;
					superArmor = Random.Range (0, 100);

					if (characterStatus.HealthPoint > 0)
					{
                        characterStatus.DecreaseHealthPoint (_damage);

						if (chance > superArmor)
						{
							if (State != CharacterState.Skill1 && State != CharacterState.Skill2 && State != CharacterState.Skill3 && State != CharacterState.Skill4)
							{
								CharState ((int)CharacterState.HitDamage);
							}
							else
							{
								//ArmorEffect
								Debug.Log ("in superA");
								superArmorEffect = Instantiate (Resources.Load<GameObject> ("Effect/MagicionPassive2"), new Vector3 (transform.position.x, transform.position.y + 1.0f, transform.position.z), Quaternion.Euler (90, 0, 90))as GameObject;
								Destroy (superArmorEffect, 0.5f);
							}
						}
						else
						{
							CharState ((int)CharacterState.HitDamage);
						}
					}
					else if (characterStatus.HealthPoint <= 0)
					{
						CharState ((int)CharacterState.Death);
						charAlive = false;
					}
				}

			}
			else if (characterStatus.SkillLevel [5] == 4)
			{
				Debug.Log (characterStatus.HealthPoint);
				if (charAlive)
				{
					if (characterStatus.HealthPoint > 0)
					{
                        characterStatus.DecreaseHealthPoint (_damage);

						CharState ((int)CharacterState.HitDamage);

						if (State != CharacterState.Skill1 && State != CharacterState.Skill2 && State != CharacterState.Skill3 && State != CharacterState.Skill4)
						{
							CharState ((int)CharacterState.HitDamage);
						}
					}
					else if (characterStatus.HealthPoint <= 0)
					{
						CharState ((int)CharacterState.Death);
						CharAudio.PlayOneShot (Skill1Sound);

						charAlive = false;
					}
				}
			}
		}
	}

	IEnumerator unbeatableCall()
	{
		poweroverwhelming = true;

		//무적시간
		yield return new WaitForSeconds (1f);

		poweroverwhelming = false;
	}
    
	public override void UsingMagicPoint(int SkillArray)
	{
		float manaFury =SkillManager.instance.SkillData.GetSkill ((int)characterStatus.HClass, 6).GetSkillData (characterStatus.SkillLevel [5]).SkillValue;
        characterStatus.DecreaseMagicPoint ((int)((float)(SkillManager.instance.SkillData.GetSkill ((int)characterStatus.HClass, SkillArray).ManaCost)* manaFury));
	}


	public override void SetClassSound()
	{

		base.SetClassSound();

        if (characterStatus.HGender == CharacterStatus.Gender.Male)
        {
            Skill1Sound = Resources.Load<AudioClip> ("Sound/ManSound/ManMageFireBall");
			Skill2Sound = Resources.Load<AudioClip> ("Sound/ManSound/ManMageDestroy");
			Skill3Sound = Resources.Load<AudioClip> ("Sound/ManSound/ManMageHowling");
			Skill4Sound = Resources.Load<AudioClip> ("Sound/ManSound/ManMageArmageddon");
			Meteor = Resources.Load<AudioClip> ("Sound/ManSound/ManMageDestroyCast");
			fireBallCastSound = Resources.Load<AudioClip> ("Sound/ManSound/ManMageFireBallCast");
			ArmageddonFinishSound = Resources.Load<AudioClip> ("Sound/ManSound/ManGiganticSwordFinish");
		}
		else
		{
			Skill1Sound = Resources.Load<AudioClip> ("Sound/WoManSound/WoManMageFireBall");
			Skill2Sound = Resources.Load<AudioClip> ("Sound/WoManSound/WoManDestroyCast");
			Skill3Sound = Resources.Load<AudioClip> ("Sound/WoManSound/WoManMageHowling");
			Skill4Sound = Resources.Load<AudioClip> ("Sound/WoManSound/WomanSwordDance");
			Meteor = Resources.Load<AudioClip> ("Sound/WoManSound/WoManMageDestroyCast");
			fireBallCastSound = Resources.Load<AudioClip> ("Sound/WoManSound/WoManMageFireBallCast");
			ArmageddonFinishSound = Resources.Load<AudioClip> ("Sound/WoManSound/WoManMageArmageddon");

		}
	}


	public void AttackSound1()
	{
		CharAudio.PlayOneShot (attack1);
	}

	public void AttackSound2()
	{
		CharAudio.PlayOneShot (attack2);
	}

	public void AttackSound3 ()
	{
		CharAudio.PlayOneShot (attack3);
	}

	public void FireBallCastSound()
	{
		CharAudio.PlayOneShot (fireBallCastSound);
	}


	public void FireBallSound()
	{
		CharAudio.PlayOneShot (Skill1Sound);
	}

	public void MeteorStrikeSound()
	{
		CharAudio.PlayOneShot (Skill2Sound);
	}		

	public void HowlingSound()
	{
		CharAudio.PlayOneShot (Skill3Sound);
	}

	public void ArmageddonSound()
	{
		CharAudio.PlayOneShot (Skill4Sound);
	}

	public void ManDie()
	{
		CharAudio.PlayOneShot (attack3);
	}
	public void ArmageddonDestroySound()
	{
		CharAudio.PlayOneShot (ArmageddonFinishSound);
	}
	public void MeteorSound()
	{
		CharAudio.PlayOneShot (Meteor);
	}
}
