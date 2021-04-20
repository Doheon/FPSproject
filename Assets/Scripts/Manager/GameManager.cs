using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance
    {
        get
        {
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (m_instance == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아 할당
                m_instance = FindObjectOfType<GameManager>();
            }

            // 싱글톤 오브젝트를 반환
            return m_instance;
        }
    }

    private static GameManager m_instance;

    public GameObject[] players;

    public event Action PlayerLevelUp;
    public Action StageClear;

    public int playerLevel;
    public int maxLevel;
    public float maxExp{
        get{
            return playerLevel * 100f + 500f;
        }
    }
    public float curExp;


    public int mainStage = 0;
    public int subStage = 0;
    public int enemyLeft = 0;


    public bool isGameover; // 게임 오버 상태
    public GameObject gameOverTab;

    public bool isStart;
    
    //커서 움직일수 있는 상태들
    public bool isPause = false;
    public bool isSelecting = false;
    public bool CursorMoveMode{
        get{
            return isPause || isSelecting || isGameover;
        }
    }
    public bool canPlayerMove = true;



    private void Awake() {
        // 씬에 싱글톤 오브젝트가 된 다른 GameManager 오브젝트가 있다면
        if (instance != this)
        {
            // 자신을 파괴
            Destroy(gameObject);
        }

        for(int i=0; i<3; i++){
            if(i==TitleSetting.playernum) players[i].SetActive(true);
            else players[i].SetActive(false);
        }
    }

    private void Start() {
        ApplyCursorLock();
        StageClear += ChangeStage;
        isStart = false;
        UIManager.instance.updateDifficulty();
        FindObjectOfType<PlayerStatus>().onDeath += GameOver;

    }

    //커서 잠금, 잠금해제
    public void ApplyCursorLock(){
        if(CursorMoveMode){
            Cursor.lockState = CursorLockMode.None;
            canPlayerMove = false;
            Cursor.visible = true;
        }
        else{
            Cursor.lockState = CursorLockMode.Locked;
            canPlayerMove = true;
            Cursor.visible = false;
        }
    }

    public void ChangeStage(){
        if(subStage <5) subStage++;
        else{
            subStage = 1;
            mainStage++;
            if(mainStage == 2) GameEnd();
        }
    }

    public void AddExp(float _exp){
        if(curExp + _exp < maxExp) {
            curExp += _exp;
        }
        else{
            curExp = _exp - maxExp + curExp;
            playerLevel++;
            if(PlayerLevelUp!=null) PlayerLevelUp();
            UIManager.instance.UpdateLevelText(playerLevel);
        }
        UIManager.instance.UpdateExp(curExp, maxExp);
    }

    public void GameOver(){
        isGameover = true;
        gameOverTab.SetActive(true);
        ApplyCursorLock();
    }

    public void GameEnd(){
        UIManager.instance.GameEnd();
        ApplyCursorLock();
    }


}
