using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ENEMY : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private float leftcap;
    [SerializeField] private float rightcap;
    [SerializeField] private float jumpLenght = 10f;
    [SerializeField] private LayerMask ground;
    private Collider2D coll;
    private Rigidbody2D rb;
    private bool facingLeft = true;
    private void Start()
    {
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (facingLeft)
        {
            if (transform.position.x > leftcap)
            {
                if (coll.IsTouchingLayers(ground))
                {
                    rb.velocity = new Vector2(-jumpLenght, 0);

                }
            }
            else
            {
                facingLeft = false;
            }
        }
        else
        {
            if (transform.position.x < rightcap)
            {
                if (coll.IsTouchingLayers(ground))
                {
                    rb.velocity = new Vector2(jumpLenght, 0);

                }
            }
            else
            {
                facingLeft = true;
            }
        }

    }
    public void JumpedOn()
    {
        rb.velocity = new Vector2(0, 0);
        Debug.Log("TRIGGER");
        anim.SetTrigger("EnemyDeath");
    }
    public void GameOver()
    {
        FindObjectOfType<GameManager>().EndGame();
    }
}
