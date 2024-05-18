using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CommandPattern
{
    public class UIManager : MonoBehaviour
    {

        public Image stepCounterBar;
        public Sprite[] stepCounterLevels;

        public GameState gameState; 

    // Start is called before the first frame update
    void Start()
        {
            stepCounterBar.sprite = stepCounterLevels[gameState.maxSteps];
        }


    public void UpdateStepCounter(int stepsRemaining)
    {
        if (gameState.stepsRemaining < 0 || gameState.stepsRemaining >= stepCounterLevels.Length)
        {
            Debug.LogError("Step count out of range of available sprites!");
            return;
        }

        stepCounterBar.sprite = stepCounterLevels[gameState.stepsRemaining];
    }



    }

}
