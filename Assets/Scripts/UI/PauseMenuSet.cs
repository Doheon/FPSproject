using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuSet : MonoBehaviour
{
    public string titleScene = "GameTitle";
    public string currentScene = "GameStage";

    public GameObject pausePanel;
    public GameObject pauseTab;

    public GameObject settingTab;
    public GameObject howToPlayTab;
    public GameObject gameEnd;

    private Setting setting;

    private void Start() {
        setting = settingTab.GetComponent<Setting>();

        pausePanel.SetActive(true);
        pausePanel.SetActive(false);
    }

    private void Update()
    {
        CheckPause();
    }

    private void CheckPause(){
        if(PlayerInput.instance.pauseGame){
            if(!GameManager.instance.isPause){
                CallMenu();
            }
            else{
                CloseMenu();
            }
        }
    }

    public void CloseMenu(){
        GameManager.instance.isPause = false;
        GameManager.instance.ApplyCursorLock();

        pausePanel.SetActive(false);
        pauseTab.SetActive(false);
        settingTab.SetActive(false);
        gameEnd.SetActive(false);

        Time.timeScale = 1f;
    }

    public void CallMenu(){
        GameManager.instance.isPause = true;
        GameManager.instance.ApplyCursorLock();

        pausePanel.SetActive(true);
        pauseTab.SetActive(true);
        settingTab.SetActive(false);

        Time.timeScale = 0f;
    }

    public void ClickExit(){
        Application.Quit();
    }

    public void ClickMainMenu(){
        Time.timeScale = 1f;
        setting.SaveSetting();
        SceneManager.LoadScene(titleScene);
    }

    public void ClickRestart(){
        Time.timeScale = 1f;
        setting.SaveSetting();
        SceneManager.LoadScene(currentScene);
    }

    public void ClickSetting(){
        settingTab.SetActive(true);
        pauseTab.SetActive(false);
    }

    public void ClickHowtoPlay(){
        howToPlayTab.SetActive(true);
        pauseTab.SetActive(false);
    }

    public void ClickBack(){
        settingTab.SetActive(false);
        howToPlayTab.SetActive(false);
        pauseTab.SetActive(true);
    }
}
