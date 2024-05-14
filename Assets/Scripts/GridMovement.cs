using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class GridMovement : MonoBehaviour
{

    public Grid grid;
    public float moveSpeed = 5f;
    public Tilemap floortiles;

    private Vector3Int _targetCell;
    private Vector2 _targetPosition;

    public GameObject playerSprite;
    private SpriteRenderer sr;

    const int maxSteps = 15;
    private int stepsRemaining;

    private Dictionary<string, int> inventory; 

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


        // initialize inventory 
        inventory = new Dictionary<string, int>();
        inventory["wood"] = 0;
        inventory["aquacell"] = 0;



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
            sr.flipX = true;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            gridMovement.x += 1;
            sr.flipX = false;
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

    void OnCollisionEnter2D(Collision2D col)
    {

        // if colliding with oil object: 
        if (col.gameObject.tag == "oil")
        {
            col.gameObject.SetActive(false); // deactivate object for rest of scene 
            stepsRemaining = maxSteps; // reset step counter
        }

        // if colliding with wood object:
        else if (col.gameObject.tag == "wood")
        {
            col.gameObject.SetActive(false); 
            inventory["wood"] += 1;
            Debug.Log("Wood amount:");
            Debug.Log(inventory["wood"]);
        }

    }

}
