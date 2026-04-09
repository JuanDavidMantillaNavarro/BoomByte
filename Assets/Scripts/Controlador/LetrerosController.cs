
using System.Collections;
using UnityEngine;

public class LetrerosController : MonoBehaviour
{
     [Header("Imagenes de tutorial")]
    public GameObject imagenAgarrarBola;
    public GameObject imagenLanzarBola;

    [Header("Prueba manual")]
    public bool agarrarBola = false;

    [Header("Tiempo de espera")]
    public float tiempoEspera = 5f;

    private bool cambioEjecutado = false;

    void Start()
    {
        if (imagenAgarrarBola != null)
            imagenAgarrarBola.SetActive(true);

        if (imagenLanzarBola != null)
            imagenLanzarBola.SetActive(false);
    }

    void Update()
    {
        if (agarrarBola && !cambioEjecutado)
        {
            cambioEjecutado = true;
            StartCoroutine(CambiarImagenTutorial());
        }
    }

    IEnumerator CambiarImagenTutorial()
    {
        yield return new WaitForSeconds(tiempoEspera);

        if (imagenAgarrarBola != null)
            imagenAgarrarBola.SetActive(false);

        if (imagenLanzarBola != null)
            imagenLanzarBola.SetActive(true);
    }
}