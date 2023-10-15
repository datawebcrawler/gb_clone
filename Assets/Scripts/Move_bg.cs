using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Move_bg : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField] int speed = 5;
    float speedMultiplier;

    [Range(1, 10)]
    [SerializeField] float acceleration = 1.0f;

    bool btnPressed;
    private Vector2 movement;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        //print("#------>>> Awake ...");
    }

    private void FixedUpdate()
    {
        UpdateSpeedMultiplier();
        float targetSpeed = speed * speedMultiplier * movement.x;
        rb.velocity = new Vector2(targetSpeed, rb.velocity.y);
        //print("#------>>> FixedUpdate ...");
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
                    transform.Rotate(0, 180, 0);
                }
                btnPressed = true;
            }
        } else if (value.canceled)
        {
            btnPressed = false;
        }
        //print("#------>>> Move ...");
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
        //print("#------>>> UpdateSpeedMultiplier ...");
    }
}
