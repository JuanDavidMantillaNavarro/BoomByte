using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class EnemyProximitySound : MonoBehaviour
{
    [Header("FMOD")]
    [SerializeField] private EventReference proximitySound;

    [Header("Configuración")]
    [SerializeField] private Transform player;
    [SerializeField] private float detectionRange = 4f;

    private EventInstance soundInstance;
    private bool isPlaying = false;

    private void Start()
    {
        soundInstance = RuntimeManager.CreateInstance(proximitySound);

        if (player == null && Camera.main != null)
            player = Camera.main.transform;
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRange && !isPlaying)
        {
            soundInstance.start();
            isPlaying = true;
        }
        else if (distance > detectionRange && isPlaying)
        {
            soundInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            isPlaying = false;
        }
    }

    private void OnDestroy()
    {
        soundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        soundInstance.release();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}