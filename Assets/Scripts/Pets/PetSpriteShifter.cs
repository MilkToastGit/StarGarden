using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetSpriteShifter : MonoBehaviour
{
    public Transform hatParentBase;
    private Transform[] hatParents;
    public PetSpriteAndHatPositions[] sprites;
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();

        hatParents = new Transform[hatParentBase.childCount];
        for (int i = 0; i < hatParentBase.childCount; i++)
            hatParents[i] = hatParentBase.GetChild(i).transform;
    }

    public void ChangeSprite(int sprite)
    {
        sr.sprite = sprites[sprite].Sprite;
        for (int i = 0; i < hatParents.Length; i++)
            hatParents[i].localPosition = sprites[sprite].LocalHatPositions[i];
    }
}

[System.Serializable]
public class PetSpriteAndHatPositions
{
    public Sprite Sprite;
    public Vector2[] LocalHatPositions;
}