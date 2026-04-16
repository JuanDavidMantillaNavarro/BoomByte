using UnityEngine;
using UnityEngine.InputSystem;

public class VRMenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject radialMenu;
    public GameObject panelSonido;
    public GameObject panelSalir;
    public Transform puntoInicio;

    [Header("Player")]
    public Transform playerCamera;

    [Header("Ray Menu")]
    public GameObject menuRayInteractor;

    [Header("Menu Position")]
    public float distanceFromCamera = 1.5f;
    public float heightOffset = -0.2f;

    private bool menuAbierto = false;

    void Start()
    {
        OcultarTodo();
    }

    void Update()
    {
        DetectarInputMenu();
    }

    void DetectarInputMenu()
    {
        // teclado simulador
        bool teclado = Keyboard.current != null && Keyboard.current.bKey.wasPressedThisFrame;

        // VR control derecho botón B
        bool vrBotonB = false;

        if (Gamepad.current != null)
        {
            vrBotonB = Gamepad.current.buttonEast.wasPressedThisFrame;
        }

        if (teclado || vrBotonB)
        {
            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {
        menuAbierto = !menuAbierto;

        if (menuAbierto)
        {
            MostrarRadial();
            PosicionarMenuFrenteJugador();
            GameController.Instance.TogglePause();

            if (menuRayInteractor != null)
                menuRayInteractor.SetActive(true);

            Debug.Log("MENÚ ABIERTO");
        }
        else
        {
            OcultarTodo();
            GameController.Instance.TogglePause();

            if (menuRayInteractor != null)
                menuRayInteractor.SetActive(false);

            Debug.Log("MENÚ CERRADO");
        }
    }

    void PosicionarMenuFrenteJugador()
    {
        if (playerCamera == null) return;

        Vector3 targetPosition =
            playerCamera.position +
            playerCamera.forward * distanceFromCamera;

        targetPosition.y += heightOffset;

        radialMenu.transform.position = targetPosition;

        // mirar exactamente como la cámara
        radialMenu.transform.rotation = Quaternion.LookRotation(playerCamera.forward);

        Debug.Log("Menú posicionado frente al jugador");
    }

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

        Debug.Log("Panel sonido abierto");
    }

    public void MostrarSalirConfirmacion()
    {
        radialMenu.SetActive(false);
        panelSonido.SetActive(false);
        panelSalir.SetActive(true);

        Debug.Log("Panel salir abierto");
    }

    public void ReanudarJuego()
    {
        Debug.Log("Reanudar presionado");

        GameObject jugador = GameObject.FindGameObjectWithTag("Player");

        if (jugador != null && puntoInicio != null)
        {
            jugador.transform.position = puntoInicio.position;
            jugador.transform.rotation = puntoInicio.rotation;

            Debug.Log("Jugador enviado al punto inicial");
        }

        Time.timeScale = 1;
        menuAbierto = false;

        if (menuRayInteractor != null)
            menuRayInteractor.SetActive(false);

        OcultarTodo();
    }

    public void PausarJuego()
    {
        ToggleMenu();
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