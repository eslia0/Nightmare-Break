using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterSelectUI : MonoBehaviour {
   
    private const int maxImage = 3;
  
    private int checkNum = 0;
    [SerializeField]
    private Button createButton;
    [SerializeField]
    private Button deleteButton;
    [SerializeField]
    private Button startButton;
	[SerializeField]
	private Image[] backImage;
	[SerializeField]
	private GameObject[] selectImage;
	[SerializeField]
	private GameObject[] characterPos;
	[SerializeField]
	private Animator[] characterAnim;
    private Color[] alphaChange = new Color[2];
 

    void Start()
    {
		backImage = new Image[maxImage];
		selectImage = new GameObject[maxImage];
		characterPos = new GameObject[maxImage];
		characterAnim = new Animator[maxImage];

        alphaChange[0] = new Color(0, 0, 0, 0);
        alphaChange[1] = new Color(0, 0, 0, 1);
        createButton = GameObject.Find("CreateButton").GetComponent<Button>();
        deleteButton = GameObject.Find("DeleteButton").GetComponent<Button>();
        startButton = GameObject.Find("StartButton").GetComponent<Button>();
		for(int i = 0; i < maxImage; i++)
        {
            backImage[i] = GameObject.Find("BackImage" + (i + 1)).GetComponent<Image>();
            selectImage[i] = GameObject.Find("SelectEdge" + (i + 1));
            characterPos[i] = GameObject.Find("Pos" + (i+1));
			if (characterPos [i].transform.GetChild (1)) {
				characterAnim [i] = characterPos [i].transform.GetChild (1).GetComponent<Animator> ();
                characterAnim[i].speed = 0;
			}
            selectImage[i].SetActive(false);
        }
    }
    
    public void Select(int _imageindex)
    {
        checkNum = _imageindex;
        for(int i = 0; i < maxImage; i++)
        {
            if (selectImage[i].activeSelf)
            {
                selectImage[i].SetActive(false);
				characterAnim [i].SetBool("Select", false);
                backImage[i].color = alphaChange[1];
                characterAnim[i].speed = 0;
            }
        }
        if(!selectImage[_imageindex].activeSelf)
        {
            backImage[_imageindex].color = alphaChange[0];
            characterPos[_imageindex].SetActive(true);
            selectImage[_imageindex].SetActive(true);
			characterAnim [_imageindex].SetBool ("Select", true);
            characterAnim[_imageindex].speed = 1;
            startButton.interactable = true;
            deleteButton.interactable = true;
        } else
        {
            return;
        }
    }
}
