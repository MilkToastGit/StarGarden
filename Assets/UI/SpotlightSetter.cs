using UnityEngine;
using UnityEngine.UI;

public class SpotlightSetter : MonoBehaviour
{
    public Camera cam;
    public Transform spotlightCentre;
    public float SpotlightRadius;

    private Image image;
    [HideInInspector] public Vector2 centre;

    private void SetCentre()
    {
        if (spotlightCentre == null)
            return;
        if (cam == null)
            cam = Camera.main;
        image = GetComponent<Image>();
        Vector2 pos = spotlightCentre.position;
        pos.y *= -1;
        centre = cam.WorldToScreenPoint(pos);
    }

    private void OnEnable()
    {
        SetProperties();
    }

    private void OnValidate()
    {
        SetProperties();
    }

    public void SetProperties()
    {
        SetCentre();

        image.material.SetFloat("_SpotX", centre.x);
        image.material.SetFloat("_SpotY", centre.y);
        image.material.SetFloat("_SpotRad", SpotlightRadius);
    }
}
