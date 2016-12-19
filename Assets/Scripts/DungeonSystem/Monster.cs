using UnityEngine;
using System.Collections;


public enum StatePosition
{
    Idle = 0,
    Run,
    Attack,
    BossOneHandAttack,
    BossJumpAttack,
    BossRoar,
    TakeDamage,
    Death
}

public enum DefenseMoveDirectionArray
{
    Up = 1,
    Down,
    Middle,
    case4,
    Comback
}

public class Monster : MonoBehaviour
{
    public bool normalMode;
    public enum TargetPlayerPosition
    {
        Zero = 1,
        Left,
        Right,
        Up,
        Down

    }
    bool direction;
    public TargetPlayerPosition target;

    public const bool right = true;
    public const bool left = false;

    protected Animator animator;
    protected AnimatorStateInfo aniState;
    [SerializeField]
    protected BoxCollider HittedBox;
    [SerializeField]
    protected MonsterWeapon[] attackCollider;
    [SerializeField]
    protected GameObject[] bulletInstantiate;

    public GameObject[] player;
    protected GameObject[] wall;
    protected GameObject nearWall;
    [SerializeField]
    protected GameObject targetPlayer;
    [SerializeField]
    protected Vector3 movePoint;

    protected int monsterRunAttackAround;

    protected int randomStandby;
    public int RandomStandby
    {
        set { randomStandby = value; }
    }
    //mode,gateArraynumber,monsterArraynumber
    protected bool moveAble;

    protected int monsterIndex;
    protected MonsterId monsterId;
    protected string _name;
    protected int level;
    [SerializeField]
    protected int currentHP;
    [SerializeField]
    protected int maxHP;
    [SerializeField]
    protected int attack;
    protected int defense;
    [SerializeField]
    protected int moveSpeed;


    //monster getting variable;
    [SerializeField]
    protected float RunRange;
    [SerializeField]
    protected float attackRange;
    [SerializeField]
    protected float attackCycle;
    [SerializeField]
    protected float currentDisTance;
    [SerializeField]
    protected float searchRange;
    StatePosition statePosition;

    public bool isAlive;
    protected bool isAttack;
    protected bool isHited;

    //boss skill 
    protected int bossPatternCount;
    [SerializeField]
    protected bool bossNormalAttackCycle;
    protected bool bossSkill;
    [SerializeField]
    protected int bossRandomPattern;
    public int shootNumber;

    public GameObject chasePlayer;



    protected float[] playerToMonsterDamage;
    private float[] aggroRank; //playertoMonsterdamage/currentdistancePlayer;
    private float changeTargetTime = 0;

    [SerializeField]
    private float[] currentDisTanceArray;
    [SerializeField]
    private float[] currentDisTanceWall;
    [SerializeField]
    protected Vector3 checkDirection; // monster chaseplayer and move variable;
    [SerializeField]
    protected Vector3[] pointVector;
    GameObject roarField;
    public bool roarBreak;

    public AudioSource MonsterAudio;
    public AudioClip bearRoar;
    public AudioClip frogAttack;
    public AudioClip frogDeath;
    public AudioClip frogHit;
    public AudioClip middleBossAttack1;
    public AudioClip middleBossAttack2;
    public AudioClip middleBossDeath;
    public AudioClip middleBossWave;
    public AudioClip rabbitAttack;
    public AudioClip rabbitDeath;
    public AudioClip rabbitHit;
    /*
    public void SetMonsterSound()
    {
        MonsterAudio = this.GetComponent<AudioSource>();
        MonsterAudio.volume = 0.5f;
        bearRoar = Resources.Load<AudioClip>("Sound/MonsterSound/MiddleBossWave");
        frogAttack = Resources.Load<AudioClip>("Sound/MonsterSound/FrogAttack");
        frogDeath = Resources.Load<AudioClip>("Sound/MonsterSound/FrogDeath");
        frogHit = Resources.Load<AudioClip>("Sound/MonsterSound/FrogHit");
        middleBossAttack1 = Resources.Load<AudioClip>("Sound/MonsterSound/MiddleBossAttack1");
        middleBossAttack2 = Resources.Load<AudioClip>("Sound/MonsterSound/MiddleBossAttack2");
        middleBossDeath = Resources.Load<AudioClip>("Sound/MonsterSound/middleBossDeath");
        middleBossWave = Resources.Load<AudioClip>("Sound/MonsterSound/bearRoar");
        rabbitAttack = Resources.Load<AudioClip>("Sound/MonsterSound/RabbitAttack");
        rabbitDeath = Resources.Load<AudioClip>("Sound/MonsterSound/RabbitDeath");
        rabbitHit = Resources.Load<AudioClip>("Sound/MonsterSound/RabbitHit");
    }

    public void BearRoarSound()
    {
        MonsterAudio.PlayOneShot(bearRoar);
    }
    public void FrogAttackSound()
    {
        MonsterAudio.PlayOneShot(frogAttack);
    }
    public void FrogDeathSound()
    {
        MonsterAudio.PlayOneShot(frogDeath);
    }
    public void FrogHitSound()
    {
        MonsterAudio.PlayOneShot(frogHit);
    }
    public void MiddleBossAttack1Sound()
    {
        MonsterAudio.PlayOneShot(middleBossAttack1);
    }
    public void MiddleBossAttack2Sound()
    {
        MonsterAudio.PlayOneShot(middleBossAttack2);
    }
    public void MiddleBossDeathSound()
    {
        MonsterAudio.PlayOneShot(middleBossDeath);
    }
    public void MiddleWaveSound()
    {
        MonsterAudio.PlayOneShot(middleBossWave);
    }
    public void RabbitAttackSound()
    {
        MonsterAudio.PlayOneShot(rabbitAttack);
    }
    public void RabbitDeathSound()
    {
        MonsterAudio.PlayOneShot(rabbitDeath);
    }
    public void RabbitHitSound()
    {
        MonsterAudio.PlayOneShot(rabbitHit);
    }
    */

    public int MonsterIndex
    {
        get { return monsterIndex; }
        set { monsterIndex = value; }
    }

    public bool MoveAble
    {
        get { return moveAble; }
        set { moveAble = value; }
    }
    public bool IsAttack
    {
        get { return isAttack; }
        set { isAttack = value; }
    }
    public bool IsAlive
    {
        get { return isAlive; }
        set { isAlive = value; }
    }
    public bool IsHited
    {
        get { return isHited; }
        set { isHited = value; }
    }
    public int MaxHP
    {
        get { return maxHP; }
        set { maxHP = value; }
    }

    public int CurrentHP
    {
        get { return currentHP; }
        set { currentHP = value; }
    }

    public Vector3 MovePoint
    {
        get { return movePoint; }
        set { movePoint = value; }
    }

    public bool Direction
    {
        get { return direction; }
        set { direction = value; }
    }

    public StatePosition _StatePosition { get { return statePosition; } }

    public int Attack { get { return attack; } }
    public MonsterId MonsterId { get { return monsterId; } set { monsterId = value; } }

    public void MonsterSet(MonsterBaseData monster)
    {
        player = GameObject.FindGameObjectsWithTag("Player");
        wall = GameObject.FindGameObjectsWithTag("Wall");
        currentDisTanceWall = new float[wall.Length];
        //SetMonsterSound();
        animator = this.gameObject.GetComponent<Animator>();
        HittedBox = this.gameObject.GetComponent<BoxCollider>();

        isAlive = true;
        isHited = false;
        moveAble = true;
        randomStandby = 0;
        roarBreak = false;

        _name = monster.Name;
        level = monster.MonsterLevelData[0].Level;
        attack = monster.MonsterLevelData[0].Attack;
        defense = monster.MonsterLevelData[0].Defense;
        currentHP = monster.MonsterLevelData[0].HealthPoint;
        maxHP = monster.MonsterLevelData[0].HealthPoint;
        moveSpeed = monster.MonsterLevelData[0].MoveSpeed;

        //      

        if (monsterId == MonsterId.Rabbit)
        {
            searchRange = 12;
            attackRange = 4;
            attackCollider = this.transform.GetComponentsInChildren<MonsterWeapon>();
            attackCollider[0].MonsterWeaponSet();
        }


        if (monsterId == MonsterId.Duck || monsterId == MonsterId.Frog)
        {
            if (monsterId == MonsterId.Duck)
            {
                bulletInstantiate = new GameObject[1];
                bulletInstantiate[0] = Resources.Load<GameObject>("Effect/Monster_ShockWave");
            }
            if (monsterId == MonsterId.Frog)
            {
                bulletInstantiate = new GameObject[1];
                bulletInstantiate[0] = Resources.Load<GameObject>("Monster/FrogBullet");
            }
            searchRange = 15;
            attackRange = 9;
        }

        if (monsterId == MonsterId.Bear || monsterId == MonsterId.BlackBear)
        {
            if (monsterId == MonsterId.BlackBear)
            {
                bulletInstantiate = new GameObject[1];
                bulletInstantiate[0] = Resources.Load<GameObject>("Effect/BossDarkSphere");
            }
            attackCollider = this.transform.GetComponentsInChildren<MonsterWeapon>();
            for (int i = 0; i < attackCollider.Length; i++)
            {
                attackCollider[i].MonsterWeaponSet();
            }
            searchRange = 12;
            attackRange = 3;
            RunRange = 30;
            shootNumber = 6;

        }

        currentDisTanceArray = new float[player.Length];
        aggroRank = new float[player.Length];
        playerToMonsterDamage = new float[player.Length];

        if (NetworkManager.Instance.MyIndex == 0)
        {
            StartCoroutine(MonsterUpdate());
            StartCoroutine(ChangeRandomStanby());

            normalMode = DungeonManager.Instance.NormalMode;
            normalMode = true;
            MonsterAIStart(normalMode);

            DataSender.Instance.UnitPositionSend(gameObject);
        }
    }

    IEnumerator BossNormalAttackCycleSet()
    {
        while (IsAlive)
        {
            bossNormalAttackCycle = true;

            yield return new WaitForSeconds(3.0f);
        }
    }

    public void MonsterAIStart(bool _normalMode)
    {//moveAI->StartAI , 
        if (!_normalMode)
        {

            if (monsterIndex > 18)
            {
                DefenseMoveSet(DefenseMoveDirectionArray.Up);
            }
            else if (monsterIndex > 9)
            {
                DefenseMoveSet(DefenseMoveDirectionArray.Middle);
            }
            else if (monsterIndex >= 0)
            {
                DefenseMoveSet(DefenseMoveDirectionArray.Down);
            }
            StartCoroutine(MonsterDefenceWaveControll(_normalMode));
        }

        if (_normalMode)
        {
            if (monsterId == MonsterId.Rabbit)
            {
                StartCoroutine(MonsterMoveAI(_normalMode));
                StartCoroutine(MonsterActAI(_normalMode));
            }

            if (monsterId == MonsterId.Duck || monsterId == MonsterId.Frog)
            {
                StartCoroutine(MonsterMoveAI(_normalMode));
                StartCoroutine(MonsterActAIADC(_normalMode));
            }

            if (monsterId == MonsterId.Bear || monsterId == MonsterId.BlackBear)
            {
                bossSkill = false;
                StartCoroutine(BossActAI());
                StartCoroutine(BossNormalAttackCycleSet());
                StartCoroutine(SetTargetPlayer());


                if (monsterId == MonsterId.BlackBear)
                {
                    StartCoroutine(BossSkillAI());
                }
                if (monsterId == MonsterId.Bear)
                {

                    StartCoroutine(MiddleBossSkillAI());
                }

            }
        }
    }

    public IEnumerator MonsterDefenceWaveControll(bool _normalMode)
    {
        if (monsterIndex >= 9)
        {
            this.gameObject.transform.position = new Vector3(this.transform.position.x, 100, this.transform.position.z);
        }
        if (monsterIndex < 9)
        {
            if (monsterId == MonsterId.Rabbit)
            {
                StartCoroutine(MonsterMoveAI(_normalMode));
                StartCoroutine(MonsterActAI(_normalMode));
            }

            if (monsterId == MonsterId.Duck || monsterId == MonsterId.Frog)
            {
                StartCoroutine(MonsterMoveAI(_normalMode));
                StartCoroutine(MonsterActAIADC(_normalMode));
            }
        }
        yield return new WaitForSeconds(10);
        if (monsterIndex < 18 && monsterIndex >= 9)
        {
            this.gameObject.transform.position = new Vector3(this.transform.position.x, 0, this.transform.position.z);
        }
        if (monsterIndex < 18 && monsterIndex >= 9)
        {
            if (monsterId == MonsterId.Rabbit)
            {
                StartCoroutine(MonsterMoveAI(_normalMode));
                StartCoroutine(MonsterActAI(_normalMode));
            }

            if (monsterId == MonsterId.Duck || monsterId == MonsterId.Frog)
            {
                StartCoroutine(MonsterMoveAI(_normalMode));
                StartCoroutine(MonsterActAIADC(_normalMode));
            }
        }
        yield return new WaitForSeconds(10);
        if (monsterIndex < 27 && monsterIndex >= 18)
        {
            this.gameObject.transform.position = new Vector3(this.transform.position.x, 0, this.transform.position.z);
        }
        if (monsterIndex < 27 && monsterIndex >= 18)
        {
            if (monsterId == MonsterId.Rabbit)
            {
                StartCoroutine(MonsterMoveAI(_normalMode));
                StartCoroutine(MonsterActAI(_normalMode));
            }

            if (monsterId == MonsterId.Duck || monsterId == MonsterId.Frog)
            {
                StartCoroutine(MonsterMoveAI(_normalMode));
                StartCoroutine(MonsterActAIADC(_normalMode));
            }
        }
    }


    public IEnumerator MonsterUpdate()
    {
        while (isAlive)
        {

            ChasePlayer();
            aniState = this.animator.GetCurrentAnimatorStateInfo(0);
            if (aniState.IsName("Idle") || aniState.IsName("Run"))
            {
                LookAtDirection();
            }

            if (aniState.IsName("Run"))
            {
                if (monsterId != MonsterId.Bear && monsterId != MonsterId.BlackBear)
                {
                    if (normalMode)
                    {
                        if (Vector3.Distance(transform.position, targetPlayer.transform.position) > 1f)
                        {
                            if (movePoint.normalized.z < 0)
                                this.transform.Translate(-movePoint.normalized * moveSpeed * Time.deltaTime);
                            else
                                this.transform.Translate(movePoint.normalized * moveSpeed * Time.deltaTime);
                        }
                    }
                    if (!normalMode)
                    {
                        this.transform.Translate(movePoint.normalized * moveSpeed * Time.deltaTime);
                    }

                }
                else
                {
                    this.transform.Translate((targetPlayer.transform.position - this.transform.position).normalized * moveSpeed * Time.deltaTime, 0);
                }
            }

            yield return null;
        }
    }


    IEnumerator MonsterMoveAI(bool _normalMode)
    {
        while (IsAlive)
        {
            yield return null;
            if (_normalMode)
            {
                if (targetPlayer != null)
                {
                    if (currentDisTance < searchRange)
                    {
                        if (randomStandby != 0)
                        {
                            SetTargetPlayerPosition();
                        }
                    }
                }
            }
            while (!_normalMode)
            {
                yield return null;
                if (isAlive)
                {
                    if (!isHited)
                    {
                        for (int i = 0; i < pointVector.Length; i++)
                        {
                            if (i > 0 && i < pointVector.Length - 1)
                            {
                                movePoint = pointVector[i];
                                pointVector[i] = pointVector[i + 1];
                                pointVector[i + 1] = movePoint;
                            }

                            if (i == pointVector.Length - 1)
                            {
                                movePoint = pointVector[i];
                                pointVector[i] = pointVector[0];
                                pointVector[0] = movePoint;
                            }
                        }
                    }
                    if (isHited)
                    {
                        if (targetPlayer.transform.position.z > transform.position.z)
                        {
                            movePoint = (targetPlayer.transform.position - transform.position);
                        }
                        else
                            isHited = false;
                    }
                    yield return new WaitForSeconds(2f);
                }
            }
        }
    }



    public void SetTargetPlayerPosition()
    {
        switch (randomStandby)
        {
            case 1:

                targetPlayerPosition(TargetPlayerPosition.Zero);
                break;

            case 2:

                targetPlayerPosition(TargetPlayerPosition.Right);
                break;

            case 3:

                targetPlayerPosition(TargetPlayerPosition.Left);
                break;

            case 4:

                targetPlayerPosition(TargetPlayerPosition.Up);
                break;

            case 5:

                targetPlayerPosition(TargetPlayerPosition.Down);
                break;
        }
    }

    public IEnumerator ChangeRandomStanby()
    {
        while (isAlive)
        {
            randomStandby = Random.Range(2, 6);
            yield return new WaitForSeconds(2);
            randomStandby = 1;
            yield return new WaitForSeconds(2);

        }
    }

    public void targetPlayerPosition(TargetPlayerPosition targetposition)
    {
        switch (targetposition)
        {
            case TargetPlayerPosition.Zero:
                movePoint = new Vector3(targetPlayer.transform.position.x - transform.position.x, 0, targetPlayer.transform.position.z - transform.position.z);
                break;
            case TargetPlayerPosition.Left:
                movePoint = new Vector3(checkDirection.x, 0, targetPlayer.transform.position.z - transform.position.z - (3));
                break;
            case TargetPlayerPosition.Right:
                movePoint = new Vector3(checkDirection.x, 0, targetPlayer.transform.position.z - transform.position.z + (3));
                break;
            case TargetPlayerPosition.Up:
                movePoint = new Vector3(targetPlayer.transform.position.x - transform.position.x + (1.5f), 0, checkDirection.z);
                break;
            case TargetPlayerPosition.Down:
                movePoint = new Vector3(targetPlayer.transform.position.x - transform.position.x - (1.5f), 0, checkDirection.z);
                break;
        }
    }

    IEnumerator MonsterActAI(bool _normal)
    {
        while (_normal)
        {
            if (isAlive)
            {
                yield return new WaitForSeconds(0.2f);
                if (targetPlayer != null)
                {
                    currentDisTance = Vector3.Distance(targetPlayer.transform.position, this.gameObject.transform.position);
                    checkDirection = targetPlayer.transform.position - this.gameObject.transform.position;
                    if (currentDisTance > searchRange)
                    {
                        statePosition = StatePosition.Idle;
                        Pattern(statePosition);
                    }
                    else if (currentDisTance <= searchRange)
                    {
                        {
                            if (currentDisTance > attackRange)
                            {
                                moveAble = true;
                                isAttack = false;
                                statePosition = StatePosition.Run;
                                Pattern(statePosition);
                            }
                            if (currentDisTance <= attackRange)
                            {
                                if (!isAttack)
                                {
                                    isAttack = true;
                                    moveAble = false;
                                }
                                statePosition = StatePosition.Attack;
                                Pattern(statePosition);
                                yield return new WaitForSeconds(0.5f);
                            }
                        }
                    }
                }

            }

            while (!_normal)
            {
                if (isAlive)
                {
                    if (targetPlayer != null)
                    {
                        currentDisTance = Vector3.Distance(targetPlayer.transform.position, this.gameObject.transform.position);
                        checkDirection = targetPlayer.transform.position - this.gameObject.transform.position;
                    }
                    attackCycle += 0.2f;
                    //int 
                    int randomvariable = Random.Range(1, 4);
                    if (attackCycle > randomvariable)
                    {
                        statePosition = StatePosition.Idle;
                        Pattern(statePosition);
                        yield return new WaitForSeconds(0.5f);

                        moveAble = false;
                        isAttack = true;
                        statePosition = StatePosition.Attack;
                        Pattern(statePosition);
                        yield return new WaitForSeconds(1.0f);
                        attackCycle = 0;

                    }
                    if (attackCycle <= randomvariable)
                    {
                        moveAble = true;
                        isAttack = false;
                        statePosition = StatePosition.Run;
                        Pattern(statePosition);
                    }
                    yield return new WaitForSeconds(0.2f);
                }
            }
        }
    }



    IEnumerator MonsterActAIADC(bool _normalMode)
    {
        while (_normalMode)
        {
            if (isAlive)
            {
                if (targetPlayer != null)
                {
                    currentDisTance = Vector3.Distance(targetPlayer.transform.position, this.gameObject.transform.position);
                    checkDirection = targetPlayer.transform.position - this.gameObject.transform.position;

                    if (Mathf.Abs(targetPlayer.transform.position.z - transform.position.z) < attackRange && Mathf.Abs(targetPlayer.transform.position.x - this.gameObject.transform.position.x) <= 1.5f)
                    {
                        if (!isAttack)
                        {
                            isAttack = true;
                            moveAble = false;
                        }
                        statePosition = StatePosition.Attack;
                        Pattern(statePosition);
                        yield return new WaitForSeconds(0.5f);
                    }

                    else if (currentDisTance > searchRange)
                    {
                        moveAble = false;
                        isAttack = false;
                        statePosition = StatePosition.Idle;
                        Pattern(statePosition);

                    }
                    else
                    {
                        moveAble = true;
                        isAttack = false;
                        statePosition = StatePosition.Run;
                        Pattern(statePosition);
                    }

                }
                yield return new WaitForSeconds(0.2f);
            }
            else
                yield return new WaitForSeconds(0.2f);
        }
        while (!_normalMode)
        {
            if (targetPlayer != null)
            {
                currentDisTance = Vector3.Distance(targetPlayer.transform.position, this.gameObject.transform.position);
                checkDirection = targetPlayer.transform.position - this.gameObject.transform.position;
            }
            attackCycle += 0.2f;

            int randomvariable = Random.Range(1, 4);
            if (attackCycle > 1 + randomvariable)
            {
                statePosition = StatePosition.Idle;
                Pattern(statePosition);
                yield return new WaitForSeconds(1.3f);
                moveAble = false;
                isAttack = true;
                statePosition = StatePosition.Attack;
                Pattern(statePosition);
                yield return new WaitForSeconds(2f);
                attackCycle = 0;

            }
            if (attackCycle <= 1 + randomvariable)
            {
                LookAtPattern(right);
                moveAble = true;
                isAttack = false;
                statePosition = StatePosition.Run;
                Pattern(statePosition);
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator BossActAI()
    {
        while (isAlive)
        {
            if (!bossSkill)
            {
                aniState = this.animator.GetCurrentAnimatorStateInfo(0);
                if (targetPlayer != null)
                {
                    currentDisTance = Vector3.Distance(targetPlayer.transform.position, transform.position);
                    checkDirection = targetPlayer.transform.position - transform.position;
                    if (!aniState.IsName("Attack"))
                    {
                        currentDisTance = Vector3.Distance(targetPlayer.transform.position, transform.position);
                        checkDirection = targetPlayer.transform.position - transform.position;
                    }

                    if (currentDisTance < attackRange && bossNormalAttackCycle)
                    {
                        statePosition = StatePosition.Attack;
                        bossNormalAttackCycle = false;
                        Pattern(statePosition);
                        yield return new WaitForSeconds(1);

                    }
                    if (currentDisTance > searchRange)
                    {
                        statePosition = StatePosition.Idle;
                        Pattern(statePosition);
                        if (aniState.IsName("Idle"))
                        {
                            BosschangeDirection();
                        }
                    }
                    else if (currentDisTance <= searchRange && currentDisTance > attackRange && moveAble)
                    {
                        statePosition = StatePosition.Run;
                        Pattern(statePosition);
                        movePoint = checkDirection;
                        if (aniState.IsName("Run"))
                        {
                            BosschangeDirection();
                            transform.Translate((targetPlayer.transform.position - transform.position) * moveSpeed * Time.deltaTime, 0);//반대로 걸어 가서 수정

                        }

                    }

                    if (aniState.IsName("Attack"))
                    {
                        for (int i = 0; i < attackCollider.Length; i++)
                        {
                            attackCollider[i].enabled = true;
                        }

                    }
                    else if (aniState.IsName("Attack"))
                    {
                        for (int i = 0; i < attackCollider.Length; i++)
                        {
                            attackCollider[i].enabled = false;
                        }
                    }
                }

            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator MiddleBossSkillAI()
    {
        roarBreak = true;
        aniState = this.animator.GetCurrentAnimatorStateInfo(0);

        while (IsAlive)
        {

            yield return new WaitForSeconds(10f);
            bossRandomPattern = Random.Range(0, 1);      // pattern = 1;
            bossSkill = true;
            animator.SetBool("BossSkill", true);

            checkDirection = targetPlayer.transform.position - transform.position;
            if (checkDirection.z > 0)
            {
                movePoint = new Vector3(checkDirection.x, 0, checkDirection.z - 3f);
            }
            if (checkDirection.z < 0)
            {
                movePoint = new Vector3(checkDirection.x, 0, checkDirection.z + 3f);
            }
            bossRandomPattern = 0;
            statePosition = StatePosition.BossOneHandAttack;
            Pattern(statePosition);


            yield return new WaitForSeconds(1.0f);

            bossSkill = false;
            animator.SetBool("BossSkill", false);
        }
    }


    IEnumerator BossSkillAI()
    {
        aniState = this.animator.GetCurrentAnimatorStateInfo(0);

        bossRandomPattern = 0;
        while (IsAlive)
        {
            yield return new WaitForSeconds(10f);
            bossSkill = true;
            animator.SetBool("BossSkill", true);

            if (bossRandomPattern == 0)
            {
                statePosition = StatePosition.BossJumpAttack;
                Pattern(statePosition);

            }
            else if (bossRandomPattern == 1)
            {
                if (!roarBreak)
                {
                    roarBreak = true;
                    statePosition = StatePosition.BossOneHandAttack;
                    Pattern(statePosition);
                }
            }
            else if (bossRandomPattern == 2)
            {
                    statePosition = StatePosition.BossRoar;
                    Pattern(statePosition);
            }

            yield return new WaitForSeconds(1.0f);

            bossRandomPattern++;
            if (bossRandomPattern > 1)
            {
                bossRandomPattern = 0;
            }
                
            bossSkill = false;
            animator.SetBool("BossSkill", false);
        }
    }


    public void Pattern(StatePosition state)
    {
        switch (state)
        {
            case StatePosition.Idle:
                {
                    if (!aniState.IsName("Idle"))
                    {
                        animator.SetInteger("State", 0);
                    }
                    break;
                }

            case StatePosition.Attack:
                {
                    AttackProcess(isAttack);
                    break;
                }

            case StatePosition.Run:
                {
                    animator.SetInteger("State", 1);
                    break;
                }

            case StatePosition.TakeDamage:
                {
                    animator.SetTrigger("TakeDamage");
                    break;
                }

            case StatePosition.Death:
                {
                    animator.SetTrigger("Death");

                    //MonsterArrayEraser(this.gameObject);
                    break;
                }

            case StatePosition.BossOneHandAttack:
                {
                    animator.SetInteger("State", 3);
                    break;
                }

            case StatePosition.BossJumpAttack:
                {
                    animator.SetInteger("State", 4);
                    break;
                }
            case StatePosition.BossRoar:
                {
                    animator.SetInteger("State", 5);
                    break;
                }
        }

        if (NetworkManager.Instance.MyIndex == 0)
        {
            DataSender.Instance.MonsterStateSend((byte)state, monsterIndex);
        }
    }

    public void AttackProcess(bool isAttack)
    {
        if (monsterId == MonsterId.Bear || monsterId == MonsterId.BlackBear)
        {
            animator.SetInteger("State", 2);
        }
        else
           if (isAttack)
        {

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Run"))
            {
                animator.SetInteger("State", 0);
            }
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Idle"))
            {
                animator.SetInteger("State", 2);
            }
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Attack"))
            {
                moveAble = false;
            }

        }
    }

    public void LookAtPattern(bool dir)
    {
        switch (dir)
        {
            case true:
                {
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    break;
                }
            case false:
                {
                    transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                    break;
                }
        }
    }

    public void LookAtDirection()
    {
        if (movePoint.z > 0)
        {
            LookAtPattern(right);
        }
        if (movePoint.z < 0)
        {
            LookAtPattern(left);
        }
    }

    public void ChasePlayer()
    {
        if (player[0] != null)
        {
            if (!isHited)
            {
                changeTargetTime += 0.2f;
                if (changeTargetTime >= 3)
                {
                    changeTargetTime = 0;
                    NormalchasePlayer();
                }
            }
            if (isHited)
            {
                changeTargetTime += 0.2f;
                if (changeTargetTime >= 2)
                {
                    changeTargetTime = 0;
                    HitedchasePlayer();
                }
            }
        }
    }
    public void NormalchasePlayer()
    {
        for (int i = 0; i < player.Length; i++)
        {
            currentDisTanceArray[i] = Vector3.Distance(player[i].transform.position, transform.position);
        }
        for (int j = 0; j < player.Length; j++)
        {
            if (currentDisTanceArray[j] <= Mathf.Min(currentDisTanceArray))
            {
                targetPlayer = player[j];
            }
        }

    }
    public void HitedchasePlayer()
    {
        for (int i = 0; i < player.Length; i++)
        {
            currentDisTanceArray[i] = Vector3.Distance(player[i].transform.position, transform.position);
            if (currentDisTanceArray[i] < 2f)
            {
                currentDisTanceArray[i] = 2f;
            }
            aggroRank[i] = playerToMonsterDamage[i] * (1 / (currentDisTanceArray[i] * 0.5f));
        }

        for (int j = 0; j < player.Length; j++)
        {
            if (aggroRank[j] <= Mathf.Max(aggroRank))
            {
                targetPlayer = player[j];
            }
        }
    }

    public void DefenseMoveSet(DefenseMoveDirectionArray defenseMoveDirectionArray)
    {
        pointVector = new Vector3[7];
        switch (defenseMoveDirectionArray)
        {
            case DefenseMoveDirectionArray.Up:
                {
                    pointVector[0] = new Vector3(1, 0, 1);
                    pointVector[1] = new Vector3(0, 0, 1);
                    pointVector[2] = new Vector3(-1, 0, 1);
                    pointVector[3] = new Vector3(-1, 0, 1);
                    pointVector[4] = new Vector3(0, 0, 1);
                    pointVector[5] = new Vector3(1, 0, 1);
                    pointVector[6] = new Vector3(0, 0, 1);
                    break;
                }
            case DefenseMoveDirectionArray.Middle:
                {
                    pointVector[0] = new Vector3(0, 0, 1);
                    pointVector[1] = new Vector3(1, 0, 1);
                    pointVector[2] = new Vector3(1, 0, 1);
                    pointVector[3] = new Vector3(-1, 0, 1);
                    pointVector[4] = new Vector3(-1, 0, 1);
                    pointVector[5] = new Vector3(0, 0, 1);
                    pointVector[6] = new Vector3(0, 0, 1);
                    break;
                }
            case DefenseMoveDirectionArray.Down:
                {
                    pointVector[0] = new Vector3(-1, 0, 1);
                    pointVector[1] = new Vector3(0, 0, 1);
                    pointVector[2] = new Vector3(1, 0, 1);
                    pointVector[3] = new Vector3(1, 0, 1);
                    pointVector[4] = new Vector3(0, 0, 1);
                    pointVector[5] = new Vector3(-1, 0, 1);
                    pointVector[6] = new Vector3(0, 0, 1);
                    break;
                }

        }
    }

    //monster animation event;
    public void AttackStart()
    {
        moveAble = false;
        isAttack = true;
    }
    public void AnimatorReset()
    {
        //animator.SetInteger ("State", 0);
    }
    public void AttackBlitz()
    {
        if (monsterId == MonsterId.Rabbit || monsterId == MonsterId.Bear || monsterId == MonsterId.BlackBear)
        {
            for (int i = 0; i < attackCollider.Length; i++)
            {
                attackCollider[i].AttackColliderOn();
            }
        }
        if (monsterId == MonsterId.Duck || monsterId == MonsterId.Frog)
        {
            if (direction == left)
            {
                GameObject bullet = (GameObject)Instantiate(bulletInstantiate[0], new Vector3(this.transform.position.x, this.transform.position.y + 1.5f, this.transform.position.z - 0.53f), this.transform.rotation);
                if (monsterId == MonsterId.Duck)
                {
                    bullet.GetComponent<ShockWave>().SetDamage(attack, this);
                }
                if (monsterId == MonsterId.Frog)
                {
                    bullet.GetComponent<FrogBullet>().SetDamage(attack, this.gameObject);
                    bullet.GetComponent<FrogBullet>().SetMoveVector(new Vector3(0, 0, 1));
                }
            }
            if (direction == right)
            {
                GameObject bullet = (GameObject)Instantiate(bulletInstantiate[0], new Vector3(this.transform.position.x, this.transform.position.y + 1.5f, this.transform.position.z + 0.53f), this.transform.rotation);
                if (monsterId == MonsterId.Duck)
                {
                    bullet.GetComponent<ShockWave>().SetDamage(attack, this);
                }
                if (monsterId == MonsterId.Frog)
                {
                    bullet.GetComponent<FrogBullet>().SetDamage(attack, this.gameObject);
                    bullet.GetComponent<FrogBullet>().SetMoveVector(new Vector3(0, 0, -1));
                }
            }

        }

    }
    public void AttackEnd()
    {
        animator.SetInteger("State", 0);
        moveAble = true;
        isAttack = false;



        if (monsterId == MonsterId.Rabbit || monsterId == MonsterId.Bear || monsterId == MonsterId.BlackBear)
        {
            for (int i = 0; i < attackCollider.Length; i++)
            {
                attackCollider[i].AttackColliderOff();
            }
        }
    }


    IEnumerator Shooting()
    {

        int shootNum = 0;
        while (shootNum < shootNumber)
        {
            yield return new WaitForSeconds(0.2f);

            int xPos = Random.Range(-2, 2);
            int zPos = Random.Range(-2, 2);
            GameObject bullet = Instantiate(Resources.Load<GameObject>("Effect/BossDarkSphere"), attackCollider[0].transform.position + (Vector3.right * xPos) + (Vector3.forward * zPos), Quaternion.Euler(0, 0, 0)) as GameObject;
            bullet.GetComponent<OneHandAttack>().SetDamage(attack, this.gameObject);

            shootNum++;
        }
        yield return new WaitForSeconds(0.2f);
    }

    //monsterdie event;
    public void MonsterArrayEraser(GameObject thisGameObject)
    {
        this.gameObject.SetActive(false);

        //DungeonManager dun = GameObject.Find ("DungeonManager").GetComponent<DungeonManager> ();


    }

    public void HitDamage(int _Damage)
    {
        currentHP -= _Damage;

        Debug.Log("in boss");
        if (currentHP > 0)
        {
            if (monsterId != MonsterId.Bear || monsterId != MonsterId.BlackBear)
            {
                statePosition = StatePosition.TakeDamage;
                Pattern(statePosition);
            }
        }
        else
        {
            currentHP = 0;
            //   IsAlive = false;
            HittedBox.enabled = false;
            statePosition = StatePosition.Death;
            Pattern(statePosition);
            Destroy(this.gameObject, 3f);
        }
    }

    void OnTriggerStay(Collider coll)
    {
        Rigidbody rigid = this.gameObject.GetComponent<Rigidbody>();
        rigid.velocity = Vector3.zero;
    }

    void SetStateDefault()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        animator.SetTrigger("IdleState");
        animator.SetBool("Run", false);
    }

    IEnumerator SetTargetPlayer()
    {
        while (IsAlive)
        {
            int chaseIndex = Random.Range(0, player.Length);

            targetPlayer = player[chaseIndex];

            yield return new WaitForSeconds(15.0f);
        }
    }


    public void BosschangeDirection()
    {
        //캐릭터 이동시 보스가 보는 방향을 정한다.
        Vector3 vecLookPos = targetPlayer.transform.position;
        vecLookPos.z = transform.position.z;
        vecLookPos.x = transform.position.x;

        checkDirection = targetPlayer.transform.position - this.gameObject.transform.position;
        checkDirection = new Vector3(checkDirection.x, 0, checkDirection.z);
        if (checkDirection.z > 0)
        {
            LookAtPattern(right);
        }
        else if (checkDirection.z < 0)
        {
            LookAtPattern(left);
        }
    }

    //bossmonster onehandpattern;
    public void MiddleBossNiddleshot()
    {
        if (monsterId == MonsterId.Bear)
        {
            BossWSwingAttack();
        }

        if (monsterId == MonsterId.BlackBear)
        {
            //real boss
            StartCoroutine(BossWaveAttack());

        }

    }
    public void BossWSwingAttack()
    {
        if (this.transform.rotation == Quaternion.Euler(new Vector3(0, 0, 0)))
        {
            GameObject bullet = Instantiate(Resources.Load<GameObject>("Effect/BossSwing"), new Vector3(this.transform.position.x, this.transform.position.y + 2f, this.transform.position.z), Quaternion.Euler(-90, 0, 0)) as GameObject;
            bullet.GetComponent<BossDarkSwing>().SetDamage(attack, this.gameObject);
            //Instantiate (Resources.Load<GameObject> ("Effect/BossSwing"), new Vector3 (this.transform.position.x, this.transform.position.y + 2f, this.transform.position.z), Quaternion.Euler (-90, 0, 0));
        }
        else if (this.transform.rotation == Quaternion.Euler(new Vector3(0, 180, 0)))
        {
            GameObject bullet = Instantiate(Resources.Load<GameObject>("Effect/BossSwing"), new Vector3(this.transform.position.x, this.transform.position.y + 2f, this.transform.position.z), Quaternion.Euler(-90, -90, -90)) as GameObject;
            bullet.GetComponent<BossDarkSwing>().SetDamage(attack, this.gameObject);
            //Instantiate (Resources.Load<GameObject> ("Effect/BossSwing"), new Vector3(this.transform.position.x,this.transform.position.y+2f,this.transform.position.z),Quaternion.Euler (-90, -90, -90));
        }
    }

    IEnumerator BossWaveAttack()
    {
        int wave = 0;
        int waveLength = 3;
        float darkWaveSize = 0.5f;
        GameObject[] darkWave;
        darkWave = new GameObject[3];
        //Instantiate (Resources.Load<GameObject> ("Effect/BossWave"), new Vector3(this.transform.position.x,this.transform.position.y+0.5f,this.transform.position.z+(3f)), this.transform.rotation);
        while (wave < waveLength)
        {


            darkWaveSize += 0.3f;
            yield return new WaitForSeconds(0.2f);

            if (this.transform.rotation == Quaternion.Euler(new Vector3(0, 0, 0)))
            {
                darkWave[wave] = Instantiate(Resources.Load<GameObject>("Effect/BossWave"), new Vector3(this.transform.position.x, this.transform.position.y + 0.8f, this.transform.position.z + (4f * (wave + 1))), this.transform.rotation) as GameObject;
                darkWave[wave].transform.localScale = new Vector3(darkWaveSize, darkWaveSize, darkWaveSize);
            }
            else if (this.transform.rotation == Quaternion.Euler(new Vector3(0, 180, 0)))
            {
                darkWave[wave] = Instantiate(Resources.Load<GameObject>("Effect/BossWave"), new Vector3(this.transform.position.x, this.transform.position.y + 0.8f, this.transform.position.z - (4f * (wave + 1))), Quaternion.Euler(0, 0, 0)) as GameObject;

            }

            wave++;
        }
        yield return new WaitForSeconds(0.2f);
    }

    public void GetTargetPlayer(GameObject _TargerPlayer)
    {

    }

    public void SendTargetPlayer()
    {

    }

    public void RoarHitWarning()
    {
        if (this.transform.rotation == Quaternion.Euler(new Vector3(0, 0, 0)))
        {
            roarField = Instantiate(Resources.Load<GameObject>("Effect/WarningEffect"), new Vector3(-3.55f, 0.15f, this.transform.position.z + 10f), Quaternion.Euler(-90, 0, 0)) as GameObject;
        }
        else if (this.transform.rotation == Quaternion.Euler(new Vector3(0, 180, 0)))
        {
            roarField = Instantiate(Resources.Load<GameObject>("Effect/WarningEffect"), new Vector3(-3.55f, 0.15f, this.transform.position.z - 10f), Quaternion.Euler(-90, 0, 0)) as GameObject;

        }
    }

    public void RoarHit()
    {
        Destroy(roarField);
        GameObject Howling;
        if (this.transform.rotation == Quaternion.Euler(new Vector3(0, 0, 0)))
        {
            Howling = Instantiate(Resources.Load<GameObject>("Effect/BossRoar"), new Vector3(this.transform.position.x, this.transform.position.y + 2.15f, this.transform.position.z + 1f), Quaternion.Euler(0, 0, 0)) as GameObject;
            Howling.GetComponent<BearRoar>().SetDamage(attack, this.gameObject);
        }
        else if (this.transform.rotation == Quaternion.Euler(new Vector3(0, 180, 0)))
        {
            Howling = Instantiate(Resources.Load<GameObject>("Effect/BossRoar"), new Vector3(this.transform.position.x, this.transform.position.y + 2.15f, this.transform.position.z - 1f), Quaternion.Euler(180, 0, 0)) as GameObject;
            Howling.GetComponent<BearRoar>().SetDamage(attack, this.gameObject);
        }
    }
}