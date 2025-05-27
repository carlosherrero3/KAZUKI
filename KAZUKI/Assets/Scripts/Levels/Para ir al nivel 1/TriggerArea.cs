using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerArea : MonoBehaviour
{
    public string sceneName;               // Nombre de la escena a cargar
    public GameObject uiPanel;             // Panel que se mostrará
    public float delayBeforeSceneLoad = 2f; // Tiempo antes de cargar la escena

    private bool yaActivado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!yaActivado && other.CompareTag("Player"))
        {
            yaActivado = true;

            if (uiPanel != null)
            {
                uiPanel.SetActive(true);
            }

            Invoke("LoadScene", delayBeforeSceneLoad);
        }
    }

    void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
