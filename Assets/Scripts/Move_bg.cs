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
    public bool facingLeft = true;

    [SerializeField] int speed = 5;
    float speedMultiplier;

    [Range(1, 10)]
    [SerializeField] float acceleration = 1.0f;

    bool btnPressed;
    private Vector2 movement;

    public GameObject seta;

    // Impulso do proj�til
    [Range (1.0f, 50.0f)]
    public float impulsoDoProjetil = 10.0f;

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
        zRotate = 84.9f;
    }

    private void Update()
    {
        InputControl();
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
        // Speed � uma grandeza escalar enquanto velocity cont�m al�m da taxa a dire��o do movimento.
        // Portanto usamos o speed para controlar a taxa com a qual o objeto se desloca em uma determinada dire��o
        UpdateSpeedMultiplier();
        float targetSpeed = speed * speedMultiplier * movement.x;
        rb.velocity = new Vector2(targetSpeed, rb.velocity.y);
    }

    public void Move(InputAction.CallbackContext value)
    {
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
            if ((5.0f < zRotate && zRotate > 0.0f) || (-5.0f > zRotate && zRotate < 0.0f))
            {
                zRotate -= inc;
            }
        }


        if (Input.GetKey(KeyCode.S))
        {
            if ((zRotate < 85.0f && zRotate > 0.0f) || (zRotate > -85.0f && zRotate < 0.0f))
            {
                zRotate += inc;
            }
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
}
