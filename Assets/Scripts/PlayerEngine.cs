using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NameSpaceName;

public class PlayerEngine : EngineBase
{

    void Update()
    {
        if (gm)
        {
            if (gm.CurrentGameState != GAMESTATE.NONE)
            {
                GetInput();
                CalculateCrashZone();
                if (!complete)
                {
                    IncrIndex();
                    CalculateSteering();
                    CalculateMovement();
                    CalculateEngineSound();
                    CalculateCompletion();
                    CalculateRace();
                }
            }
        }
    }


    public override void GetInput()
    {
     
            if (Input.GetMouseButton(0))
            {
                inputMove += Time.deltaTime;
                inputMove = Mathf.Clamp01(inputMove);
            }
            else
            {
                inputMove -= Time.deltaTime;
                inputMove = Mathf.Clamp01(inputMove);
            }
      
        base.GetInput();
    }
}
