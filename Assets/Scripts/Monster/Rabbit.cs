using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Rabbit : Monster {
//	private float searchRange = 8.0f;

//	public float currentDisTance;
	float middleBossToMonsterLimitDistanceMonsterToCenter = 6.0f;
//	private float middleBossToMonsterMinDistance = 1.5f;

        


//	public override void HitDamage(int _Damage,GameObject attacker)
//	{
//		IsHited = true;
//		currentHP -= _Damage;
//		if (currentHP > 0) {
//			for (int i = 0; i < player.Length; i++) {
//				if (player [i] == attacker) {
//					playerToMonsterDamage [i] += _Damage;
//					targetPlayer = player [i];
//				}
//			}
//			Pattern (StatePosition.TakeDamage);
//		}
//		if (currentHP <= 0) {
//			currentHP = 0;
//			IsAlive = false;
//			HittedBox.enabled = false;
//			monsterState = StatePosition.Death;
//			Pattern (monsterState);
//		}
//	}

	//private Vector3 boomPoint = new Vector3(100,100,100);



	public StatePosition monsterState;


	public Vector3[] PointVector{
		get {return pointVector; }
		set{pointVector = value; }
	}

	public Vector3[] TransitionVector;



	public IEnumerator pointVectorchange()
	{
		while (true)
		{
			for (int i = 0; i < pointVector.Length; i++)
			{
				if (i > 0 && i < pointVector.Length - 1)
				{
					//transitionVector = pointVector[i];
					pointVector[i] = pointVector[i + 1];
					//pointVector[i + 1] = transitionVector;
				}

				if (i == pointVector.Length - 1)
				{
					//transitionVector = pointVector[i];
					pointVector[i] = pointVector[0];
					//pointVector[0] = transitionVector;
				}
			}
			yield return new WaitForSeconds(0.5f);
		}
	}


	//animation Set; move;
//	public void Pattern(StatePosition state)
//	{
//		switch (state)
//		{
//		case StatePosition.Idle:
//			{
//				this.transform.Translate(idlePoint * Time.deltaTime, 0);
//				animator.SetInteger("State", 0);
//				break;
//			}
//		
//		case StatePosition.Attack:
//			{
//				AttackProcess(isAttack);
//				break;
//			}
//		case StatePosition.Run:
//			{
//				AnimatorReset();
//
//				animator.SetInteger("State", 2);
//
//				break;
//			}
//		case StatePosition.TakeDamage:
//			{
//				animator.SetTrigger ("TakeDamage");
//				break;
//			}
//		case StatePosition.Death:
//			{
//				animator.SetTrigger ("Death");
//				MonsterArrayEraser(this.gameObject);
//				break;
//			}
//		}
//	}
//
//
//	public void AttackProcess(bool isAttack){
//		if (isAttack) {
//
//			if(animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Run")){
//				animator.SetInteger ("State", 0);
//			}
//			if (animator.GetCurrentAnimatorStateInfo (0).IsName ("Base Layer.Idle")) {
//				animator.SetInteger ("State", 3);
//			}
//			if (animator.GetCurrentAnimatorStateInfo (0).IsName ("Base Layer.Attak")) {
//				moveAble = false;
//			}
//		}
//	}

	public void middleBossPositionGetting(Vector3 _Position){
		//boomObjectPosition = _Position;
	}


	public void MonSterPatternUpdateConduct(bool NormalMode){
		//attackCollider = transform.GetComponentInChildren<MonsterWeapon> ();
		if (NormalMode) {
			StartCoroutine (PatternNormalChange ());
		} else if (!NormalMode) {
			StartCoroutine (PatternDefenceChange ());
		}
	}

//	public override void MonsterMoveAI(bool _normalMode){
//		if (_normalMode) {
//			StartCoroutine (AttackAroundRun ());
//		} else if (!_normalMode) {
//		
//		}
//	}

//	public IEnumerator AttackAroundRun(){
//		while (true) {
//
//			if(IsAlive){
//				if (targetPlayer != null) {
//					monsterRunAttackAround = Random.Range (0, 4);
//					if (monsterRunAttackAround == 0 || monsterRunAttackAround == 1) {
//						movePoint = new Vector3 (checkDirection.x, 0, checkDirection.z);
//						yield return new WaitForSeconds (2f);
//						movePoint = new Vector3 (checkDirection.x, 0, checkDirection.z);
//						yield return new WaitForSeconds (2f);
//						movePoint = new Vector3 (checkDirection.x, 0, checkDirection.z);
//						yield return new WaitForSeconds (2f);
//						movePoint = new Vector3 (checkDirection.x, 0, checkDirection.z);
//						yield return new WaitForSeconds (2f);
//					}
//
//					if (monsterRunAttackAround == 2) {
//						int i = Random.Range (0, 2);
//						if (i == 0) {
//							if (checkDirection.z >= 0) {
//								movePoint = new Vector3 (1, 0, 1);
//								yield return new WaitForSeconds (2f);
//								movePoint = new Vector3 (-1, 0, 1);
//								yield return new WaitForSeconds (2f);
//								movePoint = new Vector3 (-1, 0, -1);
//								yield return new WaitForSeconds (2f);
//								movePoint = new Vector3 (1, 0, 1);
//								yield return new WaitForSeconds (2f);
//							} else if (checkDirection.z < 0) {
//								movePoint = new Vector3 (1, 0, -1);
//								yield return new WaitForSeconds (2f);
//								movePoint = new Vector3 (-1, 0, -1);
//								yield return new WaitForSeconds (2f);
//								movePoint = new Vector3 (-1, 0, 1);
//								yield return new WaitForSeconds (2f);
//								movePoint = new Vector3 (1, 0, 1);
//								yield return new WaitForSeconds (2f);
//							}
//						}
//						else if(i ==1){
//							if (checkDirection.z >= 0) {
//								movePoint = new Vector3 (-1, 0, 1);
//								yield return new WaitForSeconds (2f);
//								movePoint = new Vector3 (1, 0, 1);
//								yield return new WaitForSeconds (2f);
//								movePoint = new Vector3 (1, 0, -1);
//								yield return new WaitForSeconds (2f);
//								movePoint = new Vector3 (-1, 0, 1);
//								yield return new WaitForSeconds (2f);
//							} else if (checkDirection.z < 0) {
//								movePoint = new Vector3 (-1, 0, -1);
//								yield return new WaitForSeconds (2f);
//								movePoint = new Vector3 (1, 0, -1);
//								yield return new WaitForSeconds (2f);
//								movePoint = new Vector3 (1, 0, 1);
//								yield return new WaitForSeconds (2f);
//								movePoint = new Vector3 (-1, 0, 1);
//								yield return new WaitForSeconds (2f);
//							}
//						}
//					}
//					if (monsterRunAttackAround == 3) {
//						movePoint = new Vector3 (checkDirection.x, 0, checkDirection.z);
//						int k = Random.Range(0,1);
//						if(k == 0){
//							movePoint = new Vector3 (-checkDirection.x, 0, -checkDirection.z);
//							yield return new WaitForSeconds (2f);
//							movePoint = new Vector3 (checkDirection.x, 0, -checkDirection.z);
//							yield return new WaitForSeconds (2f);
//							movePoint = new Vector3 (checkDirection.x, 0, -checkDirection.z);
//							yield return new WaitForSeconds (2f);
//							movePoint = new Vector3 (-checkDirection.x, 0, -checkDirection.z);
//							yield return new WaitForSeconds (2f);
//						}
//						if(k == 1){
//							movePoint = new Vector3 (checkDirection.x, 0, -checkDirection.z);
//							yield return new WaitForSeconds (2f);
//							movePoint = new Vector3 (-checkDirection.x, 0, -checkDirection.z);
//							yield return new WaitForSeconds (2f);
//							movePoint = new Vector3 (-checkDirection.x, 0, -checkDirection.z);
//							yield return new WaitForSeconds (2f);
//							movePoint = new Vector3 (checkDirection.x, 0, -checkDirection.z);
//							yield return new WaitForSeconds (2f);
//						}
//					}
//					yield return new WaitForSeconds (2f);
//				} else
//					yield return new WaitForSeconds (2f);
//			}
//			else if(!IsAlive){
//
//				yield return false;
//			}
//		}
//	}

	public IEnumerator PatternNormalChange(){
		while(IsAlive){
			if (targetPlayer != null) {	
				currentDisTance = Vector3.Distance (targetPlayer.transform.position, this.gameObject.transform.position);
				checkDirection = targetPlayer.transform.position - this.gameObject.transform.position;
				if (currentDisTance > searchRange) {
					monsterState = StatePosition.Idle;
					Pattern (monsterState);
				}
				//if this object get Attackmotion pattern(stateposition.boom -> attack), and this monsterlife is 20%, boomPattern start;
				else if (currentDisTance <= searchRange) {
					movePoint = new Vector3 (checkDirection.x, 0, checkDirection.z);
					if (currentDisTance > searchRange * 0.2f) {
						moveAble = true;
						isAttack = false;
						monsterState = StatePosition.Run;
						Pattern (monsterState);
					}
					if (currentDisTance <= searchRange * 0.3f) {
						if (!isAttack) {
							isAttack = true;
							moveAble = false;
						}
						monsterState = StatePosition.Attack;
						Pattern (monsterState);
						yield return new WaitForSeconds (0.5f); 
					}
				}
			}
			yield return new WaitForSeconds(0.2f);
		}
	}
	public IEnumerator PatternDefenceChange(){
		while(IsAlive){
//			if(){
//				
//			}









			if (!IsHited) {
				

				//transform.Translate (transitionVector * moveSpeed * 0.5f * Time.deltaTime);
			}
			if (IsHited) {
				currentDisTance = Vector3.Distance(targetPlayer.transform.position, this.gameObject.transform.position);
				checkDirection = targetPlayer.transform.position - this.gameObject.transform.position;

				if (checkDirection.z > 0) {
					LookAtPattern (right);
				}
				if (checkDirection.z <= 0) {
					LookAtPattern (left);
				}



				if (currentDisTance < middleBossToMonsterLimitDistanceMonsterToCenter*1.5f) {
					movePoint = new Vector3 (checkDirection.x, 0, checkDirection.z);
					transform.Translate(movePoint.normalized * moveSpeed * Time.deltaTime, 0);
					if (currentDisTance >= searchRange * 0.2f)
					{
						if (moveAble) {
							Pattern (StatePosition.Run);
							Debug.Log ("Run");
						}
					}
					if (currentDisTance < searchRange * 0.2f)
					{
						attackCycle += Time.deltaTime;
						if (attackCycle > 5) {
							attackCycle = 0;
							if (!isAttack) {
								isAttack = true;
								Pattern (StatePosition.Attack);
							}
						}
					}
				}
				if (currentDisTance >= middleBossToMonsterLimitDistanceMonsterToCenter*1.5f) {
					LookAtPattern (right);
					IsHited = false;
					targetPlayer = null;
					//transform.Translate (boomObjectPosition*Time.deltaTime);
				}
			}
		}
		yield return new WaitForSeconds (0.2f);
	}


	public void UpdateNormalMode()
	{
		aniState = this.animator.GetCurrentAnimatorStateInfo (0);

		if (aniState.IsName ("Run")) 
		{
			if (moveAble) 
			{
				//if (Mathf.Abs (transform.position.x + movePoint.x) <= 5 || Mathf.Abs (transform.position.z + movePoint.z) <= 30) {
				this.transform.Translate (movePoint.normalized * moveSpeed * Time.deltaTime, 0);
				//}/
				//				if (Wall [0].transform.position.x - transform.position.x <= 0.2f || Wall [1].transform.position.x - transform.position.x >= 0.2f) {
				//					this.transform.Translate ((movePoint - 2 * new Vector3(movePoint.x,0,0)).normalized * moveSpeed * Time.deltaTime, 0);
				//				}
				//				if (Wall [2].transform.position.z - transform.position.z >= 0.2f || Wall[3].transform.position.z - transform.position.z <= 0.2f) {
				//					this.transform.Translate ((movePoint - 2 * new Vector3 (0, 0, movePoint.z)).normalized * moveSpeed * Time.deltaTime, 0);
				//				}

			}
		}
		ChasePlayer ();
	}
	public void UpdateDefenceMode(){
		if (!IsHited) {
			//transform.Translate (transitionVector * moveSpeed * 0.5f * Time.deltaTime);
		}
		if (IsHited) {

			if (checkDirection.z > 0) {
				LookAtPattern (right);
			}
			if (checkDirection.z <= 0) {
				LookAtPattern (left);
			}

			currentDisTance = Vector3.Distance(targetPlayer.transform.position, this.gameObject.transform.position);
			checkDirection = targetPlayer.transform.position - this.gameObject.transform.position;



			if (currentDisTance < middleBossToMonsterLimitDistanceMonsterToCenter*1.5f) {
				movePoint = new Vector3 (checkDirection.x, 0, checkDirection.z);
				transform.Translate(movePoint.normalized * moveSpeed * Time.deltaTime, 0);
				if (currentDisTance >= searchRange * 0.2f)
				{
					if (moveAble) {
						Pattern (StatePosition.Run);
						Debug.Log ("Run");
					}
				}
				if (currentDisTance < searchRange * 0.2f)
				{
					attackCycle += Time.deltaTime;
					if (attackCycle > 5) {
						attackCycle = 0;
						if (!isAttack) {
							isAttack = true;
							Pattern (StatePosition.Attack);
						}
						//					}
					}
				}
			}
			if (currentDisTance >= middleBossToMonsterLimitDistanceMonsterToCenter*1.5f) {
				LookAtPattern (right);
				IsHited = false;
				targetPlayer = null;
				//transform.Translate (boomObjectPosition*Time.deltaTime);
			}
		}
	}


	// server code;
	public void SendMonsterState(StatePosition _state, bool _isAttack, bool _moveAble, Vector3 _movePoint, GameObject _Player){
		//send to server;

	}

	public void RecibeMonsterState(StatePosition _state, bool _isAttack, bool _moveAble, Vector3 _movePoint, GameObject _Player){
		if (_state == StatePosition.Run) {
			movePoint = _movePoint;
			monsterState = _state;
		}

		if (_state == StatePosition.Attack) {
			monsterState = _state;
		}
		if (_state == StatePosition.TakeDamage) {
			monsterState = _state;
		}
		if (_state == StatePosition.Idle) {
			monsterState = _state;
		}
		if (_state == StatePosition.Death) {
			monsterState = _state;
		}
			

		monsterState = _state;
		isAttack = _isAttack;
		moveAble = _moveAble;
		Pattern (monsterState);
		if (_Player != null) {
			targetPlayer = _Player;
		}
	}

	public IEnumerator GuestMonsterPatternChange(){
		while (IsAlive) {
			Pattern (monsterState);
			yield return new WaitForSeconds (0.2f);
		}
	}




}
