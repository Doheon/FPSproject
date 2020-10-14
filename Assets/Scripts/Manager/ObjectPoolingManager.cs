using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
    public static ObjectPoolingManager instance;

    //bullet
    public GameObject bulletPrefab;
    public Queue<GameObject> bulletQueue = new Queue<GameObject>();

    //tbullet
    public GameObject tBulletPrefab;
    public Queue<GameObject> tBulletQueue = new Queue<GameObject>();

    //expball
    public GameObject expBallPrefab;
    public Queue<GameObject> expBallQueue = new Queue<GameObject>();

    //expPiece
    public GameObject expPiecePrefab;
    public Queue<GameObject> expPieceQueue = new Queue<GameObject>();

    void Start()
    {
        instance = this;
        MakeObjects(bulletPrefab, bulletQueue, 20); //bullets
        MakeObjects(tBulletPrefab, tBulletQueue, 10); //tbullets
        MakeObjects(expBallPrefab, expBallQueue, 20); //expBalls
        MakeObjects(expPiecePrefab, expPieceQueue, 100); //expPieces
    }

    private void MakeObjects(GameObject _prefab, Queue<GameObject> _queue, int _number){
        for(int i=0; i<_number; i++){
            GameObject _object = Instantiate(_prefab, Vector3.zero, Quaternion.identity, transform);
            _queue.Enqueue(_object);
            _object.SetActive(false);
        }
    }

    public void InsertQueue(GameObject _object, Queue<GameObject> _queue){
        _queue.Enqueue(_object);
        _object.SetActive(false);
    }

    public GameObject GetQueue(Queue<GameObject> _queue){
        if(_queue.Count == 0) Debug.Log("not enough object");
        
        GameObject _object = _queue.Dequeue();
        _object.SetActive(true);
        return _object;
    }
}
