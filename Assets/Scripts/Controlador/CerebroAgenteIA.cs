using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class AgentBrain : MonoBehaviour
{
    [Header("Hiperparámetros")]
    public float TasaAprendizaje = 0.01f; 
    public float Descuento       = 0.9f;

    private float[] pesosPolitica = new float[4];
    private float   ValEstimado   = 0f;

    private static readonly Vector2[] Direcciones =
    {
        Vector2.up, Vector2.down, Vector2.left, Vector2.right
    };

    void Start()
    {
        CargarMemoria(); 
    }

    public int ElegirAccion(Vector2 estado)
    {
        Vector2 dirHaciaJugador = estado.normalized;
        float[] pref = new float[4];

        for (int i = 0; i < 4; i++)
        {
            float alineacion = Vector2.Dot(Direcciones[i], dirHaciaJugador);
            pref[i] = alineacion + pesosPolitica[i];
        }

        int MejorAccion = 0;
        float maxVal = pref[0];
        for (int i = 1; i < pref.Length; i++)
        {
            if (pref[i] > maxVal) { maxVal = pref[i]; MejorAccion = i; }
        }

        // Lógica de zonas (IDÉNTICA al original)
        float distancia = estado.magnitude;
        if (distancia > 3f)
        {
            if (Random.value < 0.05f) MejorAccion = Random.Range(0, 4);
        }
        else if (distancia > 1f && distancia <= 3f)
        {
            if (Random.value < 0.7f) MejorAccion = System.Array.IndexOf(pref, Mathf.Max(pref));
        }
        else
        {
            if (Random.value < 0.3f) MejorAccion = Random.Range(0, 4);
        }

        return MejorAccion;
    }

    public void Aprende(Vector2 estado, int accion, float recompensa)
    {
        // SEGURIDAD EXTRA: Si la acción no es válida, salimos del método
        if (accion < 0 || accion >= pesosPolitica.Length)
        {
            Debug.LogWarning("Acción inválida recibida: " + accion);
            return; 
        }
        
        float nuevoVal = recompensa + Descuento * ValEstimado;
        float tdError  = nuevoVal - ValEstimado;

        ValEstimado += TasaAprendizaje * tdError; 
        pesosPolitica[accion] += TasaAprendizaje * tdError; 

    }

    public void DarRecompensa(float r)
    {
        ValEstimado += r;

            GuardarMemoria();
        
    }
    public void GuardarMemoria()
    {
        try {
            string ruta = Application.persistentDataPath + "/memoriaAgente.dat";
            BinaryFormatter bf = new BinaryFormatter();
            FileStream archivo = File.Create(ruta);
            bf.Serialize(archivo, pesosPolitica);
            archivo.Close();
        } catch { Debug.LogWarning("Error al guardar memoria"); }
    }

    public void CargarMemoria()
    {
        string ruta = Application.persistentDataPath + "/memoriaAgente.dat";
        if (File.Exists(ruta))
        {
            try {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream archivo = File.Open(ruta, FileMode.Open);
                pesosPolitica = (float[])bf.Deserialize(archivo);
                archivo.Close();
            } catch { Debug.LogWarning("Error al cargar memoria"); }
        }
    }

}