//using UnityEngine;
//using System.Collections;
//using UnityEngine.UI;


//public class TestMonster : Monster
//{
//    public enum BigBearBossPatternName
//    {
//        BigBearBossIdle = 0,
//        BigBearBossRun,
//        BigBearBossAttack,
//        BigBearBossOneHandAttack,
//        BigBearJumpAttack,
//        BigBearBossRoar,
//        BigBearBossDeath
//    };

//    //	public float searchRange;
//    //	public float moveSpeed;
//    public AudioClip bossStartSound;
//    private AudioSource bossAudio;
//    //public float searchRange;
//    float AttackTime;
//    //	int shootNumber;
//    public const int bossPatternCount = 3;

//    public BoxCollider[] MonsterWeapon;
//    public BigBearBossPatternName BigBearBossState;
//    AnimatorStateInfo stateInfo;
//    bool monsterAttack;
//    [SerializeField]
//    Image skillInsertImage;
//    public float imageSpeed = 1.0f;
//    public float imageLerpTime;

//    public GameObject handPos;
//    public GameObject handSphere;
//    public OneHandAttack handAttack;

//    public GameObject[] bossplayer;
//    public GameObject roar;
    
//    public bool notMove;

//    public int ChaseCount = 0;
//    public bool normalAttackCycle;

//    public SoundManager sound;

//    int pattern;

//    void Start()
//    {
//        //shootNumber = 6;
//        RunRange = 30;
//        attackRange = 3;
//        //		UIManager.Instance = GameObject.FindWithTag ("UIManager").GetComponent<UIManager> ();


//        this.bossAudio = this.gameObject.GetComponent<AudioSource>();
//        bossStartSound = Resources.Load<AudioClip>("Sound/BossStart");
//        bossplayer = GameObject.FindGameObjectsWithTag("Player");
//        chasePlayer = null;
//        animator = GetComponent<Animator>();
//        BoxCollider[] MonsterWeapon = new BoxCollider[2];
//        StartCoroutine(SetTargetPlayer());
//        //StartCoroutine (BossAI());
//        StartCoroutine(CoChasePlayer());

//    }

//    void Update()
//    {

//        if (IsAlive)
//        {

//            if (!bossSkill)
//            {
//                stateInfo = this.animator.GetCurrentAnimatorStateInfo(0);
//                searchRange = Vector3.Distance(chasePlayer.transform.position, transform.position);


//                if (searchRange < attackRange && normalAttackCycle)
//                {
//                    BigBearBossPattern((int)BigBearBossPatternName.BigBearBossAttack);
//                    normalAttackCycle = false;
//                }

//                if (searchRange > RunRange)
//                {
//                    BigBearBossPattern((int)BigBearBossPatternName.BigBearBossIdle);
//                    if (stateInfo.IsName("BigBearBossIdle"))
//                    {
//                        changeDirection();
//                    }

//                }
//                else if (searchRange <= RunRange && searchRange > attackRange && !notMove)
//                {
//                    BigBearBossPattern((int)BigBearBossPatternName.BigBearBossRun);

//                    if (stateInfo.IsName("BigBearBossRun"))
//                    {
//                        changeDirection();
//                        transform.Translate((chasePlayer.transform.position - transform.position) * moveSpeed * Time.deltaTime, 0);//반대로 걸어 가서 수정
//                    }
//                }

//                if (stateInfo.IsName("BigBearBossAttack"))
//                {
//                    for (int i = 0; i < MonsterWeapon.Length; i++)
//                    {
//                        MonsterWeapon[i].enabled = true;
//                    }
//                }
//                else if (!stateInfo.IsName("BigBearBossAttack"))
//                {
//                    for (int i = 0; i < MonsterWeapon.Length; i++)
//                    {
//                        MonsterWeapon[i].enabled = false;
//                    }
//                }
//            }
//        }
//        //		else
//        //		{
//        //			Destroy (this.gameObject, 5);
//        //		}

//    }

//    void SetStateDefault()
//    {
//        if (animator == null)
//        {
//            animator = GetComponent<Animator>();
//        }

//        animator.SetTrigger("IdleState");
//        //animator.SetBool ("Run", false);
//    }

//    public IEnumerator SetTargetPlayer()
//    {
//        while (IsAlive)
//        {
//            int chaseIndex = Random.Range(0, bossplayer.Length - 1);

//            chasePlayer = bossplayer[chaseIndex];

//            yield return new WaitForSeconds(15.0f);
//        }
//    }

//    public IEnumerator BossAI()
//    {
//        stateInfo = this.animator.GetCurrentAnimatorStateInfo(0);

//        while (IsAlive)
//        {
//            yield return new WaitForSeconds(10f);

//            pattern = Random.Range(0, bossPatternCount);
//            // pattern = 1;
//            bossSkill = true;
//            animator.SetBool("BossSkill", true);

//            if (pattern == 0)
//            {
//                BigBearBossPattern((int)BigBearBossPatternName.BigBearJumpAttack);
//            }
//            else if (pattern == 1)
//            {
//                BigBearBossPattern((int)BigBearBossPatternName.BigBearBossRoar);

//            }
//            else if (pattern == 2)
//            {
//                BigBearBossPattern((int)BigBearBossPatternName.BigBearBossOneHandAttack);
//            }


//            yield return new WaitForSeconds(1.0f);

//            bossSkill = false;
//            animator.SetBool("BossSkill", false);
//        }
//    }
//    IEnumerator CoChasePlayer()
//    {
//        while (IsAlive)
//        {
//            normalAttackCycle = true;

//            yield return new WaitForSeconds(3.0f);
//        }
//    }

//    public void AnimatorReSet()
//    {
//    }

//    public IEnumerator Shooting()
//    {
//        int shootNum = 0;

//        while (shootNum < shootNumber)
//        {
//            yield return new WaitForSeconds(0.2f);

//            int xPos = Random.Range(-2, 2);
//            int zPos = Random.Range(-2, 2);

//            Instantiate(Resources.Load<GameObject>("Effect/BossDarkSphere"), handPos.transform.position + (Vector3.right * xPos) + (Vector3.forward * zPos), Quaternion.Euler(0, 0, 0));

//            shootNum++;
//        }
//    }

//    public void RoarHit()
//    {
//        if (Direction == right)
//        {
//            Instantiate(Resources.Load<GameObject>("Effect/WarningEffect"), new Vector3(-3.55f, 0.15f, this.transform.position.z + 10f), Quaternion.Euler(-90, 0, 0));
//        }
//        else if (Direction == left)
//        {
//            Instantiate(Resources.Load<GameObject>("Effect/WarningEffect"), new Vector3(3.55f, 0.15f, this.transform.position.z - 10f), Quaternion.Euler(-90, 0, 0));
//        }

//        //GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<RedRenderImage> ().enabled = true;
//    }


//    public void changeDirection()
//    {
//        //캐릭터 이동시 보스가 보는 방향을 정한다.
//        Vector3 vecLookPos = chasePlayer.transform.position;
//        vecLookPos.y = transform.position.y;
//        vecLookPos.x = transform.position.x;

//        checkDirection = chasePlayer.transform.position - this.gameObject.transform.position;

//        if (checkDirection.z > 0)
//        {
//            LookAtPattern(right);
//        }
//        else if (checkDirection.z < 0)
//        {
//            LookAtPattern(left);
//        }

//        transform.LookAt(vecLookPos);
//    }


//    //public override void HitDamage(int _Damage, GameObject attacker)
//    //{
//    //    stateInfo = this.animator.GetCurrentAnimatorStateInfo(0);

//    //    if (IsAlive)
//    //    {
//    //        MaxHP -= _Damage;

//    //        //			UIManager.Instance.bossHp.fillAmount = maxLife / currentLife;
//    //        if (MaxHP > 0)
//    //        {
//    //            //hitanimation
//    //        }
//    //        else if (MaxHP <= 0)
//    //        {
//    //            if (!stateInfo.IsName("BigBearBossDeath"))
//    //            {
//    //                BigBearBossPattern((int)BigBearBossPatternName.BigBearBossDeath);
//    //                IsAlive = false;
//    //                return;
//    //            }
//    //        }
//    //    }
//    //}

//    public void BigBearBossPattern(int bossState)
//    {

//        switch (bossState)
//        {

//            case 0:
//                BigBearBossState = BigBearBossPatternName.BigBearBossIdle;
//                animator.SetInteger("state", 0);

//                break;
//            case 1:
//                BigBearBossState = BigBearBossPatternName.BigBearBossRun;
//                animator.SetInteger("state", 1);

//                break;
//            case 2:
//                BigBearBossState = BigBearBossPatternName.BigBearBossAttack;
//                animator.SetInteger("state", 2);
//                monsterAttack = true;
//                //			sound.BossSound (1,searchRange);
//                //this.bossAudio.PlayOneShot (normalAttack);
//                //this.bossAudio.loop = false;
//                break;

//            case 3:
//                BigBearBossState = BigBearBossPatternName.BigBearBossOneHandAttack;
//                animator.SetInteger("state", 3);

//                //this.bossAudio.PlayOneShot (oneHandAttack);
//                //this.bossAudio.loop = false;

//                break;

//            case 4:
//                BigBearBossState = BigBearBossPatternName.BigBearJumpAttack;
//                animator.SetInteger("state", 4);
//                //this.bossAudio.PlayOneShot (jumpAttack);
//                //this.bossAudio.loop = false;
//                break;

//            case 5:
//                BigBearBossState = BigBearBossPatternName.BigBearBossRoar;
//                animator.SetInteger("state", 5);
//                //this.bossAudio.PlayOneShot (bossHowling);
//                //this.bossAudio.loop = false;
//                break;

//            case 6:
//                BigBearBossState = BigBearBossPatternName.BigBearBossDeath;
//                animator.SetTrigger("BigBearBossDeath");
//                //this.bossAudio.PlayOneShot (bossDeath);
//                //this.bossAudio.loop = false;
//                break;
//        }
//    }

//}