using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

using XRInputDevice = UnityEngine.XR.InputDevice;
using XRNode = UnityEngine.XR.XRNode;
using XRCommonUsages = UnityEngine.XR.CommonUsages;

public class VRMenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject radialMenu;
    public GameObject panelSonido;
    public GameObject panelSalir;
    public Transform puntoInicio;

    [Header("Fade")]
    public CanvasGroup fadeMenu;
    public float duracionFade = 0.4f;

    [Header("Player")]
    public Transform playerCamera;
    public Transform jugador;

    [Header("Ray Menu")]
    public GameObject menuRayInteractor;

    [Header("Menu Position")]
    public float distanceFromCamera = 1.5f;
    public float heightOffset = -0.2f;

    private bool menuAbierto = false;
    private bool animando = false;
    private bool avisoControlMostrado = false;

    void Start()
    {
        Debug.Log("VRMenuManager iniciado");
        OcultarTodoInstantaneo();
    }

    void Update()
    {
        DetectarInputMenu();
    }

    bool BotonBVR()
    {
        XRInputDevice rightHand =
            InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        if (!rightHand.isValid)
        {
            if (!avisoControlMostrado)
            {
                Debug.LogWarning("Control derecho no detectado (normal en simulador PC)");
                avisoControlMostrado = true;
            }

            return false;
        }

        bool botonB = false;

        bool presionado = rightHand.TryGetFeatureValue(
            XRCommonUsages.secondaryButton,
            out botonB
        ) && botonB;

        return presionado;
    }

    void DetectarInputMenu()
    {
        if (animando) return;

        bool teclado =
            Keyboard.current != null &&
            Keyboard.current.bKey.wasPressedThisFrame;

        if (teclado || BotonBVR())
        {
            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {
        if (animando) return;

        menuAbierto = !menuAbierto;

        if (menuAbierto)
            StartCoroutine(AbrirMenu());
        else
            StartCoroutine(CerrarMenu());
    }

    IEnumerator AbrirMenu()
    {
        animando = true;

        MostrarRadial();
        PosicionarMenuFrenteJugador();

        if (GameController.Instance != null)
            GameController.Instance.isPaused = true;

        Time.timeScale = 0f;

        if (menuRayInteractor != null)
            menuRayInteractor.SetActive(true);

        if (radialMenu != null)
            radialMenu.SetActive(true);

        if (fadeMenu != null)
        {
            fadeMenu.alpha = 0f;

            float tiempo = 0f;

            while (tiempo < duracionFade)
            {
                tiempo += Time.unscaledDeltaTime;

                fadeMenu.alpha =
                    Mathf.Lerp(0f, 1f, tiempo / duracionFade);

                yield return null;
            }

            fadeMenu.alpha = 1f;
        }

        animando = false;
    }

    IEnumerator CerrarMenu()
    {
        animando = true;

        if (fadeMenu != null)
        {
            float tiempo = 0f;

            while (tiempo < duracionFade)
            {
                tiempo += Time.unscaledDeltaTime;

                fadeMenu.alpha =
                    Mathf.Lerp(1f, 0f, tiempo / duracionFade);

                yield return null;
            }
        }

        OcultarTodoInstantaneo();

        if (GameController.Instance != null)
            GameController.Instance.isPaused = false;

        Time.timeScale = 1f;

        if (menuRayInteractor != null)
            menuRayInteractor.SetActive(false);

        animando = false;
    }

    void PosicionarMenuFrenteJugador()
    {
        if (playerCamera == null || radialMenu == null) return;

        Vector3 targetPosition =
            playerCamera.position +
            playerCamera.forward * distanceFromCamera;

        targetPosition.y += heightOffset;

        radialMenu.transform.position = targetPosition;
        radialMenu.transform.rotation =
            Quaternion.LookRotation(playerCamera.forward);
    }

    void PosicionarPanel(GameObject panel)
    {
        if (playerCamera == null || panel == null) return;

        Vector3 targetPosition =
            playerCamera.position +
            playerCamera.forward * distanceFromCamera;

        targetPosition.y += heightOffset;

        panel.transform.position = targetPosition;
        panel.transform.rotation =
            Quaternion.LookRotation(playerCamera.forward);
    }

    public void MostrarRadial()
    {
        if (radialMenu != null)
            radialMenu.SetActive(true);

        if (panelSonido != null)
            panelSonido.SetActive(false);

        if (panelSalir != null)
            panelSalir.SetActive(false);

        PosicionarPanel(radialMenu);
    }

    public void MostrarSonido()
    {
        if (radialMenu != null)
            radialMenu.SetActive(false);

        if (panelSonido != null)
            panelSonido.SetActive(true);

        if (panelSalir != null)
            panelSalir.SetActive(false);

        PosicionarPanel(panelSonido);
    }

    public void MostrarSalirConfirmacion()
    {
        if (radialMenu != null)
            radialMenu.SetActive(false);

        if (panelSonido != null)
            panelSonido.SetActive(false);

        if (panelSalir != null)
            panelSalir.SetActive(true);

        PosicionarPanel(panelSalir);
    }

    public void ReanudarJuego()
    {
        Debug.Log("Reiniciando juego");

        if (jugador != null && puntoInicio != null)
        {
            jugador.position = puntoInicio.position;
            jugador.rotation = puntoInicio.rotation;
        }

        if (GameController.Instance != null)
            GameController.Instance.ReiniciarEstado();

        menuAbierto = false;

        StartCoroutine(CerrarMenu());
    }

    public void PausarJuego()
    {
        menuAbierto = false;
        StartCoroutine(CerrarMenu());
    }

    public void SalirJuego()
    {
        Application.Quit();
    }

    public void CancelarSalir()
    {
        MostrarRadial();
    }

    void OcultarTodoInstantaneo()
    {
        if (radialMenu != null)
            radialMenu.SetActive(false);

        if (panelSonido != null)
            panelSonido.SetActive(false);

        if (panelSalir != null)
            panelSalir.SetActive(false);
    }
}