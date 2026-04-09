using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Comfort;

public class UIManagerVR : MonoBehaviour
{
    public GameObject panelConfiguracion;
    public TunnelingVignetteController vignetteController;
    public ExplosionVignetteProvider explosionProvider;
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