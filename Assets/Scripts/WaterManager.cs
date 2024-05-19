using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace CommandPattern
{
    public class WaterManager : MonoBehaviour
    {

        public Tilemap ditchTilemap;
        public Tilemap damTilemap;
        public Tilemap waterTilemap;

        public Tile waterTile;

        private void Start()

        {


        }

        public bool IsAdjacentWater(Vector3Int position)
        // checks if there is water adjacent to that position
        {
            foreach (Vector3Int dir in new Vector3Int[] { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right })
            {
                Vector3Int neighbor = position + dir;
                if (waterTilemap.HasTile(neighbor))  // Check if there is a water tile adjacent
                {
                    return true;  // Water is adjacent, so return true
                }
            }
            return false;  // No water adjacent, return false
        }

        public void FloodFillWater(Vector3Int start)
        { // use the flood fill algorithm to calculate water flow
            Queue<Vector3Int> queue = new Queue<Vector3Int>();
            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                Vector3Int current = queue.Dequeue();

                foreach (Vector3Int dir in new Vector3Int[] { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right })
                {
                    Vector3Int neighbor = current + dir;


                    if (!damTilemap.HasTile(neighbor) && ditchTilemap.HasTile(neighbor))
                    {
                        if (!waterTilemap.HasTile(neighbor))
                        {
                            waterTilemap.SetTile(neighbor, waterTile); // set water tile
                            queue.Enqueue(neighbor);
                        }
                    }
                }
            }
        }

        public Vector3Int? FindTilePosition(Tilemap tilemap, Tile targetTile)
        {
            BoundsInt bounds = tilemap.cellBounds;
            for (int x = bounds.xMin; x <= bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y <= bounds.yMax; y++)
                {
                    Vector3Int localPlace = new Vector3Int(x, y, 0);
                    if (tilemap.GetTile(localPlace) as Tile == targetTile)
                    {
                        return localPlace; // Return the position as soon as the tile is found
                    }
                }
            }
            return null; // Return null if the tile is not found
        }


    }
}