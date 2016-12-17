using UnityEngine;
using System.Collections;

public class TitleSceneEvent : MonoBehaviour {

    public GameObject[] loginObject;
    public GameObject logoObject;
    public bool input;
    public Coroutine inputCheck;

    void Start()
    {
        StartCoroutine(InputCheck());   
    }

   IEnumerator InputCheck()
    {
        while(!input)
        {
            if(Input.anyKeyDown)
            {
                input = true;
                logoObject.SetActive(false);
                for(int i = 0; i < loginObject.Length; i++)
                {
                    loginObject[i].SetActive(true);
                }
            }
            yield return null;
        }
    }

}
