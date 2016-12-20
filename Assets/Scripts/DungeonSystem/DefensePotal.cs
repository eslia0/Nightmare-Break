using UnityEngine;
using System.Collections;

public class DefensePotal : MonoBehaviour {
	protected GameObject[] monster;
	protected int monsterFrogCount;
	protected int monsterDuckCount;
	protected int monsterRabbitCount;



	public void DefensePotalSetting(){
		monster = GameObject.FindGameObjectsWithTag ("Enermy");
		monsterFrogCount = 0;
		monsterDuckCount = 0;
		monsterRabbitCount = 0;
	}

	public IEnumerator DefenseEnd(){
		yield return new WaitForSeconds (60f);

	}


	public void SetFalse(){

	}




	void OnTriggerEnter(Collider coll){
		if (coll.gameObject.layer == LayerMask.NameToLayer ("Enermy")) {
            try
            {
                if (coll.gameObject.GetComponent<Monster>().MonsterId == MonsterId.Frog)
                {
                    monsterFrogCount++;
                }
                if (coll.gameObject.GetComponent<Monster>().MonsterId == MonsterId.Duck)
                {
                    monsterDuckCount++;
                }
                if (coll.gameObject.GetComponent<Monster>().MonsterId == MonsterId.Rabbit)
                {
                    monsterRabbitCount++;
                }
            }
            catch
            {
                Debug.Log(coll);
            }

            Destroy(coll.gameObject);
        }
	}
}


