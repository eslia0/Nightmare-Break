using UnityEngine;
using System.Collections;

public class StagePortal : MonoBehaviour {
	public GameObject[] players;
	public GameObject sceneObject;
	public bool[] inPlayer;
    bool playerCheck;
	int playerCount;

    public void InitializePortal()
    {
        Debug.Log("포탈 초기화");
        players = DungeonManager.Instance.Players;
		inPlayer = new bool[players.Length];
        playerCount = 0;
        playerCheck = true;
        StagePortalDeactivate();
    }

	public void StagePortalDeactivate(){
		gameObject.SetActive (false);
	}

	public void StagePortalActivate(){
		gameObject.SetActive (true);
	}

	public void OnTriggerEnter(Collider coll)
    {
        if (playerCheck)
        {
            if (coll.gameObject.layer == LayerMask.NameToLayer("Player"))
            {

                for (int i = 0; i < players.Length; i++)
                {
                    if (coll.gameObject == players[i])
                    {
                        if (!inPlayer[i])
                        {
                            inPlayer[i] = true;
                            playerCount++;
                        }
                    }
                }

                if (playerCount >= players.Length)
                {
                    if (DungeonManager.Instance.StageNum + 1 >= DungeonManager.Instance.SceneList.Length)
                    {
                        return;
                    }
                    SceneChanger.Instance.SceneChange(DungeonManager.Instance.SceneList[DungeonManager.Instance.StageNum + 1], false);
                }
            }
        }
	}

	public void OnTriggerExit(Collider coll){
		if(coll.gameObject.layer == LayerMask.NameToLayer("Player")){
			for(int i=0 ; i< players.Length; i++){
				if(coll.gameObject == players[i]){
                    if (inPlayer[i])
                    {
                        inPlayer[i] = false;
                        playerCount--;
                    }
				}
			}
		}
	}
}