using UnityEngine;
using TMPro;

public class MapaNivelController : MonoBehaviour
{
    [Header("Texto")]
    public TMP_Text textoMapa;

    [Header("Resaltados")]
    public GameObject resaltadoLaboratorio;
    public GameObject resaltadoTaller;
    public GameObject resaltadoPlaza;

    private int nivelActual = 1;

    void Start()
    {
        CambiarNivel(1);
    }

    public void CambiarNivel(int nivel)
    {
        nivelActual = nivel;

        if (resaltadoLaboratorio != null)
            resaltadoLaboratorio.SetActive(false);

        if (resaltadoTaller != null)
            resaltadoTaller.SetActive(false);

        if (resaltadoPlaza != null)
            resaltadoPlaza.SetActive(false);

        switch (nivel)
        {
            case 1:

                if (textoMapa != null)
                {
                    textoMapa.text =
                        "Ubicación actual:\nNivel 1: Laboratorio MAC";
                }

                if (resaltadoLaboratorio != null)
                    resaltadoLaboratorio.SetActive(true);

                break;

            case 2:

                if (textoMapa != null)
                {
                    textoMapa.text =
                        "Ubicación actual:\nNivel 2: Taller de Diseño";
                }

                if (resaltadoTaller != null)
                    resaltadoTaller.SetActive(true);

                break;

            case 3:

                if (textoMapa != null)
                {
                    textoMapa.text =
                        "Ubicación actual:\nNivel Final";
                }

                if (resaltadoPlaza != null)
                    resaltadoPlaza.SetActive(true);

                break;
        }

        Debug.Log(
            "MAPA CAMBIADO A NIVEL "
            + nivelActual
        );
    }

    public int ObtenerNivelActual()
    {
        return nivelActual;
    }
}