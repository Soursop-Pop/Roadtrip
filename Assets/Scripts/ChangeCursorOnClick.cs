using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class ChangeCursorOnClick : MonoBehaviour
{
    //texture for the mouse
    //default mouse, hand is open
    [SerializeField] private Texture2D cursorDefault;
    
    //when mouse is clicked show closed hand
    [SerializeField] private Texture2D cursorClosed;
    
    private Vector2 cursorHotspotDefault;
    private Vector2 cursorHotspotClosed;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("OnMouseUp Sprite, Hand is open");
        cursorHotspotDefault = new Vector2(cursorDefault.width / 2, cursorDefault.height/2);
        Cursor.SetCursor(cursorDefault, cursorHotspotDefault, CursorMode.Auto);
        

    }

    // Update is called once per frame
    void Update()
    {
        //Default hand - back to the open hand 
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("Default Sprite, Hand is open");
            cursorHotspotDefault = new Vector2(cursorDefault.width / 2, cursorDefault.height/2);
            Cursor.SetCursor(cursorDefault, cursorHotspotDefault, CursorMode.Auto);
        }
        
        //Closed hand - when hand grabs an object
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Closed Sprite, Hand is closed grabbing something");
            cursorHotspotClosed = new Vector2(cursorClosed.width / 2, cursorClosed.height/2);
            Cursor.SetCursor(cursorClosed, cursorHotspotClosed, CursorMode.Auto);
        }
    }
    

    

}
