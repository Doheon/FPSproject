using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Enemy enemy1;
    public Enemy enemyElite;
    public Enemy enemyElite2;
    public Enemy enemySniper;
    public Enemy enemyBoss1;


    public Transform[] spawnPointsNormal;
    public Transform[] spawnPointsSniper;
    public Transform spawnPointBoss;
    
    public bool isClear;

    public int stageMain{
        get{
            return GameManager.instance.mainStage;
        }
    }
    public int stageSub{
        get{
            return GameManager.instance.subStage;
        }
    }
    public int enemyLeft{
        get{
            return GameManager.instance.enemyLeft;
        }
        set{
            GameManager.instance.enemyLeft =  value;
        }
    }
    
    public float damage{
        get{
            return (((stageMain-1) * 5 + stageSub)*10 + 10f) * TitleSetting.enemyDamage;
        }
    }
    public float health{
        get{
            return (((stageMain-1)*5 + stageSub) * 100 + 100f)*TitleSetting.enemyHealth;
        }
    }

    public float exp{
        get{
            return ((stageMain-1) * 5 + stageSub) *50 + 100f;
        }
    }

    public int spawnCount = 5;


    void Update()
    {
        CheckStageClear();
    }

    private void CheckStageClear(){
        if(enemyLeft == 0 && !isClear){
            isClear = true;
            StartCoroutine(DelayStageClearCoroutine());
        }
        UIManager.instance.UpdateEnemyLeftText(enemyLeft);
    }

    IEnumerator DelayStageClearCoroutine(){
        float _time = 0f;
        if(GameManager.instance.subStage != 0){
            _time = 3f;
            UIManager.instance.StageClearText();
        }
        yield return new WaitForSeconds(_time);
        GameManager.instance.StageClear();
        UIManager.instance.UpdateStageText(stageMain, stageSub);

        if(stageSub ==1) EnemySpawn1();
        if(stageSub ==2) EnemySpawn2();
        else if(stageSub ==3) EnemySpawn3();
        else if(stageSub ==4) EnemySpawn4();
        else if(stageSub == 5) EnemySpawn5();

    }



    private void EnemySpawn1(){
        Transform pos;
        for(int i=0; i<4; i++){
            pos = spawnPointsNormal[Random.Range(0, spawnPointsNormal.Length)];
            CreateEnemy(enemy1, health, damage, 3f, exp, Color.white, pos, false);
        }
        pos = spawnPointsNormal[Random.Range(0, spawnPointsNormal.Length)];
        CreateEnemy(enemyElite, health * 5, damage, 3f, exp*2, Color.white, pos, false);
        isClear = false;
    }

    private void EnemySpawn2(){
        Transform pos;
        for(int i=0; i<5; i++){
            pos = spawnPointsNormal[Random.Range(0, spawnPointsNormal.Length)];
            CreateEnemy(enemy1, health, damage, 3f, exp, Color.white, pos, false);
        }
        pos = spawnPointsNormal[Random.Range(0, spawnPointsNormal.Length)];
        CreateEnemy(enemyElite, health * 5, damage, 3f, exp*2, Color.white, pos, false);
        isClear = false;
    }

    private void EnemySpawn3(){
        Transform pos;
        for(int i=0; i<3; i++){
            pos = spawnPointsNormal[Random.Range(0, spawnPointsNormal.Length)];
            CreateEnemy(enemy1, health, damage, 3f, exp, Color.white, pos, false);
        }
        for(int i=0; i<3; i++){
            pos = spawnPointsSniper[i];
            CreateEnemy(enemySniper, health/2, damage*2, 3f, exp, Color.white, pos, false);
        }
        pos = spawnPointsNormal[Random.Range(0, spawnPointsNormal.Length)];
        CreateEnemy(enemyElite, health * 5, damage, 3f, exp*2, Color.white, pos, false);
        isClear = false;
    }

    private void EnemySpawn4(){
        Transform pos;
        for(int i=0; i<3; i++){
            pos = spawnPointsNormal[Random.Range(0, spawnPointsNormal.Length)];
            CreateEnemy(enemy1, health, damage, 3f, exp, Color.white, pos, false);
        }
        for(int i=0; i<4; i++){
            pos = spawnPointsSniper[i];
            CreateEnemy(enemySniper, health/2, damage*2, 3f, exp, Color.white, pos, false);
        }
        pos = spawnPointsNormal[Random.Range(0, spawnPointsNormal.Length)];
        CreateEnemy(enemyElite2, health * 5, damage, 3f, exp*2, Color.yellow, pos, false);
        isClear = false;
    }

    private void EnemySpawn5(){
        Transform pos;
        pos = spawnPointBoss;
        CreateEnemy(enemyBoss1, health*50, damage, 3f, exp*10, Color.white, pos, true);
    }



    private void MakeExpBall(Enemy _enemy, float _exp, bool _isBoss){
        GameObject expBall = ObjectPoolingManager.instance.GetQueue(ObjectPoolingManager.instance.expBallQueue);
        expBall.transform.position = _enemy.transform.position + Vector3.up * 1.8f;
        ExpBall _expball = expBall.GetComponent<ExpBall>();
        if(!_isBoss) _expball.SetRemoveTime(TitleSetting.ballRemoveTime);
        _expball.SetExpBallHP(health/3);
        _expball.onDeath += () => _expball.DestroyExpBall(_exp);
    }


    private void CreateEnemy(Enemy _enemytype, float _health, float _damage, float _speed, float _exp, Color _color, Transform _pos, bool _isBoss){
        Enemy _enemy = Instantiate(_enemytype, _pos.position, _pos.rotation);
        _enemy.Setup(_health, _damage, _speed, _exp, _color);
        enemyLeft++;

        _enemy.onDeath += () => enemyLeft--;
        _enemy.onDeath += () => MakeExpBall(_enemy, _enemy.exp, _isBoss);
    }
}
