using UnityEngine;
using UnityEngine.InputSystem;

public class LoopGameManager : MonoBehaviour
{
    [System.Serializable]
    public class Puzzle
    {
        public int width;
        public int height;
        public LoopPuzzlePiece[ , ] pieces;
        
    }
    
    public Puzzle puzzle;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector2 dimensions = CheckDimensions();
        
        puzzle.width = (int)dimensions.x;
        puzzle.height = (int)dimensions.y;

        puzzle.pieces = new LoopPuzzlePiece[puzzle.width, puzzle.height];
            
        foreach (var piece in GameObject.FindGameObjectsWithTag("Piece"))
        {
            puzzle.pieces[(int)piece.transform.position.x, (int)piece.transform.position.y] = piece.GetComponent<LoopPuzzlePiece>();
        }

        foreach (var item in puzzle.pieces)
        {
            Debug.Log(item.gameObject.name);
        }
        
    }
    
    //check dimensions of the puzzle 
    Vector2 CheckDimensions()
    {
        Vector2 aux = Vector2.zero;
        
        GameObject[] pieces = GameObject.FindGameObjectsWithTag("Piece");
        
        //for each 
        foreach (var p in pieces)
        {
            if (p.transform.position.x > aux.x)
            {
                aux.x = p.transform.position.x;
            }

            if (p.transform.position.y > aux.y)
            {
                aux.y = p.transform.position.y;
            }
            
        }

        aux.x++;
        aux.y++;
        
        return aux;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
