using UnityEngine;
using System.Collections;

public class SceneChangeObject : MonoBehaviour {
	public GameObject[] Players;
	public int lookAtSceneNumber;
	public GameObject sceneObject;
	public int ColliderEnterCount;


//	public void SceneChangeObjectSet(GameObject[] _Players, int playerLength){
//		Players = new GameObject[playerLength];
//		Players = _Players;
//		sceneObject = this.gameObject;
//		ColliderEnterCount = 0;
//		DungeonManager.Instance = transform.GetComponentInParent<DungeonManager>();
//	}
	void Start(){
		
	}

	public void SceneChangeObjectSet(int _number){
		//Players = new GameObject[playerLength];
		//Players = _Players;
		sceneObject = this.gameObject;
		ColliderEnterCount = 0;
		lookAtSceneNumber = _number;
		Players = GameObject.FindGameObjectsWithTag ("Player");

		sceneObject.SetActive (false);
	}




	
	public void OnTriggerEnter(Collider coll){
		if (coll.gameObject.layer == LayerMask.NameToLayer ("Player")) {
			Debug.Log ("a");
			DungeonManager dungeonManager=GameObject.Find("DungeonManager").GetComponent<DungeonManager>();
			dungeonManager.SceneChange ();

		}
	}

//	public void OnTriggerExit(Collider coll){
//		if(coll.gameObject.layer == LayerMask.NameToLayer("Player")){
//			ColliderEnterCount -= 1;
//		}
//	}
}
