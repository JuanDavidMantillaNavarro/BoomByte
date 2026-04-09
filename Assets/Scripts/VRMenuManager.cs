using UnityEngine;

public class VRMenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject radialMenu;
    public GameObject panelSonido;
    public GameObject panelSalir;

    public void MostrarRadial()
    {
        radialMenu.SetActive(true);
        panelSonido.SetActive(false);
        panelSalir.SetActive(false);
    }

    public void MostrarSonido()
    {
        radialMenu.SetActive(false);
        panelSonido.SetActive(true);
        panelSalir.SetActive(false);
    }

    public void MostrarSalirConfirmacion()
    {
        radialMenu.SetActive(false);
        panelSonido.SetActive(false);
        panelSalir.SetActive(true);
    }

    public void ReanudarJuego()
    {
        Time.timeScale = 1;
        OcultarTodo();
    }

    public void PausarJuego()
    {
        Time.timeScale = 0;
        MostrarRadial();
    }

    public void SalirJuego()
    {
        Debug.Log("Salir confirmado");
        Application.Quit();
    }

    public void CancelarSalir()
    {
        Debug.Log("Cancelar salida");
        MostrarRadial();
    }

    public void OcultarTodo()
    {
        radialMenu.SetActive(false);
        panelSonido.SetActive(false);
        panelSalir.SetActive(false);
    }
}