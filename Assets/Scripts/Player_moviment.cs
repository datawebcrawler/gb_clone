using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player_moviment : MonoBehaviour
{

    [SerializeField] private int mov_speed = 5;
    [SerializeField] private int boost_speed = 3;
    [SerializeField] private int jump_speed = 5;
    [SerializeField] private bool is_jumping = false;
    [SerializeField] private bool is_running = false;
    public bool is_facingRight = false;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private CircleCollider2D cc;
    
    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        cc = GetComponent<CircleCollider2D>();

    }

    // Update is called once per frame
    void Update()
    {

        float move_h = Input.GetAxisRaw("Horizontal");

        if(is_running == true)
        {

            rb.velocity = new Vector2((mov_speed + boost_speed) * move_h, rb.velocity.y);

        }else
        {

            rb.velocity = new Vector2(mov_speed * move_h, rb.velocity.y);

        }
        

        if (move_h > 0) { sr.flipX = false; is_facingRight = true; } else if (move_h < 0 ){ sr.flipX = true; is_facingRight = false; };

        if (is_jumping == false && Input.GetButtonDown("Jump"))
        {

            is_jumping = true;
            rb.AddForce(new Vector2(0, jump_speed), ForceMode2D.Impulse);


        }

        if (Input.GetButtonDown("Fire3"))
        {

            is_running = true;

        }
        else if (Input.GetButtonUp("Fire3"))
        {

            is_running = false;

        };

    }


    private void FixedUpdate()
    {


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Ground"))
        {

            is_jumping = false;

        }


    }

}
