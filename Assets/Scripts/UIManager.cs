using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<UIManager>();
            }

            return m_instance;
        }
    }
    private static UIManager m_instance;

    public Gun gun;
    public GameObject go_BulletHUD;
    
    //총알 텍스트
    public Text BulletText;
    public GameObject theSniperMode;
    public GameObject theCrossHair;

    private void start(){
        BulletText.text = gun.currentBullet + "/" + gun.maxBullet;
        theSniperMode.SetActive(false);
    }

    public void updateBulletText(int _cur, int _max){
        BulletText.text = _cur + "/" + _max;
    }
    public void SniperMode(){
        theSniperMode.SetActive(true);
        theCrossHair.SetActive(false);

    }
    public void cancelSniperMode(){
        theSniperMode.SetActive(false);
        theCrossHair.SetActive(true);
    }
}
