using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BoardState))]
public class BoardEditor : Editor
{
    private BoardState board;

    void OnEnable()
    {
        board = (BoardState)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // keeps normal Inspector fields (if any)

        GUILayout.Space(10);
        GUILayout.Label("Board State (8x8)", EditorStyles.boldLabel);

        // Draw an 8x8 grid
        for (int y = 7; y >= 0; y--) // flip so 7 is at the top
        {
            GUILayout.BeginHorizontal();
            for (int x = 0; x < 8; x++)
            {
                int newVal = EditorGUILayout.IntField(board.state[x, y], GUILayout.Width(30));
                if (newVal != board.state[x, y])
                {
                    Undo.RecordObject(board, "Edit Board State");
                    board.state[x, y] = newVal;
                    EditorUtility.SetDirty(board);
                }
            }
            GUILayout.EndHorizontal();
        }
    }
}
