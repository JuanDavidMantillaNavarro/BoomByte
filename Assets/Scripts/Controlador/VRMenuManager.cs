using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

using XRInputDevice = UnityEngine.XR.InputDevice;
using XRNode = UnityEngine.XR.XRNode;
using XRCommonUsages = UnityEngine.XR.CommonUsages;
using UnityEngine.XR;

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

    // NUEVO: evita spam en consola
    private bool controlNoValidoMostrado = false;

    // NUEVO: evita doble toggle por mantener presionado
    private bool botonBPresionadoAnterior = false;

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
            if (!controlNoValidoMostrado)
            {
                Debug.LogWarning("Control derecho no válido");
                controlNoValidoMostrado = true;
            }

            return false;
        }

        // Si vuelve a ser válido, reseteamos el aviso
        controlNoValidoMostrado = false;

        bool botonB = false;

        bool presionado =
            rightHand.TryGetFeatureValue(
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

        bool botonActual = BotonBVR();

        // SOLO detecta el primer frame del click
        bool botonVRPresionadoEsteFrame =
            botonActual && !botonBPresionadoAnterior;

        botonBPresionadoAnterior = botonActual;

        if (teclado)
            Debug.Log("Tecla B presionada en simulador");

        if (botonVRPresionadoEsteFrame)
            Debug.Log("Botón B VR presionado");

        if (teclado || botonVRPresionadoEsteFrame)
        {
            Debug.Log("Toggle menú ejecutado");
            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {
        if (animando)
        {
            Debug.Log("Animación en curso, ignorando toggle");
            return;
        }

        menuAbierto = !menuAbierto;

        Debug.Log("Estado menú: " + menuAbierto);

        if (menuAbierto)
            StartCoroutine(AbrirMenu());
        else
            StartCoroutine(CerrarMenu());
    }

    IEnumerator AbrirMenu()
    {
        Debug.Log("Iniciando apertura de menú");
        animando = true;

        MostrarRadial();
        PosicionarMenuFrenteJugador();

        if (GameController.Instance != null)
        {
            GameController.Instance.isPaused = true;
            Debug.Log("Juego pausado");
        }

        Time.timeScale = 0f;

        if (menuRayInteractor != null)
        {
            menuRayInteractor.SetActive(true);
            Debug.Log("Ray interactor activado");
        }

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

        Debug.Log("MENÚ ABIERTO COMPLETAMENTE");
        animando = false;
    }

    IEnumerator CerrarMenu()
    {
        if (animando) yield break;

        Debug.Log("Iniciando cierre de menú");
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

            fadeMenu.alpha = 0f;
        }

        OcultarTodoInstantaneo();

        if (GameController.Instance != null)
        {
            GameController.Instance.isPaused = false;
            Debug.Log("Juego reanudado");
        }

        Time.timeScale = 1f;

        if (menuRayInteractor != null)
        {
            menuRayInteractor.SetActive(false);
            Debug.Log("Ray interactor desactivado");
        }

        Debug.Log("MENÚ CERRADO COMPLETAMENTE");
        animando = false;
    }

    void PosicionarMenuFrenteJugador()
    {
        if (playerCamera == null)
        {
            Debug.LogWarning("PlayerCamera no asignada");
            return;
        }

        Vector3 targetPosition =
            playerCamera.position +
            playerCamera.forward * distanceFromCamera;

        targetPosition.y += heightOffset;

        radialMenu.transform.position = targetPosition;
        radialMenu.transform.rotation =
            Quaternion.LookRotation(playerCamera.forward);

        Debug.Log("Menú posicionado frente al jugador");
    }

    void PosicionarPanel(GameObject panel)
    {
        if (playerCamera == null || panel == null)
        {
            Debug.LogWarning("Panel o cámara faltante");
            return;
        }

        Vector3 targetPosition =
            playerCamera.position +
            playerCamera.forward * distanceFromCamera;

        targetPosition.y += heightOffset;

        panel.transform.position = targetPosition;
        panel.transform.rotation =
            Quaternion.LookRotation(playerCamera.forward);

        Debug.Log("Panel posicionado: " + panel.name);
    }

    public void MostrarRadial()
    {
        Debug.Log("Mostrando menú radial");

        radialMenu.SetActive(true);
        panelSonido.SetActive(false);
        panelSalir.SetActive(false);

        PosicionarPanel(radialMenu);
    }

    public void MostrarSonido()
    {
        Debug.Log("Mostrando panel sonido");

        radialMenu.SetActive(false);
        panelSonido.SetActive(true);
        panelSalir.SetActive(false);

        PosicionarPanel(panelSonido);
    }

    public void MostrarSalirConfirmacion()
    {
        Debug.Log("Mostrando confirmación salir");

        radialMenu.SetActive(false);
        panelSonido.SetActive(false);
        panelSalir.SetActive(true);

        PosicionarPanel(panelSalir);
    }

    public void ReanudarJuego()
    {
        Debug.Log("Reiniciando desde inicio");

        GameObject jugador =
            GameObject.FindGameObjectWithTag("Player");

        if (jugador != null && puntoInicio != null)
        {
            jugador.transform.position = puntoInicio.position;
            jugador.transform.rotation = puntoInicio.rotation;

            Debug.Log("Jugador enviado al punto inicial");
        }

        if (GameController.Instance != null)
        {
            GameController.Instance.ReiniciarEstado();
            Debug.Log("Estado del juego reiniciado");
        }

        menuAbierto = false;

        if (!animando)
            StartCoroutine(CerrarMenu());
    }

    public void PausarJuego()
    {
        Debug.Log("Cerrar menú / continuar");

        menuAbierto = false;

        if (!animando)
            StartCoroutine(CerrarMenu());
    }

    public void SalirJuego()
    {
        Debug.Log("Salir del juego");
        Application.Quit();
    }

    public void CancelarSalir()
    {
        Debug.Log("Cancelar salida");
        MostrarRadial();
    }

    void OcultarTodoInstantaneo()
    {
        radialMenu.SetActive(false);
        panelSonido.SetActive(false);
        panelSalir.SetActive(false);

        if (fadeMenu != null)
            fadeMenu.alpha = 0f;

        Debug.Log("Todo ocultado instantáneamente");
    }
}