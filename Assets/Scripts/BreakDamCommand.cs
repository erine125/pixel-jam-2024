using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace CommandPattern
{
    public class BreakDamCommand : ICommand
    { 
        private PlayerMover _playerMover;
        private GameState _gameState;
        private Vector3Int _position;
        private WaterManager _waterManager;

        private Dictionary<Vector3Int, Tile> savedWaterState = new Dictionary<Vector3Int, Tile>();

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

            SaveCurrentWaterState();

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

            RevertWaterState();

        }

        private void SaveCurrentWaterState()
        {
            BoundsInt bounds = _playerMover.waterTiles.cellBounds;
            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    Vector3Int pos = new Vector3Int(x, y, 0);
                    Tile tile = _playerMover.waterTiles.GetTile(pos) as Tile;
                    savedWaterState[pos] = tile;
                }
            }
        }

        private void RevertWaterState()
        {
            foreach (var pos in savedWaterState.Keys)
            {
                _playerMover.waterTiles.SetTile(pos, savedWaterState[pos]);
            }
        }

        
    }
    }