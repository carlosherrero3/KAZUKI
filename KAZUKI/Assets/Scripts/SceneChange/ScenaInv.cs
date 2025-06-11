using UnityEngine;
using UnityEngine.SceneManagement;


public class ScenaInv : MonoBehaviour

{
    public string sceneName; // Nombre de la escena a cargar (configurable en el Inspector)

    void Update()
    {
        // Si se presiona la tecla I, carga la escena especificada
        if (Input.GetKeyDown(KeyCode.I))
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}