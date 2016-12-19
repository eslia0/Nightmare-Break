using UnityEngine;
using System.Collections;

public class SceneChangeObject : MonoBehaviour {
	SceneChanger a;
	public GameObject[] Players;
	public BoxCollider enterBox;
	public GameObject sceneObject;
	public bool[] inPlayer;
	public bool AllPlayerEnter;
	int playercount;



	public void SceneChangeObjectSet(int _number){
		Players = GameObject.FindGameObjectsWithTag ("Player");
		//Players = new GameObject[playerLength];
		//Players = _Players;
		enterBox = this.gameObject.GetComponent<BoxCollider>();
		sceneObject = this.gameObject;
		inPlayer = new bool[Players.Length];
		//      sceneObject.SetActive (false);

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
					inPlayer [i] = true;
					playercount++;
				}
			}

			if (playercount >= Players.Length) {
				AllPlayerEnter = true;

				//DungeonManager dungeonManager=GameObject.Find("DungeonManager").GetComponent<DungeonManager>();
				//      dungeonManager.SceneChange ();
				//씬체인저에 씬체인지하면됨.

			}
		}
	}

	public void OnTriggerExit(Collider coll){
		if(coll.gameObject.layer == LayerMask.NameToLayer("Player")){
			for(int i=0 ; i< Players.Length; i++){
				if(coll.gameObject == Players[i]){
					inPlayer [i] = false;
					playercount--;
					AllPlayerEnter = false;
				}
			}
		}
	}
}