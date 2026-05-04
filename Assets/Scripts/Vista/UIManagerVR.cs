using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Comfort;

public class UIManagerVR : MonoBehaviour
{
    public GameObject panelConfiguracion;
    public TunnelingVignetteController vignetteController;
    public ExplosionVignetteProvider explosionProvider;

    [Header("Timer")]
    public TextMeshProUGUI timerText;

    [Header("Panels")]
    public GameObject winPanel;
    public GameObject losePanel;

    [Header("Easter Egg UI")]
    [SerializeField] private GameObject easterEggPanel;
    [SerializeField] private TextMeshProUGUI easterEggText;
    private Coroutine currentMessageRoutine;

    public void ShowEasterEggMessage(string msg)
    {
        if (easterEggPanel == null || easterEggText == null)
        {
            Debug.LogError("Faltan referencias del EasterEgg UI");
            return;
        }

        // Si ya hay un mensaje activo, lo cancelamos
        if (currentMessageRoutine != null)
        {
            StopCoroutine(currentMessageRoutine);
        }

        currentMessageRoutine = StartCoroutine(ShowMessageRoutine(msg));
    }
    private IEnumerator ShowMessageRoutine(string msg)
    {
        easterEggPanel.SetActive(true);
        easterEggText.text = msg;

        yield return new WaitForSeconds(5f);

        easterEggPanel.SetActive(false);
        currentMessageRoutine = null;
    }

    void Awake()
    {
        if (easterEggText == null)
        {
            easterEggText = FindObjectOfType<TextMeshProUGUI>(true);

            if (easterEggText == null)
                Debug.LogError("No se encontró ningún TextMeshProUGUI en la escena");
        }
    }
    void HideMessage()
    {
        easterEggText.gameObject.SetActive(false);
        easterEggPanel.gameObject.SetActive(false);
    }
    public void UpdateTimer(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    public void MostrarVictoria()
    {
        if (winPanel != null)
            winPanel.SetActive(true);

        if (losePanel != null)
            losePanel.SetActive(false);
    }

    public void MostrarDerrota()
    {
        if (losePanel != null)
            losePanel.SetActive(true);

        if (winPanel != null)
            winPanel.SetActive(false);
    }
    public void AbrirPanel()
    {
        panelConfiguracion.SetActive(true);
    }

    public void CerrarPanel()
    {
        panelConfiguracion.SetActive(false);
    }

    public void Mute()
    {
        Debug.Log("Botón Mute presionado");
    }

    public void ActivarVignette(float duration)
    {
        StartCoroutine(VignetteRoutine(duration));
    }

    private IEnumerator VignetteRoutine(float duration)
    {
        vignetteController.BeginTunnelingVignette(explosionProvider);

        yield return new WaitForSeconds(duration);

        vignetteController.EndTunnelingVignette(explosionProvider);
    }
}