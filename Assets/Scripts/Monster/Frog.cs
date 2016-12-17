using UnityEngine;
using System.Collections;


public class Frog : Monster {
//	private float searchRange = 8.0f;


//	public float currentDisTance;
	float middleBossToMonsterLimitDistanceMonsterToCenter = 6.0f;
//	private float middleBossToMonsterMinDistance = 1.5f;


	[SerializeField]GameObject middleboss;
	private Vector3 middleBossAroundPosition;


//	public override void HitDamage(int _Damage,GameObject attacker)
//	{
//		IsHited = true;
//		currentHP -= _Damage;
//		//		if(monsterState != StatePosition.Boom){
//		//			if (currentHP > 0) {
//		//				for (int i = 0; i < player.Length; i++) {
//		//					if (player [i] == attacker) {
//		//						playerToMonsterDamage [i] += _Damage;
//		//						targetPlayer = player [i];
//		//					}
//		//				}
//		//				Pattern (StatePosition.TakeDamage);
//		//			}
//		//		}
//		if (currentHP <= 0) {
//			currentHP = 0;
//			IsAlive = false;
//			HittedBox.enabled = false;
//			monsterState = StatePosition.Death;
//			Pattern (monsterState);
//		}
//	}

	private Vector3 idlePoint = new Vector3(0,0,0);





	public StatePosition monsterState;

//	[SerializeField]public Vector3[] pointVector;
//	[SerializeField]public Vector3 transitionVector;	







	//animation Set; move;


	public void BoomStart(){
		StartCoroutine (BoomCoroutine ());
	}
	IEnumerator BoomCoroutine() {
		//Instantiate (Resources.Load ("Effect/ !@"), transform.position);
		IsAlive = false;
		yield return new WaitForSeconds (3f);
		StopCoroutine (BoomCoroutine());
	}

//	public void AttackProcess(bool isAttack){
//		if (isAttack) {
//
//			if(animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Run")){
//				animator.SetInteger ("State", 0);
//			}
//			if (animator.GetCurrentAnimatorStateInfo (0).IsName ("Base Layer.Idle")) {
//				animator.SetInteger ("State", 3);
//			}
//			if (animator.GetCurrentAnimatorStateInfo (0).IsName ("Base Layer.Attack")) {
//				moveAble = false;
//			}
//		}
//	}

	public void middleBossPositionGetting(Vector3 _Position){
		middleBossAroundPosition = _Position;
	}


	public void MonSterPatternUpdateConduct(bool NormalMode){
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
//			if (IsAlive) {
//				//if(moveAble){
//				if (targetPlayer != null) {
//					monsterRunAttackAround = Random.Range (0, 3);
//					if (monsterRunAttackAround == 0) {
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
//					if (monsterRunAttackAround == 1) {
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
//						} else if (i == 1) {
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
//					if (monsterRunAttackAround == 2) {
//						movePoint = new Vector3 (checkDirection.x, 0, checkDirection.z);
//						int k = Random.Range (0, 1);
//						if (k == 0) {
//							movePoint = new Vector3 (-checkDirection.x, 0, -checkDirection.z);
//							yield return new WaitForSeconds (2f);
//							movePoint = new Vector3 (checkDirection.x, 0, -checkDirection.z);
//							yield return new WaitForSeconds (2f);
//							movePoint = new Vector3 (checkDirection.x, 0, -checkDirection.z);
//							yield return new WaitForSeconds (2f);
//							movePoint = new Vector3 (-checkDirection.x, 0, -checkDirection.z);
//							yield return new WaitForSeconds (2f);
//						}
//						if (k == 1) {
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
//				}
//				//}
//
//
//
//
//				else
//					yield return new WaitForSeconds (2f);
//			}
//			else if(!IsAlive){
//
//				yield return false;
//			}
//		}
//	}


	public IEnumerator PatternNormalChange(){
		while (IsAlive) {
			if (targetPlayer != null) {	
				currentDisTance = Vector3.Distance (targetPlayer.transform.position, this.gameObject.transform.position);
				checkDirection = targetPlayer.transform.position - this.gameObject.transform.position;
				if (currentDisTance > searchRange) {
					monsterState = StatePosition.Idle; 
					Pattern (monsterState);
					SendMonsterState (monsterState, isAttack, moveAble, movePoint, targetPlayer);
				}
				//if this object get Attackmotion pattern(stateposition.boom -> attack), and this monsterlife is 20%, boomPattern start;
				else if (currentDisTance <= searchRange) {
					//					movePoint = new Vector3(checkDirection.x,0,checkDirection.z);
					{

						if (currentDisTance > searchRange * 0.2f) {
							moveAble = true;
							isAttack = false;
							monsterState = StatePosition.Run;
							Pattern (monsterState);
							SendMonsterState (monsterState, isAttack, moveAble, movePoint, targetPlayer);
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
			}
			yield return new WaitForSeconds (0.2f);
		}
	}

	public void UpdateNormalMode()
	{
		aniState = this.animator.GetCurrentAnimatorStateInfo (0);

		if (aniState.IsName ("Run")) 
		{
			if (moveAble) 
			{
				//if (Mathf.Abs (transform.position.x) < 6 || Mathf.Abs(transform.position.z)<30) {
				this.transform.Translate (movePoint.normalized * moveSpeed * Time.deltaTime, 0);
				//}
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
					transform.Translate (middleBossAroundPosition*Time.deltaTime);
				}
			}
		}
		yield return new WaitForSeconds (0.2f);
	}

	public void defenceMode(){
		aniState = this.animator.GetCurrentAnimatorStateInfo (0);
		if (aniState.IsName ("Run")) {
			if (moveAble) {
				if (!IsHited) {
					//this.transform.Translate (transitionVector * moveSpeed * Time.deltaTime,0);
				}
				if (IsHited) {
					this.transform.Translate (movePoint.normalized * moveSpeed * Time.deltaTime,0);
				}
			}
		}
		if (aniState.IsName ("Attack")) {
			LookAtPattern (right);
		}

		if (transform.position.z > 60) {
			Destroy (this.gameObject);
		}

	}

	public void UpdateDefenceMode(){

		aniState = this.animator.GetCurrentAnimatorStateInfo (0);

		if (aniState.IsName ("Run")) {
			if (moveAble) {
				LookAtPattern (right);
				//this.transform.Translate (transitionVector * moveSpeed * 0.5f * Time.deltaTime);
			}
		}
		//Pattern (monsterState);



//		if (!IsHited) {
//			transform.Translate (transitionVector * moveSpeed * 0.5f * Time.deltaTime);
//		}
//		if (IsHited) {
//			if (checkDirection.z > 0) {
//				LookAtPattern (right);
//			}
//			if (checkDirection.z <= 0) {
//				LookAtPattern (left);
//			}
//
//			currentDisTance = Vector3.Distance(targetPlayer.transform.position, this.gameObject.transform.position);
//			checkDirection = targetPlayer.transform.position - this.gameObject.transform.position;
//
//			if (currentDisTance < middleBossToMonsterLimitDistanceMonsterToCenter*1.5f) {
//				movePoint = new Vector3 (checkDirection.x, 0, checkDirection.z);
//				transform.Translate(movePoint.normalized * moveSpeed * Time.deltaTime, 0);
//				if (currentDisTance >= searchRange * 0.2f)
//				{
//					if (moveAble) {
//						Pattern (StatePosition.Run);
//						Debug.Log ("Run");
//					}
//				}
//				if (currentDisTance < searchRange * 0.2f)
//				{
//					attackCycle += Time.deltaTime;
//					if (attackCycle > 5) {
//						attackCycle = 0;
//						if (!isAttack) {
//							isAttack = true;
//							Pattern (StatePosition.Attack);
//						}
//					}
//				}
//			}
//			if (currentDisTance >= middleBossToMonsterLimitDistanceMonsterToCenter*1.5f) {
//				LookAtPattern (right);
//				IsHited = false;
//				targetPlayer = null;
//				transform.Translate (boomObjectPosition*Time.deltaTime);
//			}
//		}
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

		if (monsterState == StatePosition.Attack) {
			monsterState = _state;
		}
		if (monsterState == StatePosition.TakeDamage) {
			monsterState = _state;
		}
		if (monsterState == StatePosition.Idle) {
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