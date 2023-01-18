using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CheckersBoard : MonoBehaviour
{
    public Piece[,] pieces = new Piece[8, 8];
    public GameObject whitePiecePrefab;
    public GameObject blackPiecePrefab;
    public Vector3 boardOffset = new Vector3(-4.0f, 0, -4.0f);
    public Vector3 pieceOffset = new Vector3(0.5f, 0, 0.5f);

    private Piece selectedPiece;
    private List<Piece> forcedPieces;

    private Vector2 mouseOver;
    private Vector2 startDrag;
    private Vector2 endDrag;

    public bool isWhite;
    private bool isPlayerWhite;
    private bool isWhiteTurn;

    private bool hasKilled;

    private void Start()
    {
        GenerateBoard();
        isWhiteTurn = true;
    }
    private void Update()
    {
        UpdateMouseOver();
        
        //if it is my turn, then select piece.
        {
            int x = (int)mouseOver.x;
            int y = (int)mouseOver.y;
            if (Input.GetMouseButtonDown(0))
                SelectPiece(x, y);
            if (Input.GetMouseButtonUp(0))
                TryMove((int)startDrag.x, (int)startDrag.y, x, y);

            if (selectedPiece != null)
                UpdatePieceDrag(selectedPiece);


        }
    }
    private void UpdateMouseOver()
    {
        //if it's my turn, 
        if(!Camera.main) //als er geen camera is, geef een error msg
        {
            Debug.Log("unable to find Main Camera");
            return;
        }
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("Board")))
        {
            mouseOver.x = (int)(hit.point.x - boardOffset.x);
            mouseOver.y = (int)(hit.point.z - boardOffset.z);
        }
        else
        {
            mouseOver.x = -1;
            mouseOver.y = -1;
        }
    }
    private void GenerateBoard()
    {
        //generate white team
       
        for(int y = 0; y<3; y++)
        {
            bool oddRow = (y % 2 == 0);
            for (int x = 0; x<8; x += 2)
            {
                //generate our Piece
                GeneratePiece((oddRow) ? x : x+1 , y);
            }
        }
        //generate black team

        for (int y = 7; y >4; y--)
        {
            bool oddRow = (y % 2 == 0);
            for (int x = 0; x < 8; x += 2)
            {
                //generate our Piece
                GeneratePiece((oddRow) ? x : x + 1, y);
            }
        }
    }
    private void GeneratePiece(int x, int y)
    {
        bool isPieceWhite = (y > 3) ? false : true; //if y is bigger than 3, piece is not white. else: ispiecewhite is true
        GameObject go = Instantiate((isPieceWhite)?whitePiecePrefab:blackPiecePrefab) as GameObject;
        go.transform.SetParent(transform);
        Piece p = go.GetComponent<Piece>();
        pieces[x, y] = p;
        MovePiece(p, x, y);
    }

    public void MovePiece(Piece p, int x, int y)
    {
        p.transform.position = (Vector3.right * x) + (Vector3.forward * y) + boardOffset + pieceOffset;
    }

    private void SelectPiece(int x, int y)
    {
        //out of bounds
        if (x < 0 || x >= 8 || y < 0 || y >= 8)
            return;

        Piece p = pieces[x, y];
        if (p != null) //if there is a piece, try to select it
        {
                selectedPiece = p;  
                startDrag = mouseOver;
            Debug.Log(selectedPiece.name);
        }
       
    }

    private void TryMove(int x1, int y1, int x2, int y2) //start position is x1,y1. end position is x2,y2
    {
        startDrag = new Vector2(x1, y1);
        endDrag = new Vector2(x2, y2);
        selectedPiece = pieces[x1, y1];

        //out of bounds
        if(x2<0 || x2>=8 || y2<0 || y2>=8)
        {
            if(selectedPiece != null)
            {
                MovePiece(selectedPiece,x1,y1);
            }
            startDrag = Vector2.zero;
            selectedPiece = null;
            return;
        }
        //is there a selected piece? 
        if(selectedPiece != null)
        {
            //if it has not moved
            if (endDrag == startDrag)
            {
                MovePiece(selectedPiece, x1, y1);
                startDrag = Vector2.zero;
                selectedPiece = null;
                return;
            }

            if (selectedPiece.ValidMove(pieces, x1, y1, x2, y2))
            {
                Debug.Log("valid move!");
                //did we kill anything?
                pieces[x2, y2] = selectedPiece;
                pieces[x1, y1] = null;
                MovePiece(selectedPiece, x2, y2);
                EndTurn();
            }
        }

       
    }

   

    private void UpdatePieceDrag(Piece p)
    {
        if (!Camera.main) //als er geen camera is, geef een error msg
        {
            Debug.Log("unable to find Main Camera");
            return;
        }
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("Board")))
        {
            p.transform.position = hit.point + Vector3.up;
        }
      
    }
    private void EndTurn()
    {
        Debug.Log("endturn");
        selectedPiece = null;
        startDrag = Vector2.zero;         
        hasKilled = false;

        isWhiteTurn = !isWhiteTurn;
        CheckVictory();
    }

    private void CheckVictory()
    {
        //later
    }

    /*private List<Piece> ScanForPossibleMove()
    {
        Debug.Log("scanning for possible moves");
        forcedPieces = new List<Piece>();
        
        //check all the pieces
        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
                if (pieces[i, j] != null && pieces[i, j].isWhite == isWhiteTurn)
                    if (pieces[i, j].IsForceToMove(pieces, i, j))
                        //forcedPieces.Add(pieces[i, j]);
       Debug.Log("forced pieces =" + forcedPieces);
        return forcedPieces;
    }*/
}
