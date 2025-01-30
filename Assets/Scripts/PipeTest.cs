using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PipeTest : MonoBehaviour
{
    //setting up a float for the possible angles the sprite could be rotated in
   float[] rotations = {0,90,180,270};

   //float to check if pipe is at the correct rotation
   public float[] correctRotation;
   
   //bool to check if its correct
   [SerializeField]
   private bool isPlaced = false;

   private int possibleRotations = 1;

   PipeGameManager pipeGameManager;
   
   private void Awake()
   {
       //Finding the PipeGameManager object and the component with the script
       pipeGameManager = GameObject.Find("PipeGameManager").GetComponent<PipeGameManager>();
   }
   
   
   private void Start()
   {
       possibleRotations = correctRotation.Length;
       
        //setting up the randomization
       int rand = Random.Range(0, rotations.Length);
       
       //using euler angles because its Vector based and rotation is Quanternion and its best not to use it for this 
       //rotate the angles of the image to a random one in the list on start
       transform.eulerAngles = new Vector3(0,0, rotations[rand]);

       if (possibleRotations > 1)
       {
           //if transform rotation is equal to the correct rotation
           if (transform.eulerAngles.z == correctRotation[0] || transform.eulerAngles.z == correctRotation[1])
           {
               //set isPlaced is true
               isPlaced = true;
               
               //call CorrectMove function from the pipe game manager
               pipeGameManager.CorrectMove();
           }
       }
       else
       {
           //if transform rotation is equal to the correct rotation
           if (transform.eulerAngles.z == correctRotation[0])
           {
               //set isPlaced is true
               isPlaced = true;
               pipeGameManager.CorrectMove();
           }
       }

       
   }

   //when mouse is down
   private void OnMouseDown()
    {
        //rotate the sprite 
        transform.Rotate(new Vector3(0, 0, 90));

        if (possibleRotations > 1)
        {
            //if the rotation is correct and the isPlaced is false 
            if (transform.eulerAngles.z == correctRotation[0] || transform.eulerAngles.z == correctRotation[1] && isPlaced == false)
            {
                //set isPlaced to true
                isPlaced = true;
                pipeGameManager.CorrectMove();
                
            }
            //else if angle is wrong and isPlaced is true
            else if(transform.eulerAngles.z != correctRotation[0] || transform.eulerAngles.z != correctRotation[1] && isPlaced == true) 
            {
                //set isPlaced to false
                isPlaced = false;
                
                //call WrongMove function from the PipeGameManager script
                pipeGameManager.WrongMove();
            }
        }
        else //if possible rotations is less than one 
        {
            //if the rotation is correct and the isPlaced is false 
            if (transform.eulerAngles.z == correctRotation[0] && isPlaced == false)
            {
                //set isPlaced to true
                isPlaced = true;
                pipeGameManager.CorrectMove();
            }
            //else if angle is wrong and isPlaced is true
            else if(transform.eulerAngles.z != correctRotation[0] && isPlaced == true) 
            {
                //set isPlaced to false
                isPlaced = false;
                pipeGameManager.WrongMove();
           
            }
        }
        
        
    }
    
}
