using UnityEngine;
using System.Collections;

public class StagePortal : MonoBehaviour {
	SceneChanger a;
	public GameObject[] Players;
	public BoxCollider enterBox;
	public GameObject sceneObject;
	public bool[] inPlayer;
	int playercount;



	public void SceneChangeObjectSet(int _number){
		Players = GameObject.FindGameObjectsWithTag ("Player");
		enterBox = this.gameObject.GetComponent<BoxCollider>();
		sceneObject = this.gameObject;
		inPlayer = new bool[Players.Length];
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
                    if(!inPlayer[i])
                    {
                        inPlayer[i] = true;
                        playercount++;
                    }
				}
			}

			if (playercount >= Players.Length) {
                SceneChanger.Instance.SceneChange(DungeonManager.Instance.SceneList[DungeonManager.Instance.StageNum], false);

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
                    if (inPlayer[i])
                    {
                        inPlayer[i] = false;
                        playercount--;
                    }
				}
			}
		}
	}
}