using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arma : MonoBehaviour
{
    // Vari�vel para armazenar o prefab do proj�til
    public GameObject projetil;
    public Move_bg mb;

    // Impulso do proj�til
    public float impulsoDoProjetil = 20.0f;

    // �ngulo de lan�amento do proj�til
    public float anguloDeLancamento = 45.0f;


    // M�todo para disparar o proj�til
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
