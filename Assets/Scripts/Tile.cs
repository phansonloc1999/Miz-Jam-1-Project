using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public delegate void SelectTileHandler(GameObject selectedChar);
    public event SelectTileHandler selectedTile;
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(MOUSE_BUTTON.PRIMARY))
        {
            selectedTile?.Invoke(this.gameObject);
        }
    }

    public void flash()
    {
        spriteRenderer.enabled = !spriteRenderer.enabled;
    }

    public void stopFlashing()
    {
        spriteRenderer.enabled = true;
    }
}
