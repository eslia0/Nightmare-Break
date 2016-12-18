using UnityEngine;
using System.Collections;

public class DefensePotal : MonoBehaviour {
	protected GameObject[] monster;
	protected int monsterFrogCount;
	protected int monsterDuckCount;
	protected int monsterRabbitCount;

	void Start () {
	
	}

	public void DefensePotalSetting(){
		monster = GameObject.FindGameObjectsWithTag ("Enermy");
		monsterFrogCount = 0;
		monsterDuckCount = 0;
		monsterRabbitCount = 0;
	}

	public void DefenseEndSend(){
		//디펜스 끝났다고 전달.
//		DungeonManager dungeonManager;
//		dungeonManager = GameObject.Find ("DungonManager");
//		dungeonManager.
		
	}


	void OnTriggerEnter(Collider coll){
		if (coll.gameObject.layer == LayerMask.NameToLayer ("Enermy")) {
			if(coll.gameObject.GetComponent<Monster>().MonsterId == MonsterId.Frog){
				monsterFrogCount++;
			}
			if (coll.gameObject.GetComponent<Monster> ().MonsterId == MonsterId.Duck) {
				monsterDuckCount++;
			}
			if (coll.gameObject.GetComponent<Monster> ().MonsterId == MonsterId.Rabbit) {
				monsterRabbitCount++;
			}
		}
		
	}
//	public void 


}
