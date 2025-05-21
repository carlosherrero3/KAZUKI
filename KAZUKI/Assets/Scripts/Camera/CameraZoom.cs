using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour

{
    public Camera targetCamera; // C�mara a la que se aplicar� el zoom
    public float zoomFOV = 30f; // Campo de visi�n al hacer zoom
    public float normalFOV = 60f; // Campo de visi�n normal
    public float zoomSpeed = 5f; // Velocidad del zoom

    private bool isZoomed = false; // Estado del zoom

    void Start()
    {
        // Asegurarse de que la c�mara tenga el FOV normal al inicio
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }

        targetCamera.fieldOfView = normalFOV;
    }

    void Update()
    {
        // Detectar la tecla F para alternar el zoom
        if (Input.GetKeyDown(KeyCode.F))
        {
            isZoomed = !isZoomed;
        }

        // Aplicar el FOV con interpolaci�n suave
        float targetFOV = isZoomed ? zoomFOV : normalFOV;
        targetCamera.fieldOfView = Mathf.Lerp(targetCamera.fieldOfView, targetFOV, Time.deltaTime * zoomSpeed);
    }
}