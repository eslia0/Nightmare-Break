using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class BossMonsterKYW : Monster {
	public enum BigBearBossPatternName
	{
		BigBearBossIdle = 0,
		BigBearBossRun,
		BigBearBossAttack,
		BigBearBossOneHandAttack,
	    BigBearBossRoar,
		BigBearBossShout,
		BigBearBossDeath}

	;

	public int[] patternReserveList;

	public int PatternRank;

	public float searchRange = 10;
	public int shoutCount;

	public float currentDisTance;
	bool halfLife;

	//public UIManager UIManager.Instance;
	public MonsterWeapon[] BossMonsterWeapon;

	public BigBearBossPatternName BigBearBossState;
	//	public PatternRank patternRank;

	AnimatorStateInfo stateInfo;

	[SerializeField] Image skillInsertImage;
	public float imageSpeed = 1.0f;
	public float imageLerpTime;

	public insertImageState imageState = insertImageState.Stop;

	public enum insertImageState
	{
		Stop = 0,
		Left,
		Right
	}
	public GameObject bullet;
	public GameObject muzzle;

	public void BossMonsterSet(int _maxlife, int _basedamage){
		RunRange = 10;
		attackRange = 4;

        if (GameObject.FindWithTag("GameManager") == null)
        {   //네트워크 없을 때 실행 함
            MonsterBaseData baseData = new MonsterBaseData((int)MonsterId.Bear, "Bear");
            baseData.AddLevelData(new MonsterLevelData(1, 20, 0, 300, 5));
            MonsterSet(baseData);
            //UIManager.Instance = GameObject.FindWithTag("UIManager").GetComponent<UIManager>();
        }

		isAttack = false;
		skillInsertImage = GameObject.Find ("InGameUICanvas").transform.Find ("BossDeadlyPatternImage").Find("BossDeadlyPattern").GetComponent<Image>();
		//skillInsertImage = transform.Find("InGameUICanvas").gameObject;
		halfLife = false;

		BossMonsterWeapon [0].MonsterWeaponSet();
		BossMonsterWeapon [1].MonsterWeaponSet();

		patternReserveList = new int[5];
		for (int listSet = 0; listSet < patternReserveList.Length; listSet++) {
			Debug.Log (Random.Range (2, 4)); // max + 1-> (min <-> max)
			//patternReserveList [listSet] = Random.Range ((int)BigBearBossPatternName.BigBearBossAttack,(int)BigBearBossPatternName.BigBearBossShout);
			patternReserveList [listSet] = Random.Range ((int)BigBearBossPatternName.BigBearBossAttack,(int)BigBearBossPatternName.BigBearBossOneHandAttack+1);
		}
	}

	public void BossMonsterUpdate(){
		/*
		if (Input.GetKeyDown (KeyCode.Q)) {
			PatternReserveListCleanUp ((int)BigBearBossPatternName.BigBearBossRoar);
			//BigBearBossState = BigBearBossPatternName.BigBearBossRoar;
			//BigBearBossPattern ((int)BigBearBossState);
		}
		if (Input.GetKeyDown (KeyCode.W)) {
			PatternReserveListCleanUp ((int)BigBearBossPatternName.BigBearBossOneHandAttack);
			//BigBearBossState = BigBearBossPatternName.BigBearBossRoar;
			//BigBearBossPattern ((int)BigBearBossState);
		}
		if (Input.GetKeyDown (KeyCode.E)) {
			PatternReserveListCleanUp ((int)BigBearBossPatternName.BigBearBossShout);
			//BigBearBossState = BigBearBossPatternName.BigBearBossRoar;
			//BigBearBossPattern ((int)BigBearBossState);
		}
		*/
		if (targetPlayer != null) {
			currentDisTance = Vector3.Distance (targetPlayer.transform.position, transform.position);		
		}


		if (IsAlive) {
			stateInfo = this.animator.GetCurrentAnimatorStateInfo (0);
			ChasePlayer ();

//			searchRange = Vector3.Distance (player [0].transform.position, transform.position);
//			attackCyc+= Time.deltaTime;
		}
		if (!IsAlive) {
			Destroy (this.gameObject, 5);
		}

		if (stateInfo.IsName ("BigBearBossRun")) {

			transform.Translate ((player [0].transform.position - transform.position) * moveSpeed * Time.deltaTime, 0);//반대로 걸어 가서 수정
			//transform.position = Vector3.Lerp (transform.position, player [0].transform.position, Time.deltaTime * moveSpeed);
		}



	}
public void shotEffect()//원거리 공격가능
{
  Instantiate (bullet, muzzle.transform.position, muzzle.transform.rotation);
}
public void changeDirection ()
{//캐릭터 이동시 보스가 보는 방향을 정한다.
		Vector3 vecLookPos = targetPlayer.transform.position;
	vecLookPos.y = transform.position.y;
	vecLookPos.x = transform.position.x;

	transform.LookAt (vecLookPos);

}


//public override void HitDamage (int _Damage, GameObject attacker)
//{
//
//	stateInfo = this.animator.GetCurrentAnimatorStateInfo (0);
//
//
//	if (IsAlive)
//	{
//		currentHP -= _Damage;
//		shoutCount +=1;
//
//            //UIManager.Instance.bossHp.fillAmount = maxLife / currentLife;
//            if (currentHP > 0)
//            {
//                for (int i = 0; i < player.Length; i++)
//                {
//                    if (player[i] == attacker)
//                    {
//                        playerToMonsterDamage[i] += _Damage;
//                        targetPlayer = player[i];
//                    }
//                }
//                if (targetPlayer != null)
//                {
//                    currentDisTance = Vector3.Distance(targetPlayer.transform.position, transform.position);
//                }
//                if (currentDisTance <= RunRange && currentDisTance > attackRange)
//                {//원거리 케릭터가 공격시 원거리 공격
//                    PatternReserveListCleanUp((int)BigBearBossPatternName.BigBearBossOneHandAttack);
//                }
//                if (shoutCount >= 100)
//                {//100회 이상 데미지를 받을 경우
//                    PatternReserveListCleanUp((int)BigBearBossPatternName.BigBearBossRoar);
//                }
//                //hitanimation
//            }
//
//            else if (currentHP <= 0)
//            {
//                currentHP = 0;
//                if (!stateInfo.IsName("BigBearBossDeath"))
//                {
//                    BigBearBossState = BigBearBossPatternName.BigBearBossDeath;
//                    BigBearBossPattern((int)BigBearBossState);
//                    IsAlive = false;
//                    HittedBox.enabled = false;
//                    return;
//                }
//            }
//	}
//}

public void BigBearBossPattern (int bossState)
{
	IsAttack = false;
	switch (bossState)
	{

	case 0:
		BigBearBossState = BigBearBossPatternName.BigBearBossIdle;
		animator.SetInteger ("state", 0);
		break;
	case 1:
		BigBearBossState = BigBearBossPatternName.BigBearBossRun;
		animator.SetInteger ("state", 1);
		break;
	case 2:
		BigBearBossState = BigBearBossPatternName.BigBearBossAttack;
		animator.SetInteger ("state", 2);
		IsAttack = true;
		break;

	case 3:
		BigBearBossState = BigBearBossPatternName.BigBearBossOneHandAttack;
		animator.SetInteger ("state", 3);
		IsAttack = true;
		break;

	case 4:
	    BigBearBossState = BigBearBossPatternName.BigBearBossRoar;
		animator.SetInteger ("state", 4);
		break;

	case 5:
		BigBearBossState = BigBearBossPatternName.BigBearBossShout;
		animator.SetInteger ("state", 5);
		break;

	case 6:
		BigBearBossState = BigBearBossPatternName.BigBearBossDeath;
		animator.SetTrigger ("BigBearBossDeath");
		break;
	}
}



IEnumerator LMoveImage ()
{  //이미지를 왼쪽이동시키는 함수
	while (imageState == insertImageState.Left)
	{		

		yield return new WaitForSeconds (0.01f);	


		float x = skillInsertImage.GetComponent<RectTransform> ().localPosition.x;//현재 x값의 위치를 받아온다.
		if (x > 103)
		{
			skillInsertImage.GetComponent<RectTransform> ().Translate (15 * -imageSpeed, 0, 0);//10씩 왼쪽으로 이동

		}
		else
		{
			//	skillInsertImage.enabled = true;
			imageLerpTime += 0.01f;

			if (imageLerpTime >3)
			{
				float ImageAlpha = (4 - imageLerpTime);
				skillInsertImage.color = new Color (255,255,255, ImageAlpha);


				if (ImageAlpha < 0.3)
				{
					skillInsertImage.enabled = false;
					imageState = insertImageState.Stop;//x값이 0보다 작을 경우 멈춤
					ImageBackPos ();
				}

			}
		}
	}
}

public void ResetBossImage()
{
	skillInsertImage.GetComponent<RectTransform> ().localPosition = new Vector3 (613, -152 , 0);
	skillInsertImage.color = new Color (255,255,255, 255);
	imageState = insertImageState.Stop;
}

public void ImageBackPos()
{
	StopCoroutine (LMoveImage ());

	imageState = insertImageState.Right;

	ResetBossImage ();


	if (imageState == insertImageState.Stop)
	{
		skillInsertImage.enabled = true;
	}
}


	public void BossMonsterPatternUpdateConduct(){
		StartCoroutine (BossMonsterPatternChange ());
	}
	public IEnumerator BossMonsterPatternChange(){
		while (IsAlive) {
			if (attackCycle >= 1) {
				attackCycle = 0;
			}
//			if (stateInfo.IsName ("BigBossBearJumpAttack") || stateInfo.IsName ("BigBearBossRoar") || stateInfo.IsName ("BigBearBossWarning")) {
//				break;
//
//			}


			else if (targetPlayer != null) {
				if (currentDisTance < attackRange) {
					if (patternReserveList [0] != 0) {
						BigBearBossPattern (patternReserveList [0]);
					}
					else if(stateInfo.IsName("BigBearBossIdle")){
						BigBearBossPattern (patternReserveList [1]);
					}
//						BigBearBossPattern ((int)BigBearBossPatternName.BigBearBossRoar);
//						//애니메이션 이벤트로 효과를 넣었음
//						
//						BigBearBossPattern ((int)BigBearBossPatternName.BigBearJumpAttack);
//						//애니메이션 이벤트로 효과를 넣었음
						

				} else if (currentDisTance > RunRange) {
					BigBearBossState = BigBearBossPatternName.BigBearBossIdle;
					BigBearBossPattern ((int)BigBearBossState);
					changeDirection ();

				} else if (currentDisTance <= RunRange && currentDisTance > attackRange) {
					BigBearBossState = BigBearBossPatternName.BigBearBossRun;
					BigBearBossPattern ((int)BigBearBossState);
					changeDirection ();
//				if (stateInfo.IsName ("BigBearBossRun")) {
//
//					//transform.LookAt(player[0].transform.position);
//					transform.Translate ((player [0].transform.position - transform.position) * moveSpeed * Time.deltaTime, 0);//반대로 걸어 가서 수정
//					//transform.position = Vector3.Lerp (transform.position, player [0].transform.position, Time.deltaTime * moveSpeed);
//				}
				}

				if (IsAttack) {
					for (int i = 0; i < BossMonsterWeapon.Length; i++) {
						BossMonsterWeapon [i].AttackColliderSizeChange (new Vector3 (3.6f, 1f, 1.1f));
					}
				} else if (!IsAttack) {
					for (int i = 0; i < BossMonsterWeapon.Length; i++) {
						BossMonsterWeapon [i].AttackColliderSizeChange (new Vector3 (0, 0, 0));
					}
				}
			} else
					BigBearBossState = BigBearBossPatternName.BigBearBossIdle;
					BigBearBossPattern ((int)BigBearBossState);
					yield return new WaitForSeconds (0.2f);
			
				}
			}
		

	

	void PatternReserveListCleanUp(int _normal){ //애니메이션 이벤트로 실행
		if (_normal == 0) {
			for (int i = 0; i < patternReserveList.Length; i++) {
				if (i < patternReserveList.Length - 1) {
					patternReserveList [i] = patternReserveList [i + 1];
				} else if (i == patternReserveList.Length-1) {
					patternReserveList [i] = Random.Range ((int)BigBearBossPatternName.BigBearBossAttack, (int)BigBearBossPatternName.BigBearBossOneHandAttack + 1); // max +1 => min <-> max;
				}
			}

		}else if(_normal == 3){
			patternReserveList[0] = _normal;
			for(int j= 1;j<patternReserveList.Length; j++){
				patternReserveList [j] = Random.Range ((int)BigBearBossPatternName.BigBearBossAttack, (int)BigBearBossPatternName.BigBearBossOneHandAttack + 1); // max +1 => min <-> max;
			}
			BigBearBossPattern (patternReserveList[0]);
			StopCoroutine (BossMonsterPatternChange());

		}else if(_normal >3){
		patternReserveList[0] = _normal;
		for(int k= 1;  k<patternReserveList.Length; k++){
			patternReserveList [k] = Random.Range ((int)BigBearBossPatternName.BigBearBossAttack, (int)BigBearBossPatternName.BigBearBossOneHandAttack + 1); // max +1 => min <-> max;
			}
			BigBearBossPattern (patternReserveList[0]);
			StopCoroutine (BossMonsterPatternChange());
		}
	} 



	// server code;
//	public void SendMonsterState(StatePosition _state, bool _isAttack, bool _moveAble, Vector3 _movePoint, GameObject _Player){
//		//send to server;
//
//	}
//
//	public void RecibeMonsterState(StatePosition _state, bool _isAttack, bool _moveAble, Vector3 _movePoint, GameObject _Player){
//		if (_state == StatePosition.Run) {
//			movePoint = _movePoint;
//		}
//		monsterState = _state;
//		isAttack = _isAttack;
//		moveAble = _moveAble;
//		Pattern (_state);
//		if (_Player != null) {
//			targetPlayer = _Player;
//		}
//	}
//
//	public void GuestMonsterUpdate(){
//		aniState = this.animator.GetCurrentAnimatorStateInfo (0);
//		if (aniState.IsName ("Run")) 
//		{
//			if (moveAble) 
//			{
//				this.transform.Translate (movePoint * moveSpeed * Time.deltaTime, 0);
//			}
//		}
//		ChasePlayer ();
//	}
//
//
//
//	public IEnumerator GuestMonsterPatternChange(){
//		while (IsAlive) {
//			Pattern (monsterState);
//			yield return new WaitForSeconds (0.2f);
//		}
//	}
//






}



//public class TestMonster : Monster
//{
//	
//	//animation Set; move;
//
//	void Update ()
//	{
//
//		if (IsAlive) {
//			stateInfo = this.animator.GetCurrentAnimatorStateInfo (0);
//			searchRange = Vector3.Distance (player [0].transform.position, transform.position);
//			AttackTime += Time.deltaTime;
//
//			if (AttackTime >= 1) {
//				AttackTime = 0;
//			}
//			if (searchRange < attackRange) {
//				if (!secondAttack) {
//					//BigBearBossPattern ((int)BigBearBossPatternName.BigBearBossAttack);
//					BigBearBossPattern ((int)BigBearBossPatternName.BigBearBossRoar);
//					//애니메이션 이벤트로 효과를 넣었음
//					secondAttack = true;
//				} else if (!secondAttack) {
//					BigBearBossPattern ((int)BigBearBossPatternName.BigBearJumpAttack);
//					//애니메이션 이벤트로 효과를 넣었음
//					secondAttack = true;
//				}
//			} else if (searchRange > RunRange) {
//				BigBearBossPattern ((int)BigBearBossPatternName.BigBearBossIdle);
//				changeDirection ();
//
//			} else if (searchRange <= RunRange && searchRange > attackRange) {
//
//				BigBearBossPattern ((int)BigBearBossPatternName.BigBearBossRun);
//				changeDirection ();
//				if (stateInfo.IsName ("BigBearBossRun")) {
//
//					//transform.LookAt(player[0].transform.position);
//					transform.Translate ((player [0].transform.position - transform.position) * moveSpeed * Time.deltaTime, 0);//반대로 걸어 가서 수정
//					//transform.position = Vector3.Lerp (transform.position, player [0].transform.position, Time.deltaTime * moveSpeed);
//				}
//			}
//
//			if (IsAttack) {
//				for (int i = 0; i < MonsterWeapon.Length; i++) {
//					//				MonsterWeapon [i].size = new Vector3 (3.6f, 1f, 1.1f);
//					MonsterWeapon [i].size = new Vector3 (0, 0, 0);
//				}
//			} else if (!IsAttack) {
//				for (int i = 0; i < MonsterWeapon.Length; i++) {
//					MonsterWeapon [i].size = new Vector3 (0, 0, 0);
//				}
//			}
//
//
//		}
//		if (!IsAlive) {
//			Destroy (this.gameObject, 5);
//		}
//
//	}
//}