using System;
using UnityEngine;

public class CharacterFlashing : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Flash()
    {
        spriteRenderer.enabled = !spriteRenderer.enabled;
    }

    public void StopFlashing()
    {
        spriteRenderer.enabled = true;
    }
}