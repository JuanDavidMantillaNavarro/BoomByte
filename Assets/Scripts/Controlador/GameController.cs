using UnityEngine;
using FMODUnity;
using System;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    [Header("Vistas")]
    public LetrerosController letrerosController;
    public UIManagerVR uiManager;
    public CameraViewManager cameraViewManager;

    [Header("Controladores")]
    public EffectManager effectManager;

    [Header("FMOD - Audio")]
    [SerializeField] private EventReference enemigoSound;

    [Header("Variables de estado")]
    public int BolasActivas = 0;
    public int MaxBolas = 1;
    public float speedMulti = 1f;
    public float explosionRadiusModifier = 0f;
    public float RadioExplosion;

    public bool abilitiesDisabled = false;

    [Header("Variables de Tiempo")]
    public float gameTime = 600f;
    private float currentTime;
    public int cameraUses = 3;

    public bool isPaused = false;
    public bool gameEnded = false;

    [Header("Efectos")]
    public RadioExplosion radioExplosionDebuff;
    public RadioExplosion radioExplosionBuff;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        currentTime = gameTime;
    }

    void Update()
    {
        if (isPaused || gameEnded) return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            currentTime = 0;
            Perdio();
        }

        uiManager.UpdateTimer(currentTime);
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void Victoria()
    {
        if (gameEnded) return;

        Debug.Log("GANASTE");
        uiManager.MostrarVictoria();
    }

    public void Perdio()
    {
        if (gameEnded) return;

        gameEnded = true;
        Debug.Log("PERDISTE");
        uiManager.MostrarDerrota();
    }

    public void ActivateCameraView()
    {
        if (cameraUses <= 0 || abilitiesDisabled) return;

        cameraUses--;
        cameraViewManager.ShowSecondaryView(5f);
    }

    public void OnBallGrab()
    {
        letrerosController.agarrarBola = true;
        TutorialHintManager.Instance.DetectarAgarre();
    }

    public void OnBallExploded(Vector3 position, float Radio, GameObject explosionPrefab)
    {
        GameObject fxExplosion = Instantiate(explosionPrefab, position, Quaternion.identity);

        var expScript = fxExplosion.GetComponent<EnergyExplosion>();
        if (expScript != null)
            expScript.Initialize(Radio);
    }

    // 🔥 AQUÍ VA EL SONIDO DEL ENEMIGO
    public void OnEnemyCollide(Vector3 position, String ENEMY)
    {
        RuntimeManager.PlayOneShot(enemigoSound, position);

        if(ENEMY=="IA")
        {
            effectManager.ApplyEffect(radioExplosionDebuff);
        }
        if(ENEMY=="ERROR")
        {
            effectManager.ApplyEffect(radioExplosionDebuff);
        }

        Debug.Log("Sonido enemigo + debuff aplicado");
    }

    public void OnNpcCollide()
    {
        effectManager.ApplyEffect(radioExplosionBuff);
    }

    public void OnPlayerExplosion()
    {
        uiManager.ActivarVignette(3f);
    }

    public bool CanSpawnBall()
    {
        return BolasActivas < MaxBolas;
    }

    public void RegisterBallSpawned()
    {
        BolasActivas++;
    }

    public void RegisterBallDestroyed()
    {
        BolasActivas--;
    }

    public void ReiniciarEstado()
    {
        currentTime = gameTime;
        gameEnded = false;
        isPaused = false;

        Time.timeScale = 1f;

        uiManager.UpdateTimer(currentTime);

        Debug.Log("Juego reiniciado + timer reseteado");
    }
}