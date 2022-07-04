using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetSpriteShifter : MonoBehaviour
{
    public Sprite[] sprites;
    private SpriteRenderer sr;

    private void Awake() => sr = GetComponentInChildren<SpriteRenderer>();

    public void ChangeSprite(int sprite) => sr.sprite = sprites[sprite];
}
