using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //Start() variablse
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;
    private Animator cherryAnim;

    //Inspector Variables
    [SerializeField] private LayerMask ground;
    [SerializeField] private float speed = 0;
    [SerializeField] private float jumpforce = 0;
    [SerializeField] private float hurtForce = 10f;
    [SerializeField] private AudioSource cherry = null;
    [SerializeField] private AudioSource footstep = null;
    [SerializeField] private AudioSource hurt = null;

    //FSM
    private enum State { Idle, Running, Jumping, Falling, Hurt }
    private State state = State.Idle;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        PermanentUI.perm.healthAmount.text = PermanentUI.perm.health.ToString();
    }

    private void Update()
    {
        if (state != State.Hurt)
        {
            Movement();
        }
        AnimationState();
        anim.SetInteger("State", (int)state); //Sets animation based on Enumerator state 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collectable")
        {
            cherryAnim = collision.gameObject.GetComponent<Animator>();
            cherry.Play();
            cherryAnim.SetTrigger("Collected");
            Destroy(collision.gameObject);
            PermanentUI.perm.cherries++;
            PermanentUI.perm.cherryText.text = PermanentUI.perm.cherries.ToString();
            if(PermanentUI.perm.cherries == 5)
            {
                PermanentUI.perm.health++;
                PermanentUI.perm.healthAmount.text = PermanentUI.perm.health.ToString();
                PermanentUI.perm.cherries -= 5;
                PermanentUI.perm.cherryText.text = PermanentUI.perm.cherries.ToString();
            }
        }
        if (collision.tag == "Enemy")
        {
            state = State.Hurt;
            hurt.Play();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (state == State.Falling)
            {
                enemy.JumpedOn();
                Jump();
            }
            else
            {
                state = State.Hurt;
                HandleHealth();//Deals with health,updating ui,and will reset level if health is <= 0
                hurt.Play();
                if (collision.gameObject.transform.position.x > transform.position.x)
                {
                    //Enemy is to my right therefore i should be damaged and move left
                    rb.velocity = new Vector2(-hurtForce, rb.velocity.y);
                }
                else
                {
                    //Enemy is to my left therefore i should be damaged and move right
                    rb.velocity = new Vector2(hurtForce, rb.velocity.y);
                }
            }
        }
    }

    private void HandleHealth()
    {
        PermanentUI.perm.health--;
        PermanentUI.perm.healthAmount.text = PermanentUI.perm.health.ToString();
        if (PermanentUI.perm.health <= 0)
        {
            PermanentUI.perm.Reset();
            SceneManager.LoadScene("YouFailed");
        }
    }

    private void Movement()
    {
        float hDirection = Input.GetAxis("Horizontal");
        //Moving Left
        if (hDirection < 0)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
        }

        //Moving Right
        else if (hDirection > 0)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
        }

        //Jumping
        if (Input.GetButtonDown("Jump") && Mathf.Abs(rb.velocity.y) < .001f)
        {
            Jump();
        }

    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpforce);
        state = State.Jumping;
    }

    private void AnimationState()
    {
        if (state == State.Jumping)
        {
            if (rb.velocity.y < .1f)
            {
                state = State.Falling;
            }
        }
        else if(rb.velocity.y < -.01)
        {
            state = State.Falling;
        }
        else if (state == State.Falling)
        {
            if (coll.IsTouchingLayers(ground))
            {
                state = State.Idle;
            }
        }
        else if (state == State.Hurt)
        {
            if (Mathf.Abs(rb.velocity.x) < .1f)
            {
                state = State.Idle;
            }
        }
        else if (Mathf.Abs(rb.velocity.x) > 2f)
        {
            state = State.Running;
        }
        else
        {
            state = State.Idle;
        }
    }

    private void Footstep()
    {
        if (coll.IsTouchingLayers(ground))
        {
            footstep.Play();
        }
    }
}
