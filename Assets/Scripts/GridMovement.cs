using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;


public class GridMovement : MonoBehaviour
{

    public Grid grid;
    public float moveSpeed = 5f;
    public Tilemap floortiles;

    private Vector3Int _targetCell;
    private Vector2 _targetPosition;

    public GameObject playerSprite;
    private SpriteRenderer sr;


    public int maxSteps = 15;
    public int stepsRemaining;


    // Start is called before the first frame update
    void Start()
    {

        // set steps to max steps 
        stepsRemaining = maxSteps;
        
        // set position and snap to grid 
        Vector3 initialPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        _targetCell = grid.WorldToCell(initialPosition);
        _targetPosition = grid.GetCellCenterWorld(_targetCell); //snaps to target cell 
        transform.position = _targetPosition;

        // initialize sprite renderer 
        sr = playerSprite.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        // press R to reload level
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        }

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
            sr.flipX = false;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            gridMovement.x += 1;
            sr.flipX = true;
        }

        if (gridMovement != Vector3Int.zero)
        {
            _targetCell += gridMovement;

            if (floortiles.HasTile(_targetCell) && stepsRemaining > 0)
            {
                _targetPosition = grid.GetCellCenterWorld(_targetCell);
                stepsRemaining -= 1;
                Debug.Log(stepsRemaining);
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

    

}
