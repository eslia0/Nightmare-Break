using UnityEngine;
using System.Collections;

public class ParticleDestroy : MonoBehaviour 
{
	private	ParticleSystem particle;

	void Start () 
	{
		particle = GetComponent<ParticleSystem> ();
		StartCoroutine (ParticleCheck ());
	}

	IEnumerator ParticleCheck()
	{
		yield return new WaitForSeconds (particle.duration);
		Destroy(gameObject);	
		if (gameObject.transform.parent != null) {
			Destroy (gameObject.transform.parent.gameObject);
		}
	}
}
