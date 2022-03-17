using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    [SerializeField] private float speed = 5f;
    [SerializeField] private float maxDistance = 5f;
    [SerializeField] private float linecastDistance = 1f;

    private int dir = 1;
    private float distance = 0f;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        anim.SetBool("GoingRight", rb.velocity.x > -0.1f);

        distance += speed * Time.deltaTime;
        if (distance >= maxDistance)
            ChangeDir();

        rb.velocity = new Vector2(dir * speed, rb.velocity.y);

        if (Physics2D.Linecast(transform.position, transform.position + (Vector3.right * dir) * linecastDistance, 1 << LayerMask.NameToLayer("Ground")))
            ChangeDir();
    }

    public void Hit() {
        anim.SetTrigger("Hit");
        Destroy(gameObject, 0.5f);
    }

    private void ChangeDir()
    {
        distance = 0f;
        dir *= -1;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")) {
            other.gameObject.GetComponent<PlayerController>().Hit();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            other.GetComponent<PlayerController>().HitJump();
            Hit();
        }
    }
}
