using UnityEngine;
using System.Collections;

public class SkillManager : MonoBehaviour {

	private SkillData skillData = new SkillData ();
    public SkillData SkillData  { get { return skillData; } }

	public static SkillManager instance = null;

	void Start()
	{
		if (instance) {
			Destroy (gameObject);
			return;
		} else {
			instance = this;
		}
		skillData.Initialize ();
	} 

}

