using UnityEngine;
using UnityEngine.EventSystems;

public class SalirAlInteractuar : MonoBehaviour, IPointerClickHandler
{
    public RectTransform panel; // Referencia al RectTransform del panel

    void Update()
    {
        // Detectar si se presiona cualquier tecla y el puntero est� dentro del panel
        if (Input.anyKeyDown && PunteroDentroDelPanel())
        {
            SalirDelJuego();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Detectar clic del rat�n dentro del panel
        if (PunteroDentroDelPanel())
        {
            SalirDelJuego();
        }
    }

    private bool PunteroDentroDelPanel()
    {
        // Obtener la posici�n del rat�n en pantalla
        Vector2 mousePosition = Input.mousePosition;

        // Convertir la posici�n del rat�n al espacio local del panel
        return RectTransformUtility.RectangleContainsScreenPoint(panel, mousePosition, Camera.main);
    }

    private void SalirDelJuego()
    {
        // Salir del juego (no funciona en el editor de Unity, pero s� en compilados)
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}