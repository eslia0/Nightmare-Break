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
		
	}


	void OnTriggerEnter(Collider coll){
		if (coll.gameObject.layer == LayerMask.NameToLayer ("Enermy")) {
			//coll.gameObject.GetComponent<Monster> ().RandomStandBy = 0;

			//			if (normalMode) {
//				coll.gameObject.GetComponent<Monster> ().MovePoint = -coll.gameObject.GetComponent<Monster> ().MovePoint;
//			}
//			if (!normalMode) {
//				coll.gameObject.GetComponent<Monster> ().MovePoint = new Vector3(-coll.gameObject.GetComponent<Monster> ().MovePoint.x,coll.gameObject.GetComponent<Monster> ().MovePoint.y,coll.gameObject.GetComponent<Monster> ().MovePoint.z);
//			}

		}
	}

	void OnTriggerExit(Collider coll){
		//coll.gameObject.GetComponent<Monster> ().WallContect = false;
	
	}
}
