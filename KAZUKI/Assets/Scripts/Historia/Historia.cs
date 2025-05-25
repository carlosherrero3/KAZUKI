using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Historia : MonoBehaviour
{
    public TextMeshProUGUI historiaTexto;
    [TextArea(3, 10)]
    public string[] parrafos;
    public float velocidadEscritura = 0.05f;

    private int indice = 0;
    private bool escribiendo = false;
    private bool finDelTexto = false;

    [System.Serializable]
    public class EventoFade
    {
        public int lineaDeTexto;
        public int indiceImagen;
    }

    public List<EventoFade> eventosFade;
    public FadeIn fadeInScript;

    // 🎵 Sonido de clic
    public AudioSource audioSource;
    public AudioClip sonidoClick;

    void Start()
    {
        StartCoroutine(EscribirTexto(parrafos[indice]));
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 🎵 Reproducir sonido de clic
            if (sonidoClick != null && audioSource != null)
                audioSource.PlayOneShot(sonidoClick);

            if (escribiendo)
            {
                StopAllCoroutines();
                historiaTexto.text = parrafos[indice];
                escribiendo = false;
            }
            else
            {
                if (finDelTexto)
                {
                    SceneManager.LoadScene("Nivel 1"); // Cambia por el nombre real de tu escena
                    return;
                }

                indice++;
                if (indice < parrafos.Length)
                {
                    StartCoroutine(EscribirTexto(parrafos[indice]));

                    // Ejecutar fade si aplica
                    foreach (var evento in eventosFade)
                    {
                        if (evento.lineaDeTexto == indice)
                        {
                            fadeInScript.DesvanecerImagenPorIndice(evento.indiceImagen);
                        }
                    }
                }
                else
                {
                    historiaTexto.text = "";
                    finDelTexto = true;
                }
            }
        }
    }

    IEnumerator EscribirTexto(string texto)
    {
        escribiendo = true;
        historiaTexto.text = "";

        foreach (char letra in texto.ToCharArray())
        {
            historiaTexto.text += letra;
            yield return new WaitForSeconds(velocidadEscritura);
        }

        escribiendo = false;
    }
}