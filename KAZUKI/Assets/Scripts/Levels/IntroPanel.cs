using UnityEngine;
using TMPro;
using System.Collections;

public class InicialPanel : MonoBehaviour
{
    [Header("Configuración del Mensaje")]
    [SerializeField] private GameObject panelMensaje;
    [SerializeField] private TextMeshProUGUI textoMensaje;
    [TextArea(3, 10)][SerializeField] private string frase = "Escribe aquí tu mensaje inicial";
    [SerializeField] private float tiempoPorLetra = 0.05f;
    [SerializeField] private float tiempoVisibleDespues = 2f;
    [SerializeField] private float velocidadFade = 1f;
    [SerializeField] private float fadeInDuration = 1f;

    [Header("Configuración de Audio")]
    [SerializeField] private AudioClip sonidoEscritura;
    [Range(0, 1)][SerializeField] private float volumenAudio = 0.5f;

    [Header("Control del Jugador")]
    [SerializeField] private MonoBehaviour[] scriptsJugador;
    [SerializeField] private bool bloquearMovimientoCamara = true;
    [SerializeField] private string nombreEscenaObjetivo = "Nivel 1";

    private CanvasGroup canvasGroup;
    private AudioSource audioSource;
    private bool mensajeMostrado = false;
    private static bool mostradoEnEstaEscena = false;

    private void Awake()
    {
        // Verificar si estamos en la escena correcta
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != nombreEscenaObjetivo)
        {
            this.enabled = false;
            return;
        }

        // Verificar si ya se mostró en esta escena
        if (mostradoEnEstaEscena)
        {
            this.enabled = false;
            return;
        }

        // Configurar componentes
        if (panelMensaje != null)
        {
            canvasGroup = panelMensaje.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = panelMensaje.AddComponent<CanvasGroup>();
            }
            panelMensaje.SetActive(false);
        }

        if (sonidoEscritura != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = sonidoEscritura;
            audioSource.loop = true;
            audioSource.volume = volumenAudio;
        }
    }

    private void Start()
    {
        // Solo mostrar si estamos en la escena correcta y no se ha mostrado antes
        if (!mensajeMostrado && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == nombreEscenaObjetivo && !mostradoEnEstaEscena)
        {
            MostrarMensaje();
        }
    }

    private void MostrarMensaje()
    {
        mostradoEnEstaEscena = true;
        mensajeMostrado = true;
        StartCoroutine(SequenciaCompleta());
    }

    private IEnumerator SequenciaCompleta()
    {
        // Inicializar componentes
        panelMensaje.SetActive(true);
        canvasGroup.alpha = 0f;
        textoMensaje.text = "";

        // Fade in
        yield return StartCoroutine(EfectoFade(0f, 1f, fadeInDuration));

        // Bloquear controles
        DesactivarControlesJugador(true);

        // Escribir texto
        yield return StartCoroutine(EscribirTexto());

        // Esperar tiempo adicional
        yield return new WaitForSecondsRealtime(tiempoVisibleDespues);

        // Fade out
        yield return StartCoroutine(EfectoFade(1f, 0f, velocidadFade));

        // Finalizar
        panelMensaje.SetActive(false);
        DesactivarControlesJugador(false);
    }

    private IEnumerator EfectoFade(float inicio, float fin, float duracion)
    {
        float tiempoTranscurrido = 0f;
        while (tiempoTranscurrido < duracion)
        {
            canvasGroup.alpha = Mathf.Lerp(inicio, fin, tiempoTranscurrido / duracion);
            tiempoTranscurrido += Time.unscaledDeltaTime;
            yield return null;
        }
        canvasGroup.alpha = fin;
    }

    private IEnumerator EscribirTexto()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }

        for (int i = 0; i <= frase.Length; i++)
        {
            textoMensaje.text = frase.Substring(0, i);
            yield return new WaitForSecondsRealtime(tiempoPorLetra);
        }

        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }

    private void DesactivarControlesJugador(bool desactivar)
    {
        // Pausar el tiempo del juego
        Time.timeScale = desactivar ? 0f : 1f;

        // Desactivar scripts del jugador
        foreach (var script in scriptsJugador)
        {
            if (script != null)
            {
                script.enabled = !desactivar;
            }
        }

        // Control del cursor
        Cursor.lockState = desactivar ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = desactivar;

        // Bloquear movimiento de cámara
        if (bloquearMovimientoCamara)
        {
            var mouseLookScripts = FindObjectsOfType<MonoBehaviour>();
            foreach (var script in mouseLookScripts)
            {
                if (script.GetType().ToString().Contains("MouseLook"))
                {
                    var sensitivity = script.GetType().GetField("sensitivity");
                    if (sensitivity != null)
                    {
                        sensitivity.SetValue(script, desactivar ? 0f : 2f);
                    }
                }
            }
        }
    }

    private void Update()
    {
        // Permitir saltar la secuencia con clic
        if (mensajeMostrado && panelMensaje.activeSelf && Input.GetMouseButtonDown(0))
        {
            StopAllCoroutines();
            textoMensaje.text = frase;
            if (audioSource != null) audioSource.Stop();
            StartCoroutine(OcultarMensajeRapido());
        }
    }

    private IEnumerator OcultarMensajeRapido()
    {
        yield return StartCoroutine(EfectoFade(canvasGroup.alpha, 0f, 0.5f));
        panelMensaje.SetActive(false);
        DesactivarControlesJugador(false);
    }
}