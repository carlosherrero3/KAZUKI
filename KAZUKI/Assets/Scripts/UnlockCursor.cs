using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockCursor : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.None; // Desbloquea el cursor
        Cursor.visible = true;                  // Muestra el cursor
    }
}
