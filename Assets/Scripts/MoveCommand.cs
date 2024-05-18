using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommandPattern
{

     
    // an example of a simple command object, implementing ICommand
    public class MoveCommand : ICommand
    {
        private PlayerMover _playerMover;
        private GameState _gameState;
        private Vector3Int _movement;
        private AudioSource _audiosource;
        private string _foundObject;
        private int _savedSteps;
        private Vector3Int _savedPosition;

        // pass parameters into the constructor
        public MoveCommand(PlayerMover player, GameState gameState, Vector3Int moveVector, string foundObject)
        {
            this._gameState = gameState;
            this._playerMover = player;
            this._movement = moveVector;
            this._audiosource = player._audiosource;
            this._foundObject = foundObject;
            this._savedSteps = 0;
        }

        public void Execute()
        {
            // play sound effect 
            _audiosource.PlayOneShot(_playerMover.moveSFX, 0.8f);

            // start movement animation coroutine 
            _playerMover.StartCoroutine(_playerMover.MoveToPosition(_movement)); 

            // decrement steps 
            _gameState.DecrementSteps();
            Debug.LogFormat("Steps remaining: {0}", _gameState.stepsRemaining);

            if (_foundObject != null ) { // if we found an object on the tile we went to... 
                if (_foundObject == "oil") // if the object was oil:
                {
                    _savedSteps = _gameState.ReplenishSteps(); // replenishSteps will return the saved number of steps, save this for undo 
                    
                    _savedPosition = _playerMover.clearObject(_movement); // remove oilcan from map. clearObject will return where it was, 

                } else if (_foundObject == "aquacell")
                {
                    _gameState.IncrementAquacells(); // add aquacell to inventory and play animation 
                    _savedPosition = _playerMover.clearObject(_movement);  // remove aquacell from map and save position
                }
            }

        }


        public void Undo()
        {

            if (_foundObject != null)
            { // if there was an object found... 
                if (_foundObject == "oil")
                {
                    _gameState.SetSteps(_savedSteps); // set steps back to the saved steps value
                    _playerMover.addObject(_savedPosition, "oil"); // add oil back to map 
                }

                else if (_foundObject == "aquacell")
                {
                    Debug.Log("Undoing Aquacell Pickup");
                    _gameState.DecrementAquacells(); // remove aquacell from inventory and play animation
                    _playerMover.addObject(_savedPosition, "aquacell"); // add aquacell back to map
                }

            }

            // move opposite direction
            _playerMover.Move(-_movement);
            _gameState.IncrementSteps();
            Debug.LogFormat("Steps remaining: {0}", _gameState.stepsRemaining);
            
        }
    }
}