using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

namespace DesignPatterns.Command
{
    // moves the player one space, checking if the space can be moved on
    public class PlayerMover : MonoBehaviour
    {
        public Grid grid;
        public Tilemap walkableTiles;
        public Tilemap pickupTiles;
        public Tilemap winTiles; 

        public Tile oilcanTile;
        public Tile woodTile;
        public Tile aquacellTile;
        public Tile floorTile;

        public float moveSpeed = 8f;

        public AudioSource _audiosource;
        public AudioClip moveSFX;
        public AudioClip cantMoveSFX; 

        private void Start()
        {
            // set position and snap to grid 
            Vector3 initialPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            Vector3Int _targetCell = grid.WorldToCell(initialPosition);
            Vector2 _targetPosition = grid.GetCellCenterWorld(_targetCell); //snaps to target cell 
            transform.position = _targetPosition;

            // initialize audio source
            _audiosource = gameObject.GetComponent<AudioSource>();

        }

        public void Move(Vector3Int gridMovement)
        // given a Vector3Int with the direction to move in, move player in that direction to the new target cell
        {

            // calculate target position given the current position and grid movement
            Vector3Int targetCell = grid.WorldToCell(transform.position) + gridMovement;

            Vector3 targetPosition = grid.GetCellCenterWorld(targetCell);

            // move player position
            transform.position = targetPosition;

        }

        public void CheckWin(Vector3Int gridMovement)
        {
            Vector3Int targetCell = grid.WorldToCell(transform.position) + gridMovement;
            if (winTiles.HasTile(targetCell)){

                Vector3Int finalMovement = new Vector3Int(3, 0, 0);
                StartCoroutine(MoveToPosition(finalMovement, LoadNextScene));
                
            }

        }

        public bool IsValidMove(Vector3Int gridMovement)
            // given a gridMovement direction, calculate the target position and check if player can move there or not
        {
            // calculate target position given the current position and grid movement
            Vector3Int targetCell = grid.WorldToCell(transform.position) + gridMovement;

            // return whether or not there is a walkable tile there
            return walkableTiles.HasTile(targetCell);
        }

        public string foundObject(Vector3Int gridMovement)
            // returns a string if there is an object found at the target cell
            // strings will be either "oil", "wood", or "aquacell"
            // returns null if no object found. 
        {
            Vector3Int targetCell = grid.WorldToCell(transform.position) + gridMovement;
            Tile tileAtCell = pickupTiles.GetTile(targetCell) as Tile;

            if (tileAtCell == woodTile)
            {
                Debug.Log("Found Wood");
                return "wood";
            } else if (tileAtCell == oilcanTile)
            {
                Debug.Log("Found Oil");
                return "oil";
            } else if (tileAtCell == aquacellTile)
            {
                Debug.Log("Found Aquacell");
                return "aquacell";
            } else
            {
                return null;
            }
        }

        public Vector3Int clearObject(Vector3Int gridMovement)
        // given gridMovement position, clear the object that is at that position (if there is one)
        {
            // calculate target position given the current position and grid movement
            Vector3Int targetCell = grid.WorldToCell(transform.position) + gridMovement;
            pickupTiles.SetTile(targetCell, null);

            return targetCell; // returns the position the object was at so it can be saved
        }

        public void addObject(Vector3Int position, string objectName)
        // given an object name, and a position, places that object on the object tile map
        {
            //Vector3Int targetCell = grid.WorldToCell(transform.position);
            Tile objectTile = null; 
            switch (objectName)
            {
                case "wood":
                    objectTile = woodTile;
                    break;
                case "oil":
                    objectTile = oilcanTile;
                    break;
                case "aquacell":
                    objectTile = aquacellTile;
                    break;
            }

            pickupTiles.SetTile(position, objectTile);
        }

        // Coroutine method for animating player movement
        public IEnumerator MoveToPosition(Vector3Int gridMovement, Action onCompleted = null)
        {
            Vector3Int currentCell = grid.WorldToCell(transform.position);
            Vector3Int targetCell = currentCell + gridMovement;
            Vector3 targetPosition = grid.GetCellCenterWorld(targetCell);

            while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }

            onCompleted?.Invoke();
        }

        private void LoadNextScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); // change this to load the next scene instead 
        }


        public void PlaceWalkableWinTiles()
        {
            BoundsInt winBounds = winTiles.cellBounds; // get bounds of win tilemap
            for (int x = winBounds.xMin; x < winBounds.xMax; x++)
            {
                for (int y = winBounds.yMin; y < winBounds.yMax; y++)
                {
                    Vector3Int position = new Vector3Int(x, y, 0);
                    if (winTiles.HasTile(position))
                    {
                        walkableTiles.SetTile(position, floorTile);
                    }
                }
            }
        }

        public void RemoveWalkableWinTiles()
        {
            BoundsInt winBounds = winTiles.cellBounds; // get bounds of win tilemap
            for (int x = winBounds.xMin; x < winBounds.xMax; x++)
            {
                for (int y = winBounds.yMin; y < winBounds.yMax; y++)
                {
                    Vector3Int position = new Vector3Int(x, y, 0);

                    if (winTiles.HasTile(position))
                    {
                        walkableTiles.SetTile(position, null);
                    }
                    
                }
            }
        }

    }
}