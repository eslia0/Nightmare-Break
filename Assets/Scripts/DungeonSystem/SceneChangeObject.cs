using UnityEngine;
using System.Collections;

public class SceneChangeObject : MonoBehaviour {
	public GameObject[] Players;
	public GameObject sceneObject;


	void Start(){
		
	}

	public void SceneChangeObjectSet(int _number){
		//Players = new GameObject[playerLength];
		//Players = _Players;
		sceneObject = this.gameObject;
		Players = GameObject.FindGameObjectsWithTag ("Player");
		sceneObject = this.gameObject;
		sceneObject.SetActive (false);
	}

	public void SceneChangeObjectSetFalse(){
		this.gameObject.SetActive (false);
	}

	public void SceneChangeObjectSettrue(){
		this.gameObject.SetActive (true);
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
