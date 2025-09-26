using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(BoardState))]
public class BoardEditor : Editor
{
    private BoardState board;

    private char[] fenChars;   // ['.', 'P', 'N', ...]
    private int[] fenValues;   // [0, Piece.Pawn|Piece.White, ...]

    void OnEnable()
    {
        board = (BoardState)target;

        // Add '.' for empty square at start
        fenChars = new char[] { '.' }.Concat(FenPieceConverter.PieceFenMap.Keys).ToArray();
        fenValues = new int[] { Piece.None }.Concat(FenPieceConverter.PieceFenMap.Values).ToArray();
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Space(10);
        GUILayout.Label("Board State (FEN)", EditorStyles.boldLabel);

        for (int y = 7; y >= 0; y--)
        {
            GUILayout.BeginHorizontal();
            for (int x = 0; x < 8; x++)
            {
                int currentPiece = board.state[x, y];

                // Find index in dropdown
                int currentIndex = System.Array.IndexOf(fenValues, currentPiece);
                if (currentIndex < 0) currentIndex = 0; // fallback to empty

                int newIndex = EditorGUILayout.Popup(currentIndex,
                    fenChars.Select(c => c.ToString()).ToArray(),
                    GUILayout.Width(30));

                if (newIndex != currentIndex)
                {
                    Undo.RecordObject(board, "Edit Board State");
                    board.state[x, y] = fenValues[newIndex];
                    EditorUtility.SetDirty(board);
                }
            }
            GUILayout.EndHorizontal();
        }
    }
}
