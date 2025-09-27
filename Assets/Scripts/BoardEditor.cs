using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(BoardState))]
public class BoardEditor : Editor
{
    private BoardState board;
    private PlacePiece placer;

    private char[] fenChars;   // ['.', 'P', 'N', ...]
    private int[] fenValues;   // [0, Piece.Pawn|Piece.White, ...]

    void OnEnable()
    {
        board = (BoardState)target;

        // Find a PlacePiece in the scene (you could also drag one in manually if you prefer)
        placer = Object.FindFirstObjectByType<PlacePiece>();

        // Add '.' for empty square at start
        fenChars = new char[] { '.' }.Concat(FenPieceConverter.PieceFenMap.Keys).ToArray();
        fenValues = new int[] { Piece.None }.Concat(FenPieceConverter.PieceFenMap.Values).ToArray();
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Space(10);
        GUILayout.Label("Board State (FEN)", EditorStyles.boldLabel);

        for (int y = 7; y >= 0; y--) // draw rows top to bottom
        {
            GUILayout.BeginHorizontal();
            for (int x = 0; x < 8; x++)
            {
                int currentPiece = board.state[x, y];

                // Find index in dropdown
                int currentIndex = System.Array.IndexOf(fenValues, currentPiece);
                if (currentIndex < 0) currentIndex = 0; // fallback to empty

                int newIndex = EditorGUILayout.Popup(
                    currentIndex,
                    fenChars.Select(c => c.ToString()).ToArray(),
                    GUILayout.Width(30));

                if (newIndex != currentIndex)
                {
                    Undo.RecordObject(board, "Edit Board State");
                    int newPiece = fenValues[newIndex];
                    board.state[x, y] = newPiece;
                    EditorUtility.SetDirty(board);

                    // --- NEW: place or clear piece on the board ---
                    if (placer != null)
                    {
                        string squareName = $"Square ({x},{y})";

                        // If clearing square, destroy existing piece children
                        if (newPiece == Piece.None)
                        {
                            var square = placer.GetSquare(x, y);
                            if (square != null)
                            {
                                foreach (Transform child in square)
                                {
                                    DestroyImmediate(child.gameObject);
                                }
                            }
                        }
                        else
                        {
                            // First clear old piece, then place new one
                            var square = placer.GetSquare(x, y);
                            if (square != null)
                            {
                                foreach (Transform child in square)
                                {
                                    DestroyImmediate(child.gameObject);
                                }
                            }

                            placer.Place(squareName, newPiece);
                        }
                    }
                }
            }
            GUILayout.EndHorizontal();
        }
    }
}
