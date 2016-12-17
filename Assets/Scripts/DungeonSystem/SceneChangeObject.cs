using UnityEngine;
using System.Collections;

public class SceneChangeObject : MonoBehaviour {
	public DungeonManager dungeonManager;
	public GameObject[] Players;
	public int lookAtSceneNumber;
	public GameObject sceneObject;
	public int ColliderEnterCount;


//	public void SceneChangeObjectSet(GameObject[] _Players, int playerLength){
//		Players = new GameObject[playerLength];
//		Players = _Players;
//		sceneObject = this.gameObject;
//		ColliderEnterCount = 0;
//		dungeonManager = transform.GetComponentInParent<DungeonManager>();
//	}

	public void SceneChangeObjectSet(int _number){
		dungeonManager = transform.parent.GetComponent<DungeonManager>();
		//Players = new GameObject[playerLength];
		//Players = _Players;
		sceneObject = this.gameObject;
		ColliderEnterCount = 0;
		lookAtSceneNumber = _number;

		sceneObject.SetActive (false);
	}




	
	public void OnTriggerEnter(Collider coll){
		if (coll.gameObject.layer == LayerMask.NameToLayer ("Player")) {
			ColliderEnterCount+=1;

			if (lookAtSceneNumber != 4) {
				if (ColliderEnterCount == 4) {
					dungeonManager.SceneChange ();
				}
			}
		}
	}

	public void OnTriggerExit(Collider coll){
		if(coll.gameObject.layer == LayerMask.NameToLayer("Player")){
			ColliderEnterCount -= 1;
		}
	}
}
