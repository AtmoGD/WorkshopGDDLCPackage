using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Direction
{
    Left,
    Right
}

public class FrogController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private float jumpTimeout = 2f;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 5f;

    public bool IsGrounded
    {
        get
        {
            return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, 1 << LayerMask.NameToLayer("Ground"));
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        StartCoroutine(Jump());
    }

    private void Update()
    {
        anim.SetFloat("MovementVertical", rb.velocity.y);
        anim.SetBool("IsGrounded", IsGrounded);
        anim.SetBool("GoingRight", rb.velocity.x > -0.1f);
    }

    public void Hit() {
        anim.SetTrigger("Hit");
        Destroy(gameObject, 0.5f);
    }

    IEnumerator Jump()
    {
        rb.velocity = new Vector2(speed, jumpForce);
        yield return new WaitForSeconds(jumpTimeout);
        rb.velocity = new Vector2(-speed, jumpForce);
        yield return new WaitForSeconds(jumpTimeout);
        StartCoroutine(Jump());
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
