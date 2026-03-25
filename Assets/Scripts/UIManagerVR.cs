using UnityEngine;

public class UIManagerVR : MonoBehaviour
{
    public GameObject panelConfiguracion;

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
}