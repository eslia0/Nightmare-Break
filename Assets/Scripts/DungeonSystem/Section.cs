using UnityEngine;
using System.Collections;

public class Section : MonoBehaviour {
	//public DungeonManager DungeonManager.Instance;
//	public Frog[] Frog;
//	public Rabbit[] Rabbit;
//	public Duck[] shockWaveMonster;
//	public GameObject middleBoss1;
//
//	public GameObject goToTheBoss;
//
//	public Vector3[] FrogPosition;
//
//	public Vector3[] pointVector;
//
//	[SerializeField] int monsterCount;
//	[SerializeField]private int gateNumber;
//	public int GateNumber{
//		get { return gateNumber;}
//		set{ gateNumber = value;}
//	}
//	//all monster objects getting;
//
//
//	protected bool modeForm;
//	public bool ModeForm{
//		get {return modeForm;}
//		set {modeForm = value;}
//	}
//
//	public enum MonSterMovePosition
//	{
//		Up = 1,
//		Down,
//		Middle,
//		case4,
//		Comback
//	};
//
//	public void MonsterSet(){
//		goToTheBoss = GameObject.Find ("GoToTheBoss");
////		DungeonManager.Instance = gameObject.transform.parent.GetComponent<DungeonManager>();
//
//		//Frog = gameObject.transform.GetComponentsInChildren<Frog> ();
//		//monsterCount = (Frog.Length + Rabbit.Length+1);
//
//		for (int i = 0; i < Frog.Length; i++) {
////			Frog [i].PlayerSearch ();
////			Frog [i].MonsterSet ();
////			Frog [i].Mode = modeForm;
////			Frog [i].GateArrayNumber = gateNumber;
////			Frog [i].MonsterArrayNumber = i;
//		}
//
//		Frog = transform.GetComponentsInChildren<Frog> ();
//		FrogPosition = new Vector3[Frog.Length];
//		pointVector = new Vector3[7];
//		if (Frog == null) {
//			Debug.LogError ("중간보스 없음!!" + gameObject.name);
//		}
//
//
//		for (int i = 0; i < Frog.Length; i++)
//		{
//			FrogPosition[i] = Frog[i].transform.position;
//			if (i >= 6) {
//				Pattern (MonSterMovePosition.Up);
//				//Frog [i].pointVectorArrayGetting (pointVector);	
//			} else if (i >= 4) {
//				Pattern (MonSterMovePosition.Middle);
//				//Frog [i].pointVectorArrayGetting (pointVector);	
//			} else
//				Pattern (MonSterMovePosition.Down);
//			//Frog [i].pointVectorArrayGetting (pointVector);
//		}
//
//
//
//	}
//
//
//
//
//	public void HostUpdateConduct(){
//		for (int FrogLength = 0; FrogLength < Frog.Length; FrogLength++) {
//			Frog [FrogLength].UpdateDefenceMode ();
//		}
//		for (int RabbitLength = 0; RabbitLength < Rabbit.Length; RabbitLength++) {
//			Rabbit [RabbitLength].UpdateDefenceMode ();
//		}
//		for (int shockWaveMonsterLength = 0; shockWaveMonsterLength < shockWaveMonster.Length; shockWaveMonsterLength++) {
////			shockWaveMonster [shockWaveMonsterLength].UpdateDefenceMode ();
//		}
//	}
//
//	public void GuestUpdateConduct(){
//		for (int FrogLength = 0; FrogLength < Frog.Length; FrogLength++) {
//			//Frog [FrogLength].GuestMonsterUpdate ();
//		}
//		for (int RabbitLength = 0; RabbitLength < Rabbit.Length; RabbitLength++) {
//			//Rabbit [RabbitLength].GuestMonsterUpdate ();
//		}
//		for (int shockWaveMonsterLength = 0; shockWaveMonsterLength < shockWaveMonster.Length; shockWaveMonsterLength++) {
//			//shockWaveMonster [shockWaveMonsterLength].GuestMonsterUpdate ();
//		}
//	}
//
//
//
//	public void Pattern(MonSterMovePosition state)
//	{
//		switch (state) {
//		case MonSterMovePosition.Up:
//			{
//				pointVector [0] = new Vector3 (1, 0, 1);
//				pointVector [1] = new Vector3 (0, 0, 1);
//				pointVector [2] = new Vector3 (-1, 0, 1);
//				pointVector [3] = new Vector3 (-1, 0, 1);
//				pointVector [4] = new Vector3 (0, 0, 1);
//				pointVector [5] = new Vector3 (1, 0, 1);
//				pointVector [6] = new Vector3 (0, 0, 1);
//				break;
//			}
//		case MonSterMovePosition.Down:
//			{
//				pointVector [0] = new Vector3 (0, 0, 1);
//				pointVector [1] = new Vector3 (1, 0, 1);
//				pointVector [2] = new Vector3 (1, 0, 1);
//				pointVector [3] = new Vector3 (-1, 0, 1);
//				pointVector [4] = new Vector3 (-1, 0, 1);
//				pointVector [5] = new Vector3 (1, 0, 1);
//				pointVector [6] = new Vector3 (0, 0, 1);
//				break;
//			}
//		case MonSterMovePosition.Middle:
//			{
//				pointVector [0] = new Vector3 (-1, 0, 1);
//				pointVector [1] = new Vector3 (0, 0, 1);
//				pointVector [2] = new Vector3 (1, 0, 1);
//				pointVector [3] = new Vector3 (1, 0, 1);
//				pointVector [4] = new Vector3 (0, 0, 1);
//				pointVector [5] = new Vector3 (-1, 0, 1);
//				pointVector [6] = new Vector3 (0, 0, 1);
//				break;
//			}
//		
//		}
//	}
//
//
//	public void SetFalse(){
//		this.gameObject.SetActive (false);
//	}
//	public void SetTrue(){
//		this.gameObject.SetActive (true);
//	}
//
//
//	public void RemoveMonsterArray(){
//		monsterCount -= 1;
//
//
////		if (monsterCount == 0) {
////			DungeonManager.Instance.SceneChange ();
////		}
//	}
//
//
}
