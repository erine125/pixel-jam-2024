using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DesignPatterns.Command
{
    // moves the player one space, checking if the space can be moved on

    public class PlayerMover : MonoBehaviour
    {
        public Grid grid;
        public Tilemap walkableTiles;
        public Tilemap obstacleTiles;

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

            Vector3Int currentCell = grid.WorldToCell(transform.position);


            Vector3Int targetCell = grid.WorldToCell(transform.position) + gridMovement;

            Vector3 targetPosition = grid.GetCellCenterWorld(targetCell);


            // move player position
            transform.position = targetPosition;

        }

        public bool IsValidMove(Vector3Int gridMovement)
            // given a gridMovement direction, calculate the target position and check if player can move there or not
        {
            // calculate target position given the current position and grid movement
            Vector3Int targetCell = grid.WorldToCell(transform.position) + gridMovement;
            return walkableTiles.HasTile(targetCell);
        }


        // Coroutine method
        public IEnumerator MoveToPosition(Vector3Int gridMovement)
        {
            Vector3Int currentCell = grid.WorldToCell(transform.position);
            Vector3Int targetCell = currentCell + gridMovement;
            Vector3 targetPosition = grid.GetCellCenterWorld(targetCell);

            while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }
        }


    }
}