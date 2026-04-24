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

    // Direcciones en el plano XZ (equivalente a las 4 del 2D)
    private static readonly Vector3[] Direcciones =
    {
        Vector3.forward,   // caso 0 — equivale a Vector2.up
        Vector3.back,      // caso 1 — equivale a Vector2.down
        Vector3.left,      // caso 2
        Vector3.right      // caso 3
    };

    void Start()
    {
        //CargarMemoria(); 
    }

    public int ElegirAccion(Vector3 estado)
    {
       // Aplanamos al plano XZ para ignorar diferencias de altura
        Vector3 estadoPlano = new Vector3(estado.x, 0f, estado.z);
        Vector3 dirHaciaJugador = estadoPlano.normalized;
 
        float[] pref = new float[4];
        for (int i = 0; i < 4; i++)
        {
            float alineacion = Vector3.Dot(Direcciones[i], dirHaciaJugador);
            pref[i] = alineacion + pesosPolitica[i];
        }
 
        // Acción con mayor preferencia
        int   mejorAccion = 0;
        float maxVal      = pref[0];
        for (int i = 1; i < pref.Length; i++)
        {
            if (pref[i] > maxVal) { maxVal = pref[i]; mejorAccion = i; }
        }
 
        // Zonas de comportamiento (idéntico al original)
        float distancia = estadoPlano.magnitude;
 
        if (distancia > 3f)
        {
            if (Random.value < 0.05f)
                mejorAccion = Random.Range(0, 4);
        }
        else if (distancia > 1f && distancia <= 3f)
        {
            if (Random.value < 0.7f)
                mejorAccion = System.Array.IndexOf(pref, Mathf.Max(pref));
        }
        else
        {
            if (Random.value < 0.3f)
                mejorAccion = Random.Range(0, 4);
        }
 
        return mejorAccion;
    }

    public void Aprende(Vector3 estado, int accion, float recompensa)
    {
        if (accion < 0 || accion >= pesosPolitica.Length)
        {
            Debug.LogWarning("[AgentBrain] Acción inválida: " + accion);
            return;
        }
 
        float nuevoVal = recompensa + Descuento * ValEstimado;
        float tdError  = nuevoVal - ValEstimado;
 
        ValEstimado           += TasaAprendizaje * tdError;
        pesosPolitica[accion] += TasaAprendizaje * tdError;
 
        if (tdError > 0.01f)
            GuardarMemoria();

    }

    public void DarRecompensa(float r)
    {
        ValEstimado += r;
        if (r > 5f)
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