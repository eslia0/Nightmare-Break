using UnityEngine;
using System.Collections;

public class SceneChangeObject : MonoBehaviour {
	SceneChanger a;
	public GameObject[] Players;
	public BoxCollider enterBox;
	public GameObject sceneObject;
	public GameObject[] inPlayer;
	int playercount;

	void Start(){
		SceneChangeObjectSet (1);
	}

	public void SceneChangeObjectSet(int _number){
		Players = GameObject.FindGameObjectsWithTag ("Player");
		//Players = new GameObject[playerLength];
		//Players = _Players;
		enterBox = this.gameObject.GetComponent<BoxCollider>();
		sceneObject = this.gameObject;
		inPlayer = new GameObject[Players.Length];
//		sceneObject.SetActive (false);

	}

	public void SceneChangeObjectSetFalse(){
		this.gameObject.SetActive (false);
	}

	public void SceneChangeObjectSettrue(){
		this.gameObject.SetActive (true);
	}



	
	public void OnTriggerEnter(Collider coll){
		if (coll.gameObject.layer == LayerMask.NameToLayer ("Player")) {
			
			for(int i=0 ; i< Players.Length; i++){
				if(coll.gameObject == Players[i]){
					inPlayer [i] = Players [i];
					playercount++;
				}
			}

			if (playercount >= Players.Length) {
				DungeonManager dungeonManager=GameObject.Find("DungeonManager").GetComponent<DungeonManager>();
//				dungeonManager.SceneChange ();
			

			}

		
		}
	}

	public void OnTriggerExit(Collider coll){
		if(coll.gameObject.layer == LayerMask.NameToLayer("Player")){
			for(int i=0 ; i< Players.Length; i++){
				if(coll.gameObject == Players[i]){
					inPlayer [i] = null;
					playercount--;
				}
			}
		}
	}
}
