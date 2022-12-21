using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LocalVersion.Game
{
    public class MarkerPrefabScript : MonoBehaviour
    {

        [SerializeField] private GameObject prefab;
        [SerializeField] private GameObject outerBoxCollider;

        public void ShowPrefab()
        {
            if (prefab != null)
            {
                prefab.SetActive(true);
            }
        }

        public void HidePrefab()
        {
            if (prefab != null)
            {
                prefab.SetActive(false);
            }
        }

        public void ActivateOuterBoxCollider()
        {
            outerBoxCollider.SetActive(true);
        }
        
        public void DeactivateOuterBoxCollider()
        {
            outerBoxCollider.SetActive(false);
        }
    }
}

