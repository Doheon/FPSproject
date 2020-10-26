using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyBoss1 : Enemy
{
    public Enemy spawned;
    public int spawnCount;
    public GameObject projector;
    public Animator animator;
    
    public Image fillImage;
    public TextMeshProUGUI healthText;


    private int curSpawnNum = 0;
    private bool isCharging = false;
    private bool isNoDamage{
        get{
            return curSpawnNum != 0 || isCharging;
        }
    }


    public override void Update() {
        DoPattern();
        checkIsNodamage();
    }

    public override void OnDamage(float damage, bool isCrit, Vector3 hitPoint, Vector3 hitNormal, Collider hitCollider)
    {
        if(!isNoDamage) base.OnDamage(damage, isCrit, hitPoint, hitNormal, hitCollider);
        healthText.text = Mathf.RoundToInt(HP/startHP * 100f).ToString() + "%";
    }

    private void checkIsNodamage(){
        Color _color = fillImage.color;
        if(isNoDamage){
            _color.a = 100f/255f;
            fillImage.color = _color;
        }
        else{
            _color.a = 1f;
            fillImage.color = _color;
        }
    }


    public void DoPattern(){
        if(hasTarget && Time.time > lastAttackTime + fireRate){
            lastAttackTime = Time.time;
            
            int _seed = Random.Range(0,6); 
            
            if(_seed >=4) {
                SpawnEnemy();
                fireRate = 7f;
            }
            else if(_seed >= 1) {
                ShootBullet();
                fireRate = 5f;
            }
            else if(_seed == 0) {
                Jump();
                fireRate = 5f;
            }
        }
    }

    public void SpawnEnemy(){
        for(int i=0; i<spawnCount; i++){
            CreateEnemy(spawned, 400f, 50f, 3f, 0f, Color.black, transform);
            curSpawnNum++;
        }
    }

    private void CreateEnemy(Enemy _enemytype, float _health, float _damage, float _speed, float _exp, Color _color, Transform _pos){
        Enemy _enemy = Instantiate(_enemytype, _pos.position, _pos.rotation);
        _enemy.Setup(_health, _damage, _speed, _exp, _color);

        _enemy.onDeath += () => curSpawnNum--;
        _enemy.onDeath += () => Destroy(_enemy.gameObject);
    }

    public override void ShootBullet(){
        for(int i=0; i<10; i++){
            GameObject bullet = ObjectPoolingManager.instance.GetQueue(ObjectPoolingManager.instance.tBullet2Queue);
            if(bullet != null){
                bullet.transform.position = transform.position + Vector3.up * capsuleCollider.bounds.extents.y;

                Tbullet2 _bullet = bullet.GetComponent<Tbullet2>();
                _bullet.damage = damage;
                _bullet.transform.localScale = new Vector3(0.4f,0.4f,0.4f);
                
                _bullet.SetRemoveTime(8);
                _bullet.SetVelocityStart(50f,Quaternion.Euler(0,36f * i,0) *transform.forward, 0.3f);
            }
        }
    }

    public void Jump(){
        StartCoroutine(JumpCoroutine());
    }

    IEnumerator JumpCoroutine(){
        projector.SetActive(true);
        isCharging = true;

        yield return new WaitForSeconds(3f);
        animator.SetBool("Jump", true);
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("Jump", false);

        Collider[] players = Physics.OverlapSphere(transform.position, 68f, TargetLayer);
        if(players.Length >0){
            PlayerStatus player = players[0].GetComponent<PlayerStatus>();
            player.OnDamage(100f, false, Vector3.zero, Vector3.zero, players[0]);
        }
        projector.SetActive(false);
        isCharging = false;
        

    }
}

