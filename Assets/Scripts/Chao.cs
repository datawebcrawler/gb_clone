using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chao : MonoBehaviour
{
    public Texture2D baseTexture;
    Texture2D cloneTexture;
    SpriteRenderer sr;

    float widthWorld, heightWorld;
    int widthPixel, heightPixel;

    public float WidthWorld
    {
        get
        {
            if (widthWorld == 0)
                widthWorld = sr.bounds.size.x;
            return widthWorld;
        }

    }
    public float HeightWorld
    {
        get
        {
            if (heightWorld == 0)
                heightWorld = sr.bounds.size.y;
            return heightWorld;
        }

    }
    public int WidthPixel
    {
        get
        {
            if (widthPixel == 0)
                widthPixel = sr.sprite.texture.width;

            return widthPixel;
        }
    }
    public int HeightPixel
    {
        get
        {
            if (heightPixel == 0)
                heightPixel = sr.sprite.texture.height;

            return heightPixel;
        }
    }


    // Use this for initialization
    void Start()
    {


        sr = GetComponent<SpriteRenderer>();        
        cloneTexture = Instantiate(baseTexture);
        cloneTexture.alphaIsTransparency = true;

        if (cloneTexture.format != TextureFormat.RGBA32)
            Debug.LogWarning("Texture must be RGBA32");
        if (cloneTexture.wrapMode != TextureWrapMode.Clamp)
            Debug.LogWarning("wrapMode must be Clamp");

        UpdateTexture();
        gameObject.AddComponent<PolygonCollider2D>();

    }

    void MakeAHole(CircleCollider2D col)
    {
        print(string.Format("{0},{1},{2},{3}", WidthPixel, HeightPixel, WidthWorld, heightWorld)); //Log

        Vector2Int centro = World2Pixel(col.bounds.center); // Pega o vetor do centro da colisão com as coordenadas do obejto colidindo(col)
        int raio = Mathf.RoundToInt(col.bounds.size.x * WidthPixel / WidthWorld); // define o raio de acordo com a largura do collider###não entendi o final###

        int x_positivo, x_negativo, y_positivo, y_negativo, distancia_pixel; // define as variaveis pra decidir o que fica transparente ou não
        for (int i = 0; i <= raio; i++) //itera por todos os pixels do quadrado para saber quais estão dentro do circulo.
        {
            distancia_pixel = Mathf.RoundToInt(Mathf.Sqrt(raio * raio - i * i)); //Teorema de Pitagoras pra marcar quais pixels ficam transparentes a partir do raio e x do triangulo
            for (int j = 0; j <= distancia_pixel; j++)
            {
                x_positivo = centro.x + i;
                x_negativo = centro.x - i;
                y_positivo = centro.y + j;
                y_negativo = centro.y - j;

                cloneTexture.SetPixel(x_positivo, y_positivo, Color.clear); //1º Quadrante
                cloneTexture.SetPixel(x_negativo, y_positivo, Color.clear); //2º
                cloneTexture.SetPixel(x_positivo, y_negativo, Color.clear); //3º
                cloneTexture.SetPixel(x_negativo, y_negativo, Color.clear); //4º
            }
        }
        cloneTexture.Apply(); //Aplica as mudanças na textura clonada
        UpdateTexture(); //Atualiza a textura para a textura que foi clonada nesse método

        Destroy(gameObject.GetComponent<PolygonCollider2D>()); // destroi o colisor do cenário
        gameObject.AddComponent<PolygonCollider2D>(); // recria o colisor do cenário

    }



    void UpdateTexture()
    {
        sr.sprite = Sprite.Create(cloneTexture,
                            new Rect(0, 0, cloneTexture.width, cloneTexture.height),
                            new Vector2(0.5f, 0.5f),
                            50f
                            );
    }

    Vector2Int World2Pixel(Vector2 pos)
    {
        Vector2Int v = Vector2Int.zero;

        var dx = (pos.x - transform.position.x);
        var dy = (pos.y - transform.position.y);

        v.x = Mathf.RoundToInt(0.5f * WidthPixel + dx * (WidthPixel / WidthWorld));
        v.y = Mathf.RoundToInt(0.5f * HeightPixel + dy * (HeightPixel / HeightWorld));

        return v;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (!collision.CompareTag("Explosivo"))
            return;
        if (!collision.GetComponent<CircleCollider2D>())
            return;

        MakeAHole(collision.GetComponent<CircleCollider2D>());
        Destroy(collision.gameObject, 0.1f);
    }

}
