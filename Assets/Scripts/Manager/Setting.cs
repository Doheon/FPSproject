using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Setting : MonoBehaviour
{
    public AudioSource[] audioAll;
    public PlayerMovement playerMovement;

    public Slider masterVolumeSlider;

    public TextMeshProUGUI audioText;
    public TextMeshProUGUI sensitivityText;



    void Awake()
    {
        audioAll = Resources.FindObjectsOfTypeAll<AudioSource>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        MasterVolume(0.5f);
        MouseSensitivity(0.5f);
    }

    public void MasterVolume(float _volume){
        for(int i = 0; i<audioAll.Length; i++){
            audioAll[i].volume = _volume;
        }
        audioText.text = Mathf.RoundToInt(_volume * 100f).ToString();
    }

    public void MouseSensitivity(float _sen){
        playerMovement.lookSensitivity = 30f + _sen * 300f;
        sensitivityText.text = Mathf.RoundToInt(_sen * 100f).ToString();
    }
}
