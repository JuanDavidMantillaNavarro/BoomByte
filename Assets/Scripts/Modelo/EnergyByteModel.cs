using UnityEngine;

[CreateAssetMenu(fileName = "EnergyByteModel", menuName = "Game/EnergyByte Model")]
public class EnergyByteModel : ScriptableObject
{
    [Header("Configuración magnetismo")]
    public float magnetForce = 10f;
    public float extraThrowForce = 1.5f;

    [Header("Configuración de explosión")]
    public float delayBeforeExplode = 2f;
    public float explosionGridRadius = 2f;

    [Header("Detección")]
    public string groundTag = "Ground";
    public float raycastDistance = 10f;
}