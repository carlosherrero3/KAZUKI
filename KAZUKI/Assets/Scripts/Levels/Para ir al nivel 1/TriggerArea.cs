using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerArea : MonoBehaviour
{
    public string sceneName;                // Nombre de la escena a cargar
    public GameObject uiPanel;              // Panel que se mostrará
    public float delayBeforeSceneLoad = 2f; // Tiempo antes de cargar la escena

    private bool yaActivado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!yaActivado && other.CompareTag("Player"))
        {
            yaActivado = true;
            StartCoroutine(ShowPanelAndLoad());
        }
    }

    private IEnumerator ShowPanelAndLoad()
    {
        if (uiPanel != null)
            uiPanel.SetActive(true);  // Muestra el panel

        yield return new WaitForSeconds(delayBeforeSceneLoad);

        if (uiPanel != null)
            uiPanel.SetActive(false); // Oculta el panel (opcional)

        SceneManager.LoadScene(sceneName);
    }
}
