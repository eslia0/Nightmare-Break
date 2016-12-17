using UnityEngine;
using System.Collections;

public class MiddleBossMonster : Monster {
	public enum MiddleBossPatternName //중간 보스 패턴
	{
		MiddleBossIdle = 0,
		MiddleBossRun,
		MiddleBossAttack,
		MiddleBossOneHandAttack,
		MiddleBossJumpAttack,
		MiddleBossDeath}

	;

	public MiddleBossPatternName MiddleBossState;
	AnimatorStateInfo stateInfo;
//	public float moveSpeed;//이동 속도
	public int ChaseCount = 0; //동작 시간 카운트
	public bool MonsterUp = true; //몬스터 이동 방향 위일때 true
	public float currentDistance; //중간 보스와 플레이어의 거리

	public GameObject[] bossplayer; //플레이어 배열
	public GameObject chasePlayer;// 현재 쫓는 플레이어
	int randomAttack; //어택 패턴값

	void Start () {
		animator = GetComponent<Animator> ();
		moveSpeed = 3;
		IsAlive = true;
		StartCoroutine (CoChasePlayer());
		bossplayer = GameObject.FindGameObjectsWithTag ("Player");
		chasePlayer = null;
		attackRange = 3;
		//StartCoroutine (BossAI());
		moveAble= true;
	}
	IEnumerator CoChasePlayer(){ //동작 시간 및 공격 패턴 코루틴

		while (IsAlive) {
			yield return new WaitForSeconds (1.0f); //1초마다 동작
			ChaseCount++;
			switch (ChaseCount) {
			case 4:                              //4초일때 공격
				randomAttack = Random.Range (1, 4);
				moveAble = false;                 //공격시 이동하지 않는다.
				MiddleBossPattern (randomAttack);   //공격패턴 랜덤

				break;
			case 8:                               //8초일때 공격
				randomAttack = Random.Range (1, 4);
				moveAble = false;                 //공격시 이동하지 않는다.
				MiddleBossPattern (randomAttack);

				break;
			}
			if (ChaseCount > 17) {  //17초를 넘길시 0값으로 초기화
				ChaseCount = 0;
			}
		}
	}
	public void MoveableSet(){ //애니메이션 이벤트 공격 끝날때 이동 가능하게
		moveAble = true;
		Debug.Log ("in");
	}
//	public override void HitDamage (int _Damage, GameObject attacker)
//	{
//		stateInfo = this.animator.GetCurrentAnimatorStateInfo (0);
//
//		if (IsAlive)
//		{
//			maxHP -= _Damage;
//
//			//			UIManager.Instance.bossHp.fillAmount = maxLife / currentLife;
//			if (maxHP > 0)
//			{
//				//hitanimation
//			}
//			else if (maxHP <= 0)
//			{
//				if (!stateInfo.IsName ("MiddleBossDeath"))
//				{
//					MiddleBossPattern ((int)MiddleBossPatternName.MiddleBossDeath);
//					IsAlive = false;
//					return;
//				}
//			}
//		}
//	}
	public void changeDirection ()
	{//캐릭터 이동시 보스가 보는 방향을 정한다.
		for (int i = 0; i < bossplayer.Length; i++) {
			currentDistance = Vector3.Distance (bossplayer [i].transform.position, transform.position);//Middle Boss와 플레이어와의 거리 계산
			if (chasePlayer == null) {       //추적하는 플레이어 없을 시 
				chasePlayer = bossplayer [i]; 
			}
			else if(currentDistance < Vector3.Distance (chasePlayer.transform.position, transform.position)) {//젤 가까운 플레이어 추적
				chasePlayer = bossplayer [i];
			}
		}
		Vector3 vecLookPos =chasePlayer.transform.position; 
		vecLookPos.y = transform.position.y;
		vecLookPos.x = transform.position.x;

		transform.LookAt (vecLookPos); // 보는 방향 설정

	}
	//IEnumerator BossAI(){

	//}
	// Update is called once per frame
	void Update () 
	{
		if (moveAble)
		{ //이동 가능 할때
			if (ChaseCount < 15 && transform.position.x < 2.85 && MonsterUp == true)
			{ //15초 이내 위로 이동
				changeDirection ();


				MiddleBossPattern ((int)MiddleBossPatternName.MiddleBossRun);   
				transform.Translate (Vector3.right * moveSpeed * Time.deltaTime, 0); //윗방향 이동


				if (transform.position.x >= 2.85)
				{ //특정 거리 이내 이동
					MonsterUp = false; //아랫방향으로 변경
				}

			}
			else if (ChaseCount < 15 && transform.position.x > -2.85 && MonsterUp == false)
			{//15초 이내 아래로 이동
				changeDirection ();


				MiddleBossPattern ((int)MiddleBossPatternName.MiddleBossRun);
				transform.Translate (Vector3.left * moveSpeed * Time.deltaTime, 0);


				if (transform.position.x <= -2.85)
				{//특정 거리 이내 이동
					MonsterUp = true; //윗 방향으로 변경
				}
			}
			else if (ChaseCount >= 15)
			{ //15초 지날때
				changeDirection ();
				if (Vector3.Distance (chasePlayer.transform.position, transform.position) > attackRange)
				{ //공격 거리 이내 일때 
					MiddleBossPattern ((int)MiddleBossPatternName.MiddleBossRun);
					this.transform.Translate ((chasePlayer.transform.position - transform.position).normalized * moveSpeed * Time.deltaTime, 0);//중간보스가 캐릭터를 추적
				}
			}
		}

	}
	public void MiddleBossPattern (int bossState) //중간 보스 패턴 
	{
		//monsterAttack = false;
		switch (bossState)
		{

		case 0:
			MiddleBossState = MiddleBossPatternName.MiddleBossIdle;
			animator.SetInteger ("state", 0);

			break;
		case 1:
			MiddleBossState = MiddleBossPatternName.MiddleBossRun;
			animator.SetInteger ("state", 1);

			break;
		case 2:
			MiddleBossState = MiddleBossPatternName.MiddleBossAttack;
			animator.SetInteger ("state", 2);

			break;

		case 3:
			MiddleBossState = MiddleBossPatternName.MiddleBossOneHandAttack;
			animator.SetInteger ("state", 3);

			break;

		case 4:
			MiddleBossState = MiddleBossPatternName.MiddleBossJumpAttack;
			animator.SetInteger ("state", 4);

			break;
		case 5:
			MiddleBossState = MiddleBossPatternName.MiddleBossDeath;
			animator.SetTrigger ("MiddleBossDeath");
			break;
		}
	}
}
