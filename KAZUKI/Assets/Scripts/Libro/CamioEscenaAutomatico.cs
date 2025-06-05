using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CamioEscenaAutomatico : MonoBehaviour
{
    public string nextScene = "Nivel 2b";
    public float delay = 5f;

    void Start()
    {
        StartCoroutine(LoadNextSceneAfterDelay());
    }

    IEnumerator LoadNextSceneAfterDelay()
    {
        yield return new WaitForSeconds(delay);

        if (SceneManager.GetActiveScene().name != nextScene)
        {
            Debug.Log("Cambiando a escena: " + nextScene);
            SceneManager.LoadScene(nextScene);
        }
    }
}