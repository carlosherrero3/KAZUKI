using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ImagenConClicks
{
    public Image imagen;
    public int clicsParaDesvanecer = 1;
    [HideInInspector] public bool yaDesvanecida = false;
}

public class FadeIn : MonoBehaviour
{
    public List<ImagenConClicks> capasNegras;
    public float duracionFade = 1.5f;

    private int clicsTotales = 0;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clicsTotales++;

            foreach (var capa in capasNegras)
            {
                if (!capa.yaDesvanecida && clicsTotales >= capa.clicsParaDesvanecer)
                {
                    StartCoroutine(FadeOut(capa));
                    break; // Solo desvanece una por clic
                }
            }
        }
    }

    public void DesvanecerImagenPorIndice(int indice)
    {
        if (indice >= 0 && indice < capasNegras.Count)
        {
            var capa = capasNegras[indice];
            if (!capa.yaDesvanecida)
            {
                StartCoroutine(FadeOut(capa));
            }
        }
    }

    IEnumerator FadeOut(ImagenConClicks capa)
    {
        capa.yaDesvanecida = true;
        Image imagen = capa.imagen;
        Color color = imagen.color;
        float tiempo = 0f;

        while (tiempo < duracionFade)
        {
            tiempo += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, tiempo / duracionFade);
            imagen.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        imagen.color = new Color(color.r, color.g, color.b, 0f);
    }
}