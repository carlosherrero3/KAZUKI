using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ObjetoInteractivo : MonoBehaviour
{

    [Header("Configuración de UI")]
    [SerializeField] private RawImage rawImage;
    [SerializeField] private float fadeDuration = 0.5f;

    [Header("Configuración de Escenas")]
    [SerializeField] private string primeraEscena;
    [SerializeField] private float tiempoEnPrimeraEscena = 5f; // 5 segundos fijos
    [SerializeField] private string escenaFinal = "Nivel 2b";

    private bool jugadorEnRango = false;
    private CanvasGroup rawImageCanvasGroup;

    private void Start()
    {
        // Configurar el RawImage
        if (rawImage != null)
        {
            rawImageCanvasGroup = rawImage.GetComponent<CanvasGroup>();
            if (rawImageCanvasGroup == null)
            {
                rawImageCanvasGroup = rawImage.gameObject.AddComponent<CanvasGroup>();
            }
            rawImageCanvasGroup.alpha = 0f;
            rawImage.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnRango = true;
            MostrarRawImage();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnRango = false;
            OcultarRawImage();
        }
    }

    private void Update()
    {
        if (jugadorEnRango && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(CambiarEscenas());
        }
    }

    private void MostrarRawImage()
    {
        if (rawImage == null) return;

        rawImage.gameObject.SetActive(true);
        StartCoroutine(FadeRawImage(0f, 1f, fadeDuration));
    }

    private void OcultarRawImage()
    {
        if (rawImage == null) return;

        StartCoroutine(FadeRawImage(1f, 0f, fadeDuration, () =>
        {
            rawImage.gameObject.SetActive(false);
        }));
    }

    private IEnumerator FadeRawImage(float from, float to, float duration, System.Action onComplete = null)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            rawImageCanvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        rawImageCanvasGroup.alpha = to;
        onComplete?.Invoke();
    }

    private IEnumerator CambiarEscenas()
    {
        // Cargar la primera escena
        SceneManager.LoadScene(primeraEscena);

        // Esperar exactamente 5 segundos
        yield return new WaitForSeconds(5f);

        // Cargar Nivel 2b automáticamente
        SceneManager.LoadScene("Nivel 2b");
    }
}