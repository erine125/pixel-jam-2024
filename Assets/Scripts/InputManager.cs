using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using UnityEngine.Tilemaps;

namespace CommandPattern
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private PlayerMover player;
        [SerializeField] private GameState gameState;

        // Keybindings
        public KeyCode forwardKey = KeyCode.W;
        public KeyCode downKey = KeyCode.S;
        public KeyCode leftKey = KeyCode.A;
        public KeyCode rightKey = KeyCode.D;
        public KeyCode undoKey = KeyCode.E;
        public KeyCode resetKey = KeyCode.R;

        public AudioSource _audiosource;
        public AudioClip moveSFX;
        public AudioClip cantMoveSFX;

        public Grid grid;
        public Tilemap ditchTilemap; 

        //private void Start()
        //{

        //}

        private void Update()
        {

            CheckKeyboardInput();
            CheckMouseInput();

        }

        private void CheckMouseInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int clickedCell = ditchTilemap.WorldToCell(position); // get the cell that player clicked on 
                //RunDamCommand(player, gameState, clickedCell);

            }
        }

        private void CheckKeyboardInput()
        {
            // create new vector3int representing how much movement to do 
            Vector3Int gridMovement = new Vector3Int();

            // update gridMovement vector depending on which key is pressed
            if (Input.GetKeyDown(forwardKey))
            {
                gridMovement.y += 1;
                RunPlayerCommand(player, gameState, gridMovement);
            }

            if (Input.GetKeyDown(downKey))
            {
                gridMovement.y -= 1;
                RunPlayerCommand(player, gameState, gridMovement);
            }

            if (Input.GetKeyDown(leftKey))
            {
                gridMovement.x -= 1;
                RunPlayerCommand(player, gameState, gridMovement);
            }

            if (Input.GetKeyDown(rightKey))
            {
                gridMovement.x += 1;
                RunPlayerCommand(player, gameState, gridMovement);
            }

            // press undo key to undo 
            if (Input.GetKeyDown(undoKey))
            {
                CommandInvoker.UndoCommand();
            }

            // press R to reload level
            if (Input.GetKeyDown(resetKey))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

        }

        // create a function that invokes the build/break commands

        //private void RunDamCommand(PlayerMover playerMover, GameState gameState, Vector3Int clickedCell)
        //{
        //    Vector3Int playerCell = tilemap.WorldToCell(player.transform.position); // get cell that player is standing in 

        //    // first, check if player is adjacent to the position. if not, do nothing.
        //    if (Math.Abs(clickedCell.x - playerCell.x) <= 1 && Math.Abs(clickedCell.y - playerCell.y) <= 1)
        //    {

        //        // TODO: check if the item they clicked on is in the Dam tilemap. If so, call BreakDamCommand.

        //        // otherwise, check if the item they clicked on is in the Ditch tilemap and is NOT in the water tilemap.

        //    }


        //        // TODO: Check if there is a dam tile at the clicked cell. if so, call the BREAKDAM command.

        //        // only place if: player has wood, player is adjacent to position, position is a ditch position
        //        if (Math.Abs(clickedCell.x - playerCell.x) <= 1 && Math.Abs(clickedCell.y - playerCell.y) <= 1) // check if adjacent to player cell position
        //    {
        //        Tile tileAtClickedCell = ditchTilemap.GetTile(clickedCell) as Tile; // get tile from ditch, cast to Tile object 
        //        if (ditchTilemap.HasTile(tileAtClickedCell)) // check if the tile we're trying to build on is a ditch tile (not ground or water)
        //        {
        //            if (gameState.woodCount > 0) // check if player has wood
        //            {

        //            }
        //        }
        //    }
        //}



        private void RunPlayerCommand(PlayerMover playerMover, GameState gameState, Vector3Int gridMovement)
        {
            if (playerMover == null || gameState == null)
            {
                return;
            }

            if (gridMovement != Vector3Int.zero)
            {
                // check if movement is allowed
                if (playerMover.IsValidMove(gridMovement))
                {
                    // check if player has any steps remaining
                    if (gameState.stepsRemaining > 0)
                    {

                        playerMover.CheckWin(gridMovement); // check if the space you are trying to move to is a winning space, if so we can simply exit 

                        // invoke command. sfx will be played when command executes.
                        ICommand command = new MoveCommand(playerMover, gameState, gridMovement, playerMover.foundObject(gridMovement));
                        CommandInvoker.ExecuteCommand(command);
                    }
                    else
                    {
                        // play sfx indicating can't move
                        _audiosource.PlayOneShot(playerMover.cantMoveSFX, 0.8f);
                    }


                }
            }
        }
    }
}

