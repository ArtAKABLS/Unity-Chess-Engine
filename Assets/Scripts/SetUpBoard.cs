using UnityEngine;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using Unity.Collections;

public class SetUpBoard : MonoBehaviour
{
    public PlacePiece piecePlacer;
    static int[,] fenStringInterpreter(string fen)
    {


        int[,] board = new int[8, 8];
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
            }
            else if (FenPieceConverter.PieceFenMap.ContainsKey(c))
            {
                int piece = FenPieceConverter.PieceFenMap[c];
                board[rank, file] = piece;
                file++;
            }
        }
        return board;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        string startingFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR";
        int[,] board = fenStringInterpreter(startingFen);
         for (int rank = 0; rank < 8; rank++)
            {
                for (int file = 0; file < 8; file++)
                {
                    int piece = board[rank, file];
                    if (piece != Piece.None)
                    {
                        // Translate rank/file into your square naming convention
                        string squareName = $"Square ({file},{rank})";

                        Debug.Log($"Placing pieceNum={piece} at {squareName}");
                        // Ask PlacePiece to handle instantiation
                        piecePlacer.Place(squareName, piece);
                    }
                }
            }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}