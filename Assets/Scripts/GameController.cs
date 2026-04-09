using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    [Header("Vistas")]
    public LetrerosController letrerosController;
    public UIManagerVR uiManager;

    public int BolasActivas = 0; //Bolas actualmente en el mundo
    public int MaxBolas = 1; //Limite Max de Bolas permitidas para spawn

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
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