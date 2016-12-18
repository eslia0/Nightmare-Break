using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ComboSystem : MonoBehaviour {

    [SerializeField]
    private const float comboCheckTime = 1.5f;
	private Animator comboAnim;
    private Text comboText;

    void Start()
    {
      //  instance = this.gameObject.GetComponent<ComboSystem>();
        comboText = GameObject.Find("ComBoObject").transform.GetChild(0).GetComponent<Text>();
		comboAnim = comboText.transform.parent.GetComponent<Animator>();
        comboText.transform.parent.gameObject.SetActive(false);
    }

    public void ComboProcess(int count)
    {
        if(!comboText.transform.parent.gameObject.activeSelf)
        {
            comboText.transform.parent.gameObject.SetActive(true);
        }
        comboText.text = count.ToString();
		comboAnim.Play ("ComboAnim", -1, 0);
    }

    public void ComboAction(Transform mCamera)
    {
        mCamera.gameObject.transform.position = new Vector3(Random.Range(mCamera.position.x - 0.09f, mCamera.position.x + 0.09f), Random.Range(mCamera.position.y - 0.09f, mCamera.position.y + 0.09f), mCamera.position.z);
       
    }

    public void ComboEnd()
    {
        comboText.transform.parent.gameObject.SetActive(false);
        comboText.text = "";
    }
}
