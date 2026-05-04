using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    [Header("Vistas")]
    public LetrerosController letrerosController;
    public UIManagerVR uiManager;
    public CameraViewManager cameraViewManager;

    [Header("Controladores")]
    public EffectManager effectManager;

    [Header("Variables de estado")]
    public int BolasActivas = 0; //Bolas actualmente en el mundo
    public int MaxBolas = 1; //Limite Max de Bolas permitidas para spawn
    public float speedMulti = 1f; //Multiplicador de velocidad
    public float explosionRadiusModifier = 0f; //Modificador de radio de explosion de EnergyByte
    public float RadioExplosion;

    public bool abilitiesDisabled = false; //Desabilitar habilidades

    [Header("Variables de Tiempo")]
    public float gameTime = 120f; // 2 minutos
    private float currentTime;
    public int cameraUses = 3;

    public bool isPaused = false;
    public bool gameEnded = false;

    [Header("Efectos")]
    public RadioExplosion radioExplosionDebuff;
    public RadioExplosion radioExplosionBuff;


    [Header("UI")]


    [Header("Easter Eggs")]
    public int eggsFound = 0;

    public string[] easterEggMessages = new string[]
    {

    };

    void Awake()
    {

        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (easterEggMessages == null || easterEggMessages.Length == 0)
        {
            easterEggMessages = new string[]
            {
            "¡Cuac! Mi nombre viene de un pato real que vivía en el Campus. Los estudiantes empezaron a llamarlo Patroclo, y luego la Universidad lo adoptó como mascota oficial. ¡Soy leyenda viva! ",
            "¡Cuac! En enero de 2001 éramos solo 25 valientes. Ahora el programa ha crecido como una ola. ¿Sabías que en ese entonces no existía 'Ingeniería Multimedia' en ningún otro lado de Colombia? ",
            "¡Cuac! El Grupo de Investigación en Multimedia (GIM) nació hace naños y sigue vigente. Lo fundaron el profesor Wilson Sarmiento, Helioth Sánchez y Alexander Cerón. ¡Todos ellos están en este juego!",
            "¡Cuac! El semillero SAMI (Aplicaciones Multimedia Interactivas) crea personajes digitales con inteligencia artificial. Algunos de sus proyectos han ganado premios internos. ¡Este pato es fan!",
            "¡Cuac! Multus es otro semillero, pero enfocado en narrativas digitales, realidad virtual y videojuegos. ¿Te gusta crear historias? Allí puedes volar. ",
            "¡Cuac! El Imaginatio es la muestra de proyectos más importante del programa. En 2025 vamos por la versión XVII. Es como un gran escaparate de videojuegos, animaciones y experiencias interactivas.  ",
            "¡Cuac! En Colombia 4.0 (el evento de tecnología más grande del país) homenajearon a nuestra querida profesora Marta Gama. ¡Ella es un ícono de la producción multimedia!",
            "¡Cuac! En 2025, estudiantes viajaron a Ingolstadt, Alemania, con el proyecto EnGlobe Connect. Allí compartieron con otras universidades. ¿Te gustaría salir del país? ¡Prepárate! ",
            "¡Cuac! En 2026 nace MULTIFEST, un evento dedicado a los videojuegos. Organizado por estudiantes de 10° semestre y con apoyo del Club de Videojuegos. ¡No te lo pierdas! ",
            "¡Cuac! En 2026 cumplimos 25 años desde aquella primera clase del 18 de enero de 2001. ¡Cuarto de siglo formando ingenieros multimedia! ¡A celebrar!",
            "¡Cuac! Si ves al ingeniero Eduardo Sierra por los pasillos de la Calle 100, dile gracias. Él es el director del programa en el Campus. Jorge Jaramillo lo es en Bogotá. ¡Llevan el timón! "
            };
        }
    }

    public void CollectEasterEgg()
    {
        eggsFound++;

        if (easterEggMessages == null || easterEggMessages.Length == 0)
        {
            Debug.LogError("No hay mensajes configurados");
            return;
        }

        int index = Mathf.Clamp(eggsFound - 1, 0, easterEggMessages.Length - 1);

        string message = easterEggMessages[index];

        if (uiManager != null)
        {
            uiManager.ShowEasterEggMessage(message);
        }
    }



    public void ActivateCameraView()
    {
        if (cameraUses <= 0 || abilitiesDisabled) return;

        cameraUses--;

        cameraViewManager.ShowSecondaryView(5f);
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

        //gameEnded = true;
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

    //Se llama cuando choca con el Enemigo
    public void OnEnemyCollide()
    {
        effectManager.ApplyEffect(radioExplosionDebuff);
    }

    //Se llama cuando choca con el Profesor
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
}