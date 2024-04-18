using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponcursor : MonoBehaviour
{
    public Vector2 cursor;
    [SerializeField]private Texture2D texture;


     void Start()
    {
        /*
        cursor = new Vector2(texture.width /2 , texture.height / 2);
        Cursor.SetCursor(texture,cursor,CursorMode.Auto);
        */
    }
}
