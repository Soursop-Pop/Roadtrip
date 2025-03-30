using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoopGameManager : MonoBehaviour
{

    private VehicleController vehicle;
    //if we want to generate random map or not
    public bool GenerateRandom;


    public GameObject canvas;

    //array of possible pieces we can use to make a puzzle
    public GameObject[] piecePrefabs;

    [System.Serializable]
    public class Puzzle
    {
        public int winValue; //equal to half the number of exits in the puzzle (add the exit values and divide by 2 = number of Connections 
        public int currentValue;

        public int width;
        public int height;
        public LoopPuzzlePiece[,] pieces;

    }

    public Puzzle puzzle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //in case we forget to turn it off in inspector it will be off on start
        canvas.SetActive(false);

        vehicle = FindFirstObjectByType<VehicleController>();

        if (GenerateRandom)
        {
            if (puzzle.width == 0 || puzzle.height == 0)
            {
                Debug.LogError("Please set the dimensions of the puzzle");
                Debug.Break();
            }

            GeneratePuzzle();


        }
        else
        {
            Vector2 dimensions = CheckDimensions();

            puzzle.width = (int)dimensions.x;
            puzzle.height = (int)dimensions.y;

            puzzle.pieces = new LoopPuzzlePiece[puzzle.width, puzzle.height];

            foreach (var piece in GameObject.FindGameObjectsWithTag("Piece"))
            {
                puzzle.pieces[(int)piece.transform.position.x, (int)piece.transform.position.y] =
                    piece.GetComponent<LoopPuzzlePiece>();
            }
        }

        foreach (var item in puzzle.pieces)
        {
            Debug.Log(item.gameObject.name);
        }

        puzzle.winValue = GetWinValue();

        Shuffle();

        //count the number of current connections ONLY after the shuffle at the start of the game 
        puzzle.currentValue = Sweep();
    }

    void GeneratePuzzle()
    {
        puzzle.pieces = new LoopPuzzlePiece[puzzle.width, puzzle.height];

        //have to put the [] behind int as Rider doesn't like the simplified version
        int[] auxValues = { 0, 0, 0, 0 };


        for (int h = 0; h < puzzle.height; h++)
        {
            for (int w = 0; w < puzzle.width; w++)
            {
                //this method without brackets only works when the code is 1 line long!!!
                if (w == 0)
                    auxValues[3] = 0;
                else
                    auxValues[3] = puzzle.pieces[w - 1, h].sideValues[1];

                if (w == puzzle.width - 1)
                    auxValues[1] = 0;
                else
                    auxValues[1] = Random.Range(0, 2);

                //height restrictions
                if (h == 0)
                    auxValues[2] = 0;
                else
                    auxValues[2] = puzzle.pieces[w, h - 1].sideValues[0];

                if (h == puzzle.height - 1)
                    auxValues[0] = 0;
                else
                    auxValues[0] = Random.Range(0, 2);

                //tells us piece type 
                int valueSum = auxValues[0] + auxValues[1] + auxValues[2] + auxValues[3];

                if (valueSum == 2 && auxValues[0] != auxValues[2])
                    valueSum = 5;

                GameObject go = (GameObject)Instantiate(piecePrefabs[valueSum], new Vector3(w, h, 0), Quaternion.identity);

                //make piece in correct rotation so that it understands the connections it needs to make when generating 
                while (go.GetComponent<LoopPuzzlePiece>().sideValues[0] != auxValues[0] ||
                       go.GetComponent<LoopPuzzlePiece>().sideValues[1] != auxValues[1] ||
                       go.GetComponent<LoopPuzzlePiece>().sideValues[2] != auxValues[2] ||
                       go.GetComponent<LoopPuzzlePiece>().sideValues[3] != auxValues[3])
                {
                    go.GetComponent<LoopPuzzlePiece>().RotatePiece();
                }

                puzzle.pieces[w, h] = go.GetComponent<LoopPuzzlePiece>();

            }
        }

    }


    //TO GO GET THE CURRENT NUMBER OF CONNECTIONS 
    public int Sweep()
    {
        int value = 0;

        //start at height 0 and move up
        for (int h = 0; h < puzzle.height; h++)
        {
            //look along the width until there are no more pieces
            for (int w = 0; w < puzzle.width; w++)
            {
                //compares top until the last line
                if (h != puzzle.height - 1)
                    if (puzzle.pieces[w, h].sideValues[0] == 1 && puzzle.pieces[w, h + 1].sideValues[2] == 1)
                        value++;


                //compares right only if there might be another piece there 
                if (w != puzzle.width - 1)
                    if (puzzle.pieces[w, h].sideValues[1] == 1 && puzzle.pieces[w + 1, h].sideValues[3] == 1)
                        value++;



            }
        }
        return value;
    }

    //what we want to happen on Win! - this is most likely to change when we do the car engine implementation
    public void Win()
    {
        canvas.SetActive(true);
       
    }

    //move to the next level or wherever we need to on completion of the puzzle
    //public void NextLevel(string nextLevel)
    //{
    //    FindFirstObjectByType<LoopGameManager>().ExitMinigame();

    //    SceneManager.LoadScene(nextLevel);
    //}

    public void ExitMinigame()
    {
        if (vehicle != null)
        {
            vehicle.Repair(); // Fix the car
        }

        SceneManager.UnloadSceneAsync("PipeRotationPuzzle_CAR"); // Unload the minigame
    }

    //to optimize the sweep code 
    public int QuickSweep(int w, int h)
    {
        //check bottom, top, right and left of a piece 
        int value = 0;

        //compares top until the last line
        if (h != puzzle.height - 1)
            //numbers within the square brackets are representing the sideValue placements
            if (puzzle.pieces[w, h].sideValues[0] == 1 && puzzle.pieces[w, h + 1].sideValues[2] == 1)
                value++;


        //compares right only if there might be another piece there 
        if (w != puzzle.width - 1)
            if (puzzle.pieces[w, h].sideValues[1] == 1 && puzzle.pieces[w + 1, h].sideValues[3] == 1)
                value++;

        //compare left
        if (w != 0)
            if (puzzle.pieces[w, h].sideValues[3] == 1 && puzzle.pieces[w - 1, h].sideValues[1] == 1)
                value++;


        //compare bottom
        if (h != 0)
            if (puzzle.pieces[w, h].sideValues[2] == 1 && puzzle.pieces[w, h - 1].sideValues[0] == 1)
                value++;


        return value;
    }



    int GetWinValue()
    {
        int winValue = 0;
        foreach (var piece in puzzle.pieces)
        {
            //gives the number of exits as it adds all the sideValues up
            foreach (var j in piece.sideValues)
            {
                winValue += j;
            }
        }

        //divide the winValue by 2 to get the number of NEEDED Connections for the win
        winValue /= 2;

        return winValue;

    }


    //shuffling the pieces on start - random between 90, 180, 270 or nothing
    void Shuffle()
    {
        foreach (var piece in puzzle.pieces)
        {
            int k = Random.Range(0, 4);

            for (int i = 0; i < k; i++)
            {
                //get the piece and rotate the piece 
                piece.RotatePiece();

            }

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
                aux.x = p.transform.position.x;


            if (p.transform.position.y > aux.y)
                aux.y = p.transform.position.y;

        }

        aux.x++;
        aux.y++;

        return aux;
    }

   

}