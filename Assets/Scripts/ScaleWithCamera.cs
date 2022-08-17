using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarGarden
{
    public class ScaleWithCamera : MonoBehaviour
    {
        public Core.IslandNavigation islandNavigation;
        public Camera cam;

        Vector3 initialScale;

        private void Awake()
        {
            if (!cam)
                cam = Camera.main;
            if (!islandNavigation)
                islandNavigation = cam.GetComponent<Core.IslandNavigation>();
            initialScale = transform.localScale;
        }

        private void OnEnable()
        {
            transform.localScale = initialScale * cam.orthographicSize / islandNavigation.zoomedInSize;
        }
    }
}