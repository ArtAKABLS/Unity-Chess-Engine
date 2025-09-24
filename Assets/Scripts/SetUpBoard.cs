using UnityEngine;
using System.Collections.Generic;

public class SetUpBoard : MonoBehaviour
{

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
        fenStringInterpreter(startingFen);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}