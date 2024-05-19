using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommandPattern
{
    public class BreakDamCommand : ICommand
    { 
        private PlayerMover _playerMover;
        private GameState _gameState;
        private Vector3Int _position;
        private WaterManager _waterManager;

        // pass parameters into the constructor
        public BreakDamCommand(PlayerMover player, GameState gameState, WaterManager waterManager, Vector3Int position)
        {
            this._playerMover = player; 
            this._gameState = gameState;
            this._waterManager = waterManager;  
            this._position = position;

        }

        public void Execute()
        {

            // remove the dam object 
            _playerMover.damTiles.SetTile(_position, null);

            // remove object on walkable tilemap
            _playerMover.walkableTiles.SetTile(_position, null);

            // check if there's water next to the dam, if so, flow water 
            if (_waterManager.IsAdjacentWater(_position))
            {
                _waterManager.FloodFillWater(_position);
            }

        }


        public void Undo()
        {

            // create the dam object 
            _playerMover.damTiles.SetTile(_position, _playerMover.damTile);

            // create object on walkable tilemap 
            _playerMover.walkableTiles.SetTile(_position, _playerMover.damTile);

        }
    }
}