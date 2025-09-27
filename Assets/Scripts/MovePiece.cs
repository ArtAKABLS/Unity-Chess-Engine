using UnityEngine;
using UnityEngine.EventSystems;
using System.Text;

public class MovePiece : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private Vector3 worldOffset;
    private Camera mainCamera;
    
    private PieceIdentifier pieceIdentifier; 

    // --- REQUIRED REFERENCES (Set in Inspector or Start) ---
    public BoardState boardState; 
    public Transform boardTransform; 
    public LayerMask squareLayerMask; 
    
    // --- PIECE DATA ---
    private Transform originalParent; 

    void Awake()
    {
        pieceIdentifier = GetComponent<PieceIdentifier>();
        if (pieceIdentifier == null)
        {
            Debug.LogError("PieceIdentifier script missing on this piece! Cannot determine piece type.");
        }
    }

    void Start()
    {
        mainCamera = Camera.main; 
        
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found! Ensure your camera is tagged 'MainCamera'.");
        }
        
        // Dynamic lookup for dependencies
        if (boardState == null)
        {
            boardState = FindFirstObjectByType<BoardState>();
        }
        if (boardTransform == null)
        {
            boardTransform = FindFirstObjectByType<PlacePiece>()?.transform;
        }

        if (boardState == null || boardTransform == null)
        {
             Debug.LogError("BoardState or BoardTransform not found. Piece movement may be inconsistent.");
        }
    }

    private int GetPieceValue()
    {
        if (pieceIdentifier == null) return 0;
        return pieceIdentifier.Type | pieceIdentifier.Color;
    }

    private (int x, int y) ParseSquareCoords(string squareName)
    {
        if (string.IsNullOrEmpty(squareName)) return (-1, -1);
        
        string coordsString = squareName.Replace("Square (", "").Replace(")", "").Trim();
        string[] coords = coordsString.Split(',');
        
        if (coords.Length == 2 && 
            int.TryParse(coords[0].Trim(), out int x) && 
            int.TryParse(coords[1].Trim(), out int y))
        {
            if (x >= 0 && x <= 7 && y >= 0 && y <= 7)
            {
                 return (x, y);
            }
        }
        return (-1, -1);
    }
    
    private void PrintBoardState()
    {
        if (boardState == null) return;

        StringBuilder sb = new StringBuilder("\n--- Board State After Move ---\n");

        for (int y = 7; y >= 0; y--)
        {
            sb.Append($"{y + 1} | "); 
            for (int x = 0; x < 8; x++)
            {
                int piece = boardState.state[x, y];
                sb.Append(piece.ToString().PadLeft(2, ' '));
                sb.Append(" ");
            }
            sb.Append("\n");
        }
        sb.Append("  -----------------\n");
        sb.Append("    0 1 2 3 4 5 6 7 (X-Coords)\n");

        Debug.Log(sb.ToString());
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        originalParent = transform.parent;
        
        if (boardTransform != null)
        {
             transform.SetParent(boardTransform); 
        }

        Vector3 mouseWorldPoint = mainCamera.ScreenToWorldPoint(eventData.position);
        worldOffset = transform.position - mouseWorldPoint;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (mainCamera == null) return;

        Vector3 newMouseWorldPoint = mainCamera.ScreenToWorldPoint(eventData.position);
        Vector3 newWorldPosition = newMouseWorldPoint + worldOffset;
        
        // Maintain the original Z-depth (which you stated is 0)
        newWorldPosition.z = transform.position.z; 
        
        transform.position = newWorldPosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Transform targetSquare = null;
        bool validDrop = false;

        // 1. SWITCH TO 2D RAYCAST
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero, 0f, squareLayerMask);
        
        // If the piece is dropped directly onto a square's collider (most reliable 2D check)
        if (hit.collider != null) 
        {
            targetSquare = hit.collider.transform;
            validDrop = true;
            Debug.Log("SUCCESS: 2D Raycast hit target square: " + targetSquare.name);
        }
        else
        {
            Debug.LogWarning("FAILURE: 2D Raycast missed. Check Collider2D/LayerMask.");
        }

        if (validDrop)
        {
            // 2. Handle Potential Capture
            if (targetSquare.childCount > 0)
            {
                foreach (Transform child in targetSquare)
                {
                    if (child.gameObject != this.gameObject)
                    {
                        Destroy(child.gameObject);
                        Debug.Log($"Piece captured on {targetSquare.name}.");
                        break; 
                    }
                }
            }
            
            // 3. Snap Piece Position and Parenting
            Vector3 newPos = targetSquare.position;
            newPos.z = transform.position.z; 
            transform.position = newPos;
            transform.SetParent(targetSquare);
            
            // 4. Update BoardState ARRAY DIRECTLY
            if (boardState != null)
            {
                int currentPieceValue = GetPieceValue(); 
                
                (int oldX, int oldY) = ParseSquareCoords(originalParent?.name);
                (int newX, int newY) = ParseSquareCoords(targetSquare.name);
                
                // Clear the old position
                if (oldX >= 0)
                {
                    boardState.state[oldX, oldY] = 0;
                }
                
                // Set the new position
                if (newX >= 0)
                {
                    boardState.state[newX, newY] = currentPieceValue; 
                }
                else
                {
                     Debug.LogError($"CRITICAL: Target square '{targetSquare.name}' has invalid coordinates.");
                }
                
                PrintBoardState();
            }
        }
        else
        {
            // --- MOVEMENT FAILED: REVERT LOGIC ---
            Debug.LogWarning("Reverting piece to original position.");
            
            if (originalParent != null)
            {
                Vector3 originalPos = originalParent.position;
                originalPos.z = transform.position.z; 
                transform.position = originalPos;
                transform.SetParent(originalParent);
            }
        }
        
        originalParent = null;
    }
}