using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class Move_bg : MonoBehaviour
{
    // Posi��o seta
    [SerializeField] Transform posStart;
    [SerializeField] Image setaImg;
    public float zRotate;
    public float anguloMira;
    
    Rigidbody2D rb;

    [SerializeField] int speed = 5;
    float speedMultiplier;

    [Range(1, 10)]
    [SerializeField] float acceleration = 1.0f;

    bool btnPressed;
    private Vector2 movement;

    public GameObject seta;

    // Impulso do proj�til
    [Range (1.0f, 50.0f)]
    public float impulsoDoProjetil = 20.0f;

    //variavel da instancia do projetil
    public GameObject projetil;
    GameObject projectileInstance = null;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        posStart = GetComponent<Transform>();

    }

    private void Start()
    {
        movement = new Vector2(-1.0f, 0.0f);
        zRotate = 90;
    }

    private void Update()
    {
        RotacaoSeta();
        InputControl();
        RotacaoBala();
  
    }

    private void RotacaoBala()
    {

        if (projectileInstance != null) 
        {

            Rigidbody2D rb_proj = projectileInstance.GetComponent<Rigidbody2D>(); 
            Vector3 v = rb_proj.velocity;
            float angle = (Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg) - 90.0f;
            projectileInstance.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        }

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
        transform.Rotate(0, 180, 0);
        zRotate = -1 * zRotate;
        setaImg.rectTransform.Rotate(0, 0, zRotate);
    }

    void RotacaoSeta()
    {
        setaImg.rectTransform.eulerAngles = Quaternion.Euler(0, 0, zRotate).eulerAngles;

    }

    void InputControl()
    {
        float inc = 0.5f;
        if (zRotate < 0)
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
        if (Input.GetButtonDown("Fire1"))
        {
            Fire();
        }
    }

    void Fire()
    {

        if(projectileInstance == null)
        {
            
            projectileInstance = Instantiate(projetil, setaImg.transform.position, Quaternion.identity) as GameObject;
            Rigidbody2D rb = projectileInstance.GetComponent<Rigidbody2D>();
            rb.AddForce(Quaternion.Euler(0, 0, zRotate) * transform.up * impulsoDoProjetil, ForceMode2D.Impulse);


        }
                

    }

    //Rota��o do player no momento que colide com o terreno
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            posStart.transform.localScale = new Vector3(Mathf.CeilToInt(zRotate/Mathf.Abs(zRotate)), 1, 1);

        }
    }

}
