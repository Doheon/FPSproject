using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Enemy enemy1;
    public Enemy enemy2;

    public Transform[] spawnPoints;
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
            return ((stageMain-1) * 5 + stageSub)*10 + 10f;
        }
    }
    public float health{
        get{
            return ((stageMain-1)*5 + stageSub) * 100 + 100f;
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

        EnemySpawn1();

    }



    private void EnemySpawn1(){
        for(int i=0; i<spawnCount; i++){
            CreateEnemy(enemy1, health, damage, 3f, exp, Color.white);
        }
        CreateEnemy(enemy2, health * 5, damage, 3f, exp*2, Color.red);

        isClear = false;
    }



    private void MakeExpBall(Enemy _enemy, float _exp){
        GameObject expBall = ObjectPoolingManager.instance.GetQueue(ObjectPoolingManager.instance.expBallQueue);
        expBall.transform.position = _enemy.transform.position + Vector3.up *4f * expBall.GetComponent<SphereCollider>().radius;
        ExpBall _expball = expBall.GetComponent<ExpBall>();
        _expball.SetRemoveTime(2);
        _expball.SetExpBallHP(health/3 + _enemy.startHP/10f);

        _expball.onDeath += () => _expball.DestroyExpBall(_exp);
    }


    private void CreateEnemy(Enemy _enemytype, float _health, float _damage, float _speed, float _exp, Color _color){
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Enemy _enemy = Instantiate(_enemytype, spawnPoint.position, spawnPoint.rotation);
        _enemy.Setup(_health, _damage, _speed, _exp, _color);
        enemyLeft++;

        _enemy.onDeath += () => Destroy(_enemy.gameObject);
        _enemy.onDeath += () => enemyLeft--;
        _enemy.onDeath += () => MakeExpBall(_enemy, _enemy.exp);
    }
}
