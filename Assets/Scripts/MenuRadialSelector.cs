using UnityEngine;
using UnityEngine.UI;

public class MenuRadialSelector : MonoBehaviour
{
    public Image[] opciones;
    public Color colorNormal = Color.white;
    public Color colorSeleccionado = Color.yellow;

    public int indiceActual = -1;

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector2 direccion = new Vector2(x, y);

        if (direccion.magnitude > 0.5f)
        {
            float angulo = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
            indiceActual = ObtenerIndice(angulo);
            Resaltar(indiceActual);
        }
        else
        {
            indiceActual = -1;
            ResetearColores();
        }
    }

    int ObtenerIndice(float angulo)
    {
        if (angulo >= -45 && angulo < 45) return 0;      // derecha
        if (angulo >= 45 && angulo < 135) return 1;      // arriba
        if (angulo >= -135 && angulo < -45) return 3;    // abajo
        return 2;                                        // izquierda
    }

    void Resaltar(int index)
    {
        for (int i = 0; i < opciones.Length; i++)
        {
            opciones[i].color = (i == index) ? colorSeleccionado : colorNormal;
        }
    }

    void ResetearColores()
    {
        for (int i = 0; i < opciones.Length; i++)
        {
            opciones[i].color = colorNormal;
        }
    }
}