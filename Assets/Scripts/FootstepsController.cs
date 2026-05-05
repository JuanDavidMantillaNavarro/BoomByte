using UnityEngine;
using UnityEngine.InputSystem;
using FMODUnity;
using FMOD.Studio;

public class FootstepsController : MonoBehaviour
{
    [Header("FMOD")]
    [SerializeField] private EventReference footstepsEvent;

    private EventInstance footstepsInstance;
    private bool isPlaying = false;

    void Start()
    {
        footstepsInstance = RuntimeManager.CreateInstance(footstepsEvent);
    }

    void Update()
    {
        Keyboard teclado = Keyboard.current;
        if (teclado == null) return;

        bool moving =
            teclado.wKey.isPressed ||
            teclado.aKey.isPressed ||
            teclado.sKey.isPressed ||
            teclado.dKey.isPressed;

        if (moving && !isPlaying)
        {
            footstepsInstance.start();
            isPlaying = true;
        }

        if (!moving && isPlaying)
        {
            footstepsInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            isPlaying = false;
        }
    }

    private void OnDestroy()
    {
        footstepsInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        footstepsInstance.release();
    }
}