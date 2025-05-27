using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestaurarPosicion : MonoBehaviour
{
    void Start()
    {
        string escenaActual = SceneManager.GetActiveScene().name;
        string ultimaEscena = PlayerPrefs.GetString("UltimaEscena", "");

        if (escenaActual == ultimaEscena)
        {
            float x = PlayerPrefs.GetFloat("PosX", transform.position.x);
            float y = PlayerPrefs.GetFloat("PosY", transform.position.y);
            float z = PlayerPrefs.GetFloat("PosZ", transform.position.z);

            transform.position = new Vector3(x, y, z);
        }
    }
}
