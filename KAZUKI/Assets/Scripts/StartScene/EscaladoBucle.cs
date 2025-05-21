using System.Collections;
using UnityEngine;

public class EscaladoBucle : MonoBehaviour
{
    public RectTransform panel; // Referencia al RectTransform del panel
    public Vector3 escalaMaxima = new Vector3(1.5f, 1.5f, 1f); // Tama�o m�ximo del panel
    public float velocidad = 1f; // Velocidad del escalado

    private Vector3 escalaOriginal; // Tama�o original del panel
    private bool creciendo = true; // Controla si est� creciendo o disminuyendo

    void Start()
    {
        // Guardar el tama�o original del panel
        escalaOriginal = panel.localScale;
    }

    void Update()
    {
        // Escalar el panel din�micamente
        if (creciendo)
        {
            panel.localScale = Vector3.Lerp(panel.localScale, escalaMaxima, Time.deltaTime * velocidad);
            if (Vector3.Distance(panel.localScale, escalaMaxima) < 0.01f)
            {
                creciendo = false;
            }
        }
        else
        {
            panel.localScale = Vector3.Lerp(panel.localScale, escalaOriginal, Time.deltaTime * velocidad);
            if (Vector3.Distance(panel.localScale, escalaOriginal) < 0.01f)
            {
                creciendo = true;
            }
        }
    }
}