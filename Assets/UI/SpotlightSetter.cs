using UnityEngine;
using UnityEngine.UI;

public class SpotlightSetter : MonoBehaviour
{
    public Vector2 SpotlightCentre;
    public float SpotlightRadius;

    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
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
        if (image == null) Awake();

        image.material.SetFloat("_SpotX", SpotlightCentre.x);
        image.material.SetFloat("_SpotY", SpotlightCentre.y);
        image.material.SetFloat("_SpotRad", SpotlightRadius);
    }
}
