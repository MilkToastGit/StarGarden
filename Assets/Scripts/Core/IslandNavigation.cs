using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarGarden.Core
{
    public class IslandNavigation : MonoBehaviour
    {
        public float zoomedOutSize, zoomedInSize;
        public ParticleSystem cloudsIn, cloudsOut;
        public SpriteRenderer cloudFill;
        public GameObject islandSelect;

        private bool busyZoomin = false;
        private Camera cam;
        private CameraControl camControl;
        private PreviewIsland[] previewIslands;

        private void Awake()
        {
            cam = Camera.main;
            camControl = cam.GetComponent<CameraControl>();
            previewIslands = islandSelect.GetComponentsInChildren<PreviewIsland>();
        }

        public void ZoomOut() { if (!busyZoomin) StartCoroutine(IZoomOut()); }
        private IEnumerator IZoomOut()
        {
            busyZoomin = true;
            camControl.enabled = false;
            cloudsOut.Play();

            float targetScale = zoomedOutSize / zoomedInSize;
            //Vector3 cloudFillStartSize = cloudFill.transform.localScale;
            Color cloudFillStart = cloudFill.color;
            cloudFillStart.a = 0f;
            Color cloudFillTarget = cloudFill.color;
            cloudFillTarget.a = 1f;

            bool switchPerformed = false;
            float zoomTime = 3f;
            for (float elapsed = 0; elapsed < zoomTime; elapsed += Time.deltaTime)
            {
                float t = elapsed / zoomTime;
                t = t * t;// * (3f - 2f * t);

                cam.orthographicSize = Mathf.Lerp(zoomedInSize, zoomedOutSize, t);
                cloudsOut.transform.localScale = Mathf.Lerp(1f, targetScale, t) * Vector2.one;
                //cloudFill.transform.localScale = cloudFillStartSize * targetScale;
                cloudFill.color = Color.Lerp(cloudFillStart, cloudFillTarget, t < 0.5f ? t * 2 : 1 - (t - 0.5f) * 2);

                if (!switchPerformed && t > 0.5f)
                {
                    IslandManager.Main.DisableActiveIsland();
                    islandSelect.SetActive(true);
                    switchPerformed = true;
                    transform.position = new Vector3(0, 0, -10);
                }

                yield return null;
            }
            busyZoomin = false;
        }

        public void ZoomIntoIsland(int island) { if (!busyZoomin) StartCoroutine(IZoomIntoIsland(island)); }
        private IEnumerator IZoomIntoIsland(int island)
        {
            busyZoomin = true;
            camControl.enabled = false;
            cloudsIn.Play();

            Vector3 targetPos = IslandManager.Main.Islands[island].Bounds.center;
            targetPos.z = -10;
            float cloudStartScale = zoomedOutSize / zoomedInSize;
            Color cloudFillStart = cloudFill.color;
            cloudFillStart.a = 0f;
            Color cloudFillTarget = cloudFill.color;
            cloudFillTarget.a = 1f;

            bool switchPerformed = false;
            float zoomTime = 3f;
            for (float elapsed = 0; elapsed < zoomTime; elapsed += Time.deltaTime)
            {
                float t = elapsed / zoomTime;
                t = t * t * (3f - 2f * t);

                cam.orthographicSize = Mathf.Lerp(zoomedOutSize, zoomedInSize, t);
                cloudsIn.transform.localScale = Mathf.Lerp(cloudStartScale, 1f, t) * Vector2.one;
                cloudFill.color = Color.Lerp(cloudFillStart, cloudFillTarget, t < 0.5f ? t * 2 : 1 - (t - 0.5f) * 2);
                    
                if (!switchPerformed && t > 0.5f)
                {
                    IslandManager.Main.SetActiveIsland(island);
                    islandSelect.SetActive(false);
                    switchPerformed = true;
                }

                yield return null;
            }

            camControl.enabled = true;
            busyZoomin = false;
        }
    }
}