using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Examples;
namespace NameSpaceName
{
    public class Statistics : MonoBehaviour
    {
        public string playerName;
        public int finalRank;
        public int rank;
        public float completion;
        public bool isPlayer;

        EngineBase eb;
        GameManager gm;
  
        void OnEnable()
        {
            gm = FindObjectOfType<GameManager>();
            eb = GetComponent<EngineBase>();

            if (isPlayer)
            {
                isPlayer = true;
                FindObjectOfType<VirtualCameraScript>().SetCameraTarget(this.gameObject.transform);
            }
            else
            {
                GetComponentInChildren<Canvas>().gameObject.SetActive(false);
            }

            gm.AddToStats(this);

            gm.OnGameStateChangedAction += OnGameStateChanged;
        }

        private void OnDisable()
        {
            gm.OnGameStateChangedAction -= OnGameStateChanged;
        }

        // Update is called once per frame
        void Update()
        {
            if (gm.CurrentGameState == GAMESTATE.GAMESTART)
            {
                if (eb)
                {
                    completion = eb.completion;
                }
            }
        }

        public void ReachedGoal()
        {
            finalRank = gm.reachedDestination;
            gm.reachedDestination++;
            if (isPlayer)
            {
                gm.SetGameState(GAMESTATE.RACECOMPLETE);
            }
        }

        void OnGameStateChanged(GAMESTATE gs)
        {

            switch (gs)
            {
                case GAMESTATE.GAMESTART:
                   // Debug.Log("off");
                    if (isPlayer)
                        GetComponentInChildren<Canvas>().gameObject.SetActive(false);

                    break;
                case GAMESTATE.RACECOMPLETE:
              
                    break;
                case GAMESTATE.GAMEOVER:
                    //  gm.SetGameState(GAMESTATE.GAMEOVER);
                    break;
                case GAMESTATE.GAMECOMPLETE:
                    break;
            }
        }

    }
}