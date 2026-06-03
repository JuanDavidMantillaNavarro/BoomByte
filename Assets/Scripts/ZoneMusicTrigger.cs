using UnityEngine;
using FMODUnity;

public class ZoneMusicTrigger : MonoBehaviour
{
    [Header("FMOD - Música de esta zona")]
    [SerializeField] private EventReference musicaZona;

    private bool activado = false;

   private void OnTriggerEnter(Collider other)
{
    Debug.Log("Entró al trigger: " + other.name + " | Tag: " + other.tag);

    if (activado) return;

    if (other.CompareTag("MainCamera") || other.CompareTag("Player"))
    {
        activado = true;

        Debug.Log("Activando música de zona: " + gameObject.name);

        AudioManagerFMOD.Instance.CambiarMusicaZona(musicaZona);
    }
}
}