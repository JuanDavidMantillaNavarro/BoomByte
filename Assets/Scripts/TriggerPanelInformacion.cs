using UnityEngine;

public class TriggerPanelInformacion : MonoBehaviour
{
    public PanelInformacionNivel panelInfo;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        panelInfo.MostrarPanel();
    }
}