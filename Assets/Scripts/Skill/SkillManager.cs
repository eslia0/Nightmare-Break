using UnityEngine;
using System.Collections;

public class SkillManager : MonoBehaviour {

	private SkillData skillData = new SkillData ();
    public SkillData SkillData{ get { return skillData; } }

    private static SkillManager instance;

    public static SkillManager Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType(typeof(SkillManager)) as SkillManager;

                if (!instance)
                {
                    GameObject container = new GameObject();
                    container.name = "SkillManager";
                    instance = container.AddComponent(typeof(SkillManager)) as SkillManager;   
                }
            }
            return instance;
        }
    }

    void Start()
    {
        instance = this;
        skillData.Initialize();
    }
}

