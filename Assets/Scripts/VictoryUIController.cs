using UnityEngine;
using UnityEngine.InputSystem;

public class VictoryUIController : MonoBehaviour
{
    [Header("UI")]
    public GameObject panelVictoria;

    [Header("Referencias")]
    public Transform puntoInicio;
    public Transform player;

    private bool activo = false;

    void Start()
    {
        if (panelVictoria != null)
            panelVictoria.SetActive(false);
    }

    void Update()
    {
        if (!activo) return;

        bool tecla = Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame;

        bool botonA = false;
        if (Gamepad.current != null)
        {
            botonA = Gamepad.current.buttonSouth.wasPressedThisFrame; // A
        }

        if (tecla || botonA)
        {
            CerrarPanel();
        }
    }

    public void MostrarVictoria()
    {
        activo = true;

        if (panelVictoria != null)
            panelVictoria.SetActive(true);

        Time.timeScale = 0f;
    }

    void CerrarPanel()
    {
        activo = false;

        if (panelVictoria != null)
            panelVictoria.SetActive(false);

        Time.timeScale = 1f;

        // opcional: reiniciar posici�n
        if (player != null && puntoInicio != null)
        {
            player.position = puntoInicio.position;
            player.rotation = puntoInicio.rotation;
        }
        GameController.Instance.ReiniciarEstado();
    }
}

