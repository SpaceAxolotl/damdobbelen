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
    private bool isWhiteTurn = true;

    private bool hasKilled;

    private void Start()
    {
        GenerateBoard();
        forcedPieces = new List<Piece>();
    }
    private void Update()
    {
        UpdateMouseOver();
       
        //if it is my turn, then select piece.
        {
            int x = (int)mouseOver.x;
            int y = (int)mouseOver.y;
        if (Input.GetMouseButtonDown(0))
            SelectPiece(x,y);
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
        bool isPieceWhite = (y > 3) ? false : true;
        GameObject go = Instantiate((isPieceWhite)?whitePiecePrefab:blackPiecePrefab) as GameObject;
        go.transform.SetParent(transform);
        Piece p = go.GetComponent<Piece>();
        pieces[x, y] = p;
        MovePiece(p, x, y);
    }

    private void MovePiece(Piece p, int x, int y)
    {
        p.transform.position = (Vector3.right * x) + (Vector3.forward * y) + boardOffset + pieceOffset;
    }

    private void SelectPiece(int x, int y)
    {
        //out of bounds
        if(x < 0 || x>=8 || y<0 || y>= 8)
            return;

        Piece p = pieces[x, y];
        if (p != null ) //should be isWhite == isWhite, but I'm getting an error (&& p.isWhite == isWhite)
        {
            if (forcedPieces.Count == 0)
            {
                selectedPiece = p;
                startDrag = mouseOver;
                Debug.Log("je kunt niemand slaan");
                
            }
            else //if (forcedPieces.Count>=1) probleem: wil validmove gebruiken maar kan geen parameters meegeven want die bestaan niet
            {
                //look for the piece under our forced pieces list
                if (forcedPieces.Find(fp => fp == p) == null)
                    return;
                selectedPiece = p;
                startDrag = mouseOver;
                Debug.Log("je kunt iemand slaan!");
            }
        }
    }

    

    private void TryMove(int x1, int y1, int x2, int y2)
    {
        forcedPieces = ScanForPossibleMove();

        //multiplayer support
        startDrag = new Vector2(x1, y1);
        endDrag = new Vector2(x2, y2);
        selectedPiece = pieces[x1, y1];

      
        //check if we are out of bounds
        if(x2<0 || x2 >=8 || y2<0 || y2 >= 8)
        {
            if (selectedPiece != null)
                MovePiece(selectedPiece, x1,y1);

            startDrag = Vector2.zero;
            selectedPiece = null;
            return;
        }
        
        if (selectedPiece != null)
        {
            //checks if the piece has not moved
            if (endDrag == startDrag)
            {
                MovePiece(selectedPiece, x1, y1);
                startDrag = Vector2.zero;
                selectedPiece = null;
                return;
            }

            //checks if it's a valid move
            if (selectedPiece.ValidMove(pieces, x1, y1, x2, y2))
            {
                //did we kill anything?
                //if this is a jump:
                if (Mathf.Abs(x2 - x1) == 2)
                {
                    Piece p = pieces[(x1 + x2) / 2, (y1 + y2) / 2];
                    if (p != null)
                    {
                        pieces[(x1 + x2) / 2, (y1 + y2) / 2] = null;
                        Destroy(p.gameObject);
                        hasKilled = true;
                    }
                }

                //were we supposed to kill anything?

                if(forcedPieces.Count != 0 && !hasKilled)
                {
                    MovePiece(selectedPiece, x1, y1);
                    startDrag = Vector2.zero;
                    selectedPiece = null;
                    return;
                }

                pieces[x2, y2] = selectedPiece;
                pieces[x1, y1] = null;
                MovePiece(selectedPiece, x2, y2);
                EndTurn();
            }
            else
            {
                MovePiece(selectedPiece, x1, y1);
                startDrag = Vector2.zero;
                selectedPiece = null;
                return;
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
        selectedPiece = null;
        startDrag = Vector2.zero;         
        hasKilled = false;

        if (isWhiteTurn)
        {
            isWhiteTurn = !isWhiteTurn;
            Debug.Log("not white's turn");
        }
        else
        {
            isWhiteTurn = true;
            Debug.Log("white's turn");
        }
        CheckVictory();
    }

    private void CheckVictory()
    {

    }

    private List<Piece> ScanForPossibleMove()
    {
        forcedPieces = new List<Piece>();
        //check all the pieces
        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
                if (pieces[i, j] != null && pieces[i, j].isWhite == isWhite)
                    if (pieces[i, j].IsForceToMove(pieces, i, j))
                        forcedPieces.Add(pieces[i, j]);
        return forcedPieces;
    }
}
