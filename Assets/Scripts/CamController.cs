using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    public Transform target;
    public Vector3 posOffset;  // Configura a posição da camera manualmente.
    [Range(0.0f,10.0f)]
    public float smooth;    // Configura delay com que a camera segue o player
    Vector3 velocity;

    private void Start()
    {
        posOffset = new Vector3(0, 0, -10);
        smooth = 1.0f;
}

    private void LateUpdate()
    {
        // Lerp faz uma interpolação entre dois valores. Serve pra suavizar o movimento de um ponto a outro.
        // Aqui os valores de smooth devem tá entre 1 e 10
        // transform.position = Vector3.Lerp(transform.position, target.position + posOffset, smooth * Time.deltaTime);

        // Outra forma de fazer - aqui os valores de smooth devem estar entre 0 e 1
        transform.position = Vector3.SmoothDamp(transform.position, target.position + posOffset, ref velocity, smooth);
    }

}
