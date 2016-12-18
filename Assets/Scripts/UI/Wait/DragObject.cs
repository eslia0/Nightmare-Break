using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour {

    public GameObject uiObject;
    public bool onDrag;

    IEnumerator Drag()
    {
        while(onDrag)
        {
            uiObject.transform.position = Input.mousePosition * Time.deltaTime * 10f;
        yield return null;
        }

    }

    public void DragEvent()
    {
        onDrag = true;
        StartCoroutine(Drag());
    }

    public void DragOutEvnet()
    {
        onDrag = false;
    }
}
