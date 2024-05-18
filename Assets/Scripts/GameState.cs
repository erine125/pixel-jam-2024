using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public int maxSteps = 15;
    public int stepsRemaining; // initialize to maxSteps

    public int woodCount = 0;
    public int aquacellCount = 0;

    public int totalAquacells = 2; // can set to however many total aquacells there are in this scene

    public void Start()
    {
        stepsRemaining = maxSteps; 
    }

    public void DecrementSteps()
    {
        if (stepsRemaining > 0)
            stepsRemaining--;
    }

    public void IncrementSteps()
    {
        stepsRemaining++;
    }

    public int ReplenishSteps()
        // replenish steps back to max.
        // return the # of steps remaining before replenishing, so it can be saved
    {
        int savedSteps = stepsRemaining;
        if (stepsRemaining < maxSteps)
        {
            stepsRemaining = maxSteps; 
        }

        return savedSteps;
    }

    public void SetSteps(int steps)
    { // set steps to a specific value. this is needed for the undo methods. 
        stepsRemaining = steps;
    }

    public void IncrementWood()
    {
        woodCount++;
        // TODO: update UI
    }

    public void DecrementWood()
    {
        woodCount--;
        // TODO: update UI
    }

    public void IncrementAquacells()
    {
        aquacellCount++;
        // TODO: play pedestal animation 
        Debug.LogFormat("Aquacell Count: {0}", aquacellCount);
    }

    public void DecrementAquacells()
    {
        aquacellCount--;
        // TODO: unlight pedestals
        Debug.LogFormat("Aquacell Count: {0}", aquacellCount);
    }

    public bool AllAquacellsCollected()
    {
        return (aquacellCount == totalAquacells);
    }



}