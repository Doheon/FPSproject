using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public string sceneName = "GameStage";
    public GameObject difficulty;
    public GameObject titleMenu;
    public GameObject characterSelect;
    public GameObject howToPlay;

    public void ClickStart(){
        titleMenu.SetActive(false);
        characterSelect.SetActive(true);
    }
    public void ClickCharacter1(){
        TitleSetting.playernum = 0;
        SceneManager.LoadScene(sceneName);
    }
    public void ClickCharacter2(){
        TitleSetting.playernum = 1;
        SceneManager.LoadScene(sceneName);
    }
    public void ClickCharacter3(){
        TitleSetting.playernum = 2;
        SceneManager.LoadScene(sceneName);
    }

    public void ClickHowtoPlay(){
        titleMenu.SetActive(false);
        howToPlay.SetActive(true);
    }

    public void ClickDifficulty(){
        titleMenu.SetActive(false);
        difficulty.SetActive(true);
    }

    public void ClickEasy(){
        TitleSetting.difficulty = 1;
    }

    public void ClickNormal(){
        TitleSetting.difficulty = 2;
    }

    public void ClickHard(){
        TitleSetting.difficulty = 3;
    }

    public void ClickHell(){
        TitleSetting.difficulty = 4;
    }

    public void ClickExit(){
        Application.Quit();
    }

    public void ClickBack(){
        difficulty.SetActive(false);
        characterSelect.SetActive(false);
        howToPlay.SetActive(false);
        titleMenu.SetActive(true);
    }
    
}
