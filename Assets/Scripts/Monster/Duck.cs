using UnityEngine;
using System.Collections;

public class Duck : Monster {
	float middleBossToMonsterLimitDistanceMonsterToCenter = 6.0f;
//	private float middleBossToMonsterMinDistance = 1.5f;


	[SerializeField]GameObject middleboss;
	private Vector3 boomObjectPosition;



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
//			IsAlive = false;
//			HittedBox.enabled = false;
//			monsterState = StatePosition.Death;
//			Pattern (monsterState);
//		}
//	}
	private Vector3 idlePoint = new Vector3(0,0,0);

	//private Vector3 boomPoint = new Vector3(100,100,100);



	public StatePosition monsterState;

//	[SerializeField]public Vector3[] pointVector;
//	[SerializeField]public Vector3 transitionVector;	
	public Vector3[] PointVector{
		get {return pointVector; }
		set{pointVector = value; }
	}
//	public void Pattern(StatePosition state){
//		switch (state)
//		{
//		case StatePosition.Idle:
//			{
//				//this.transform.Translate(idlePoint * Time.deltaTime, 0);
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
//				animator.SetInteger("State", 2);
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
//
//				//				MonsterArrayEraser(this.gameObject);
//				break;
//			}
//		case StatePosition.BossOneHandAttack:
//			{
//				animator.SetInteger ("State",3);
//				break;
//			}
//		case StatePosition.BossJumpAttack:
//			{
//				animator.SetInteger ("State",4);
//				break;
//			}
//
//		}
//	}
//





	//animation Set; move;




	public void middleBossPositionGetting(Vector3 _Position){
		boomObjectPosition = _Position;
	}


	public void MonSterPatternUpdateConduct(bool NormalMode){
		if (NormalMode) {
			StartCoroutine (PatternNormalChange ());
		} else if (!NormalMode) {
			StartCoroutine (PatternDefenceChange ());
		}
	}

//	public override void MonsterMoveAI(bool _normalMode){
//		StartCoroutine (MonsterNormalMoveAI ());
//	}

	public IEnumerator MonsterNormalMoveAI(){
		while (true) {
			if(IsAlive){
				if (targetPlayer != null) {
					if (Mathf.Abs(targetPlayer.transform.position.z-transform.position.z) >8 || Mathf.Abs(targetPlayer.transform.position.x-this.gameObject.transform.position.x) > 0.6f )
					//if (currentDisTance > searchRange * 0.3f) 
					{
						randomStandby = Random.Range(0,3);
						if (randomStandby == 0) {
							//for 문 -> Fuck go;
							if (checkDirection.z>0) {
								movePoint = new Vector3 (checkDirection.x, 0, checkDirection.z-3f);
								yield return new WaitForSeconds (2f);
								movePoint = new Vector3 (checkDirection.x, 0, checkDirection.z-3f);
								yield return new WaitForSeconds (2f);
								movePoint = new Vector3 (checkDirection.x, 0, checkDirection.z-3f);
								yield return new WaitForSeconds (2f);
								movePoint = new Vector3 (checkDirection.x, 0, checkDirection.z-3f);
								yield return new WaitForSeconds (2f);
							}
							if (checkDirection.z<0) {
								movePoint = new Vector3 (checkDirection.x, 0, checkDirection.z+3f);
								yield return new WaitForSeconds (2f);
								movePoint = new Vector3 (checkDirection.x, 0, checkDirection.z+3f);
								yield return new WaitForSeconds (2f);
								movePoint = new Vector3 (checkDirection.x, 0, checkDirection.z+3f);
								yield return new WaitForSeconds (2f);
								movePoint = new Vector3 (checkDirection.x, 0, checkDirection.z+3f);
								yield return new WaitForSeconds (2f);
							}
						}

						if (randomStandby == 1) {
							
							int a = Random.Range (0, 4);
							if (a == 0) {
								movePoint = new Vector3 (targetPlayer.transform.position.x-transform.position.x+1.5f, 0, checkDirection.z);
								yield return new WaitForSeconds (2f);
								movePoint = new Vector3 (checkDirection.x, 0, targetPlayer.transform.position.z-transform.position.z-1.5f);
								yield return new WaitForSeconds (2f);
								movePoint = new Vector3 (targetPlayer.transform.position.x-transform.position.x-1.5f, 0, checkDirection.z);
								yield return new WaitForSeconds (2f);
								movePoint = new Vector3 (checkDirection.x, 0, targetPlayer.transform.position.z-transform.position.z+1.5f);
								yield return new WaitForSeconds (2f);
							}
							if (a == 1) {
								movePoint = new Vector3 (targetPlayer.transform.position.x-transform.position.x+1.5f, 0, checkDirection.z);
								yield return new WaitForSeconds (2f);
								movePoint = new Vector3 (checkDirection.x, 0, targetPlayer.transform.position.z-transform.position.z+1.5f);
								yield return new WaitForSeconds (2f);
								movePoint = new Vector3 (targetPlayer.transform.position.x-transform.position.x-1.5f, 0, checkDirection.z);
								yield return new WaitForSeconds (2f);
								movePoint = new Vector3 (checkDirection.x, 0, targetPlayer.transform.position.z-transform.position.z-1.5f);
								yield return new WaitForSeconds (2f);
							}
							if (a == 2) {
								movePoint = new Vector3 (targetPlayer.transform.position.x-transform.position.x-1.5f, 0, checkDirection.z);
								yield return new WaitForSeconds (2f);
								movePoint = new Vector3 (checkDirection.x, 0, targetPlayer.transform.position.z-transform.position.z-1.5f);
								yield return new WaitForSeconds (2f);
								movePoint = new Vector3 (targetPlayer.transform.position.x-transform.position.x+1.5f, 0, checkDirection.z);
								yield return new WaitForSeconds (2f);
								movePoint = new Vector3 (checkDirection.x, 0, targetPlayer.transform.position.z-transform.position.z+1.5f);
								yield return new WaitForSeconds (2f);
							}
							if (a == 3) {
								movePoint = new Vector3 (targetPlayer.transform.position.x-transform.position.x-1.5f, 0, checkDirection.z);
								yield return new WaitForSeconds (2f);
								movePoint = new Vector3 (checkDirection.x, 0, targetPlayer.transform.position.z-transform.position.z+1.5f);
								yield return new WaitForSeconds (2f);
								movePoint = new Vector3 (targetPlayer.transform.position.x-transform.position.x+1.5f, 0, checkDirection.z);
								yield return new WaitForSeconds (2f);
								movePoint = new Vector3 (checkDirection.x, 0, targetPlayer.transform.position.z-transform.position.z-1.5f);
								yield return new WaitForSeconds (2f);
							}
						}
						if (randomStandby == 2) {
							int a = Random.Range (0, 4);
							if (a == 0) {
								movePoint = new Vector3 (checkDirection.x, 0, -checkDirection.z);
								yield return new WaitForSeconds (2f);
								movePoint = new Vector3 (-checkDirection.x, 0, -checkDirection.z);
								yield return new WaitForSeconds (2f);
								movePoint = new Vector3 (checkDirection.x, 0, -checkDirection.z);
								yield return new WaitForSeconds (2f);
								movePoint = new Vector3 (-checkDirection.x, 0, -checkDirection.z);
								yield return new WaitForSeconds (2f);
							}
							if (a == 1) {
								movePoint = new Vector3 (0, 0, -checkDirection.z);
								yield return new WaitForSeconds (2f);
								movePoint = new Vector3 (0, 0, -checkDirection.z);
								yield return new WaitForSeconds (2f);
								movePoint = new Vector3 (checkDirection.x, 0, -checkDirection.z);
								yield return new WaitForSeconds (2f);
								movePoint = new Vector3 (-checkDirection.x, 0, -checkDirection.z);
								yield return new WaitForSeconds (2f);
							}
							if (a == 2) {
								movePoint = new Vector3 (checkDirection.x, 0, -checkDirection.z);
								yield return new WaitForSeconds (2f);
								movePoint = new Vector3 (-checkDirection.x, 0, -checkDirection.z);
								yield return new WaitForSeconds (2f);
								movePoint = new Vector3 (0, 0, -checkDirection.z);
								yield return new WaitForSeconds (2f);
								movePoint = new Vector3 (0, 0, -checkDirection.z);
								yield return new WaitForSeconds (2f);
							}
							if (a == 3) {
								movePoint = new Vector3 (-checkDirection.x, 0, -checkDirection.z);
								yield return new WaitForSeconds (1f);
								movePoint = new Vector3 (checkDirection.x, 0, -checkDirection.z);
								yield return new WaitForSeconds (1f);
								movePoint = new Vector3 (0, 0, -checkDirection.z);
								yield return new WaitForSeconds (1f);
								movePoint = new Vector3 (0, 0, -checkDirection.z);
								yield return new WaitForSeconds (1f);
							}
						}
						yield return new WaitForSeconds (0.2f);
					} else if(Mathf.Abs(targetPlayer.transform.position.z-transform.position.z) <8 && Mathf.Abs(targetPlayer.transform.position.x-this.gameObject.transform.position.x) <= 0.6f ) {
						aniState = this.animator.GetCurrentAnimatorStateInfo (0);
						if (!aniState.IsName ("Attack")) {
							if (moveAble) {
								if (checkDirection.z > 0) {
									if (checkDirection.x > 0) {
										movePoint = new Vector3 (-checkDirection.x, 0, 0);
									}
									if (checkDirection.x < 0) {
										movePoint = new Vector3 (checkDirection.x, 0, 0);
									}
								}
								if (checkDirection.z <= 0) {
									if (checkDirection.x > 0) {
										movePoint = new Vector3 (-checkDirection.x, 0, 0);
									}
									if (checkDirection.x < 0) {
										movePoint = new Vector3 (checkDirection.x, 0, 0);
									}
								}
							}
						}
						yield return new WaitForSeconds (2f);
					}
				} else
				yield return new WaitForSeconds (3f);
		}
			else if(!IsAlive){

				yield return false;
			}

		}
	}








	//name change  -> MonsterNormalActAI;
	public IEnumerator PatternNormalChange(){
		while(IsAlive){
			if (targetPlayer != null) {	
				currentDisTance = Vector3.Distance (targetPlayer.transform.position, this.gameObject.transform.position);
				checkDirection = targetPlayer.transform.position - this.gameObject.transform.position;

				if (Mathf.Abs(targetPlayer.transform.position.z-transform.position.z) <8 && Mathf.Abs(targetPlayer.transform.position.x-this.gameObject.transform.position.x) <= 0.6f ){
					if (!isAttack) {
						isAttack = true;
						moveAble = false;
					}
					monsterState = StatePosition.Attack;
					if (checkDirection.z > 0) {
						LookAtPattern (right);
					}
					if (checkDirection.z < 0) {
						LookAtPattern (left);
					}
					Pattern (monsterState);
					yield return new WaitForSeconds (0.5f);

				}

					else if(currentDisTance > searchRange){
						
							moveAble = false;
							isAttack = false;
							monsterState = StatePosition.Idle;
							Pattern (monsterState);
						
					}
					else 
				{
						moveAble = true;
						isAttack = false;
						monsterState = StatePosition.Run;
						Pattern (monsterState);
				}

			}
			yield return new WaitForSeconds(0.2f);
		}
	}
	public IEnumerator PatternDefenceChange(){
		while(IsAlive){
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
					transform.Translate (boomObjectPosition*Time.deltaTime);
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

		aniState = this.animator.GetCurrentAnimatorStateInfo (0);

		if (aniState.IsName ("Run")) {
			if (moveAble) {
				//this.transform.Translate (transitionVector * moveSpeed * 0.5f * Time.deltaTime);
			}
		}

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
						LookAtPattern (right);
						Pattern (StatePosition.Run);
						Debug.Log ("Run");
					}
				}
				if (currentDisTance < searchRange * 0.2f)
				{
					attackCycle += Time.deltaTime;
					if (attackCycle > 5) {
						if (!isAttack) {
							isAttack = true;
							Pattern (StatePosition.Attack);
							attackCycle = 0;
						}
					}
				}
			}
			if (currentDisTance >= middleBossToMonsterLimitDistanceMonsterToCenter*1.5f) {
				LookAtPattern (right);
				IsHited = false;
				targetPlayer = null;
				transform.Translate (boomObjectPosition*Time.deltaTime);
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
		if (monsterState == StatePosition.Death) {
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
