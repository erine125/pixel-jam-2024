using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.Linq;

public class BuildDam : MonoBehaviour
{

    public Tilemap tilemap; // tilemap that dam will be drawn on
    public Tilemap walkableTilemap; // tilemap that contains walkable tiles
    public Tile damTile; // tilemap sprite for dam (when built)
    public Tile[] ditchTiles; // array of tilemap sprites containing ditch tiles

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int clickedCell = tilemap.WorldToCell(position); // get the cell that player clicked on 
            Vector3Int playerCell = tilemap.WorldToCell(gameObject.transform.position); // get cell that player is standing in 


            // only place if: player has wood, player is adjacent to position, position is a ditch position
            if (Math.Abs(clickedCell.x - playerCell.x) <= 1 && Math.Abs(clickedCell.y - playerCell.y) <= 1) // check if adjacent to player cell position
            {
                Tile tileAtClickedCell = tilemap.GetTile(clickedCell) as Tile; // get tile from tilemap, cast to Tile object 
                if (ditchTiles.Contains(tileAtClickedCell)) // check if the tile we're trying to build on is a ditch tile (not ground or water)
                {

                    if (gameObject.GetComponent<PickupManager>().woodCount > 0) // check if player has wood
                    {
                        gameObject.GetComponent<PickupManager>().woodCount--; // decrement wood
                        tilemap.SetTile(tilemap.WorldToCell(position), damTile); // place tile
                        walkableTilemap.SetTile(tilemap.WorldToCell(position), damTile); // place tile on walkable tilemap so player can walk there
                    }
                    
                }

                
            } 



            
        }
    }
}
