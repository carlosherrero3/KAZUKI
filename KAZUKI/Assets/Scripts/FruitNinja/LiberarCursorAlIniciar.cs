using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiberarCursorAlIniciar : MonoBehaviour
{
    void Start()
    {
        // Mostrar el cursor y desbloquearlo
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
