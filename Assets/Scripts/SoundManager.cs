using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	private AudioSource bossAudio;
	public AudioClip bossStartSound;
	public AudioClip bossHowling;
	public AudioClip bossWait;
	public AudioClip jumpAttack;
	public AudioClip bossDeath;
	public AudioClip oneHandAttack;
	public AudioClip normalAttack;
	// Use this for initialization
	void Start () 
	{
		this.bossAudio = this.gameObject.GetComponent<AudioSource> ();

	
	}

	public void BossSound(int _num, float _Range)
	{


		float soundRange = _Range * 0.01f;



		if (soundRange > 0.9f)
		{
			soundRange = 1;
		}

		//bossAudio.volume = soundRange;

		if (_num == 1)
		{
			bossStartSound = Resources.Load<AudioClip> ("Sound/BossStart");
		}
		bossHowling = Resources.Load<AudioClip> ("Sound/Howling");
		bossWait =  Resources.Load<AudioClip> ("Sound/bossWait");
		jumpAttack = Resources.Load<AudioClip> ("Sound/JumpAttack");
		bossDeath = Resources.Load<AudioClip> ("Sound/BossDeath");
		oneHandAttack =  Resources.Load<AudioClip> ("Sound/OneHandAttack");
		normalAttack = Resources.Load<AudioClip> ("Sound/NormalAttack");

		this.bossAudio.clip = this.bossStartSound;
		this.bossAudio.loop = false;
		this.bossAudio.PlayOneShot (bossStartSound);
		
	}

}
