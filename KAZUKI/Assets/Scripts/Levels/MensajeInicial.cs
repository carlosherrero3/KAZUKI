using UnityEngine;
using TMPro;
using System.Collections;

public class MensajeInicial: MonoBehaviour
{
    [Header("Configuración del Mensaje")]
    [SerializeField] public GameObject panelMensaje;
    [SerializeField] private TextMeshProUGUI textoMensaje;
    [TextArea(3, 10)][SerializeField] private string frase = "Escribe aquí tu mensaje inicial";
    [SerializeField] private float tiempoPorLetra = 0.05f;
    [SerializeField] private float tiempoVisibleDespues = 2f;
    [SerializeField] private float velocidadFade = 1f;
    [SerializeField] private float fadeInDuration = 1f;

    [Header("Control del Jugador")]
    [SerializeField] private MonoBehaviour[] scriptsJugador;
    [SerializeField] private AudioClip sonidoEscritura;
    [SerializeField] private bool bloquearMovimientoCamara = true;

    private CanvasGroup canvasGroup;
    private AudioSource audioSource;
    private bool bloqueoActivo = false;

    private void Awake()
    {
        // Asegurarse de que el panel está activo para configurar el CanvasGroup
        if (panelMensaje != null)
        {
            canvasGroup = panelMensaje.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = panelMensaje.AddComponent<CanvasGroup>();
            }
        }

        if (sonidoEscritura != null && audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = sonidoEscritura;
            audioSource.loop = true;
        }
    }

    private void OnEnable()
    {
        // Opcional: puedes llamar ReiniciarMensaje() aquí si quieres que siempre inicie al activarse
        // ReiniciarMensaje();
    }

    // Método público para reiniciar el mensaje desde fuera
    public void ReiniciarMensaje()
    {
        StopAllCoroutines();
        InitializeComponents();
        StartCoroutine(SequenciaCompleta());
    }

    private void InitializeComponents()
    {
        if (panelMensaje != null)
        {
            panelMensaje.SetActive(true);
            canvasGroup.alpha = 0f; // Empieza invisible
        }

        if (textoMensaje != null)
        {
            textoMensaje.text = "";
        }
    }

    private IEnumerator SequenciaCompleta()
    {
        yield return StartCoroutine(EfectoFade(0f, 1f, fadeInDuration));

        DesactivarControlesJugador(true);

        yield return StartCoroutine(EscribirTexto());

        yield return new WaitForSeconds(tiempoVisibleDespues);

        yield return StartCoroutine(EfectoFade(1f, 0f, velocidadFade));

        panelMensaje.SetActive(false);
        DesactivarControlesJugador(false);
    }

    private IEnumerator EfectoFade(float inicio, float fin, float duracion)
    {
        float tiempoTranscurrido = 0f;
        while (tiempoTranscurrido < duracion)
        {
            canvasGroup.alpha = Mathf.Lerp(inicio, fin, tiempoTranscurrido / duracion);
            tiempoTranscurrido += Time.deltaTime;
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
            yield return new WaitForSeconds(tiempoPorLetra);
        }

        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }

    private void DesactivarControlesJugador(bool desactivar)
    {
        bloqueoActivo = desactivar;

        foreach (var script in scriptsJugador)
        {
            if (script != null)
            {
                script.enabled = !desactivar;
            }
        }

        Cursor.lockState = desactivar ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = desactivar;

        if (bloquearMovimientoCamara)
        {
            var mouseLook = Camera.main.GetComponent<MonoBehaviour>();
            if (mouseLook != null)
            {
                var sensitivity = mouseLook.GetType().GetProperty("sensitivity");
                if (sensitivity != null)
                {
                    sensitivity.SetValue(mouseLook, desactivar ? 0f : 2f);
                }
            }
        }
    }

    public void SaltarEscritura()
    {
        StopAllCoroutines();
        textoMensaje.text = frase;
        StartCoroutine(DesvanecerDespuesDeSaltar());
    }

    private IEnumerator DesvanecerDespuesDeSaltar()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }

        yield return new WaitForSeconds(tiempoVisibleDespues);

        yield return StartCoroutine(EfectoFade(canvasGroup.alpha, 0f, velocidadFade));

        panelMensaje.SetActive(false);
        DesactivarControlesJugador(false);
    }

    private void Update()
    {
        if (bloqueoActivo && bloquearMovimientoCamara)
        {
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                var mouseLook = Camera.main.GetComponent<MonoBehaviour>();
                if (mouseLook != null)
                {
                    var xRotation = mouseLook.GetType().GetProperty("xRotation");
                    var yRotation = mouseLook.GetType().GetProperty("yRotation");
                    if (xRotation != null) xRotation.SetValue(mouseLook, 0f);
                    if (yRotation != null) yRotation.SetValue(mouseLook, 0f);
                }
            }
        }
    }

    public void ForzarMostrarMensaje()
    {
        StopAllCoroutines();

        if (panelMensaje != null)
        {
            panelMensaje.SetActive(true);
            canvasGroup.alpha = 0f;
        }

        if (textoMensaje != null)
        {
            textoMensaje.text = "";
        }

        StartCoroutine(SequenciaCompleta());
    }
}