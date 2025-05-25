using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MensajeOnLevelLoad : MonoBehaviour
{
    private void Start()
    {
        MensajeInicial mensaje = FindObjectOfType<MensajeInicial>();

        if (mensaje != null)
        {
            mensaje.ForzarMostrarMensaje(); // Forzar la secuencia de mensaje
        }
        else
        {
            Debug.LogWarning("No se encontró el script MensajeInicial.");
        }
    }
}
