using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Move_bg : MonoBehaviour
{
    // Posição seta
    [SerializeField] Transform posStart;
    [SerializeField] Image setaImg;
    public float zRotate;

    Rigidbody2D rb;
    private bool facingLeft = true;

    [SerializeField] int speed = 5;
    float speedMultiplier;

    [Range(1, 10)]
    [SerializeField] float acceleration = 1.0f;

    bool btnPressed;
    private Vector2 movement;

    public GameObject seta;

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
}
