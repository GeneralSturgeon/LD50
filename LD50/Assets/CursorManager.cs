using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager instance;

    public Texture2D normal;
    public Texture2D hold;

    public Vector2 hotspot;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }
    }

    public void SetCursorHold()
    {
        Cursor.SetCursor(hold, hotspot, CursorMode.Auto);
    }

    public void SetCursorNormal()
    {
        Cursor.SetCursor(normal, hotspot, CursorMode.Auto);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            SetCursorHold();
        }

        if(Input.GetMouseButtonUp(0))
        {
            SetCursorNormal();
        }

        if (Input.GetMouseButtonDown(1))
        {
            SetCursorHold();
        }

        if (Input.GetMouseButtonUp(1))
        {
            SetCursorNormal();
        }

    }
}
