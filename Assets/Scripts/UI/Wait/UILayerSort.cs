using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UILayerSort : MonoBehaviour {

    public enum LayerSort
    {
        firstSort,
        lastSort,
        selectSort
    }
    public int sortIndex;
    public LayerSort layerSort;

    void Start()
    {
        switch (layerSort) {
            case LayerSort.firstSort:
                break;
            case LayerSort.lastSort:
                transform.SetAsLastSibling();
                break;
            case LayerSort.selectSort:
                transform.SetSiblingIndex(sortIndex);
                break;
        }
    }

}
