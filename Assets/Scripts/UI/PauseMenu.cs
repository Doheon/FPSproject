using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public string titleScene = "GameTitle";
    public string currentScene = "GameStage";

    public GameObject pausePanel;
    public GameObject pauseTab;
    public GameObject settingTab;

    private void Start() {
        pausePanel.SetActive(true);
        pausePanel.SetActive(false);
    }

    void Update()
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
        SceneManager.LoadScene(titleScene);
    }

    public void ClickRestart(){
        Time.timeScale = 1f;
        SceneManager.LoadScene(currentScene);
    }

    public void ClickSetting(){
        settingTab.SetActive(true);
        pauseTab.SetActive(false);
    }

    public void ClickBack(){
        settingTab.SetActive(false);
        pauseTab.SetActive(true);
    }
}
