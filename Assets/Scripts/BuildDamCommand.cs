using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace CommandPattern
{
    public class BuildDamCommand : ICommand
    {
        private PlayerMover _playerMover;
        private GameState _gameState;
        private Vector3Int _position;

        // pass parameters into the constructor
        public BuildDamCommand(PlayerMover player, GameState gameState, Vector3Int position)
        {
            this._playerMover = player;
            this._gameState = gameState;
            this._position = position;

            Debug.Log("Invoking build dam command");
        }

        public void Execute()
        {
            // decrement wood count 
            _gameState.DecrementWood();

            // play sfx 
            _gameState.audiosource.PlayOneShot(_gameState.buildDamSFX, 0.2f);

            // create the dam object 
            _playerMover.damTiles.SetTile(_position, _playerMover.damTile);

            // create object on walkable tilemap 
            _playerMover.walkableTiles.SetTile(_position, _playerMover.damTile);

        }


        public void Undo()
        {

            // increment wood count 
            _gameState.IncrementWood();

            // remove the dam object 
            _playerMover.damTiles.SetTile(_position, null);

            // remove object on walkable tilemap
            _playerMover.walkableTiles.SetTile(_position, null);


        }
    }
}