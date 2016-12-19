using UnityEngine;
using System.Collections;

public class WarriorManager : CharacterManager
{
    //GiganticSword
    public GameObject GiganticSword;
    public float giganticSwordSpeed;
    public GameObject giganticSwordTemp;

    public GameObject SwordDance;
    public bool rise = false;
    public CharWeapon bloodingWeapon;
    public float riseCooltime;
    private GameObject wind;
    public GameObject swordCircle;
    [SerializeField]
    private TrailRenderer trailRenderer;
    private GameObject[] attackEffect;
    int skillLv;
    public AudioClip swordFinishSound;
    public AudioClip giganticSwordFinishSound;
    public EspadaSwordEffect espadasword;
    public AudioClip mealStromFinishSound;
    public bool mealStromTranslate;
    public bool poweroverwhelming;

    public override void NormalAttack()
    {
        base.NormalAttack();
    }

    public override void SetClassEffect()
    {
        attackEffect = new GameObject[3];

        for (int i = 0; i < attackEffect.Length; i++)
        {
            attackEffect[i] = Resources.Load<GameObject>("Effect/WarriorNormalAttack" + (i + 1));
        }
    }

    //warrior mealstrom
    public override void ProcessSkill1()
    {
        float maelstromSpeed = 0.5f;
        float maelstromDistance;
        skillTime += Time.deltaTime;

        if (wind)
        {
            //transform.Translate ((Vector3.forward * testinput.vertical - Vector3.right * testinput.horizontal) * Time.deltaTime * (characterStatus.MoveSpeed - 5f), Space.World);
        }

        if (enermy != null)
        {
            for (int i = 0; i < enermy.Length; i++)
            {
                if (enermy[i] != null)
                {
                    maelstromDistance = Vector3.Distance(this.transform.position, enermy[i].transform.position);

                    if (maelstromDistance < 10)
                    {
                        enermy[i].transform.Translate((this.transform.position - enermy[i].transform.position) * maelstromSpeed * Time.deltaTime, Space.World);
                    }
                }
            }
        }

        if (skillTime >= 1.5f)
        {
            skillTime = 0;
        }

    }

    public void MealStromFinish()
    {
        mealStromTranslate = false;
        //	if (testinput.vertical < 0)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 180.0f, 0));
            charDir = false;
        }
        //else if (testinput.vertical > 0)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0.0f, 0));
            charDir = true;
        }
    }

    public void WindEffect()
    {
        wind = Instantiate(Resources.Load<GameObject>("Effect/Wind"), new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z), Quaternion.identity) as GameObject;
        wind.transform.parent = this.gameObject.transform;
    }

    //Warrior Cutoff
    public override void ProcessSkill2()
    {
        skillTime += Time.deltaTime;


        animator.speed = 1;
        skillTime = 0;


    }

    public override void ProcessSkill3()
    {

    }

    public override void ProcessSkill4()
    {

    }

    public void SwordDanceEffect()
    {
        if (!SwordDance)
        {
            if (charDir)
            {
                SwordDance = Instantiate(Resources.Load<GameObject>("Effect/SwordDance"), new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z + 0.5f), Quaternion.Euler(-90, 0, 0)) as GameObject;
            }
            else
            {
                SwordDance = Instantiate(Resources.Load<GameObject>("Effect/SwordDance"), new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z - 0.5f), Quaternion.Euler(90, 0, 0)) as GameObject;
            }
        }
    }

    public void CutOffMove()
    {
        Instantiate(Resources.Load<GameObject>("Effect/SwordShadow"), new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z), Quaternion.identity);

        Ray cutOffDistance = new Ray(this.transform.position, transform.forward);
        RaycastHit rayHit;
        Debug.Log(cutOffDistance);
        if (Physics.Raycast(cutOffDistance, out rayHit, 5f, 1 << LayerMask.NameToLayer("Map")))
        {
            transform.Translate(0, 0, rayHit.distance - 0.5f);

        }
        else
        {
            transform.Translate(0, 0, 5);
        }
    }

    public void CutoffStop()
    {
        //animator.speed = 0;
    }

    public void SwordDanceBoxSummon()
    {
        Instantiate(Resources.Load<GameObject>("Effect/SwordDanceBox"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);

    }
    public void GiganticSwordCast()
    {
        float giganticSwordCastPos;
        if (charDir)
        {
            giganticSwordCastPos = 7.0f;
        }
        else
        {
            giganticSwordCastPos = -7.0f;
        }
        swordCircle = Instantiate(Resources.Load<GameObject>("Effect/SwordSummonCircle"), transform.position + new Vector3(0.0f, 0.2f, giganticSwordCastPos), Quaternion.Euler(new Vector3(-90.0f, 0, 0))) as GameObject;
    }
    public void GiganticSwordSummon()
    {
        float giganticSwordPos;
        if (charDir)
        {
            giganticSwordPos = 7.0f;
        }
        else
        {
            giganticSwordPos = -7.0f;
        }

        giganticSwordTemp = Instantiate(Resources.Load<GameObject>("GiganticSword"), transform.position + new Vector3(0.0f, 20.0f, giganticSwordPos), Quaternion.Euler(new Vector3(90.0f, 90.0f, -180.0f))) as GameObject;
        espadasword = giganticSwordTemp.GetComponent<EspadaSwordEffect>();
        giganticSwordTemp.gameObject.GetComponent<Rigidbody>().AddForce(-Vector3.up * giganticSwordSpeed, ForceMode.Impulse);
    }

    public override void HitDamage(int _damage)
    {
        if (!poweroverwhelming)
        {

            if (characterStatus.SkillLevel[5] < 4)
            {
                if (charAlive)
                {
                    if (charAlive)
                    {
                        if (characterStatus.HealthPoint > 0)
                        {
                            int deFendDamage;
                            deFendDamage = _damage - (characterStatus.SkillLevel[5] * 1);
                            Debug.Log(deFendDamage);
                            if (deFendDamage < 0)
                            {
                                deFendDamage = 0;
                            }
                            characterStatus.DecreaseHealthPoint(deFendDamage);

                            if (State != CharacterState.Skill1 && State != CharacterState.Skill2 && State != CharacterState.Skill3 && State != CharacterState.Skill4)
                            {
                                CharState((int)CharacterState.HitDamage);
                            }
                        }
                        if (characterStatus.HealthPoint <= 0)
                        {
                            CharState((int)CharacterState.Death);
                            charAlive = false;
                        }
                    }
                }
            }
            else if (characterStatus.SkillLevel[5] == 4)
            {
                Debug.Log(characterStatus.HealthPoint);
                if (charAlive)
                {
                    if (characterStatus.HealthPoint > 0)
                    {
                        int deFendDamage;
                        deFendDamage = _damage - (characterStatus.SkillLevel[5] * 1);

                        if (deFendDamage < 0)
                        {
                            deFendDamage = 0;
                        }
                        characterStatus.DecreaseHealthPoint(deFendDamage);

                        CharState((int)CharacterState.HitDamage);

                        if (State != CharacterState.Skill1 && State != CharacterState.Skill2 && State != CharacterState.Skill3 && State != CharacterState.Skill4)
                        {
                            CharState((int)CharacterState.HitDamage);
                        }
                    }
                    else if (characterStatus.HealthPoint <= 0)
                    {

                        CharState((int)CharacterState.Death);

                        charAlive = false;

                        if (!rise)
                        {
                            rise = true;
                            charAlive = true;
                            animator.SetBool("Rise", false);
                            StartCoroutine(colltimeCheck());
                            characterStatus.DecreaseHealthPoint((-100));

                        }
                        else if (rise)
                        {
                            animator.SetBool("Rise", true);
                        }
                    }
                }
            }
        }
    }

    IEnumerator unbeatableCall()
    {
        poweroverwhelming = true;

        //무적시간
        yield return new WaitForSeconds(1f);

        poweroverwhelming = false;
    }

    public void NormalAttackEffect2(int _attackNum)
    {
        if (_attackNum == 0)
        {
            GameObject attack1 = Instantiate(attackEffect[_attackNum], gameObject.transform.position, attackEffect[_attackNum].transform.localRotation) as GameObject;
            attack1.transform.position = new Vector3(transform.position.x + 0.05f, transform.position.y + 1.1f, transform.position.z + 2.7f);
        }
        else if (_attackNum == 1)
        {
            GameObject attack2 = Instantiate(attackEffect[_attackNum], gameObject.transform.position, attackEffect[_attackNum].transform.localRotation) as GameObject;
            attack2.transform.position = new Vector3(transform.position.x - 0.3f, transform.position.y + 0.7f, transform.position.z + 2.2f);
        }
        else
        {
            GameObject attack3 = Instantiate(attackEffect[_attackNum], gameObject.transform.position, attackEffect[_attackNum].transform.localRotation) as GameObject;
            attack3.transform.position = new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z + 2.5f);
        }
    }

    public void AttackEffect(int _attack)
    {
        if (_attack == 0)
        {
            //     trailRenderer.enabled = true;
        }
        else
        {
            //            trailRenderer.enabled = false;
        }
    }

    public IEnumerator colltimeCheck()
    {
        while (rise)
        {
            riseCooltime += 1f;
            yield return new WaitForSeconds(1f);
            Debug.Log(riseCooltime);
            if (riseCooltime > 10)
            {
                riseCooltime = 0;
                rise = false;
            }
        }
    }

    public override void SetClassSound()
    {
        base.SetClassSound();

        if (characterStatus.HGender == CharacterStatus.Gender.Male)
        {
            Skill1Sound = Resources.Load<AudioClip>("Sound/ManSound/ManMealStrom");
            Skill2Sound = Resources.Load<AudioClip>("Sound/ManSound/ManCutOff");
            Skill3Sound = Resources.Load<AudioClip>("Sound/ManSound/ManSwordDance");
            Skill4Sound = Resources.Load<AudioClip>("Sound/ManSound/ManGiganticSwordStart");
            mealStromFinishSound = Resources.Load<AudioClip>("Sound/ManSound/ManSwordDanceFinish");
            giganticSwordFinishSound = Resources.Load<AudioClip>("Sound/ManSound/ManGiganticSwordFinish");
            swordFinishSound = Resources.Load<AudioClip>("Sound/ManSound/ManSwordDanceFinish");
        }
        else
        {
            Skill1Sound = Resources.Load<AudioClip>("Sound/WoManSound/WomanMealStrom");
            Skill2Sound = Resources.Load<AudioClip>("Sound/WoManSound/WomanCutOff");
            Skill3Sound = Resources.Load<AudioClip>("Sound/WoManSound/WomanSwordDance");
            Skill4Sound = Resources.Load<AudioClip>("Sound/WoManSound/WomanGiganticSwordStart");
            mealStromFinishSound = Resources.Load<AudioClip>("Sound/WoManSound/WomanMealStromFinishSound");
            giganticSwordFinishSound = Resources.Load<AudioClip>("Sound/WoManSound/WoManGiganticSwordFinish");

            swordFinishSound = Resources.Load<AudioClip>("Sound/WoManSound/WomanSwordDanceFinish");
        }

    }


    public void AttackSound1()
    {
        CharAudio.PlayOneShot(attack1);
        weapon.AttackEffectSound1();
    }
    public void AttackSound2()
    {
        CharAudio.PlayOneShot(attack2);
        weapon.AttackEffectSound2();
    }
    public void AttackSound3()
    {
        CharAudio.PlayOneShot(attack3);
        weapon.AttackEffectSound3();
    }
    public void SwordDanceSound()
    {
        CharAudio.PlayOneShot(Skill3Sound);
        weapon.SwordDanceEffectSound();
    }
    public void MealStromSound()
    {
        CharAudio.PlayOneShot(Skill1Sound);
        weapon.MealstromEffectSound();
    }

    public void MealStromFinishSound()
    {
        CharAudio.PlayOneShot(mealStromFinishSound);
    }
    public void CutOffSound()
    {
        CharAudio.PlayOneShot(Skill2Sound);
        weapon.CutOffEffectSound();
    }
    public void SwordDanceFinishSound()
    {
        CharAudio.PlayOneShot(swordFinishSound);
        weapon.SwordDanceFinishEffectSound();
    }
    public void GiganticSwordSoundStart()
    {
        CharAudio.PlayOneShot(Skill4Sound);
        weapon.GiganticSwordSound();
    }
    public void GiganticSwordSoundFinish()
    {
        CharAudio.PlayOneShot(giganticSwordFinishSound);
    }
}
