using UnityEngine;
using System.Collections;

namespace Catch
{
    public class ParticleSorting : MonoBehaviour
    {
        // The sorting order can be set in the inspector for the particle effects
        public int sortingOrder;

        void Start()
        {
            // Assign the sorting layer and sorting order
            GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingLayerName = "Sparks";
            GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingOrder = sortingOrder;
        }
    }
}