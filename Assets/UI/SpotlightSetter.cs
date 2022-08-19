using UnityEngine;
using UnityEngine.UI;

public class SpotlightSetter : MonoBehaviour
{
    public Camera cam;
    public Transform centreTransform;
    public float SpotlightRadius;
    public Vector2 spotlightCentre;

    [SerializeField] private Image image;

    private void SetCentre()
    {
        if (centreTransform == null)
            return;
        if (cam == null)
            cam = Camera.main;
        Vector2 pos = centreTransform.position;
        pos.y *= -1;
        spotlightCentre = cam.WorldToScreenPoint(pos);
    }

    private void OnEnable()
    {
        SetCentre();
        SetProperties();
    }

    private void OnValidate()
    {
        SetCentre();
        SetProperties();
    }

    public void SetProperties()
    {
        image.material.SetFloat("_SpotX", spotlightCentre.x);
        image.material.SetFloat("_SpotY", spotlightCentre.y);
        image.material.SetFloat("_SpotRad", SpotlightRadius);
    }
}
