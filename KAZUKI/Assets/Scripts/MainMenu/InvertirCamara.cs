using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvertirCamara : MonoBehaviour
{
    public Camera targetCamera; // C�mara que se invertir�
    public Toggle horizontalToggle; // Toggle para inversi�n horizontal
    public Toggle verticalToggle; // Toggle para inversi�n vertical

    private bool isHorizontalInverted = false;
    private bool isVerticalInverted = false;

    void Start()
    {
        // Asegurar que los toggles reflejan el estado actual
        if (horizontalToggle != null)
            horizontalToggle.onValueChanged.AddListener(ToggleInvertHorizontal);

        if (verticalToggle != null)
            verticalToggle.onValueChanged.AddListener(ToggleInvertVertical);
    }

    public void ToggleInvertHorizontal(bool value)
    {
        isHorizontalInverted = value;
        UpdateCamera();
    }

    public void ToggleInvertVertical(bool value)
    {
        isVerticalInverted = value;
        UpdateCamera();
    }

    private void UpdateCamera()
    {
        if (targetCamera == null) return;

        // Restablecer la rotaci�n a 0 inicialmente
        targetCamera.transform.localRotation = Quaternion.identity;

        // Invertir horizontalmente (rotaci�n en el eje Y)
        if (isHorizontalInverted)
            targetCamera.transform.Rotate(0f, 180f, 0f, Space.Self);

        // Invertir verticalmente (rotaci�n en el eje X)
        if (isVerticalInverted)
            targetCamera.transform.Rotate(180f, 0f, 0f, Space.Self);
    }

    private void OnDestroy()
    {
        // Eliminar los listeners al destruir el objeto
        if (horizontalToggle != null)
            horizontalToggle.onValueChanged.RemoveListener(ToggleInvertHorizontal);

        if (verticalToggle != null)
            verticalToggle.onValueChanged.RemoveListener(ToggleInvertVertical);
    }
}