using System;
using System.Collections;
using UnityEngine;

public class LoopPuzzlePiece : MonoBehaviour
{
    //we will assign a value for each side of the pipe 0 for nothing 1 for a connecting point and have them rotate when the sprite rotates 
    //creating the array that will store the values of each piece type
    public int[] sideValues; // clockwise from top = Element 0 will be top, 1 will be right, 2 will  be bottom, 3 will be left
    
    public float speed = 0.3f;
    
    float realRotation;
    
    //using an instance we will check sweep on each turn from the game manager
    public LoopGameManager gm;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<LoopGameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // if transform.euler angles is not equal to real rotation
        if (transform.eulerAngles.z != realRotation)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, realRotation), speed);
        }
    }

    //when mouse clicks 
    private void OnMouseDown()
    {
        int difference = -gm.QuickSweep((int)transform.position.x, (int)transform.position.y);
        
        
        RotatePiece();

        difference += gm.QuickSweep((int)transform.position.x, (int)transform.position.y);
        
        //Check current value based off the move made 
        gm.puzzle.currentValue += difference;

        if (gm.puzzle.currentValue == gm.puzzle.winValue)
        {
            gm.Win();
        }
        
        

    }

    public void RotatePiece()
    {
        //set rotation value
        realRotation += 90;

        if (realRotation == 360)
        {
            realRotation = 0;
        }
        
        RotateSideValues();
    }
    
    
    //rotate the sideValues along with the sprite
    public void RotateSideValues()
    {
        //temp variable for the last element 
        int aux = sideValues[0];
        
        
        for (int i = 0; i < sideValues.Length - 1; i++)
        {
            //when it rotates move the value from the current space to the next one in the array
            sideValues[i] = sideValues[i + 1];
        }
        
        //move the value from the last element to the first one
        sideValues[3] = aux;
    }
    
}
