using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DesignPatterns.Command
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

        //private void Start()
        //{

        //}

        private void Update()
        {

            CheckKeyboardInput();

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

