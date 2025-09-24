using Unity.Collections;
using UnityEngine;

[ExecuteInEditMode]
public class ChessBoardCreator : MonoBehaviour
{

    public GameObject lightSquarePrefab;
    public GameObject darkSquarePrefab;

    private const int BOARD_SIZE = 8;
    private float squareSize;

    // Start is called before the first frame update
    void Start()
    {
        // Check if the prefabs are assigned to avoid NullReferenceException
        if (lightSquarePrefab == null || darkSquarePrefab == null)
        {
            Debug.LogError("Light or dark square prefab is not assigned in the Inspector!");
            return;
        }

        // Determine the size of a single square from one of the prefabs.
        // Assumes both prefabs have the same size.
        squareSize = lightSquarePrefab.GetComponent<SpriteRenderer>().bounds.size.x;

        GenerateBoard();
    }

    /// <summary>
    /// Generates an 8x8 chessboard using the provided light and dark square prefabs.
    /// </summary>
    void GenerateBoard()
    {
        // Loop through the grid to create each square
        for (int i = 0; i < BOARD_SIZE; i++)
        {
            for (int j = 0; j < BOARD_SIZE; j++)
            {
                // Use a simple checkerboard pattern logic
                bool isLightSquare = (i + j) % 2 == 0;
                
                // Select the correct prefab based on the color
                GameObject squarePrefab = isLightSquare ? lightSquarePrefab : darkSquarePrefab;

                // Calculate the position for the current square
                Vector3 position = new Vector3(i * squareSize, j * squareSize, 0);

                // Instantiate the prefab at the calculated position.
                // The 'transform' argument parents the new square to this GameObject,
                // keeping the Hierarchy clean.
                GameObject square = Instantiate(squarePrefab, position, Quaternion.identity, transform);
                
                // Give the square a meaningful name for easier debugging
                square.name = $"Square ({i},{j})";
            }
        }
    }
}

