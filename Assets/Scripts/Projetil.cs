using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projetil : MonoBehaviour
{

    private void Update()
    {

        if (gameObject.transform.position.x < -50 || gameObject.transform.position.x > 50 || gameObject.transform.position.y < -10)
        {

            Destroy(gameObject);

        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Verificar se o projétil não está colidindo com o jogador
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            // Destruir o projétil
            Destroy(gameObject);
        }

    }


}
