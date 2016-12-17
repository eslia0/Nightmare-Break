using UnityEngine;
using System.Collections;

public class ExplosionSound : MonoBehaviour {

	AudioSource explosionSound;
	public AudioClip meteorDestroySound;
	// Use this for initialization
	void Start () 
	{
		explosionSound = this.gameObject.GetComponent<AudioSource> ();
		meteorDestroySound = Resources.Load<AudioClip> ("Sound/MageEffectSound/DestroyEffectSound");
		explosionSound.PlayOneShot (meteorDestroySound);
	}
}
