using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class SistemaMisiones : MonoBehaviour
{
    [Header("Configuraci�n de Objeto")]
    [SerializeField] private GameObject objetoRequerido;
    [SerializeField] private string tagObjeto = "Arma";
    [SerializeField] private Transform puntoEnMano;

    [Header("Configuraci�n de Paneles")]
    [SerializeField] private PanelDialogo panelExito;
    [SerializeField] private PanelDialogo panelFracaso;
    [SerializeField] private float tiempoCambioEscena = 3f;

    [Header("Configuraci�n de Escena")]
    [SerializeField] private string nombreSiguienteEscena;

    private bool tieneObjeto = false;
    private GameObject objetoRecogido;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (tieneObjeto)
            {
                // Mostrar mensaje de �xito y cambiar escena
                panelExito.MostrarPanel();
                StartCoroutine(CambiarEscenaDespuesDeTiempo());
            }
            else
            {
                // Mostrar mensaje de que falta el objeto
                panelFracaso.MostrarPanel();
            }
        }
    }

    private IEnumerator CambiarEscenaDespuesDeTiempo()
    {
        yield return new WaitForSeconds(tiempoCambioEscena);
        SceneManager.LoadScene(nombreSiguienteEscena);
    }

    // M�todo para llamar cuando se recoge el objeto
    public void RecogerObjeto(GameObject objeto)
    {
        if (objeto.CompareTag(tagObjeto))
        {
            tieneObjeto = true;
            objetoRecogido = objeto;

            // Mover objeto a la mano del jugador
            if (puntoEnMano != null)
            {
                objeto.transform.SetParent(puntoEnMano);
                objeto.transform.localPosition = Vector3.zero;
                objeto.transform.localRotation = Quaternion.identity;
            }
        }
    }
}

[System.Serializable]
public class PanelDialogo
{
    public GameObject panel;
    public TextMeshProUGUI texto;
    public AudioClip sonidoEscritura;
    public AudioClip sonidoAparicion;
    [TextArea(3, 10)] public string mensaje;
    public float tiempoPorLetra = 0.05f;
    public float tiempoVisible = 2f;
    public float fadeInDuration = 1f;
    public float fadeOutDuration = 1f;

    private CanvasGroup canvasGroup;
    private AudioSource audioSource;

    public void MostrarPanel()
    {
        MonoBehaviour mono = panel.GetComponent<MonoBehaviour>();
        mono.StartCoroutine(SecuenciaDialogo());
    }

    private IEnumerator SecuenciaDialogo()
    {
        InitializeComponents();

        // 1. Fade In
        yield return EfectoFade(0f, 1f, fadeInDuration, sonidoAparicion);

        // 2. Escribir texto
        yield return EscribirTexto();

        // 3. Mantener visible
        yield return new WaitForSeconds(tiempoVisible);

        // 4. Fade Out
        yield return EfectoFade(1f, 0f, fadeOutDuration, null);

        panel.SetActive(false);
    }

    private void InitializeComponents()
    {
        panel.SetActive(true);
        texto.text = "";

        canvasGroup = panel.GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = panel.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;

        audioSource = panel.GetComponent<AudioSource>();
        if (audioSource == null) audioSource = panel.AddComponent<AudioSource>();
    }

    private IEnumerator EfectoFade(float inicio, float fin, float duracion, AudioClip sonido)
    {
        if (sonido != null) audioSource.PlayOneShot(sonido);

        float timer = 0f;
        while (timer < duracion)
        {
            canvasGroup.alpha = Mathf.Lerp(inicio, fin, timer / duracion);
            timer += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = fin;
    }

    private IEnumerator EscribirTexto()
    {
        if (sonidoEscritura != null)
        {
            audioSource.clip = sonidoEscritura;
            audioSource.loop = true;
            audioSource.Play();
        }

        for (int i = 0; i <= mensaje.Length; i++)
        {
            texto.text = mensaje.Substring(0, i);
            yield return new WaitForSeconds(tiempoPorLetra);
        }

        if (audioSource != null && audioSource.loop)
        {
            audioSource.Stop();
        }
    }
}