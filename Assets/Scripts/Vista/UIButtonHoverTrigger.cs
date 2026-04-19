using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonHoverTrigger : MonoBehaviour, IPointerEnterHandler
{
    public VRMenuManager menuManager;
    public string accion;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Hover sobre: " + accion);

        switch (accion)
        {
            case "Sonido":
                menuManager.MostrarSonido();
                break;

            case "Salir":
                menuManager.MostrarSalirConfirmacion();
                break;

            case "Reanudar":
                menuManager.ReanudarJuego();
                break;
        }
    }
}
