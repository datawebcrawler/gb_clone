using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamante : MonoBehaviour
{


    void OnCollisionEnter2D(Collision2D collision)
    {
        // Verificar se o proj�til n�o est� colidindo com o jogador
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            // Destruir o proj�til
            Destroy(gameObject);
        }
    }


}
