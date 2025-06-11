using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangerInventario : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = "Inventario";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ChangeScene();
        }
    }

    private void ChangeScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("El nombre de la escena no está especificado.");
        }
    }
}
