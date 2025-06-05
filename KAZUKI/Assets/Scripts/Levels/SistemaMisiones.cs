using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class SistemaMisiones : MonoBehaviour
{
    [Header("Configuración de Objeto")]
    [SerializeField] private GameObject objetoRequerido;
    [SerializeField] private string tagObjeto = "Arma";
    [SerializeField] private Transform puntoEnMano;

    [Header("Configuración de Paneles")]
    [SerializeField] private PanelDialogo panelExito;
    [SerializeField] private PanelDialogo panelFracaso;
    [SerializeField] private float tiempoCambioEscena = 3f;

    [Header("Configuración de Escena")]
    [SerializeField] private string nombreSiguienteEscena;

    private bool tieneObjeto = false;
    private GameObject objetoRecogido;
    private bool jugadorCerca = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
        }
    }

    private void Update()
    {
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            if (tieneObjeto)
            {
                panelExito.MostrarPanel();
                StartCoroutine(CambiarEscenaDespuesDeTiempo());
            }
            else
            {
                panelFracaso.MostrarPanel();
            }
        }
    }

    private IEnumerator CambiarEscenaDespuesDeTiempo()
    {
        yield return new WaitForSeconds(tiempoCambioEscena);

        // Ocultar ambos paneles antes de cambiar de escena
        if (panelExito != null && panelExito.panel != null)
        {
            panelExito.panel.SetActive(false);
        }
        if (panelFracaso != null && panelFracaso.panel != null)
        {
            panelFracaso.panel.SetActive(false);
        }

        SceneManager.LoadScene(nombreSiguienteEscena);
    }

    // Método para llamar cuando se recoge el objeto
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
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Si la escena cargada es FruitNinja, ocultar los paneles
        if (scene.name == nombreSiguienteEscena)
        {
            if (panelExito != null && panelExito.panel != null)
            {
                panelExito.panel.SetActive(false);
            }
            if (panelFracaso != null && panelFracaso.panel != null)
            {
                panelFracaso.panel.SetActive(false);
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
        if (panel == null) return;

        MonoBehaviour mono = panel.GetComponent<MonoBehaviour>();
        if (mono == null) return;

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