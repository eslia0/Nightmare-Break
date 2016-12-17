using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

public class XmlSkillData : MonoBehaviour {
    
	public string fileName;

    void XmlFileLoad()
    {
        TextAsset baseSkillTextAsset = Resources.Load("Xml" + fileName) as TextAsset;
        XmlDocument xmlSkillData = new XmlDocument();
        xmlSkillData.LoadXml(baseSkillTextAsset.text);

    }

}
