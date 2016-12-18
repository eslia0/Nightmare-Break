using UnityEngine;
using System.Collections;

public class SwordCircle : MonoBehaviour 
{
	public Renderer circle;
	public Renderer circleChild;
	public Material a;
	public float alpha;
	public float alphaChild;
	// Use this for initialization
	void Start () 
	{
		circle = this.GetComponent<Renderer> ();
		alpha = 1;
		alphaChild = 0.3f;
		StartCoroutine (EffectFadeOut());
		//Destroy (this.gameObject, 4f);
	}
	IEnumerator	EffectFadeOut ()
	{
		yield return new WaitForSeconds (3f);
		while (true)
		{
			alpha -= 0.1f;
			alphaChild -= 0.1f;
			circle.material.color = new Color (1f, 1f, 1f, alpha);
			//circleChild.material.color = new Color (1f, 1f, 1f, (alpha-0.6f));
			circleChild.material.SetColor ("_TintColor",new Color(1f,1f,1f, (alphaChild)));
			yield return new WaitForSeconds (0.1f);
		}
	}

}
