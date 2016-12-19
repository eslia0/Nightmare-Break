using UnityEngine;
using System.Collections;

public class Magic_Meteor : MonoBehaviour {

	public GameObject meteorEffect;

	private float timeInterval;
    Vector3 pos;

    void Start()
	{
		timeInterval = 0.1f;
    
        StartCoroutine(meteorBurst());
	}

	IEnumerator meteorBurst()
	{
        GameObject[] meteor = new GameObject[15];
      //  while (currentTime < time) {
            for (int i = 0; i < meteor.Length; i++)
            {
            pos = new Vector3(transform.parent.position.x + Random.Range(-3, 3), transform.parent.position.y + Random.Range(-3, 3), transform.parent.position.z);
			meteor[i] = Instantiate(meteorEffect, pos, transform.rotation) as GameObject;
                yield return new WaitForSeconds(timeInterval);
            }
          //  yield return new WaitForSeconds(0.5f);
          //  currentTime += 0.5f;
    //    }
       // currentTime = 0;
        Destroy(transform.parent.gameObject);

	}
}
