using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private Transform player;
    void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;
    }
    void Update()
    {
        if(!player) return;
        
        transform.position = new Vector3(player.position.x, transform.position.y, transform.position.z);
    }
}
