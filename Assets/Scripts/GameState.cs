using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommandPattern
{
    public class GameState : MonoBehaviour
    {
        public int maxSteps = 15;
        public int stepsRemaining; // initialize to maxSteps

        public int woodCount = 0;
        public int aquacellCount = 0;

        public int totalAquacells = 2; // can set to however many total aquacells there are in this scene

        public GameObject door;
        private Animator doorAnimator;

        public GameObject[] pedestals;
        private Animator[] pedestalAnimators;

        private PlayerMover _playerMover;

        public UIManager _UIManager;

        public void Start()
        {
            // initialize steps remaining
            stepsRemaining = maxSteps;

            // initialize animators
            doorAnimator = door.GetComponent<Animator>();
            pedestalAnimators = new Animator[totalAquacells];

            for (int i = 0; i < totalAquacells; i++)
            {
                GameObject pedestal = pedestals[i];
                Animator pedestalAnimator = pedestal.GetComponent<Animator>();
                pedestalAnimators[i] = pedestalAnimator;
            }

            // initialize playerMover
            _playerMover = GetComponent<PlayerMover>();
        }


        public void DecrementSteps()
        {
            if (stepsRemaining > 0)
                stepsRemaining--;

            _UIManager.UpdateStepCounter(stepsRemaining);
        }

        public void IncrementSteps()
        {
            stepsRemaining++;
            _UIManager.UpdateStepCounter(stepsRemaining);
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

            _UIManager.UpdateStepCounter(stepsRemaining);
            return savedSteps;
        }

        public void SetSteps(int steps)
        { // set steps to a specific value. this is needed for the undo methods. 
            stepsRemaining = steps;
            _UIManager.UpdateStepCounter(stepsRemaining);
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
            pedestalAnimators[aquacellCount - 1].Play("Pedestal_LightUp"); // light up next pedestal

            if (AllAquacellsCollected())
            {
                doorAnimator.Play("DoorOpen"); // play door open animation
                _playerMover.PlaceWalkableWinTiles();
            }
        }

        public void DecrementAquacells()
        {
            aquacellCount--;
            pedestalAnimators[aquacellCount].Play("Idle"); // unlight most recently lit pedestal

            if (!AllAquacellsCollected())
            {
                doorAnimator.Play("Idle");
                _playerMover.RemoveWalkableWinTiles();
            }
        }

        public bool AllAquacellsCollected()
        {
            return (aquacellCount == totalAquacells);
        }


    }
}