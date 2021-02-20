using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoTab : MonoBehaviour
{
    private PlayerStatus player;
    
    public GameObject infoTab;
    public TextMeshProUGUI[] texts;

    void Start()
    {
        player = FindObjectOfType<PlayerStatus>();
    }

    void Update()
    {
        if(PlayerInput.instance.infoTab) ActiveInfoTab();
        else infoTab.SetActive(false);
    }

    private void ActiveInfoTab(){
        infoTab.SetActive(true);
        texts[0].text = player.attackDamage.ToString();
        texts[1].text = player.attackSpeed.ToString() + "%";
        texts[2].text = player.criticalDam.ToString() + "%";
        texts[3].text = player.criticalProb.ToString() + "%";
        texts[4].text = player.armor.ToString() + "(-" + Mathf.RoundToInt(player.damageReduction * 100f) + "%)";
        texts[5].text = player.skillHaste.ToString() + "(-" + Mathf.RoundToInt(player.coolReduction * 100f) + "%)";
        texts[6].text = player.skillDam.ToString() + "%";
        texts[7].text = player.lifeSteal + "%";
    }
}
