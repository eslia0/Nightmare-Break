using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DragTest : MonoBehaviour {

    public Image testImage;
    public bool isDrag;

    public void CreateRay()
    {
        Debug.Log("클릭함ㅋ");
        RaycastHit rayHit = new RaycastHit();
        Ray ray = new Ray();
        Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray,out rayHit))
        {
            Debug.Log(rayHit.point);
        }
    }

    void Update()
    {
        CreateRay();
    }

    IEnumerator DragEvent()
    {
        isDrag = true;
        while (isDrag)
        {
            testImage.rectTransform.position = Input.mousePosition * Time.deltaTime;
            yield return null;
        }
    }

    public void InDrag()
    {
        StartCoroutine(DragEvent());
    }

    public void OutDrag()
    {
        isDrag = false;
    }


}
