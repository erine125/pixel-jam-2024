using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour
{

    public int woodCount = 0; // amount of wood in inventory
    public int aquacellCount = 0; // amount of aquacell in inventory
    public int totalAquacells = 2; // amount of aquacells needed

    public GameObject door;
    private Animator doorAnimator;

    public GameObject[] pedestals;
    private Animator[] pedestalAnimators;

    void Start()
    {
        doorAnimator = door.GetComponent<Animator>();
        pedestalAnimators = new Animator[totalAquacells];

        for (int i = 0; i < totalAquacells; i++)
        {
            GameObject pedestal = pedestals[i];
            Animator pedestalAnimator = pedestal.GetComponent<Animator>();
            pedestalAnimators[i] = pedestalAnimator;
        }
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
        }


        // if colliding with aquacell object:
        else if (col.gameObject.tag == "aquacell")
        {
            col.gameObject.SetActive(false); // deactivate aquacell object 
            aquacellCount++; 
            pedestalAnimators[aquacellCount - 1].Play("Pedestal_LightUp"); // light up next pedestal

            // if we reach the total number of needed aquacells... 
            if (aquacellCount == totalAquacells)
            {
                // open door
                doorAnimator.Play("DoorOpen");
                door.GetComponent<BoxCollider2D>().isTrigger = true; // disable collision by setting collider to trigger 
                
            }
        }

    }

}
