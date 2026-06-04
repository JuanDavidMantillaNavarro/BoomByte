using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;

public class PrimerEnemigoTutorial : MonoBehaviour
{
    [Header("Jugador")]
    public Transform jugador;

    [Header("Distancia")]
    public float distanciaActivacion = 2f;

    [Header("Movimiento Real")]
    public CharacterController characterController;
    public MonoBehaviour movimientoScript;

    [Header("UI")]
    public GameObject fondoNegro;
    public GameObject imagenPrimerEnemigo;
    public GameObject imagenEfecto;

    [Header("Tiempo")]
    public float duracionMensajeEnemigo = 6f;
    public float duracionMensajeEfecto = 4f;

    [Header("Movimiento VR")]
    public MonoBehaviour locomotionProvider;
    public MonoBehaviour turnProvider;

    [Header("FMOD - Audio")]
    [SerializeField] private EventReference sonidoAlertaEnemigo;

    private bool activado = false;

    void Start()
    {
        Debug.Log("[Tutorial] Start");

        if (fondoNegro != null)
            fondoNegro.SetActive(false);

        if (imagenPrimerEnemigo != null)
            imagenPrimerEnemigo.SetActive(false);

        if (imagenEfecto != null)
            imagenEfecto.SetActive(false);
    }

    void Update()
    {
        if (activado)
            return;

        if (jugador == null)
            return;

        float distancia = Vector3.Distance(transform.position, jugador.position);

        if (distancia <= distanciaActivacion)
        {
            Debug.Log("[Tutorial] Jugador cerca. Distancia = " + distancia);

            activado = true;

            RuntimeManager.PlayOneShot(sonidoAlertaEnemigo, transform.position);

            StartCoroutine(SecuenciaTutorial());
        }
    }

    IEnumerator SecuenciaTutorial()
    {
        Debug.Log("PASO 1");

        if (GameController.Instance != null)
        {
            GameController.Instance.isPaused = true;
            Debug.Log("GameController pausado");
        }

        Time.timeScale = 0f;

        if (locomotionProvider != null)
        {
            locomotionProvider.enabled = false;
            Debug.Log("Locomotion desactivada");
        }

        if (turnProvider != null)
        {
            turnProvider.enabled = false;
            Debug.Log("Turn Provider desactivado");
        }

        if (movimientoScript != null)
        {
            movimientoScript.enabled = false;
            Debug.Log("Movimiento Script desactivado");
        }

        if (characterController != null)
        {
            characterController.enabled = false;
            Debug.Log("Character Controller desactivado");
        }

        fondoNegro.SetActive(true);
        imagenPrimerEnemigo.SetActive(true);

        yield return new WaitForSecondsRealtime(duracionMensajeEnemigo);

        imagenPrimerEnemigo.SetActive(false);
        imagenEfecto.SetActive(true);

        yield return new WaitForSecondsRealtime(duracionMensajeEfecto);

        imagenEfecto.SetActive(false);
        fondoNegro.SetActive(false);

        Debug.Log("Reactivando controles");

        if (locomotionProvider != null)
            locomotionProvider.enabled = true;

        if (turnProvider != null)
            turnProvider.enabled = true;

        if (movimientoScript != null)
            movimientoScript.enabled = true;

        if (characterController != null)
            characterController.enabled = true;

        if (GameController.Instance != null)
            GameController.Instance.isPaused = false;

        Time.timeScale = 1f;

        Debug.Log("PASO 10 - FIN");
    }
}