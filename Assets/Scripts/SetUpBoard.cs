using UnityEngine;
using System.Collections.Generic;

public class SetUpBoard : MonoBehaviour
{

public static readonly Dictionary<char, int> PieceFenMap = new Dictionary<char, int>
    {
    // Black pieces (lowercase characters)
    {'p', Piece.Pawn | Piece.Black},
    {'n', Piece.Knight | Piece.Black},
    {'b', Piece.Bishop | Piece.Black},
    {'r', Piece.Rook | Piece.Black},
    {'q', Piece.Queen | Piece.Black},
    {'k', Piece.King | Piece.Black},
    
    // White pieces (uppercase characters)
    {'P', Piece.Pawn | Piece.White},
    {'N', Piece.Knight | Piece.White},
    {'B', Piece.Bishop | Piece.White},
    {'R', Piece.Rook | Piece.White},
    {'Q', Piece.Queen | Piece.White},
    {'K', Piece.King | Piece.White},
    };

    void fenStringInterpreter(string fen)
    {


        int[,] board = new int[8,8];
        int file = 0;
        int rank = 0;

        foreach (char c in fen)
        {
            if (c == '/')
            {
                rank++;
                file = 0;
            }
            else if (char.IsDigit(c))
            {
                file += int.Parse(c.ToString());
            } else if (FenPi)
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
