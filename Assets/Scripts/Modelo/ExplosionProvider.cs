using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Comfort;

public class ExplosionVignetteProvider : MonoBehaviour, ITunnelingVignetteProvider
    {
        public VignetteParameters parameters;
        public VignetteParameters vignetteParameters => parameters;
    }