using UnityEngine;

public class CharWeapon : MonoBehaviour
{
    GameObject character;
    CharacterManager characterManager;
    CharacterStatus characterStatus;

    public int damage = 0;
    int skillLv;
	public AudioSource attackSound;
	public AudioClip attack1;
	public AudioClip attack2;
	public AudioClip attack3;
	public AudioClip cutOffEffectSound;
	public AudioClip swordDanceEffectSound;
	public AudioClip mealstromEffectSound;
	public AudioClip swordDanceFinishEffectSound;
	public AudioClip giganticSwordSound;

    public void InitializeCharacterWeapon(GameObject player)
    {
        character = player;
        characterManager = character.GetComponent<CharacterManager>();
        characterStatus = characterManager.CharacterStatus;
        attackSound = GetComponent<AudioSource>();
        attack1 = Resources.Load<AudioClip>("Sound/EffectSound/AttackEffectSound1");
        attack2 = Resources.Load<AudioClip>("Sound/EffectSound/AttackEffectSound2");
        attack3 = Resources.Load<AudioClip>("Sound/EffectSound/AttackEffectSound3");
        cutOffEffectSound = Resources.Load<AudioClip>("Sound/WarriorEffectSound/CutOffEffectSound");
        swordDanceEffectSound = Resources.Load<AudioClip>("Sound/WarriorEffectSound/SwordDanceEffectSound");
        mealstromEffectSound = Resources.Load<AudioClip>("Sound/WarriorEffectSound/MealstromEffectSound");
        swordDanceFinishEffectSound = Resources.Load<AudioClip>("Sound/WarriorEffectSound/SwordDanceFinishEffectSound");
        giganticSwordSound = Resources.Load<AudioClip>("Sound/WarriorEffectSound/GiganticSwordSummonSound");
        attackSound.volume = 0.5f;
        skillLv = characterStatus.SkillLevel[5];
    }

	public void AttackEffectSound1()
	{
		attackSound.PlayOneShot (attack1);
	}

	public void AttackEffectSound2()
	{
		attackSound.PlayOneShot (attack2);
	}

	public void AttackEffectSound3 ()
	{
		attackSound.PlayOneShot (attack3);
	}

	public void CutOffEffectSound ()
	{
		attackSound.PlayOneShot (cutOffEffectSound);
	}

	public void SwordDanceEffectSound ()
	{
		attackSound.PlayOneShot (swordDanceEffectSound);
	}

	public void SwordDanceFinishEffectSound ()
	{
		attackSound.PlayOneShot (swordDanceFinishEffectSound);
	}
	public void MealstromEffectSound ()
	{
		attackSound.PlayOneShot (mealstromEffectSound);
	}
	public void GiganticSwordSound()
	{
		attackSound.PlayOneShot (giganticSwordSound);
	}

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Enermy"))
        {
            Monster monster = coll.gameObject.GetComponent<Monster>();
         //  charManager.UIManager.BattleUIManager.monsterHpBarCalculation(monster.gameObject.name, monster.MaxHP, monster.CurrentHP);
//           ComboSystem.instance.ComboProcess(++charManager.ComboCount);
           StartCoroutine(characterManager.ComboCheck(characterManager.ComboCount));

            Instantiate(Resources.Load<GameObject>("Effect/HitEffect"), new Vector3(coll.transform.position.x, coll.transform.position.y + 1.0f, coll.transform.position.z + 0.5f), Quaternion.identity);
            if (monster != null)
            {
                if (characterManager.NormalAttackState)
                {
                    damage = characterStatus.Attack;
                }
                else if (characterManager.SkillAttackState)
                {
                    damage = characterStatus.Attack;
                }

                if (damage != 0)
                {
                    if (characterStatus.HClass == CharacterStatus.CharClass.Warrior)
                    {
                        if (characterStatus.SkillLevel[5] < 4)
                        {
                            if (characterManager.NormalAttackState)
                            {
                                int testPassiveHP;

                                testPassiveHP = (int)((SkillManager.instance.SkillData.GetSkill((int)characterStatus.HClass, 4).GetSkillData(skillLv).SkillValue) * damage);

                                if (characterStatus.MaxHealthPoint > characterStatus.HealthPoint)
                                {
                                    characterStatus.DecreaseHealthPoint(-testPassiveHP);
                                    Debug.Log("blood");
                                }
                            }
                        }
                        else if (characterStatus.SkillLevel[5] == 4)
                        {
                            Debug.Log("in Warrior");
                            int testPassiveHP;

                            testPassiveHP = (int)((SkillManager.instance.SkillData.GetSkill((int)characterStatus.HClass, 4).GetSkillData(skillLv).SkillValue) * damage);
                            if (characterStatus.MaxHealthPoint > characterStatus.HealthPoint)
                            {
                                characterStatus.DecreaseHealthPoint(-testPassiveHP);
                            }
                        }
                    }
					monster.HitDamage (damage);
                    damage = 0;
                }
            }
        }
//        else {
//            ComboSystem.instance.ComboProcess(++charManager.ComboCount);
//            StartCoroutine(charManager.ComboCheck(charManager.ComboCount));
//			ComboSystem.instance.ComboAction(Camera.main.transform);
//            Instantiate(Resources.Load<GameObject>("Effect/HitEffect"), new Vector3(coll.transform.position.x, coll.transform.position.y + 1.0f, coll.transform.position.z + 0.5f), Quaternion.identity);
//        
//        }
    }
}
