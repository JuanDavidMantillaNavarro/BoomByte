using System.Collections;
using TMPro;
using UnityEngine;

public class MensajesMurosVR : MonoBehaviour
{
    [Header("UI")]
    public GameObject panelMensaje;
    public TextMeshProUGUI textoMensaje;

    [Header("Configuración")]
    public string tagDestructible = "Destructible";
    public float tiempoMensaje = 3f;
    public float intervaloRevision = 0.5f;

    [Header("Mensajes")]
    [TextArea]
    public string mensajePrimeraPared =
        "Destruiste la primera pared destructible.";

    [TextArea]
    public string mensajeFinal =
        "Ya destruiste todas las paredes del laboratorio MAC\nLa terminal fue desbloqueada\nEncuéntrala para llegar al final del nivel";

    private int cantidadInicial;
    private int cantidadAnterior;

    private bool primerMensajeMostrado = false;
    private bool mensajeFinalMostrado = false;

    void Start()
    {
        GameObject[] destructibles =
            GameObject.FindGameObjectsWithTag(tagDestructible);

        cantidadInicial = destructibles.Length;
        cantidadAnterior = cantidadInicial;

        if (panelMensaje != null)
            panelMensaje.SetActive(false);

        Debug.Log("Cantidad inicial de muros: " + cantidadInicial);

        InvokeRepeating(nameof(RevisarMuros), 0f, intervaloRevision);
    }

    void RevisarMuros()
    {
        if (mensajeFinalMostrado) return;

        GameObject[] destructibles =
            GameObject.FindGameObjectsWithTag(tagDestructible);

        int cantidadActual = destructibles.Length;

        // Detecta destrucción
        if (cantidadActual < cantidadAnterior)
        {
            cantidadAnterior = cantidadActual;

            Debug.Log("Muros restantes: " + cantidadActual);

            // Primera pared destruida
            if (!primerMensajeMostrado)
            {
                primerMensajeMostrado = true;

                MostrarMensaje(mensajePrimeraPared);
            }

            // Mensaje de restantes
            if (cantidadActual > 0)
            {
                MostrarMensaje(
                    "Destruiste una pared.\nRestan "
                    + cantidadActual +
                    " paredes destructibles"
                );
            }
            else
            {
                mensajeFinalMostrado = true;

                MostrarMensaje(mensajeFinal);

                Debug.Log("Todas las paredes fueron destruidas");
            }
        }
    }

    void MostrarMensaje(string mensaje)
    {
        StopAllCoroutines();
        StartCoroutine(MostrarMensajeCoroutine(mensaje));
    }

    IEnumerator MostrarMensajeCoroutine(string mensaje)
    {
        panelMensaje.SetActive(true);

        textoMensaje.text = mensaje;

        yield return new WaitForSeconds(tiempoMensaje);

        panelMensaje.SetActive(false);
    }
}