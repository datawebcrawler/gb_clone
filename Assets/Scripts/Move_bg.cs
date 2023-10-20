using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class Move_bg : MonoBehaviour
{
    // Seta na qual indica a direção na qual o projeto será lançado
    [SerializeField] Image setaImg;
    public float zRotate;
    
    Rigidbody2D rb;

    
    [SerializeField] int speed = 5;
    float speedMultiplier;

    [Range(1, 10)]
    [SerializeField] float acceleration = 1.0f;

    bool btnPressed;
    private Vector2 movement;

    // Impulso do projétil
    [Range (1.0f, 30.0f)]
    public float fireImpulse = 10.0f;

    // Projectile GameObject Component;
    public GameObject projectile;
    private GameObject projectileInstance = null;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
    }

    private void FixedUpdate()
    {
        // Speed é uma grandeza escalar enquanto velocity contém além da taxa a direção do movimento.
        // Portanto usamos o speed para controlar a taxa com a qual o objeto se desloca em uma determinada direção
        UpdateSpeedMultiplier();
        UpdateVelocity();
    }

    private void UpdateVelocity()
    {
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
        /// <summary>Este método é responsável pelo lançamento de projéteis do mobile.
        /// <para>
        /// Ele pega a posição do canhão posiciona a bala em seguida instancia o objeto e lança
        /// Precisa ser configurado o ângulo de disparo, a velocidade, aceleração
        /// Quando ele atinge o solo é destuído (removido da memória)
        /// </para>
        /// </summary>
        // Aqui pegamos a posição da seta para posicionar o ponto de largada do projétil
        if (projectileInstance == null)
        {
            projectileInstance = Instantiate(projectile, setaImg.transform.position, Quaternion.identity) as GameObject;
            Rigidbody2D rb = projectileInstance.GetComponent<Rigidbody2D>();
            rb.AddForce(Quaternion.Euler(0, 0, zRotate) * transform.up * fireImpulse, ForceMode2D.Impulse);
            Quaternion targetrotation = Quaternion.LookRotation(movement);
            targetrotation = Quaternion.RotateTowards(setaImg.transform.rotation, targetrotation, 360 * Time.fixedDeltaTime);
            rb.MoveRotation(targetrotation);
        }
    }
}
