using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Effects/FlechasGuia")]
public class FlechasGuiaEffect : EffectData
{
    public GameObject flechaPrefab; // Prefab de flecha holográfica
    
    public override void Apply(GameController controller)
    {
        flechaPrefab = GameObject.FindGameObjectWithTag("Flecha");
        flechaPrefab.GetComponent<Image>().enabled = true;
        flechaPrefab.GetComponent<FlechaGuia>().enabled = true;
    }

    public override void Remove(GameController controller)
    {
        flechaPrefab = GameObject.FindGameObjectWithTag("Flecha");
        flechaPrefab.GetComponent<Image>().enabled = false;
        flechaPrefab.GetComponent<FlechaGuia>().enabled = false;
    }
}