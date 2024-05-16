using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour
{

    public int woodCount = 0; // amount of wood in inventory
    public int aquacellCount = 0; // amount of aquacell in inventory


    // Start is called before the first frame update
    void Start()
    {

        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D col)
    {

        // if colliding with oil object: 
        if (col.gameObject.tag == "oil")
        {
            col.gameObject.SetActive(false); // deactivate object for rest of scene 
            gameObject.GetComponent<GridMovement>().stepsRemaining = gameObject.GetComponent<GridMovement>().maxSteps; // reset step counter
        }

        // if colliding with wood object:
        else if (col.gameObject.tag == "wood")
        {
            col.gameObject.SetActive(false);
            woodCount++;
            Debug.Log("Wood amount:");
            Debug.Log(woodCount);
        }


        // if colliding with aquacell object:
        else if (col.gameObject.tag == "aquacell")
        {
            col.gameObject.SetActive(false);
            aquacellCount++; 
        }

    }


}
