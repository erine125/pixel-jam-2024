using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DesignPatterns.Command
{

     
    // an example of a simple command object, implementing ICommand
    public class MoveCommand : ICommand
    {
        private PlayerMover _playerMover;
        private GameState _gameState;
        private Vector3Int _movement;
        private AudioSource _audiosource;

        // pass parameters into the constructor
        public MoveCommand(PlayerMover player, GameState gameState, Vector3Int moveVector)
        {
            this._gameState = gameState;
            this._playerMover = player;
            this._movement = moveVector;
            this._audiosource = player._audiosource;

        }

        // logic of thing to do goes here
        public void Execute()
        {
            // move by vector IF player has enough steps remaining
            _audiosource.PlayOneShot(_playerMover.moveSFX, 0.8f);
            _playerMover.StartCoroutine(_playerMover.MoveToPosition(_movement)); // start coroutine
            _gameState.DecrementSteps();
            Debug.LogFormat("Steps remaining: {0}", _gameState.stepsRemaining);

        }
        // undo logic goes here
        public void Undo()
        {
            // move opposite direction
            _playerMover.StartCoroutine(_playerMover.MoveToPosition(-_movement));
            _gameState.IncrementSteps();
            Debug.LogFormat("Steps remaining: {0}", _gameState.stepsRemaining);
        }
    }
}