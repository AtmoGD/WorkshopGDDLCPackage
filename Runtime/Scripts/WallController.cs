using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        other.SendMessage("Die", SendMessageOptions.DontRequireReceiver);
        other.SendMessage("Hit", SendMessageOptions.DontRequireReceiver);
    }
}
