using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NameSpaceName;

public class AiEngine : EngineBase
{
    bool safe;
    float tempSafeTimer = 0;

    void Update()
    {
        if (gm)
        {
            if (gm.CurrentGameState != GAMESTATE.NONE)
            {
                GetInput();
                CalculateSafeZone();
                CalculateCrashZone();
                if (!complete)
                {
                    IncrIndex();
                    CalculateSteering();
                    CalculateMovement();
                    CalculateEngineSound();
                    CalculateCompletion();
                }
            }
        }
    }


    public override void GetInput()
    {
      
            if (safe && !complete)
            {
                inputMove += Time.deltaTime / 2;
                inputMove = Mathf.Clamp01(inputMove);
            }
            else
            {
                inputMove -= Time.deltaTime / 2;
                inputMove = Mathf.Clamp01(inputMove);
            }
        base.GetInput();
    }

    void CalculateSafeZone()
    {
        if (sp.angles.Count - 1 > currentIndex)
            if (sp.angles[currentIndex] < 1.5f)
            {
                safe = true;
            }
            else
            {
                tempSafeTimer += Time.deltaTime;
                if (inputMove < 0.5f)
                {
                    if (tempSafeTimer > 0.3f)
                    {
                        safe = true;
                        if (tempSafeTimer > 0.33f)
                            tempSafeTimer = 0;
                    }
                }
                else
                {
                    safe = false;
                }
            }
    }

}
