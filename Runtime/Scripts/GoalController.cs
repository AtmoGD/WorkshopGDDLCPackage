using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")) {
            other.SendMessage("Win", SendMessageOptions.DontRequireReceiver);
        }
    }
}
