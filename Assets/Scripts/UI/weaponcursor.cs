using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCursor : MonoBehaviour
{
    public Texture2D defaultCursorTexture; // Texture for the default cursor
    public Texture2D rightClickCursorTexture; // Texture for the cursor on right click
    private Vector2 cursorHotspot;

    void Start()
    {

        cursorHotspot = new Vector2(defaultCursorTexture.width / 2, defaultCursorTexture.height / 2);

        // Set the default cursor at the start
        Cursor.SetCursor(defaultCursorTexture, cursorHotspot, CursorMode.Auto);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Right mouse button
        {
            // Set the cursor to the right-click texture
            Cursor.SetCursor(rightClickCursorTexture, cursorHotspot, CursorMode.Auto);
        }
        else if (Input.GetMouseButtonUp(1)) // Right mouse button released
        {
            // Revert to the default cursor texture
            Cursor.SetCursor(defaultCursorTexture, cursorHotspot, CursorMode.Auto);
        }
    }
}
