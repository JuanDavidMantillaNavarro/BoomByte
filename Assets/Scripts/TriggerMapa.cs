using UnityEngine;

public class TriggerMapa : MonoBehaviour
{
    [Header("Controlador del mapa")]
    public MapaNivelController mapa;

    [Header("Jugador")]
    public Transform jugador;

    [Header("Nivel destino")]
    public int nivelDestino = 2;

    private bool activado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (activado)
            return;

        if (jugador == null)
            return;

        if (other.transform != jugador)
            return;

        activado = true;

        Debug.Log(
            "ENTRÓ AL TRIGGER DEL NIVEL "
            + nivelDestino
        );

        if (mapa != null)
        {
            mapa.CambiarNivel(
                nivelDestino
            );
        }
    }
}