using UnityEngine;
using System.Collections;

public class BossMonster : Monster {

	protected Animator anim;
	private AnimatorStateInfo currentBaseState;

	private float currentDistance;
	private float perceive = 20.0f;

	public enum MonsterState {Idle = 1 , Run, Walk, BigSmash, TwoHandSmash, Roar, UPSmash, DownSmash, Death };//콤보 state

	private MonsterState monsterState;

	public AnimationCurve critical;
	public AnimationCurve hurt;
	public AnimationCurve healthy;

	private float criticalValue;
	private float hurtValue;
	private float healthyValue;

	public GameObject muzzle;
	public GameObject bullet;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		 monsterState = MonsterState.Idle;
		//MonsterSet ();

		NormalchasePlayer ();

		StartCoroutine(this.CheckMonsterState ());
		StartCoroutine(this.MonsterAction ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void FuzzyHPValue(){
		criticalValue = critical.Evaluate (currentHP);
		hurtValue = hurt.Evaluate (currentHP);
		healthyValue = healthy.Evaluate (currentHP);
	}
	IEnumerator CheckMonsterState(){
		while (IsAlive) 
		{
			yield return new WaitForSeconds (0.2f);
			ChasePlayer ();


			//if(VecPlayerToMonster.x ==0){}

			//LookatChange ();//monsterlookatcontrol;
			if (targetPlayer != null) {

				currentDistance = Vector3.Distance (targetPlayer.transform.position, this.gameObject.transform.position);

				checkDirection = targetPlayer.transform.position - this.gameObject.transform.position;

				if (currentDistance > perceive) {
					monsterState = MonsterState.Idle;

				}else if (currentDistance <= perceive) {
					
					movePoint = new Vector3 (checkDirection.x, 0, checkDirection.z);

					if (currentDistance >= (perceive * 0.5f)) {
						//run
						monsterState = MonsterState.Run;

					}else if(currentDistance < (perceive * 0.5f) && currentDistance >= (perceive * 0.2f)){
						
						monsterState = MonsterState.Walk;
						//walk

					}else if(currentDistance < (perceive * 0.2f)) {
						FuzzyHPValue ();

						int attackState = Random.Range (4,9);

						monsterState = (MonsterState)attackState;

					}	
				}

				}
			yield return null;
		}
	}
	IEnumerator MonsterAction(){
		
		while (IsAlive) {
			int bulletCount = 0;

			switch (monsterState) {
			case MonsterState.Idle:
				anim.SetBool ("IsWalk", false);
				anim.SetBool ("IsRun", false);
				anim.SetInteger ("IsCombo", 0);

				break;
			case MonsterState.Run:
				anim.SetBool ("IsWalk", false);
				anim.SetBool ("IsRun", true);
				this.transform.Translate (movePoint*moveSpeed*Time.deltaTime, 0);

				break;
			case MonsterState.Walk:
				anim.SetBool ("IsWalk", true);
				anim.SetBool ("IsRun", false);
				this.transform.Translate (movePoint*moveSpeed/2*Time.deltaTime, 0);

				break;
			case MonsterState.BigSmash:
				anim.SetBool ("IsWalk", false);
				anim.SetBool ("IsRun", false);
				anim.SetInteger ("IsCombo", 1);

				break;
			case MonsterState.TwoHandSmash:
				anim.SetBool ("IsWalk", false);
				anim.SetBool ("IsRun", false);
				anim.SetInteger ("IsCombo", 2);

				break;
			case MonsterState.Roar:
				anim.SetBool ("IsWalk", false);
				anim.SetBool ("IsRun", false);
				anim.SetInteger ("IsCombo", 3);

				break;
			case MonsterState.UPSmash:
				anim.SetBool ("IsWalk", false);
				anim.SetBool ("IsRun", false);
				anim.SetInteger ("IsCombo", 4);

				break;
			case MonsterState.DownSmash:
				anim.SetBool ("IsWalk", false);
				anim.SetBool ("IsRun", false);
				anim.SetInteger ("IsCombo", 5);

				break;
			}

			currentBaseState = anim.GetCurrentAnimatorStateInfo (0);

			if (currentBaseState.IsName ("TwoHandSmash")) {
				yield return new WaitForSeconds (1.0f);
				GameObject.FindGameObjectWithTag ("Floor").GetComponent<EarthQuake> ().Running = true;
				yield return new WaitForSeconds (1.5f);

			} else if (currentBaseState.IsName ("BigSmash")) {
				
				yield return new WaitForSeconds (1.0f);

				if (bulletCount == 0) {
				    
					Instantiate (bullet, muzzle.transform.position, muzzle.transform.rotation);
					bulletCount++;
				}
				yield return new WaitForSeconds (1.0f);
			} else if (currentBaseState.IsName ("Roar")) {
				
				GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<RedRenderImage> ().enabled = true;
				yield return new WaitForSeconds (1.7f);
				GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<RedRenderImage> ().enabled = false;
			} else if (currentBaseState.IsName ("Idle")) {
				changeDirection ();
			} else if (currentBaseState.IsName ("Run")) {
				changeDirection ();
			} else if (currentBaseState.IsName ("Walk")) {
				changeDirection ();
			}
			yield return new WaitForSeconds (0.01f);
		}
	}

	public void changeDirection(){
		Vector3 vecLookPos = targetPlayer.transform.position;
		vecLookPos.y = transform.position.y;
		vecLookPos.x = transform.position.x;

		transform.LookAt (vecLookPos);

	}
	void  OnTriggerEnter(Collider coll){
		//플레이어 공격시 collider 충돌 부분

	}

}
