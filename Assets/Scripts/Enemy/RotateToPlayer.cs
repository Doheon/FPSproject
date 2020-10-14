using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToPlayer : MonoBehaviour
{
    public Transform player;
    void Start()
    {
        transform.LookAt(player);
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player);
    }
}
