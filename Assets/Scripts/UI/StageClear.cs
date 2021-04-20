using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageClear : MonoBehaviour
{
    
    public GameObject abilitySelect;

    public Image[] choiceImages;
    public TextMeshProUGUI[] choiceTexts;
    public Color[] colors;


    public PlayerStatus player;

    private delegate void Set(float _val);

    private List<List<float>> valList = new List<List<float>>();
    private List<Set> funcList = new List<Set>();
    private List<string> textList = new List<string>();

    private int[] indexes = new int[3];
    private int[] ranks = new int[3];


    private int rank{
        get{
            float _randNum = Random.Range(0f,100f);
            if(_randNum >= 99f) return 3;
            else if(_randNum >= 96f) return 2;
            else if(_randNum >= 80f) return 1;
            else return 0;
        }
    }


    Set AttackDamage, AttackSpeed, CriticalDamage, CriticalProb, HP, Armor, SkillHaste, SkillDamage, LifeSteal;
    void Start()
    {
        player = FindObjectOfType<PlayerStatus>();
        setLists();

        GameManager.instance.StageClear += StageClearReward;
        
    }

    private void setLists(){
        valList.Add(new List<float>{50f, 70f, 100f, 140f}); //attack damage
        valList.Add(new List<float>{15f, 25f, 35f, 50f}); //attack speed
        valList.Add(new List<float>{70f, 100f, 130f, 180f}); //crtical damage
        valList.Add(new List<float>{30f, 50f, 70f, 90f}); //critical probability
        valList.Add(new List<float>{80f, 120f, 160f, 200f}); //HP
        valList.Add(new List<float>{50f, 70f, 90f, 120f}); //armor
        valList.Add(new List<float>{40f, 60f, 80f, 100f}); //skill haste
        valList.Add(new List<float>{50f, 80f, 110f, 150f}); //skill damage
        valList.Add(new List<float>{5f, 8f, 12f, 18f}); //life steal

        AttackDamage = new Set(player.AddAttackDamage);
        AttackSpeed = new Set(player.AddAttackSpeed);
        CriticalDamage = new Set(player.AddCriticalDamage);
        CriticalProb = new Set(player.AddCriticalProb);
        HP = new Set(player.AddHP);
        Armor = new Set(player.AddArmor);
        SkillHaste = new Set(player.AddSkillHaste);
        SkillDamage = new Set(player.AddSkillDamage);
        LifeSteal = new Set(player.AddLifeSteal);

        funcList.Add(AttackDamage);
        funcList.Add(AttackSpeed);
        funcList.Add(CriticalDamage);
        funcList.Add(CriticalProb);
        funcList.Add(HP);
        funcList.Add(Armor);
        funcList.Add(SkillHaste);
        funcList.Add(SkillDamage);
        funcList.Add(LifeSteal);

        textList.Add("Attack\r\nDamage\r\n+");
        textList.Add("Attack\r\nSpeed\r\n+");
        textList.Add("Critical\r\nDamage\r\n+");
        textList.Add("Critical\r\nChance\r\n+");
        textList.Add("HP\r\n+");
        textList.Add("Armor\r\n+");
        textList.Add("Skill\r\nHaste\r\n+");
        textList.Add("Skill\r\nDamage\r\n+");
        textList.Add("Life\r\nSteal\r\n+");
    }

    public void StageClearReward(){
        //if(GameManager.instance.subStage == 0) return;
        int _rank;
        int[] _index = new int[3];
        _index[0] = Random.Range(1, textList.Count-1);
        _index[1] = Random.Range(0, _index[0]);
        _index[2] = Random.Range(_index[0]+1, textList.Count);

        for(int i=0; i<choiceImages.Length; i++){
            _rank = rank;
            
            choiceImages[i].color = colors[_rank];
            choiceTexts[i].text = textList[_index[i]] + valList[_index[i]][_rank];
            if(_index[i] == 1 || _index[i] == 2 || _index[i] == 3 || _index[i] == 7 || _index[i] == 8){
                choiceTexts[i].text += "%";
            }
            indexes[i] = _index[i];
            ranks[i] = _rank;
        }
        GameManager.instance.isSelecting = true;
        GameManager.instance.ApplyCursorLock();

        abilitySelect.SetActive(true);
    }

    public void ClickFirst(){
        funcList[indexes[0]](valList[indexes[0]][ranks[0]]);
        GameManager.instance.isSelecting = false;
        GameManager.instance.ApplyCursorLock();
        player.GetComponent<PlayerShooter>().gun.lastFireTime = Time.time;
        abilitySelect.SetActive(false);
    }

    public void ClickSecond(){
        funcList[indexes[1]](valList[indexes[1]][ranks[1]]);
        GameManager.instance.isSelecting = false;
        GameManager.instance.ApplyCursorLock();
        player.GetComponent<PlayerShooter>().gun.lastFireTime = Time.time;
        abilitySelect.SetActive(false);
    }

    public void ClickThird(){
        funcList[indexes[2]](valList[indexes[2]][ranks[2]]);
        GameManager.instance.isSelecting = false;
        GameManager.instance.ApplyCursorLock();
        player.GetComponent<PlayerShooter>().gun.lastFireTime = Time.time;
        abilitySelect.SetActive(false);
    }

}
