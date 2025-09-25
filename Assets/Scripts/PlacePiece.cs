using UnityEngine;

public class PlacePiece : MonoBehaviour
{
    // Prefabs (name them like FEN, e.g., "wpawn", "bpawn", etc.)
    public GameObject wpawnPrefab;
    public GameObject wrookPrefab;
    public GameObject wknightPrefab;
    public GameObject wbishopPrefab;
    public GameObject wkingPrefab;
    public GameObject wqueenPrefab;
    public GameObject bpawnPrefab;
    public GameObject brookPrefab;
    public GameObject bknightPrefab;
    public GameObject bbishopPrefab;
    public GameObject bkingPrefab;
    public GameObject bqueenPrefab;

    public Transform GetSquare(int x, int y)
    {
        string squareName = $"Square ({x},{y})";
        return transform.Find(squareName);
    }

    public void Place(string squareName, int piecenum)
    {
        Transform square = transform.Find(squareName);

        if (square != null)
        {
            GameObject prefab = GetPrefab(piecenum);

            if (prefab == null)
            {
                Debug.LogWarning("No prefab found for piece: " + piecenum);
                return;
            }

            GameObject piece = Instantiate(prefab, square.position, Quaternion.identity);
            piece.transform.SetParent(square);
        }
        else
        {
            Debug.LogWarning("Square not found: " + squareName);
        }
    }

    private GameObject GetPrefab(int piecenum)
    {
        int pieceType = piecenum & 7;

        // Now, determine the color.
        bool isWhite = (piecenum & Piece.White) != 0;

        switch (pieceType)
        {
            case Piece.King:
                return isWhite ? wkingPrefab : bkingPrefab;
            case Piece.Pawn:
                return isWhite ? wpawnPrefab : bpawnPrefab;
            case Piece.Knight:
                return isWhite ? wknightPrefab : bknightPrefab;
            case Piece.Bishop:
                return isWhite ? wbishopPrefab : bbishopPrefab;
            case Piece.Rook:
                return isWhite ? wrookPrefab : brookPrefab;
            case Piece.Queen:
                return isWhite ? wqueenPrefab : bqueenPrefab;
            default:
                Debug.LogWarning("Unknown piece type: " + pieceType);
                return null;
        }
    }
}
