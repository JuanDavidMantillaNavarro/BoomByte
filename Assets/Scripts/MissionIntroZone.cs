using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

using XRInputDevice = UnityEngine.XR.InputDevice;
using XRNode = UnityEngine.XR.XRNode;
using XRCommonUsages = UnityEngine.XR.CommonUsages;

public class MissionIntroZone : MonoBehaviour
{
    [Header("Panels")]
    public GameObject bienvenida;
    public GameObject mensajePatroclo;
    public GameObject panelObjetivo;
    public GameObject panelContinuar;
    public GameObject imagenPatroclo;

    [Header("Duraciones")]
    public float duracionBienvenida = 8f;
    public float duracionMensaje = 13f;
    public float duracionFade = 1f;

    private bool yaSeMostro = false;

    private Collider zonaCollider;

    void Start()
    {
        GameObject canvasPadre = GameObject.Find("CanvaPatrocloInfo");

        if (canvasPadre != null)
        {
            bienvenida =
                canvasPadre.transform.Find("bienvenida")?.gameObject;

            mensajePatroclo =
                canvasPadre.transform.Find("mensajePatroclo")?.gameObject;

            panelObjetivo =
                canvasPadre.transform.Find("panelObjetivo")?.gameObject;

            panelContinuar =
                canvasPadre.transform.Find("panelContinuar")?.gameObject;

            imagenPatroclo =
                canvasPadre.transform.Find("imagenPatroclo")?.gameObject;
        }

        OcultarTodos();

        zonaCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (yaSeMostro) return;

        if (other.CompareTag("Player"))
        {
            StartCoroutine(SecuenciaIntro());
        }
    }

    IEnumerator SecuenciaIntro()
    {
        yaSeMostro = true;

        Cursor.lockState = CursorLockMode.None;

        Time.timeScale = 0f;

        if (GameController.Instance != null)
            GameController.Instance.isPaused = true;

        // ================= BIENVENIDA =================

        yield return StartCoroutine(
            MostrarPanelTemporal(
                bienvenida,
                duracionBienvenida
            )
        );

        // ================= MENSAJE PATROCLO =================

        SetPanelActivo(imagenPatroclo, true);

        yield return StartCoroutine(
            MostrarPanelTemporal(
                mensajePatroclo,
                duracionMensaje
            )
        );

        SetPanelActivo(imagenPatroclo, false);

        // ================= OBJETIVO =================

        yield return StartCoroutine(FadeIn(panelObjetivo));

        yield return StartCoroutine(FadeIn(panelContinuar));

        EsperarContinuar();
    }

    void EsperarContinuar()
    {
        StartCoroutine(EsperarInputContinuar());
    }

    IEnumerator EsperarInputContinuar()
    {
        while (true)
        {
            bool teclaCerrar =
                Keyboard.current != null &&
                Keyboard.current.pKey.wasPressedThisFrame;

            bool botonA = BotonAVR();

            if (teclaCerrar || botonA)
            {
                break;
            }

            yield return null;
        }

        yield return StartCoroutine(FadeOut(panelObjetivo));
        yield return StartCoroutine(FadeOut(panelContinuar));

        FinalizarIntro();
    }

    bool BotonAVR()
    {
        XRInputDevice rightHand =
            InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        if (!rightHand.isValid)
            return false;

        bool botonA = false;

        return rightHand.TryGetFeatureValue(
            XRCommonUsages.primaryButton,
            out botonA
        ) && botonA;
    }

    void FinalizarIntro()
    {

        Cursor.lockState = CursorLockMode.Locked;

        Time.timeScale = 1f;

        if (GameController.Instance != null)
            GameController.Instance.isPaused = false;

        if (zonaCollider != null)
            zonaCollider.enabled = false;

        OcultarTodos();

        Debug.Log("INTRO FINALIZADA");
    }

    IEnumerator MostrarPanelTemporal(GameObject panel, float duracion)
    {
        yield return StartCoroutine(FadeIn(panel));

        float tiempo = 0f;

        while (tiempo < duracion)
        {
            tiempo += Time.unscaledDeltaTime;
            yield return null;
        }

        yield return StartCoroutine(FadeOut(panel));
    }

    IEnumerator FadeIn(GameObject panel)
    {
        if (panel == null) yield break;

        panel.SetActive(true);

        CanvasGroup cg = panel.GetComponent<CanvasGroup>();

        if (cg == null)
            cg = panel.AddComponent<CanvasGroup>();

        cg.alpha = 0f;

        float tiempo = 0f;

        while (tiempo < duracionFade)
        {
            tiempo += Time.unscaledDeltaTime;

            cg.alpha = Mathf.Lerp(
                0f,
                1f,
                tiempo / duracionFade
            );

            yield return null;
        }

        cg.alpha = 1f;
    }

    IEnumerator FadeOut(GameObject panel)
    {
        if (panel == null) yield break;

        CanvasGroup cg = panel.GetComponent<CanvasGroup>();

        if (cg == null)
            cg = panel.AddComponent<CanvasGroup>();

        float tiempo = 0f;

        while (tiempo < duracionFade)
        {
            tiempo += Time.unscaledDeltaTime;

            cg.alpha = Mathf.Lerp(
                1f,
                0f,
                tiempo / duracionFade
            );

            yield return null;
        }

        cg.alpha = 0f;

        panel.SetActive(false);
    }

    void SetPanelActivo(GameObject panel, bool estado)
    {
        if (panel == null) return;

        panel.SetActive(estado);

        CanvasGroup cg = panel.GetComponent<CanvasGroup>();

        if (cg == null)
            cg = panel.AddComponent<CanvasGroup>();

        cg.alpha = estado ? 1f : 0f;
    }

    void OcultarTodos()
    {
        SetPanelActivo(bienvenida, false);
        SetPanelActivo(mensajePatroclo, false);
        SetPanelActivo(panelObjetivo, false);
        SetPanelActivo(panelContinuar, false);
        SetPanelActivo(imagenPatroclo, false);
    }
}