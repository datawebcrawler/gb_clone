using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class Move_bg : MonoBehaviour
{
    // Posição seta
    [SerializeField] Transform posStart;
    [SerializeField] Image setaImg;
    public float zRotate;
    public float anguloMira;
    
    Rigidbody2D rb;
    public bool facingLeft = true;

    [SerializeField] int speed = 5;
    float speedMultiplier;

    [Range(1, 10)]
    [SerializeField] float acceleration = 1.0f;

    bool btnPressed;
    private Vector2 movement;

    public GameObject seta;

    // Impulso do projétil
    [Range (1.0f, 50.0f)]
    public float impulsoDoProjetil = 20.0f;

    //variavel da instancia do projetil
    public GameObject projetil;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        movement = new Vector2(-1.0f, 0.0f);
        zRotate = 90;
    }

    private void Update()
    {
        RotacaoSeta();
        InputDeRotacao();
        Bala();
    }

    private void FixedUpdate()
    {
        UpdateSpeedMultiplier();
        float targetSpeed = speed * speedMultiplier * movement.x;
        rb.velocity = new Vector2(targetSpeed, rb.velocity.y);
    }

    public void Move(InputAction.CallbackContext value)
    {
        // const int step = 1000;
        if (value.started)
        {
            if (value.control.name.Equals("a") || value.control.name.Equals("d"))
            {
                var ant = movement.x;
                movement = value.ReadValue<Vector2>();

                if (ant != movement.x)
                {
                    flip();
                }

                btnPressed = true;
            }
        }
        else if (value.canceled)
        {
            btnPressed = false;
        }
    }

    void UpdateSpeedMultiplier()
    {
        if(btnPressed && speedMultiplier < 1)
        {
            speedMultiplier += Time.deltaTime * acceleration;
        } else if(!btnPressed && speedMultiplier > 0)
        {
            speedMultiplier -= Time.deltaTime * acceleration;
            if (speedMultiplier < 0) speedMultiplier = 0;
        }
    }

    void flip()
    {
        facingLeft = !facingLeft;
        transform.Rotate(0, 180, 0);
        zRotate = -1 * zRotate;
        setaImg.rectTransform.Rotate(0, 0, zRotate);
    }

    void RotacaoSeta()
    {
        setaImg.rectTransform.eulerAngles = new Vector3(0, 0, zRotate);
    }

    void InputDeRotacao()
    {
        float inc = 0.5f;
        if (!facingLeft)
        {
            inc = -0.5f;
        }

        if (Input.GetKey(KeyCode.W))
        {
            zRotate -= inc;
        }
        if (Input.GetKey(KeyCode.S))
        {
            zRotate += inc;
        }

    }

    void Bala()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            GameObject proj = Instantiate(projetil, transform.position, Quaternion.identity) as GameObject;
            Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
            if (facingLeft == false)
            {
                anguloMira = zRotate;
                rb.AddForce(Quaternion.Euler(0, 0, anguloMira) * transform.up * impulsoDoProjetil, ForceMode2D.Impulse);
            }
            else
            {
                anguloMira = zRotate;
                rb.AddForce(Quaternion.Euler(0, 0, anguloMira) * transform.up * impulsoDoProjetil, ForceMode2D.Impulse);
            }
           
        }
                

    }
}
