using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

[System.Serializable] //데이터직렬화
public class SaveSetting{
    public float volume;
    public float mouseSensitivity;
}

public class Setting : MonoBehaviour
{
    private SaveSetting saveSetting = new SaveSetting();
    private string SAVE_DATA_DIRECTORY;
    private string SAVE_FILENAME = "/Settings.txt";

    public AudioSource[] audioAll;
    public PlayerMovement playerMovement;

    public Slider masterVolumeSlider;
    public Slider mouseSensitivitySlider;

    public TextMeshProUGUI audioText;
    public TextMeshProUGUI sensitivityText;

    public float volume;
    public float mouseSensitivity;



    void Awake()
    {
        
        audioAll = Resources.FindObjectsOfTypeAll<AudioSource>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        
        SAVE_DATA_DIRECTORY = Application.dataPath + "/Setting/";
        if(!Directory.Exists(SAVE_DATA_DIRECTORY)) {
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY);
        }

        MasterVolume(0.5f);
        MouseSensitivity(0.5f);
        LoadSetting();

        masterVolumeSlider.value = volume;
        mouseSensitivitySlider.value = mouseSensitivity;

    }

    public void MasterVolume(float _volume){
        volume = _volume;
        for(int i = 0; i<audioAll.Length; i++){
            audioAll[i].volume = volume/10f;
        }
        audioText.text = Mathf.RoundToInt(_volume * 100f).ToString();
    }

    public void MouseSensitivity(float _sen){
        mouseSensitivity = _sen;
        playerMovement.lookSensitivity = 30f + _sen * 100f;
        sensitivityText.text = Mathf.RoundToInt(_sen * 100f).ToString();
    }

    public void SaveSetting(){
        saveSetting.volume = volume;
        saveSetting.mouseSensitivity = mouseSensitivity;

        string json = JsonUtility.ToJson(saveSetting);
        File.WriteAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME, json);
    }

    public void LoadSetting(){
        if(File.Exists(SAVE_DATA_DIRECTORY + SAVE_FILENAME)){
            string loadJson = File.ReadAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME);
            saveSetting = JsonUtility.FromJson<SaveSetting>(loadJson);

            MasterVolume(saveSetting.volume );
            MouseSensitivity(saveSetting.mouseSensitivity);
        }
    }
}
