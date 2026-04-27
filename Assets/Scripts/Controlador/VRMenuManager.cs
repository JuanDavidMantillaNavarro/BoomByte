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

    [Header("Ray Menu")]
    public GameObject menuRayInteractor;

    [Header("Menu Position")]
    public float distanceFromCamera = 1.5f;
    public float heightOffset = -0.2f;

    private bool menuAbierto = false;
    private bool animando = false;

    private bool controlNoValidoMostrado = false;
    private bool botonBPresionadoAnterior = false;

    void Start()
    {
        OcultarTodoInstantaneo();
    }

    void Update()
    {
        DetectarInputMenu();
    }

    // ================= INPUT =================

    bool BotonBVR()
    {
        XRInputDevice rightHand =
            InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        if (!rightHand.isValid)
        {
            if (!controlNoValidoMostrado)
            {
                Debug.LogWarning("Control derecho no válido");
                controlNoValidoMostrado = true;
            }
            return false;
        }

        controlNoValidoMostrado = false;

        bool botonB = false;

        return rightHand.TryGetFeatureValue(
            XRCommonUsages.secondaryButton,
            out botonB
        ) && botonB;
    }

    bool BotonXVR()
    {
        XRInputDevice leftHand =
            InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);

        if (!leftHand.isValid)
            return false;

        bool botonX = false;

        return leftHand.TryGetFeatureValue(
            XRCommonUsages.primaryButton,
            out botonX
        ) && botonX;
    }

    void DetectarInputMenu()
    {
        if (animando) return;

        // Toggle menú
        bool tecladoB =
            Keyboard.current != null &&
            Keyboard.current.bKey.wasPressedThisFrame;

        bool botonActual = BotonBVR();

        bool botonVRFrame =
            botonActual && !botonBPresionadoAnterior;

        botonBPresionadoAnterior = botonActual;

        if (tecladoB || botonVRFrame)
        {
            Debug.Log("Toggle menú");
            ToggleMenu();
        }

        // Cámara secundaria
        bool tecladoM =
            Keyboard.current != null &&
            Keyboard.current.mKey.wasPressedThisFrame;

        if (tecladoM || BotonXVR())
        {
            Debug.Log("Activar vista cámara");
            if (GameController.Instance != null)
            {;}
                //GameController.Instance.ActivateCameraView();
        }
    }

    // ================= MENU =================

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

        radialMenu.SetActive(true);

        if (fadeMenu != null)
        {
            fadeMenu.alpha = 0f;

            float t = 0f;

            while (t < duracionFade)
            {
                t += Time.unscaledDeltaTime;
                fadeMenu.alpha = Mathf.Lerp(0f, 1f, t / duracionFade);
                yield return null;
            }

            fadeMenu.alpha = 1f;
        }

        animando = false;
        Debug.Log("MENÚ ABIERTO");
    }

    IEnumerator CerrarMenu()
    {
        if (animando) yield break;

        animando = true;

        if (fadeMenu != null)
        {
            float t = 0f;

            while (t < duracionFade)
            {
                t += Time.unscaledDeltaTime;
                fadeMenu.alpha = Mathf.Lerp(1f, 0f, t / duracionFade);
                yield return null;
            }

            fadeMenu.alpha = 0f;
        }

        OcultarTodoInstantaneo();

        if (GameController.Instance != null)
            GameController.Instance.isPaused = false;

        Time.timeScale = 1f;

        if (menuRayInteractor != null)
            menuRayInteractor.SetActive(false);

        animando = false;
        Debug.Log("MENÚ CERRADO");
    }

    // ================= POSICIONAMIENTO =================

    void PosicionarMenuFrenteJugador()
    {
        if (playerCamera == null) return;

        Vector3 pos =
            playerCamera.position +
            playerCamera.forward * distanceFromCamera;

        pos.y += heightOffset;

        radialMenu.transform.position = pos;
        radialMenu.transform.rotation =
            Quaternion.LookRotation(playerCamera.forward);
    }

    void PosicionarPanel(GameObject panel)
    {
        if (playerCamera == null || panel == null) return;

        Vector3 pos =
            playerCamera.position +
            playerCamera.forward * distanceFromCamera;

        pos.y += heightOffset;

        panel.transform.position = pos;
        panel.transform.rotation =
            Quaternion.LookRotation(playerCamera.forward);
    }

    // ================= UI =================

    public void MostrarRadial()
    {
        radialMenu.SetActive(true);
        panelSonido.SetActive(false);
        panelSalir.SetActive(false);

        PosicionarPanel(radialMenu);
    }

    public void MostrarSonido()
    {
        radialMenu.SetActive(false);
        panelSonido.SetActive(true);
        panelSalir.SetActive(false);

        PosicionarPanel(panelSonido);
    }

    public void MostrarSalirConfirmacion()
    {
        radialMenu.SetActive(false);
        panelSonido.SetActive(false);
        panelSalir.SetActive(true);

        PosicionarPanel(panelSalir);
    }

    // ================= BOTONES =================

    public void ReanudarJuego()
    {
        GameObject jugador =
            GameObject.FindGameObjectWithTag("Player");

        if (jugador != null && puntoInicio != null)
        {
            jugador.transform.position = puntoInicio.position;
            jugador.transform.rotation = puntoInicio.rotation;
        }

        if (GameController.Instance != null)
            GameController.Instance.ReiniciarEstado();

        menuAbierto = false;

        if (!animando)
            StartCoroutine(CerrarMenu());
    }

    public void PausarJuego()
    {
        ToggleMenu();
    }

    public void SalirJuego()
    {
        Application.Quit();
    }

    public void CancelarSalir()
    {
        MostrarRadial();
    }

    // ================= UTILS =================

    void OcultarTodoInstantaneo()
    {
        radialMenu.SetActive(false);
        panelSonido.SetActive(false);
        panelSalir.SetActive(false);

        if (fadeMenu != null)
            fadeMenu.alpha = 0f;
    }
}