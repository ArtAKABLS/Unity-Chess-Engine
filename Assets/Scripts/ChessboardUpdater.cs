using UnityEngine;

public class ChessboardUpdater : MonoBehaviour
{
    // Assign the NEW prefabs in the Inspector
    public GameObject newLightSquarePrefab;
    public GameObject newDarkSquarePrefab;

    /// <summary>
    /// Updates the existing chess squares with new prefabs.
    /// </summary>
    public void UpdatePrefabs()
    {
        // Check if the new prefabs are assigned
        if (newLightSquarePrefab == null || newDarkSquarePrefab == null)
        {
            Debug.LogError("New light or dark square prefab is not assigned!");
            return;
        }

        // Get all the child transforms of this object
        Transform[] squares = GetComponentsInChildren<Transform>();

        // Loop through each child square
        for (int i = 0; i < squares.Length; i++)
        {
            Transform squareTransform = squares[i];

            // Skip the parent object itself
            if (squareTransform == this.transform)
            {
                continue;
            }

            // Determine if the square is light or dark based on its position in the hierarchy
            // This assumes the squares were instantiated in a consistent order (e.g., left to right, bottom to top)
            bool isLightSquare = (i - 1) % 2 == 0; // The '-1' accounts for the parent transform at index 0

            // Select the new prefab to instantiate
            GameObject newPrefab = isLightSquare ? newLightSquarePrefab : newDarkSquarePrefab;
            
            // Get the current square's position and rotation
            Vector3 position = squareTransform.position;
            Quaternion rotation = squareTransform.rotation;
            
            // Instantiate the new prefab
            GameObject newSquare = Instantiate(newPrefab, position, rotation, this.transform);
            
            // Maintain the original name for consistency
            newSquare.name = squareTransform.name;
            
            // Destroy the old square object
            DestroyImmediate(squareTransform.gameObject);
        }
    }
}