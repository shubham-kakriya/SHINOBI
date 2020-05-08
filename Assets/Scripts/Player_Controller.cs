using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public bool attacking = false;
    private enum State {idle ,running ,jumping,falling,attack,hurt}
    private State state = State.idle;
    private Collider2D coll;
    //Inspector variables
    [SerializeField] private LayerMask ground;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jump_force = 10f;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
    }
    private void Update()
    {
        if (state != State.hurt)
        {
            DirectionManager();
        }
        VelocityState();
        anim.SetInteger("state", (int)state);
    }
    private void OnCollisionEnter2D(Collision2D Other)
    {
        Drone drone = Other.gameObject.GetComponent<Drone>();
        if (Other.gameObject.tag == "DRONEENEMY")
        {
            Debug.Log(attacking);
            if (attacking == true)
            {
                drone.JumpedOn();
            }
            else
            {
                Destroy(Other.gameObject);
                state = State.hurt;
                FindObjectOfType<GameManager>().EndGame();
            }
        }
        ENEMY Enemy = Other.gameObject.GetComponent<ENEMY>();
        if (Other.gameObject.tag == "ENEMY")
        {
            Debug.Log(attacking);
            if (attacking == true)
            {
                Enemy.JumpedOn();
            }
            else
            {
                Destroy(Other.gameObject);
                state = State.hurt;
            }
        }
    }
    private void DirectionManager()
    {
        float hDirection = Input.GetAxis("Horizontal");
        //moving left
        if (hDirection < 0)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector2(-0.5f, 0.5f);
            //moving right
        }
        else if (hDirection > 0)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector2(0.5f, 0.5f);
        }
        //jumping
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
        {
            rb.velocity = new Vector2(rb.velocity.x, jump_force);
            state = State.jumping;
        }
    }

    private void VelocityState()
    {

        if (state == State.hurt)
        {

        }
        else if (Input.GetButtonDown("Fire1"))
        {
            state = State.attack;
            attacking = true;
        }

        else if (state == State.jumping)
        {
            if(rb.velocity.y< .2f)
            {
                state = State.falling;
                attacking = false;
            }    
        }
        else if(state == State.falling)
        {
            if(coll.IsTouchingLayers(ground))
            {
                state = State.idle;
                attacking = false;
            }    
        }
        else if(Mathf.Abs(rb.velocity.x)> 0.1f)
        {
            state = State.running;
        }

        else
        {
            state = State.idle;
            attacking = false;
        }

        
    }
}

