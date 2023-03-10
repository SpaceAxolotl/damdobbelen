using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public bool isWhite;

    public bool IsForceToMove(Piece[,] board, int x, int y)
    {
        if (isWhite)
        {
            if(x>=2 && y <= 5)//top left
            {
                Debug.Log("now using top left");
                Piece p = board[x - 1, y + 1];
                //if there is a piece, and it is not the same color as ours, we need to kill it
                if(p!=null && p.isWhite != isWhite)
                {
                    //check if it is possible to land after the jump
                    if (board[x - 2, y + 2] == null)
                        Debug.Log("this space is not occupied");
                    return true;
                }
            }
       
        
            if (x <= 5 && y <= 5) //top right
            {
            Debug.Log("now using top right");
                Piece p = board[x + 1, y + 1];
                //if there is a piece, and it is not the same color as ours, we need to kill it
                if (p != null && p.isWhite != isWhite)
                {
                    //check if it is possible to land after the jump
                    if (board[x + 2, y + 2] == null)
                    Debug.Log("this space is not occupied");
                return true;
                }
            }
        }
        else //same thing, but for black team
        {
            if (x >= 2 && y >= 2)//bottom left
            {
                Piece p = board[x - 1, y - 1];
                //if there is a piece, and it is not the same color as ours, we need to kill it
                if (p != null && p.isWhite != isWhite)
                {
                    //check if it is possible to land after the jump
                    if (board[x - 2, y - 2] == null)
                        Debug.Log("this space is not occupied");
                        return true;
                }
            }
        }

        if (x <= 5 && y >= 2) //bottom right
        {
            Piece p = board[x + 1, y - 1];
            //if there is a piece, and it is not the same color as ours, we need to kill it
            if (p != null && p.isWhite != isWhite)
            {
                //check if it is possible to land after the jump
                if (board[x + 2, y - 2] == null)
                    Debug.Log("this space is not occupied");
                return true;
            }
        }
        return false;
    }
        
    


    public bool ValidMove(Piece[,] board, int x1, int y1, int x2, int y2)
    {
        //checks if you are moving on top of another piece
        if(board[x2,y2] != null)
        return false;

        int deltaMove = (int)Mathf.Abs(x1 - x2); // checks how many tiles you are jumping in x axis
        int deltaMoveY = y2 - y1; // checks how many tiles you are jumping in y axis

        if (isWhite)
        {
            if(deltaMove==1)//how many jumps can you go? (hoe ver kan je piece gaan?)
            {
                if (deltaMoveY == 1) //als deltamove en deltamoveY 1 zijn, dan is dit een "valid move" (maar niet eentje waarmee je kunt slaan)
                    return true;
            }
            else if (deltaMove == 2) //als deltamove en deltamoveY 2 zijn, dan is dit ook een valid move (maar met deze move ga je slaan)
            {
                if(deltaMoveY==2)
                {
                    Piece p = board[(x1 + x2) / 2, (y1 + y2) / 2]; //currently checking for the piece inbetween your original position and the position you want to move to
                    if (p != null && p.isWhite != isWhite)//if this piece is not the same color as ours, we can jump over it.
                        return true;
                }
            }
        }

        if (!isWhite)
        {
            if (deltaMove == 1)
            {
                if (deltaMoveY == -1)
                    return true;
            }
            else if (deltaMove == 2)
            {
                if (deltaMoveY == -2)
                {
                    Piece p = board[(x1 + x2) / 2, (y1 + y2) / 2];
                    if (p != null && p.isWhite != isWhite)
                        return true;
                }
            }
        }
        return false;
 
}
}
