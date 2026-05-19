using System.Collections;
using TMPro;
using UnityEngine;

public class MensajesMurosVR : MonoBehaviour
{
    [Header("UI")]
    public GameObject panelMensaje;
    public TextMeshProUGUI textoMensaje;

    [Header("Configuración Nivel 1")]
    public string tagDestructibleNivel1 = "Destructible";

    [Header("Configuración Nivel 2")]
    public string tagDestructibleNivel2 = "Destructible2";

    [Header("Tiempo")]
    public float tiempoMensaje = 4f;
    public float intervaloRevision = 0.5f;

    // ================= NIVEL 1 =================

    private int cantidadAnteriorNivel1;
    private bool primerMensajeNivel1 = false;
    private bool nivel1Completado = false;

    // ================= NIVEL 2 =================

    private int cantidadAnteriorNivel2;
    private bool primerMensajeNivel2 = false;
    private bool nivel2Completado = false;

    void Start()
    {
        // Nivel 1
        GameObject[] destructiblesNivel1 =
            GameObject.FindGameObjectsWithTag(
                tagDestructibleNivel1
            );

        cantidadAnteriorNivel1 =
            destructiblesNivel1.Length;

        // Nivel 2
        GameObject[] destructiblesNivel2 =
            GameObject.FindGameObjectsWithTag(
                tagDestructibleNivel2
            );

        cantidadAnteriorNivel2 =
            destructiblesNivel2.Length;

        if (panelMensaje != null)
            panelMensaje.SetActive(false);

        Debug.Log(
            "Nivel 1 paredes: " +
            cantidadAnteriorNivel1
        );

        Debug.Log(
            "Nivel 2 paredes: " +
            cantidadAnteriorNivel2
        );

        InvokeRepeating(
            nameof(RevisarMuros),
            0f,
            intervaloRevision
        );
    }

    void RevisarMuros()
    {
        RevisarNivel1();
        RevisarNivel2();
    }

    // =====================================================
    // NIVEL 1
    // =====================================================

    void RevisarNivel1()
    {
        if (nivel1Completado) return;

        GameObject[] destructibles =
            GameObject.FindGameObjectsWithTag(
                tagDestructibleNivel1
            );

        int cantidadActual =
            destructibles.Length;

        if (cantidadActual < cantidadAnteriorNivel1)
        {
            cantidadAnteriorNivel1 =
                cantidadActual;

            Debug.Log(
                "Nivel 1 restantes: " +
                cantidadActual
            );

            // Primera pared
            if (!primerMensajeNivel1)
            {
                primerMensajeNivel1 = true;

                MostrarMensaje(
                    "Destruiste la primera pared destructible del laboratorio MAC."
                );

                return;
            }

            // Restantes
            if (cantidadActual > 0)
            {
                string mensaje;

                if (cantidadActual == 1)
                {
                    mensaje =
                        "Destruiste una pared.\n" +
                        "Resta 1 pared destructible.";
                }
                else
                {
                    mensaje =
                        "Destruiste una pared.\n" +
                        "Restan " +
                        cantidadActual +
                        " paredes destructibles.";
                }

                MostrarMensaje(mensaje);
            }
            else
            {
                nivel1Completado = true;

                MostrarMensaje(
                    "Ya destruiste todas las paredes del laboratorio MAC.\n" +
                    "La terminal fue desbloqueada.\n" +
                    "Encuéntrala para llegar al final del nivel."
                );

                Debug.Log(
                    "Nivel 1 completado"
                );
            }
        }
    }

    // =====================================================
    // NIVEL 2
    // =====================================================

    void RevisarNivel2()
    {
        if (nivel2Completado) return;

        GameObject[] destructibles =
            GameObject.FindGameObjectsWithTag(
                tagDestructibleNivel2
            );

        int cantidadActual =
            destructibles.Length;

        if (cantidadActual < cantidadAnteriorNivel2)
        {
            cantidadAnteriorNivel2 =
                cantidadActual;

            Debug.Log(
                "Nivel 2 restantes: " +
                cantidadActual
            );

            // Primera pared
            if (!primerMensajeNivel2)
            {
                primerMensajeNivel2 = true;

                MostrarMensaje(
                    "Destruiste la primera pared destructible del nivel 2."
                );

                return;
            }

            // CASO ESPECIAL:
            // QUEDA SOLO 1
            if (cantidadActual == 1)
            {
                MostrarMensaje(
                    "Te queda 1 pared destructible.\n" +
                    "Busca esa pared.\n" +
                    "Ahí está la terminal final del salón de dibujo."
                );

                return;
            }

            // MÁS DE 1
            if (cantidadActual > 1)
            {
                MostrarMensaje(
                    "Destruiste una pared.\n" +
                    "Restan " +
                    cantidadActual +
                    " paredes destructibles del nivel 2."
                );
            }
            else
            {
                // Ya no quedan
                nivel2Completado = true;

                MostrarMensaje(
                    "Encontraste la pared final.\n" +
                    "La terminal del salón de dibujo fue desbloqueada."
                );

                Debug.Log(
                    "Nivel 2 completado"
                );
            }
        }
    }

    // =====================================================
    // UI
    // =====================================================

    void MostrarMensaje(string mensaje)
    {
        StopAllCoroutines();

        StartCoroutine(
            MostrarMensajeCoroutine(
                mensaje
            )
        );
    }

    IEnumerator MostrarMensajeCoroutine(
        string mensaje
    )
    {
        panelMensaje.SetActive(true);

        textoMensaje.text = mensaje;

        yield return new WaitForSeconds(
            tiempoMensaje
        );

        panelMensaje.SetActive(false);
    }
}