using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    [Header("Vistas")]
    public LetrerosController letrerosController;
    public UIManagerVR uiManager;

    [Header("Controladores")]
    public EffectManager effectManager;

    [Header("Variables de estado")]
    public int BolasActivas = 0; //Bolas actualmente en el mundo
    public int MaxBolas = 1; //Limite Max de Bolas permitidas para spawn
    public float speedMulti = 1f; //Multiplicador de velocidad
    public float explosionRadiusModifier = 0f; //Modificador de radio de explosion de EnergyByte

    public bool abilitiesDisabled = false; //Desabilitar habilidades

    [Header("Variables de Tiempo")]
    public float gameTime = 120f; // 2 minutos
    private float currentTime;

    public bool isPaused = false;
    public bool gameEnded = false;
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

        gameEnded = true;
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

    // Se llama por la clase EnergyByte cuando se agarra la bola
    public void OnBallGrab()
    {
        letrerosController.agarrarBola = true;
    }

    // Se llama por la clase EnergyByte cuando la bola explota
    public void OnBallExploded(Vector3 position, float Radio, GameObject explosionPrefab)
    {
        GameObject fxEplosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        var expScript = fxEplosion.GetComponent<EnergyExplosion>();
        if (expScript != null) expScript.Initialize(Radio);
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
}