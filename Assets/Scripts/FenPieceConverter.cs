using UnityEngine;
using System.Collections.Generic;
public class FenPieceConverter
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
}
