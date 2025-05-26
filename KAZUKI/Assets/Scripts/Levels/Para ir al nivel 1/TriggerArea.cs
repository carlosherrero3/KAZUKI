using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerArea : MonoBehaviour
{
    public string sceneName;               // Nombre de la escena a cargar
    public GameObject uiPanel;             // Panel que se mostrará
    public float delayBeforeSceneLoad = 2f; // Tiempo antes de cargar la escena (opcional)

    private bool isPlayerInside = false;

    void Update()
    {
        if (isPlayerInside && Input.GetKeyDown(KeyCode.E))
        {
            if (uiPanel != null)
            {
                uiPanel.SetActive(true);
            }
            Invoke("LoadScene", delayBeforeSceneLoad); // Espera antes de cargar la escena
        }
    }

    void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
        }
    }
}
