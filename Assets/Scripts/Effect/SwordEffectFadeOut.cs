using UnityEngine;
using System.Collections;

public class SwordEffectFadeOut : MonoBehaviour
{

	public Renderer tempRender;
	public Material a;
	float alpha;
	// Use this for initialization
	void Start ()
	{		
		tempRender = GetComponent<Renderer> ();
		alpha = 1;
		StartCoroutine (EffectFadeOut());
	}

	IEnumerator	EffectFadeOut ()
	{
		while (true)
		{
			alpha -= 0.1f;
	
			tempRender.material.SetColor ("_TintColor",new Color(1f,1f,1f, alpha));
			//a.SetColor ("_Tint Color", new Color(1,1,1,alpha));
			// = new Color(1,1,1,alpha);


			yield return new WaitForSeconds (0.04f);
		}
	}
	
	// Update is called once per frame

}
