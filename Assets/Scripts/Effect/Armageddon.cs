using UnityEngine;
using System.Collections;

public class Armageddon : MonoBehaviour 
{

	public ParticleSystem FireBallparticleSystem;
	public CharacterManager charManager;
	public GameObject character;
	public int armageddonDamage;
	public float FireBallSpeed;
	public Rigidbody FireBallRigid;
    private ParticleSystem ps;
	AudioSource armageddonSound;
	public AudioClip armageddonClipSound;


	public GameObject armageddonImpact;
	public GameObject armageddonExplosion;
	public GameObject meteorExplosion;
	public GameObject meteorCenter;
	public GameObject meteorCreatorHurt;
	public GameObject armageddonSmallExplosion;
	public BoxCollider armageddonBox;
	int skillLv;
	public Animator armageddonAni;
	public ArmageddonPiece[] armageddonPiece;
	public bool pieceAttack;

	public Rigidbody meteorCenterRigd;
	public Rigidbody meteorCreatorHurtRigd;
	public Material meteorMaterial;
	public float meteorAlpha;


    void Start()
    {
		meteorMaterial.SetFloat ("_Alpha", 1);

		character = GameObject.FindWithTag ("Player");
		charManager = character.GetComponent<CharacterManager> ();
        ps = GetComponent<ParticleSystem>();
		FireBallRigid = GetComponent<Rigidbody> ();
		armageddonAni = GetComponent<Animator> ();
		armageddonBox = this.GetComponent<BoxCollider> ();
		armageddonSound =this.gameObject.GetComponent<AudioSource> ();
		armageddonClipSound = Resources.Load<AudioClip> ("Sound/MageEffectSound/DropArmageddon");
		FireBallSpeed = 60f;
		pieceAttack = false;
		armageddonPiece = this.gameObject.GetComponentsInChildren<ArmageddonPiece> ();
		FireBallRigid.velocity =((transform.forward - transform.up) * FireBallSpeed);
		skillLv = charManager.CharacterStatus.SkillLevel [4];
		//armageddonDamage =(int) ((SkillManager.instance.SkillData.GetSkill ((int)charStatus.HClass, 4).GetSkillData (skillLv).SkillValue)*  charStatus.Attack);
		armageddonDamage = 100;

		meteorAlpha = 1f;

		armageddonSound.PlayOneShot (armageddonClipSound);
		StartCoroutine (CheckGround());

	}
	IEnumerator CheckGround()
	{
		while (true)
		{
			if (this.gameObject.transform.position.y < 0.3)
			{
				FireBallRigid.velocity =((transform.forward - transform.up) * 0);
				meteorCenter = Instantiate (Resources.Load<GameObject> ("Effect/MeteorBreakGround"), new Vector3 (this.transform.position.x, this.transform.position.y-0.2f , this.transform.position.z), Quaternion.identity)as GameObject;
				meteorCreatorHurt =Instantiate (Resources.Load<GameObject> ("Effect/MeteoBreakGroundHurt"), new Vector3 (this.transform.position.x, this.transform.position.y-0.8f , this.transform.position.z), Quaternion.Euler(-90f, 0f, 0f))as GameObject;
				Debug.Log ("메테오쨩 : "+meteorCreatorHurt.transform.rotation);
				//	meteorCreatorHurt.transform.rotation = new Quaternion (-45, 0, 0, 0);
				meteorCreatorHurtRigd = meteorCreatorHurt.GetComponent<Rigidbody> ();
				meteorCenterRigd = meteorCenter.GetComponent<Rigidbody> ();

				break;
			}
			yield return new WaitForSeconds (0.001f);
		}
	}

	public void Destroy()
	{
		for (int i = 0; i < armageddonPiece.Length; i++)
		{
			armageddonPiece [i].PieceAttack();
		}
		armageddonAni.SetBool ("Strike",true);

		armageddonImpact = Instantiate(Resources.Load<GameObject>("Effect/ArmageddonImpact"), new Vector3 (this.transform.position.x, this.transform.position.y-1f , this.transform.position.z), Quaternion.identity) as GameObject;
		armageddonExplosion =Instantiate (Resources.Load<GameObject> ("Effect/ArmageddonExplosion"), transform.position, Quaternion.identity)as GameObject;
		armageddonSmallExplosion = Instantiate (Resources.Load<GameObject> ("Effect/ArmageddonSmallExplosion"), transform.position, Quaternion.identity)as GameObject;
		meteorExplosion = Instantiate (Resources.Load<GameObject> ("Effect/ArmageddonExplosion"), transform.position, Quaternion.identity)as GameObject;

		//		meteorCreatorHurtRigd.useGravity = true;
		//		meteorCenterRigd.useGravity = true;
		//
		StartCoroutine (meteorCreatorHurtRigdGravity());
		StartCoroutine ( MeteorAlpha());

		Destroy (armageddonImpact, 0.4f);
		Destroy (armageddonExplosion, 0.4f);
		Destroy (meteorExplosion, 0.4f);
		Destroy (armageddonSmallExplosion, 0.4f);
		Destroy (meteorCreatorHurt, 2f);
		Destroy (meteorCenter, 2f);
		armageddonBox.enabled = false;

		//	FireBallRigid.velocity = (-this.transform.up)*3f;
		Destroy (this.gameObject, 3f);
	}
	IEnumerator meteorCreatorHurtRigdGravity()
	{
		yield return new WaitForSeconds (0.2f);
		meteorCreatorHurtRigd.useGravity = true;
		FireBallRigid.useGravity = true;
		meteorCenterRigd.useGravity = true;
	}

	IEnumerator MeteorAlpha()
	{
		while (true)
		{
			meteorAlpha -= 0.1f;
			meteorMaterial.SetFloat ("_Alpha", meteorAlpha);

			yield return new WaitForSeconds (0.1f);

		}
	}


	void OnTriggerEnter(Collider coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer ("Map"))
		{
			Debug.Log ("in");
		}
		if (coll.gameObject.layer == LayerMask.NameToLayer ("Enermy"))
		{
			Debug.Log ("in Armageddon");
			Monster monsterDamage = coll.gameObject.GetComponent<Monster> ();

			if (monsterDamage != null)
			{	
				Debug.Log (character);
				monsterDamage.HitDamage (armageddonDamage);
				armageddonDamage = 0;

			}
		}

	}


}
