using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridMovement : MonoBehaviour
{

    public Grid grid;
    public float moveSpeed = 5f;
    public Tilemap floortiles;

    public int stepsRemaining;

    private Vector3Int _targetCell;
    private Vector2 _targetPosition;

    public Sprite finishTile;

    const int maxSteps = 15;

    private float offset = 0.5f;


    // Start is called before the first frame update
    void Start()
    {

        stepsRemaining = maxSteps;

        

        Vector3 initialPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        _targetCell = grid.WorldToCell(initialPosition);
        _targetPosition = grid.CellToWorld(_targetCell); //snaps to target cell 
        _targetPosition.x += offset;

        transform.position = _targetPosition;


    }

    // Update is called once per frame
    void Update()
    {

        Vector3Int gridMovement = new Vector3Int();

        if (Input.GetKeyDown(KeyCode.W))
        {
            gridMovement.y += 1;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            gridMovement.y -= 1;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            gridMovement.x -= 1;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            gridMovement.x += 1;
        }

        if (gridMovement != Vector3Int.zero)
        {
            _targetCell += gridMovement;

            if (floortiles.HasTile(_targetCell - new Vector3Int(2, 2, 0)))
            {
                _targetPosition = grid.CellToWorld(_targetCell);
                _targetPosition.x += offset;

                stepsRemaining -= 1;
                Debug.Log(stepsRemaining);
                if (stepsRemaining < 0)
                {
                    Debug.Log("Game over");
                    // load game over screen
                }
            }
            else
            {
                _targetCell -= gridMovement;
            }


        }
        MoveToward(_targetPosition);

    }


    private void MoveToward(Vector2 target)
    {
        transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "oil")
        {
            if (stepsRemaining < maxSteps)
            {
                stepsRemaining = maxSteps;
            }
        }


    }

}
