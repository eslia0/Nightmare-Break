using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {
	public GameObject wall;

	public bool normalMode;
	public Vector3 getV3;

	void Start(){
		wall = this.gameObject;
	}

	public void StartWallSet(){
		wall = this.gameObject;
	}


	void OnTriggerEnter(Collider coll){
		if (coll.gameObject.layer == LayerMask.NameToLayer ("Enermy")) {
			
			//coll.gameObject.GetComponent<Monster> ().RandomStandby = 1;
		}
	}

	void OnTriggerExit(Collider coll){
	}
}
