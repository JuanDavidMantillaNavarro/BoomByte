using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonHoverTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public VRMenuManager menuManager;
    public string accion;

    private bool hovering;
    private bool yaEjecutado;

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovering = true;
        yaEjecutado = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovering = false;
        yaEjecutado = false;
    }

    void Update()
    {
        if (!hovering || menuManager == null) return;

        // 🔥 SOLO UNA VEZ POR CLICK
        if (!yaEjecutado && menuManager.ConfirmacionDown())
        {
            Ejecutar();
            yaEjecutado = true;
        }
    }

    void Ejecutar()
    {
        switch (accion)
        {
            case "Pausa":
                menuManager.PausarJuego();
                break;

            case "Salir":
                menuManager.MostrarSalirConfirmacion();
                break;

            case "CancelarSalir":
                menuManager.CancelarSalir();
                break;

            case "ConfirmarSalir":
                menuManager.ConfirmarSalir();
                break;

            case "Reanudar":
                menuManager.ReanudarJuego();
                break;

            case "Sonido":
                menuManager.MostrarSonido();
                break;

            case "Manual":
                menuManager.MostrarManual();
                break;
        }
    }
}