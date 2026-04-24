using UnityEngine;

public class VRButtonTrigger : MonoBehaviour
{
    public enum TipoBoton
    {
        Reanudar,
        Salir,
        Sonido,
        Pausa,
        ConfirmarSalir,
        CancelarSalir
    }

    public TipoBoton tipo;
    public VRMenuManager manager;

    private bool activado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (activado) return;

        if (other.CompareTag("Hand"))
        {
            activado = true;
            Ejecutar();
        }
    }

    void Ejecutar()
    {
        switch (tipo)
        {
            case TipoBoton.Reanudar:
                manager.ReanudarJuego();
                break;

            case TipoBoton.Pausa:
                manager.PausarJuego();
                break;

            case TipoBoton.Sonido:
                manager.MostrarSonido();
                break;

            case TipoBoton.Salir:
                manager.MostrarSalirConfirmacion();
                break;

            case TipoBoton.ConfirmarSalir:
                manager.SalirJuego();
                break;

            case TipoBoton.CancelarSalir:
                manager.CancelarSalir();
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            activado = false;
        }
    }
}

