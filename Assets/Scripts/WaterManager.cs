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
        public Tilemap driftwoodTilemap;
        public Tilemap walkableTilemap;

        public Tile waterTile;
        public Tile ActivatedDriftwoodTile;
        public Tile DeactivatedDriftwoodTile;

        private List<Vector3Int> driftwoodPositions = new List<Vector3Int>(); // list of driftwood positions

        public AnimatedTile[] flowAnimations; // Array of flow animations
        public AnimatedTile[] idleAnimations; // Array of idle animations
        public Tile[] ditchTiles; // Array of ditch tiles corresponding by index

        // dictionaries that map ditch tiles onto corresponding animated tiles 
        private Dictionary<Tile, AnimatedTile> flowMapping = new Dictionary<Tile, AnimatedTile>();
        private Dictionary<Tile, AnimatedTile> idleMapping = new Dictionary<Tile, AnimatedTile>();

        private void Start()
        {
            // get and save list of driftwood positions
            BoundsInt bounds = driftwoodTilemap.cellBounds;
            for (int x = bounds.xMin; x <= bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y <= bounds.yMax; y++)
                {
                    Vector3Int pos = new Vector3Int(x, y, 0);
                    if (driftwoodTilemap.GetTile(pos) != null)
                    {
                        driftwoodPositions.Add(pos);
                        Debug.Log(pos);
                    }
                }
            }

            // initialize tile to animation mappings 
            for (int i = 0; i < ditchTiles.Length; i++)
            {
                flowMapping.Add(ditchTiles[i], flowAnimations[i]);
                idleMapping.Add(ditchTiles[i], idleAnimations[i]);
            }

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

                        Tile ditchTile = ditchTilemap.GetTile(neighbor) as Tile;

                        if (!waterTilemap.HasTile(neighbor))
                        {
                            AnimatedTile flowAnimation = flowMapping[ditchTile];
                            AnimatedTile idleAnimation = idleMapping[ditchTile];

                            waterTilemap.SetTile(neighbor, flowAnimation); // Set flow animation tile
                            StartCoroutine(SwitchToIdleAnimation(neighbor, flowAnimation, idleAnimation));
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

        public void ActivateDriftwood()
        {
            foreach (Vector3Int pos in driftwoodPositions)
            {
                if (waterTilemap.HasTile(pos))
                {
                    Debug.Log("Activating driftwood at: " + pos);

                    // activate the driftwood by placing the right cosmetic tile on the driftwood map 
                    driftwoodTilemap.SetTile(pos, ActivatedDriftwoodTile);

                    // make driftwood walkable by placing a tile on walkable tilemap 
                    walkableTilemap.SetTile(pos, ActivatedDriftwoodTile);
                }
            }
        }

        public void DeactivateDriftwood()
        {
            foreach (Vector3Int pos in driftwoodPositions)
            {
                // deactivate the driftwood by placing the right cosmetic tile on the driftwood map 
                driftwoodTilemap.SetTile(pos, DeactivatedDriftwoodTile);

                // make driftwood not walkable by removing a tile on walkable tilemap 
                walkableTilemap.SetTile(pos, null);
                
            }
        }

        IEnumerator SwitchToIdleAnimation(Vector3Int tilePos, AnimatedTile flowAnimation, AnimatedTile idleAnimation)
        {
            yield return new WaitForSeconds(flowAnimation.m_MaxSpeed * flowAnimation.m_AnimatedSprites.Length);
            waterTilemap.SetTile(tilePos, idleAnimation);
        }

    }
}