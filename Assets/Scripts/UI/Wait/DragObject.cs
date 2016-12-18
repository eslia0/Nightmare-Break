using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour {

    public void DragEvent()
    {
        transform.position = Input.mousePosition;
        //StartCoroutine(Drag());
    }
}
