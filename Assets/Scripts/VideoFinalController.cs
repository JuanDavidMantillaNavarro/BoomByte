using UnityEngine;
using UnityEngine.Video;

public class VideoFinalController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public CreditosFinales creditosFinales;

    private bool reproduciendo = false;

    void Start()
    {
        gameObject.SetActive(false);

        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += VideoTerminado;
        }
    }

    public void IniciarVideo()
    {
        if (reproduciendo)
            return;

        reproduciendo = true;

        gameObject.SetActive(true);

        videoPlayer.Play();
    }

    private void VideoTerminado(VideoPlayer vp)
    {
        gameObject.SetActive(false);

        if (creditosFinales != null)
        {
            creditosFinales.IniciarCreditos();
        }
    }
}