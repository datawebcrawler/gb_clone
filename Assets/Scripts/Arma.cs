using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arma : MonoBehaviour
{
    // Variável para armazenar o prefab do projétil
    public GameObject projetil;
    public Move_bg mb;

    // Impulso do projétil
    public float impulsoDoProjetil = 20.0f;

    // Ângulo de lançamento do projétil
    public float anguloDeLancamento = 45.0f;


    // Método para disparar o projétil
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            GameObject proj = Instantiate(projetil, transform.position, Quaternion.identity) as GameObject;
            Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
            if (mb.facingLeft == false)
            {
                rb.AddForce(Quaternion.Euler(0, 0, anguloDeLancamento) * transform.right * impulsoDoProjetil, ForceMode2D.Impulse);
            }
            else
            {
                rb.AddForce(Quaternion.Euler(0, 0, anguloDeLancamento) * -transform.right * impulsoDoProjetil, ForceMode2D.Impulse);
            }
        }

    }

}
