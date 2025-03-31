using System;
using System.Collections;
using UnityEngine;

public class LoopPuzzlePiece : MonoBehaviour
{
    // Values for each side of the pipe (clockwise from top)
    public int[] sideValues; // Element 0 = top, 1 = right, 2 = bottom, 3 = left

    public float speed = 0.3f;
    float realRotation;

    // Reference to the game manager
    public LoopGameManager gm;

    // New flag to control if the piece can be rotated
    public bool canRotate = true;

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<LoopGameManager>();
    }

    void Update()
    {
        // Smoothly interpolate to the target rotation
        if (transform.eulerAngles.z != realRotation)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, realRotation), speed);
        }
    }

    // When the piece is clicked on by the player
    private void OnMouseDown()
    {
        // If rotation is disabled, ignore the input.
        if (!canRotate)
            return;

        int difference = -gm.QuickSweep((int)transform.position.x, (int)transform.position.y);
        RotatePiece();
        difference += gm.QuickSweep((int)transform.position.x, (int)transform.position.y);

        // Update the current connections count
        gm.puzzle.currentValue += difference;

        if (gm.puzzle.currentValue == gm.puzzle.winValue)
        {
            gm.Win();
        }
    }

    public void RotatePiece()
    {
        // Update target rotation
        realRotation += 90;

        if (realRotation == 360)
        {
            realRotation = 0;
        }

        RotateSideValues();
    }

    // Rotate the side values along with the sprite
    public void RotateSideValues()
    {
        // Store the first element
        int aux = sideValues[0];

        // Shift all side values one position
        for (int i = 0; i < sideValues.Length - 1; i++)
        {
            sideValues[i] = sideValues[i + 1];
        }

        // Set the last element to the saved first value
        sideValues[3] = aux;
    }
}
