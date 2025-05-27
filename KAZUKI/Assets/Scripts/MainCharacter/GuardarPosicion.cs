using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardarPosicion : MonoBehaviour
{
    public void Guardar()
    {
        Vector3 pos = transform.position;
        PlayerPrefs.SetFloat("PosX", pos.x);
        PlayerPrefs.SetFloat("PosY", pos.y);
        PlayerPrefs.SetFloat("PosZ", pos.z);
        PlayerPrefs.SetString("UltimaEscena", UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();
    }
}
