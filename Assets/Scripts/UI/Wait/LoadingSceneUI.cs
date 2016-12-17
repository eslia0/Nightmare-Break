using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingSceneUI : MonoBehaviour {

	public Image loadingBar;
	private float time;

	public void LoadingProcess(float loadData)
	{

		loadingBar.fillAmount += 1/loadData;
	}


	/*
	public IEnumerator LoadingProcess(float loadData)
	{
		time = Time.smoothDeltaTime;
		loadingBar.fillAmount = 0;
		while (loadingBar.fillAmount < 1) {
			loadingBar.fillAmount += 1 / loadData * time;
			yield return null;
		}
		time = 0;
		loadingBar.fillAmount = 0;
		yield break;
	}
*/
}
