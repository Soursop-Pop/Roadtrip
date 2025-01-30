using UnityEngine;

public class PipeGameManager : MonoBehaviour
{
    public GameObject PipeHolder;
    public GameObject[] Pipes;
    
    [SerializeField]
    int totalPipes = 0; 
    
    [SerializeField]
    int correctedPipes = 0;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        totalPipes = PipeHolder.transform.childCount;
        
        Pipes = new GameObject[totalPipes];

        for (int i = 0; i < Pipes.Length; i++)
        {
            Pipes[i] = PipeHolder.transform.GetChild(i).gameObject;
        }
    }

    public void CorrectMove()
    {
        correctedPipes += 1;
        
        Debug.Log("correct move");

        if (correctedPipes == totalPipes)
        {
            Debug.Log("You Win!");
        }
    }

    public void WrongMove()
    {
        correctedPipes -= 1;
    }
    
}
