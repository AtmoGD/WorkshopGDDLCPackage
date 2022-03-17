using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private float jumpTimeout = 0.2f;
    [SerializeField] private ParticleSystem winParticles;
    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource hitSound;
    [SerializeField] private AudioSource deathSound;
    [SerializeField] private AudioSource winSound;
    [SerializeField] private AudioSource hitJumpSound;
    [SerializeField] private AudioSource walkSound;

    [Header("Hier könnt ihr rumspielen")]
    [Space(30)]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private int amountOfJumps = 2;
    [SerializeField] private int maxLives = 3;

    private int jumpsLeft = 2;
    private float jumpTimer = 0f;
    private int livesLeft = 3;

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

        livesLeft = maxLives;
    }

    private void Update()
    {
        jumpTimer -= Time.deltaTime;

        if (IsGrounded && jumpTimer <= 0f)
            jumpsLeft = amountOfJumps;

        anim.SetFloat("MovementHorizontal", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("MovementVertical", rb.velocity.y);
        anim.SetBool("IsGrounded", IsGrounded);
        anim.SetBool("GoingRight", rb.velocity.x > -0.1f);
    }

    public void Move(float _dir)
    {
        rb.velocity = new Vector2(_dir * speed, rb.velocity.y);
    }

    public void Jump()
    {
        if((IsGrounded || jumpsLeft > 0) && jumpTimer <= 0)
        {
            PlayJumpSound();
            rb.velocity = new Vector2(rb.velocity.x, Vector2.up.y * jumpForce);
            jumpsLeft--;
            jumpTimer = jumpTimeout;
        }
    }

    public void HitJump(bool withSound = true) {
        rb.velocity = new Vector2(rb.velocity.x, Vector2.up.y * jumpForce);
        jumpsLeft = amountOfJumps;

        if (withSound)
            hitJumpSound.Play();
    }

    public void Hit()
    {
        HitJump(false);
        anim.SetTrigger("Hit");

        livesLeft--;

        if(livesLeft > 0)
            PlayHitSound();

        if (livesLeft <= 0)
            Die();
    }

    public void Die() {
        livesLeft = 0;
        PlayDeathSound();
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject, 3f);
        
        FindObjectOfType<UIController>()?.GameOver();
    }

    public void Win() {
        winParticles?.Play();
        winSound?.Play();
        FindObjectOfType<UIController>()?.GameWon();
    }

    public int GetMaxLives() {
        return maxLives;
    }

    public int GetLives() {
        return livesLeft;
    }

    public void PlayJumpSound() {
        jumpSound.Play();
    }

    public void PlayHitSound() {
        hitSound.Play();
    }

    public void PlayDeathSound() {
        deathSound.Play();
    }

    public void PlayWalkSound() {
        walkSound.Play();
    }
}
