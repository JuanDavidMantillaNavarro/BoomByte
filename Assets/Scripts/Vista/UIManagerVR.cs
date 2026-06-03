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
    public GameObject timerContainer;

    [Header("Panels")]
    public GameObject winPanel;
    public GameObject losePanel;
    [Header("Easter Egg UI")]
    public TextMeshProUGUI easterEggText;

    public void ShowEasterEggMessage(string msg)
    {
        if (easterEggText == null)
        {
            Debug.LogError("No hay texto asignado en UIManagerVR");
            return;
        }

        easterEggText.text = msg;
        easterEggText.gameObject.SetActive(true);

        CancelInvoke(nameof(HideMessage));
        Invoke(nameof(HideMessage), 8f);
    }

    void HideMessage()
    {
        easterEggText.gameObject.SetActive(false);
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
        
        StartCoroutine(CerraryReiniciar());
    }
    IEnumerator CerraryReiniciar () 
    {
        yield return new WaitForSeconds(6f);
        CerrarPanelesDerrVic();
        GameController.Instance.ReiniciarEstado();
    }
    public void CerrarPanelesDerrVic()
    {
        winPanel.SetActive(false);
        losePanel.SetActive(false);
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

    public void OcultarTimer()
    {
        if (timerContainer != null)
            timerContainer.SetActive(false);
    }

    public void MostrarTimer()
    {
        if (timerContainer != null)
            timerContainer.SetActive(true);
    }
}