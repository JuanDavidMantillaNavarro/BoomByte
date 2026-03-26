using System.Collections;
using UnityEngine;

public class LetrerosController : MonoBehaviour
{
    [Header("Imagenes de tutorial")]
    public GameObject imagenAgarrarBola;
    public GameObject imagenLanzarBola;

    [Header("Prueba manual")]
    public bool agarrarBola = false;
    public bool lanzarBola = false; 

    [Header("Tiempo de espera")]
    public float tiempoEspera = 5f;

    private bool cambioEjecutado = false;
    private bool desaparicionEjecutada = false;

    void Start()
    {
        if (imagenAgarrarBola != null)
            imagenAgarrarBola.SetActive(true);

        if (imagenLanzarBola != null)
            imagenLanzarBola.SetActive(false);
    }

    void Update()
    {
        // Cambio a "Lanzar bola"
        if (agarrarBola && !cambioEjecutado)
        {
            cambioEjecutado = true;
            StartCoroutine(CambiarImagenTutorial());
        }

        //  Desaparición del letrero "Lanzar bola"
        if (lanzarBola && !desaparicionEjecutada)
        {
            desaparicionEjecutada = true;
            StartCoroutine(DesaparecerLetreroLanzar());
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

    IEnumerator DesaparecerLetreroLanzar()
    {
        yield return new WaitForSeconds(tiempoEspera);

        if (imagenLanzarBola != null)
            imagenLanzarBola.SetActive(false);
    }
}