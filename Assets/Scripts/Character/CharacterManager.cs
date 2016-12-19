using System.Collections;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public enum CharacterState
    {
        Idle = 0,
        Run,
        Attack,
        Jump,

        Skill1,
        Skill2,
        Skill3,
        Skill4,
        HitDamage,
        Death
    }

    public Animator animator;
    public Renderer rend;
    public AnimatorStateInfo runState;
    public Rigidbody rigdbody;
    public BoxCollider charWeapon;
    public CharWeapon weapon;
    protected CharacterStatus characterStatus;

    public GameObject[] enermy;

    public float jumpPower;

    public bool charDir;
    public bool JumpMove;
    public bool normalAttackState = false;
    public bool skillAttackState = false;
    public bool charAlive = true;
    public int charBasicDamage;

    private int potionCount = 3;
    private int comboCount;
    public AudioSource CharAudio;

    public AudioClip attack1;
    public AudioClip attack2;
    public AudioClip attack3;
    public AudioClip Skill1Sound;
    public AudioClip Skill2Sound;
    public AudioClip Skill3Sound;
    public AudioClip Skill4Sound;
    public AudioClip dieSound;
    public AudioClip hitSound;
    public AudioClip MoveSound;

    bool runSoundbool;
    protected float skillTime;

    [SerializeField]
    int unitIndex;

    [SerializeField]  CharacterState state;

    public CharacterState State { get { return state; } }
    public CharacterStatus CharacterStatus { get { return characterStatus; } }
    public bool NormalAttackState { get { return normalAttackState; } }
    public bool SkillAttackState { get { return skillAttackState; } }
    public int UnitIndex { get { return unitIndex; } }
    public int ComboCount { get { return comboCount; } set { comboCount = value; } }

    void Update()
    {
        if (charAlive)
        {
            // check skill state
            if (skillAttackState)
            {
                // skill index
                // skill process ProcessSkill1()

                switch (state)
                {
                    case CharacterState.Skill1:
                        ProcessSkill1();
                        break;

                    case CharacterState.Skill2:
                        ProcessSkill2();
                        break;

                    case CharacterState.Skill3:
                        ProcessSkill3();
                        break;

                    case CharacterState.Skill4:
                        ProcessSkill4();
                        break;
                }

            }



            if (normalAttackState || skillAttackState)
            {
                charWeapon.enabled = true;

            }
            else
            {
                charWeapon.enabled = false;
            }
        }
    }

    public void InitializeCharacter()
    {
        animator = GetComponent<Animator>();
        rigdbody = GetComponent<Rigidbody>();
        CharAudio = GetComponent<AudioSource>();
        enermy = DungeonManager.Instance.Monsters;
        weapon = GetComponentInChildren<CharWeapon>();
        charWeapon = weapon.GetComponent<BoxCollider>();

        charDir = true;
        JumpMove = false;

        CharAudio.volume = 0.3f;
        jumpPower = 10;
        comboCount = 0;
        state = CharacterState.Idle;

        SetClassSound();
        SetClassEffect();
        weapon.InitializeCharacterWeapon(this);
    }

    public void SetCharacterStatus(CharacterStatus newStatus)
    {
        characterStatus = newStatus;
    }

    public void SetUnitIndex(int index)
    {
        unitIndex = index;
    }

    public void AnimationEnd()
    {
        CharState((int)CharacterState.Idle);
        JumpMove = false;
        normalAttackState = false;
        skillAttackState = false;
        //runSoundbool = false;
    }

    //char state Method
    public void Move(float ver, float hor)
    {
        if (!animator)
        {
            animator = GetComponent<Animator>();
        }

        if (state == CharacterState.Idle || state == CharacterState.Run)
        {
            runState = this.animator.GetCurrentAnimatorStateInfo(0);

            if (!animator.GetBool("Attack"))
            {
                if (ver != 0 || hor != 0)
                {
                    runSoundbool = true;
                    animator.SetFloat("Ver", ver);
                    animator.SetFloat("Hor", hor);

                    if (ver < 0)
                    {
                        transform.rotation = Quaternion.Euler(new Vector3(0, 180.0f, 0));
                        charDir = false;
                    }
                    else if (ver > 0)
                    {
                        transform.rotation = Quaternion.Euler(new Vector3(0, 0.0f, 0));
                        charDir = true;
                    }

                    if (CharacterState.Run != state)
                    {
                        CharState((int)CharacterState.Run);
                    }

                    if (runState.IsName("Run"))
                    {
                        if (hor == -1.0f || hor == 1.0f)
                        {
                            transform.Translate((Vector3.forward * ver - Vector3.right * hor) * Time.deltaTime * (characterStatus.MoveSpeed - 3.0f), Space.World);
                        }
                        else
                        {
                            transform.Translate((Vector3.forward * ver - Vector3.right * hor) * Time.deltaTime * (characterStatus.MoveSpeed), Space.World);
                        }

                    }
                }
                else if (ver == 0 && hor == 0)
                {
                    if (animator.GetBool("Run"))
                    {
                        animator.SetBool("Run", false);
                        CharState((int)CharacterState.Idle);
                    }
                }
            }
        }
        else if (state == CharacterState.Jump && JumpMove)
        {
            transform.Translate((Vector3.forward * ver - Vector3.right * hor) * Time.deltaTime * characterStatus.MoveSpeed, Space.World);
        }

    }
    public virtual void NormalAttack()
    {
        //if ( state != characterState.Skill2 && state != CharacterState.Attack  && state != characterState.Skill1 && state != characterState.Skill4 && state != CharacterState.HitDamage && state != CharacterState.Death)
        if (state != CharacterState.Skill2 && state != CharacterState.Skill1 && state != CharacterState.Skill4 && state != CharacterState.HitDamage && state != CharacterState.Death)
        {
            normalAttackState = true;
            CharState((int)CharacterState.Attack);

        }
    }

    public virtual void Skill1()
    {
        if (state == CharacterState.Idle)
        {
            if (characterStatus.MagicPoint > SkillManager.Instance.SkillData.GetSkill ((int)characterStatus.HClass, 1).ManaCost && characterStatus.ActiveSkillUse [0])
        {
            UsingMagicPoint (1);
            StartCoroutine (characterStatus.SkillCoolTimer (0, SkillManager.Instance.SkillData.GetSkill ((int)characterStatus.HClass, 1).SkillCoolTime));
            StartCoroutine (UIManager.Instance.BattleUIManager.SetSkillCoolTimeUI (0, SkillManager.Instance.SkillData.GetSkill((int)characterStatus.HClass, 1).SkillCoolTime));
           
                CharState((int)CharacterState.Skill1);
            }
        }
    }


    public void skill2()
    {
        if (state == CharacterState.Idle)
        {
            if (characterStatus.MagicPoint > SkillManager.Instance.SkillData.GetSkill((int)characterStatus.HClass, 2).ManaCost && characterStatus.ActiveSkillUse[1])
             {
            UsingMagicPoint(1);
            StartCoroutine(characterStatus.SkillCoolTimer(1, SkillManager.Instance.SkillData.GetSkill((int)characterStatus.HClass, 2).SkillCoolTime));
            StartCoroutine(UIManager.Instance.BattleUIManager.SetSkillCoolTimeUI(1, SkillManager.Instance.SkillData.GetSkill((int)characterStatus.HClass, 2).SkillCoolTime));
            
                CharState((int)CharacterState.Skill2);
            }
        }
    }

    public void skill3()
    {
        if (state == CharacterState.Idle)
        {
            if (characterStatus.MagicPoint > SkillManager.Instance.SkillData.GetSkill ((int)characterStatus.HClass, 3).ManaCost && characterStatus.ActiveSkillUse [2])
        {
            UsingMagicPoint (3);
            StartCoroutine (characterStatus.SkillCoolTimer (2, SkillManager.Instance.SkillData.GetSkill ((int)characterStatus.HClass, 3).SkillCoolTime));
            StartCoroutine(UIManager.Instance.BattleUIManager.SetSkillCoolTimeUI(2, SkillManager.Instance.SkillData.GetSkill((int)characterStatus.HClass, 3).SkillCoolTime));
           
                CharState((int)CharacterState.Skill3);
            }
        }
    }

    public void Skill4()
    {
        if (state == CharacterState.Idle)
        {
            if (characterStatus.MagicPoint > SkillManager.Instance.SkillData.GetSkill ((int)characterStatus.HClass, 4).ManaCost && characterStatus.ActiveSkillUse [3])
        {
                UsingMagicPoint (4);
                StartCoroutine (characterStatus.SkillCoolTimer (3, SkillManager.Instance.SkillData.GetSkill ((int)characterStatus.HClass, 4).SkillCoolTime));
                StartCoroutine(UIManager.Instance.BattleUIManager.SetSkillCoolTimeUI(3, SkillManager.Instance.SkillData.GetSkill((int)characterStatus.HClass, 4).SkillCoolTime));

                //giganticSwordCastSword.SetActive(true);
                CharState((int)CharacterState.Skill4);
            }
        }
    }

    public virtual void ProcessSkill1()
    {

    }

    public virtual void ProcessSkill2()
    {

    }

    public virtual void ProcessSkill3()
    {

    }

    public virtual void ProcessSkill4()
    {

    }

    //using Potion
    public void UsingPotion()
    {   //Potion Effect create
        if (characterStatus.ActiveSkillUse[4])
        {
            GameObject potionEffect = Instantiate(Resources.Load<GameObject>("Effect/Potion"), transform.position, Quaternion.identity) as GameObject;
            StartCoroutine(UIManager.Instance.BattleUIManager.PotionCoolTimeUI());
            StartCoroutine(characterStatus.SkillCoolTimer(4, 15));
            potionEffect.transform.parent = gameObject.transform;
            potionEffect.transform.position += Vector3.up;
            StartCoroutine(Potion());
        }
    }

    IEnumerator Potion()
    {
        for (int i = 0; i < potionCount; i++)
        {
            characterStatus.DecreaseHealthPoint((int)(characterStatus.HealthPoint * -0.3));
            yield return new WaitForSeconds(1f);
        }
    }

    //Animation Method
    void SetStateDefault()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        animator.SetBool("Idle", false);
        animator.SetBool("Run", false);
    }

    public void CharState(int Inputstate)
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if (charAlive)
        {
            SetStateDefault();
            //idle=0,run=1,attack=2
            switch (Inputstate)
            {
                case 0:
                    state = CharacterState.Idle;
                    animator.SetBool("Idle", true);
                    break;

                case 1:
                    state = CharacterState.Run;
                    animator.SetBool("Run", true);
                    break;

                case 2:
                    state = CharacterState.Attack;
                    animator.SetTrigger("Attack");

                    break;

                case 3:
                    state = CharacterState.Jump;
                    animator.SetTrigger("Jump");
                    break;

                case 4:
                    state = CharacterState.Skill1;
                    animator.SetTrigger("Skill1");
                    skillAttackState = true;

                    break;

                case 5:
                    state = CharacterState.Skill2;
                    animator.SetTrigger("Skill2");
                    skillAttackState = true;

                    //basicDamage = charstate.activeSkillSet [1].skillDamage
                    break;

                case 6:
                    state = CharacterState.Skill3;
                    animator.SetTrigger("Skill3");
                    skillAttackState = true;

                    //basicDamage = charstate.activeSkillSet [3].skillDamage;
                    break;

                case 7:
                    state = CharacterState.Skill4;
                    animator.SetTrigger("Skill4");
                    skillAttackState = true;

                    //basicDamage = charstate.activeSkillSet [2].skillDamage;
                    break;

                case 8:
                    state = CharacterState.HitDamage;
                    animator.SetTrigger("PlayerHitTrigger");
                    break;

                case 9:
                    state = CharacterState.Death;
                    animator.SetTrigger("PlayerDie");
                    break;
            }

            if (unitIndex == NetworkManager.Instance.MyIndex)
            {
                DataSender.Instance.CharacterStateSend(Inputstate, unitIndex);
            }
        }
    }

    public virtual void HitDamage(int damage)
    {
        if (charAlive)
        {           
            if (characterStatus.HealthPoint > 0)
            {
                characterStatus.DecreaseHealthPoint(damage);
                CharState((int)CharacterState.HitDamage);
                UIManager.Instance.BattleUIManager.hpBarCalculation(characterStatus.MaxHealthPoint, characterStatus.HealthPoint);
                print("HPcalcuUIIn!!");
            }
            if (characterStatus.HealthPoint <= 0)
            {
                //Death Animation
                CharState((int)CharacterState.Death);
                charAlive = false;
            }
        }
        
        Debug.Log(characterStatus.HealthPoint);
    }

    public void SetPosition(UnitPositionData newPositionData)
    {
        if (newPositionData.Dir)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0.0f, 0));
            charDir = true;
        }
        else
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 180.0f, 0));
            charDir = false;
        }
        transform.position = new Vector3(newPositionData.PosX, newPositionData.PosY, newPositionData.PosZ);
    }
    public virtual void SetClassEffect() { }

    public virtual void SetClassSound()
    {
        MoveSound = Resources.Load<AudioClip>("Sound/MoveSound");
        if (characterStatus.HGender == CharacterStatus.Gender.Male)
        {
            attack1 = Resources.Load<AudioClip>("Sound/ManSound/ManWarriorattack1");
            attack2 = Resources.Load<AudioClip>("Sound/ManSound/ManWarriorattack2");
            attack3 = Resources.Load<AudioClip>("Sound/ManSound/ManWarriorattack3");
            dieSound = Resources.Load<AudioClip>("Sound/ManSound/ManDie");
            hitSound = Resources.Load<AudioClip>("Sound/ManSound/ManHit");
        }
        else
        {
            attack1 = Resources.Load<AudioClip>("Sound/WoManSound/WoManattack1");
            attack2 = Resources.Load<AudioClip>("Sound/WoManSound/WoManattack2");
            attack3 = Resources.Load<AudioClip>("Sound/WoManSound/WoManattack3");
            dieSound = Resources.Load<AudioClip>("Sound/WoManSound/WomanDie");
            hitSound = Resources.Load<AudioClip>("Sound/WoManSound/WoManHit");
        }
    }

    public virtual void UsingMagicPoint(int SkillArray)
    {
        characterStatus.DecreaseMagicPoint(SkillManager.Instance.SkillData.GetSkill((int)characterStatus.HClass, SkillArray).ManaCost);
        UIManager.Instance.BattleUIManager.mpBarCalculation(characterStatus.MaxMagicPoint, characterStatus.MagicPoint);
    }

    public IEnumerator MoveSoundCol()
    {
        if (runSoundbool)
        {
            Debug.Log("in if");
            CharAudio.PlayOneShot(MoveSound);
            yield return new WaitForSeconds(0.4f);
            StartCoroutine(MoveSoundCol());
            runSoundbool = false;

        }
        else
        {
            yield return new WaitForSeconds(0.4f);
            StartCoroutine(MoveSoundCol());
            Debug.Log("in Else");
        }

    }

    public IEnumerator ComboCheck(int count)
    {
        float checkTime = Time.time;
        while (Time.time - checkTime < 1.5f)
        {
            if (comboCount != count)
            {
                break;
            }
            yield return null;
        }
        if (comboCount == count)
        {
            comboCount = 0;
            //   ComboSystem.instance.ComboEnd();
        }
        checkTime = 0;
    }
    public void HitDamageSound()
    {
        CharAudio.PlayOneShot(hitSound);
    }
    public void DieSound()
    {
        CharAudio.PlayOneShot(dieSound);
    }
}