using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    [Header("Configuración")]
    public string sceneName; // Nombre de la escena a cargar
    public Sprite interactionImage; // Imagen a mostrar

    [Header("Referencias UI")]
    public Image uiImage; // Referencia al componente Image de la UI
    public GameObject interactionPanel; // Panel que contiene la imagen y posible texto

    private bool isPlayerInRange = false;

    private void Start()
    {
        // Asegurarse de que el panel está desactivado al inicio
        if (interactionPanel != null)
        {
            interactionPanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            // Cargar la escena especificada
            if (!string.IsNullOrEmpty(sceneName))
            {
                SceneManager.LoadScene(sceneName);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;

            // Mostrar la imagen y el panel
            if (interactionPanel != null)
            {
                interactionPanel.SetActive(true);

                // Asignar la imagen si existe
                if (uiImage != null && interactionImage != null)
                {
                    uiImage.sprite = interactionImage;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;

            // Ocultar el panel
            if (interactionPanel != null)
            {
                interactionPanel.SetActive(false);
            }
        }
    }
}