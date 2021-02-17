using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public string sceneName = "GameStage";
    public GameObject settingTab;
    public GameObject titleMenu;
    public GameObject characterSelect;
    public GameObject howToPlay;

    public void ClickStart(){
        titleMenu.SetActive(false);
        characterSelect.SetActive(true);
    }
    public void ClickCharacter1(){
        PlayerManager.playernum = 0;
        SceneManager.LoadScene(sceneName);
    }
    public void ClickCharacter2(){
        PlayerManager.playernum = 1;
        SceneManager.LoadScene(sceneName);
    }
    public void ClickCharacter3(){
        PlayerManager.playernum = 2;
        SceneManager.LoadScene(sceneName);
    }

    public void ClickHowtoPlay(){
        titleMenu.SetActive(false);
        howToPlay.SetActive(true);
    }


    public void ClickSetting(){
        titleMenu.SetActive(false);
        settingTab.SetActive(true);
    }

    public void ClickExit(){
        Application.Quit();
    }

    public void ClickBack(){
        settingTab.SetActive(false);
        characterSelect.SetActive(false);
        howToPlay.SetActive(false);
        titleMenu.SetActive(true);
    }
    
}
