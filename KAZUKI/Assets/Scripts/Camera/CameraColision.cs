using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraColision : MonoBehaviour
{
    public Transform target; // Objeto que la c�mara sigue (generalmente el jugador)
    public float smoothSpeed = 0.1f; // Suavidad del movimiento
    public float cameraDistance = 5f; // Distancia deseada de la c�mara al objetivo

    private Vector3 cameraOffset;

    void Start()
    {
        cameraOffset = transform.position - target.position;
    }

    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + cameraOffset.normalized * cameraDistance;

        // Realiza un raycast desde el objetivo hacia la c�mara
        RaycastHit hit;
        if (Physics.Raycast(target.position, -cameraOffset.normalized, out hit, cameraDistance))
        {
            // Si hay colisi�n, ajusta la c�mara al punto de impacto
            desiredPosition = hit.point + cameraOffset.normalized * 0.2f;
        }

        // Mueve suavemente la c�mara a la posici�n deseada
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.LookAt(target);
    }
}
