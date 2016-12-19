using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class TestUIManager : MonoBehaviour
{
    CharacterManager charManager;
    BattleUIManager battleUIManager;

    public BattleUIManager BattleUIManager { get { return battleUIManager; } }

    void Start()
    {
        SetBattleUIManager();
    }

    public void SetBattleUIManager()
    {
        charManager = GameObject.FindWithTag("Player").GetComponent<CharacterManager>();
        battleUIManager = new BattleUIManager();
        battleUIManager.SetUIObject();
    }

    public void PointEnter(int skillIndex)
    {
        battleUIManager.SetPointEnterUI(skillIndex, 2, (int)GameManager.Instance.CharacterStatus.HClass);
    }

    public void OnPointExit()
    {
        battleUIManager.MouseOverUI.gameObject.transform.parent.gameObject.SetActive(false);
    }



}




