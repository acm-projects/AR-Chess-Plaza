using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChessBoard : MonoBehaviour
{

    [Header("Art stuff")]
    [SerializeField] private Material tileMaterial;
    [SerializeField] private float tileSize = 1.0f;
    [SerializeField] private float yOffset = 0.2f;
    [SerializeField] private Vector3 boardCenter = Vector3.zero;
    [SerializeField] private float deathSize = 0.3f;
    [SerializeField] private float deathSpacing = 0.5f;
    [SerializeField] private float dragOffset = 1.0f;

    [Header("Prefabs & Materials")]
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private Material[] teamMaterials;
    // LOGIC

    private ChessPiece[,] chessPieces;
    private ChessPiece currentlyDragging;
    private List<ChessPiece> deadWhites = new List<ChessPiece>();
    private List<ChessPiece> deadBlacks = new List<ChessPiece>();
    private const int TILE_COUNT_X = 8;
    private const int TILE_COUNT_Y = 8;
    private GameObject[,] tiles;
    private Camera currentCamera;
    private Vector2Int currentHover;
    private Vector3 bounds;

    private void Awake() {
        GenerateAllTiles(tileSize, TILE_COUNT_X, TILE_COUNT_Y);
        SpawnAllPieces();
        PositionAllPieces();
    }

    // Update is called once per frame
    void Update()
    {
        if(!currentCamera) {
            currentCamera = Camera.main;
            return;
        }

        RaycastHit info;
        Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out info, 100, LayerMask.GetMask("Tile", "Hover"))) { // max distance = 100
            // Get the indexes of the tile i've hit
            Vector2Int hitPosition = LookupTileIndex(info.transform.gameObject);
            // Debug.Log($"hitPosition: {hitPosition}");

            // Debug.Log($"{Input.mousePosition}");

            // MARK: MOUSE HOVERING STUFFS

            // if (currentHover == -Vector2Int.one) { // if not hovering on anything before
            //     currentHover = hitPosition;
            //     tiles[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Hover");
            // }

            // // if were already hovering on a tile before
            // if (currentHover != hitPosition) {
            //     tiles[currentHover.x, currentHover.y].layer = LayerMask.NameToLayer("Tile");
            //     currentHover = hitPosition;
            //     tiles[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Hover");
            // }

            
            // MARK: DRAG AND DROP

            // // if left click
            // if (Input.GetMouseButtonDown(0)) { 
            //     if (chessPieces[hitPosition.x, hitPosition.y] != null) {
            //         // check if it is our turn
            //         if (true) { // temporary
            //             currentlyDragging = chessPieces[hitPosition.x, hitPosition.y]; // get the reference

            //         }
            //     }
            // }


            // // if release left click after holding a piece
            // if (currentlyDragging != null && Input.GetMouseButtonUp(0)) { 
            //     Vector2Int previousPostion = new Vector2Int(currentlyDragging.currentX, currentlyDragging.currentY);

            //     bool validMove = MoveTo(currentlyDragging, hitPosition.x, hitPosition.y);
            //     if (!validMove)  {
            //         currentlyDragging.SetPosition(GetTileCenter(previousPostion.x, previousPostion.y));
            //         currentlyDragging = null; // remove the reference
            //     } else {
            //         currentlyDragging = null;
            //     }
            // }

            // MARK: END OF DRAG AND DROP 1



            // Try implementing click to move
            if (Input.GetMouseButtonDown(0) && currentlyDragging == null) { 
                if (chessPieces[hitPosition.x, hitPosition.y] != null) {
                    // check if it is our turn
                    if (true) { // temporary
                        currentlyDragging = chessPieces[hitPosition.x, hitPosition.y]; // get the reference

                    }
                }
            } else if (Input.GetMouseButtonDown(0) && currentlyDragging != null) {
                Vector2Int previousPostion = new Vector2Int(currentlyDragging.currentX, currentlyDragging.currentY);
                bool validMove = MoveTo(currentlyDragging, hitPosition.x, hitPosition.y);
                // if (!validMove)  {
                //     currentlyDragging.SetPosition(GetTileCenter(previousPostion.x, previousPostion.y));
                //     currentlyDragging = null; // remove the reference
                // } else {
                //     currentlyDragging = null;
                // }
            }


        } else { // cursor outside of the board
            if (currentHover != -Vector2Int.one) {
                tiles[currentHover.x, currentHover.y].layer = LayerMask.NameToLayer("Tile");
                currentHover = -Vector2Int.one;
            }

            // if (currentlyDragging && Input.GetMouseButtonUp(0)) {
            //     currentlyDragging.SetPosition(GetTileCenter(currentlyDragging.currentX, currentlyDragging.currentY));
            //     currentlyDragging = null;
            // }

        } 

        // MARK: DRAG AND DROP 2
        // // if draggin a piece, do the levitation effect
        // if (currentlyDragging) {
        //     Plane horizontalPlane = new Plane(Vector3.up, Vector3.up * yOffset);
        //     float distance = 0.0f;
        //     if (horizontalPlane.Raycast(ray, out distance)) {
        //         currentlyDragging.SetPosition(ray.GetPoint(distance) + Vector3.up * dragOffset);
        //     }
        // }

        // MARK: END DRAG AND DROP 2

        
    }


    private void GenerateAllTiles(float tileSize, int tileCountX, int tileCountY) { 
        yOffset += transform.position.y;
        bounds = new Vector3((tileCountX / 2) * tileSize, 0, (tileCountY / 2) * tileSize) + boardCenter;
        tiles = new GameObject[tileCountX, tileCountY];
        for (int x = 0; x < tileCountX; x++) { 
            for (int y = 0; y < tileCountY; y++) {
                tiles[x,y] = GenerateSingleTile(tileSize, x, y);
            }
        } 
    }

    private GameObject GenerateSingleTile(float tileSize, int x, int y) {
        GameObject tileObject = new GameObject(string.Format("X:{0}, Y:{1}", x, y));
        tileObject.transform.parent = transform;

        Mesh mesh = new Mesh();
        tileObject.AddComponent<MeshFilter>().mesh = mesh;
        tileObject.AddComponent<MeshRenderer>().material = tileMaterial;

        Vector3[] vertices = new Vector3[4];
        vertices[0] = new Vector3(x * tileSize, yOffset, y * tileSize) - bounds;
        vertices[1] = new Vector3(x * tileSize, yOffset, (y + 1) * tileSize) - bounds;
        vertices[2] = new Vector3((x + 1) * tileSize, yOffset, y * tileSize) - bounds;
        vertices[3] = new Vector3((x + 1) * tileSize, yOffset, (y + 1) * tileSize) - bounds;

        int[] tris = new int[] {0, 1, 2, 1, 3, 2};

        mesh.vertices = vertices;
        mesh.triangles = tris;
        mesh.RecalculateNormals();

        tileObject.layer = LayerMask.NameToLayer("Tile"); 
        tileObject.AddComponent<BoxCollider>();
        

        return tileObject;
    }

    // Spawning chess pieces
    private void SpawnAllPieces() {
        chessPieces = new ChessPiece[TILE_COUNT_X, TILE_COUNT_Y];
        int whiteTeam = 0, blackTeam = 1;

        // White team
        chessPieces[0, 0] = SpawnSinglePiece(ChessPieceType.Rook, whiteTeam); 
        chessPieces[1, 0] = SpawnSinglePiece(ChessPieceType.Knight, whiteTeam); 
        chessPieces[2, 0] = SpawnSinglePiece(ChessPieceType.Bishop, whiteTeam); 
        chessPieces[3, 0] = SpawnSinglePiece(ChessPieceType.Queen, whiteTeam); 
        chessPieces[4, 0] = SpawnSinglePiece(ChessPieceType.King, whiteTeam); 
        chessPieces[5, 0] = SpawnSinglePiece(ChessPieceType.Bishop, whiteTeam); 
        chessPieces[6, 0] = SpawnSinglePiece(ChessPieceType.Knight, whiteTeam); 
        chessPieces[7, 0] = SpawnSinglePiece(ChessPieceType.Rook, whiteTeam); 

        for (int i = 0; i < TILE_COUNT_X; i++) {
            chessPieces[i, 1] = SpawnSinglePiece(ChessPieceType.Pawn, whiteTeam); 
        }


        // Black team
        chessPieces[0, 7] = SpawnSinglePiece(ChessPieceType.Rook, blackTeam); 
        chessPieces[1, 7] = SpawnSinglePiece(ChessPieceType.Knight, blackTeam); 
        chessPieces[2, 7] = SpawnSinglePiece(ChessPieceType.Bishop, blackTeam); 
        chessPieces[3, 7] = SpawnSinglePiece(ChessPieceType.Queen, blackTeam); 
        chessPieces[4, 7] = SpawnSinglePiece(ChessPieceType.King, blackTeam); 
        chessPieces[5, 7] = SpawnSinglePiece(ChessPieceType.Bishop, blackTeam); 
        chessPieces[6, 7] = SpawnSinglePiece(ChessPieceType.Knight, blackTeam); 
        chessPieces[7, 7] = SpawnSinglePiece(ChessPieceType.Rook, blackTeam); 

        for (int i = 0; i < TILE_COUNT_X; i++) {
            chessPieces[i, 6] = SpawnSinglePiece(ChessPieceType.Pawn, blackTeam); 
        }
    }

    private ChessPiece SpawnSinglePiece(ChessPieceType type, int team) {
        int prefabIndex = team * 6 + (int)type; 
        ChessPiece cp = Instantiate(prefabs[prefabIndex], transform).GetComponent<ChessPiece>();

        cp.type = type;
        cp.team = team;
       
       
        // cp.GetComponent<MeshRenderer>().material = teamMaterials[team]; 
        // Use the material on all children component
        // MeshRenderer[] childrenRenderers = cp.GetComponentsInChildren<MeshRenderer>();
        // foreach(MeshRenderer child in childrenRenderers) {
        //     child.material = teamMaterials[team];
        // }
        

        return cp;
    }


    // Positioning
    private void PositionAllPieces() {
        for (int x = 0; x < TILE_COUNT_X; x++) { 
            for (int y = 0; y < TILE_COUNT_Y; y++) {
                if (chessPieces[x, y] != null) {
                    PositionSinglePiece(x, y, true);
                }
            }
        }
    }

    private void PositionSinglePiece(int x, int y, bool force = false) {
        // the force parameter indicates whether you want smooth transition to the position (false)
        // or just appear to that position instantly (true)

        chessPieces[x, y].currentX = x;
        chessPieces[x, y].currentY = y; 

        chessPieces[x, y].SetPosition(GetTileCenter(x, y), force); 
    }

    private Vector3 GetTileCenter(int x, int y) {
        Vector3 middleOfTile = new Vector3(tileSize / 2, 0, tileSize / 2);
        // without the above vector, the chess piece will spawn at the edge of the tile

        return new Vector3(x * tileSize, yOffset, y * tileSize) - bounds + middleOfTile; 
    }

    private void PositionAttackingPiece(int x, int y) {
        chessPieces[x, y].currentX = x;
        chessPieces[x, y].currentY = y; 

        chessPieces[x, y].SetPosition(GetTileCorner(x, y, chessPieces[x, y].team)); 

    }

    private Vector3 GetTileCorner(int x, int y, int attackerTeam) {
        Vector3 edgeOfTile;
        if (attackerTeam == 0) { // white
            edgeOfTile = new Vector3(tileSize / 2, 0, 0);
        } else { // black
            edgeOfTile = new Vector3(tileSize / 2, 0, tileSize);
        }

        return new Vector3(x * tileSize, yOffset, y * tileSize) - bounds + edgeOfTile;
    }


    // Operations

    public async void CapturingAnimations(ChessPiece cp, ChessPiece otherCP, int x, int y) {
        Debug.Log("Started async");

        // STEP 1: Move attacker to the edge of the tile
        chessPieces[x, y] = cp;
        PositionAttackingPiece(x, y);

        while (!cp.hasFinishedAnimation) {
            await Task.Yield();
        }

        Debug.Log("Finished async for moving to edge tile");


        // STEP 2: Attack animation

        cp.triggerAttack();
        Debug.Log("waiting for Attacking animation");
        while(!cp.AttackAnimationIsPlaying()) { // before attacking animation
            await Task.Yield();
        }
        // the reason for the above is that the animation doesn't get trigger immediately.
        // so the condition below will fail before the animation actually starts.
        Debug.Log("Attacking animation started");
        while(cp.AttackAnimationIsPlaying()) { // while the attacking animation
            await Task.Yield();
        }
        Debug.Log("Attacking animation ended");


        // STEP 3: Death animation

        otherCP.triggerDie();
        Debug.Log("Waiting for die animation ");
        while(!otherCP.DieAnimationIsPlaying()) { // while the death animation
            await Task.Yield();
        }

        Debug.Log("Die animation started");
        while(otherCP.DieAnimationIsPlaying()) { // while the death animation
            await Task.Yield();
        }
        Debug.Log("Die animation ended");


        // STEP 4: Captured piece moves out of the board

        if (otherCP.team == 0) { // white team
            deadWhites.Add(otherCP);
            otherCP.SetScale(Vector3.one * deathSize);
            otherCP.SetPosition(
                new Vector3(TILE_COUNT_Y * tileSize, yOffset, -1 * tileSize) // this will move to the 9th tile which is out of the board (tile start at 0)
                - bounds 
                + new Vector3(tileSize / 2, 0, tileSize / 2) // center of the square
                + (Vector3.forward * deathSpacing) * deadWhites.Count); // line of dead pieces
        }
        if (otherCP.team == 1) { // black team
            deadBlacks.Add(otherCP);
            otherCP.SetScale(Vector3.one * deathSize);
            otherCP.SetPosition(
                new Vector3(-1 * tileSize, yOffset, TILE_COUNT_Y * tileSize) // this will move to the 9th tile to the other direction which is out of the board (tile start at 0)
                - bounds 
                + new Vector3(tileSize / 2, 0, tileSize / 2) // center of the square
                + (Vector3.back * deathSpacing) * deadBlacks.Count);
        }
        
        // ^ remove the chess piece at the previous position, which is the chess piece before the move

        // STEP 5: move the attacker to the middle of the tile
        PositionSinglePiece(x, y, true); // force the piece to appear in the middle of the tile. Don't really need movement here

        currentlyDragging = null;
    }

    // return whether the move is valid or not
    private bool MoveTo(ChessPiece cp, int x, int y) {
        Vector2Int previousPosition = new Vector2Int(cp.currentX, cp.currentY);

        // TODO: Integrate Stockfish validation here

        // check if there is another piece at the target position
        if (chessPieces[x, y] != null) { 
            ChessPiece otherCP = chessPieces[x, y];

            // if on the same team, move back to the old position
            if (cp.team == otherCP.team) { 
                currentlyDragging.SetPosition(GetTileCenter(previousPosition.x, previousPosition.y));
                currentlyDragging = null; // remove the reference
                return false;
            } 
            // enemy team


            CapturingAnimations(cp, otherCP, x, y);



            chessPieces[previousPosition.x, previousPosition.y] = null; 

            return true;
        }

        chessPieces[x, y] = cp; // set the tile at position x, y to be the cp chess piece
        
        chessPieces[previousPosition.x, previousPosition.y] = null; 
        // ^ remove the chess piece at the previous position, which is the chess piece before the move

        PositionSinglePiece(x, y);
        currentlyDragging = null;
        return true;
    }

    // return index (x, y) instead of returning the object
    private Vector2Int LookupTileIndex(GameObject hitInfo) {
        for (int x = 0; x < TILE_COUNT_X; x++) {
            for (int y = 0; y < TILE_COUNT_Y; y++) {
                if (tiles[x, y] == hitInfo) {
                    return new Vector2Int(x, y);
                }
                    
            }
        }

        return -Vector2Int.one; 
        // if cannot find the index, it would be an invalid state (shouldn't happen at all)
        // return the (-1, -1) should crash the game :))
    }
}
