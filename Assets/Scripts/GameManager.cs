using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Linq;
namespace NameSpaceName {

    public enum GAMESTATE { NONE, GAMESTART, GAMEOVER, GAMECOMPLETE, PAUSE, RESUME ,RACECOMPLETE };
   
    public class GameManager : MonoBehaviour
    {

        #region Variables
        public int reachedDestination = 0;
        public List<EngineBase> engines =  new List<EngineBase>();
        public GAMESTATE CurrentGameState
        {
           private set;
            get;
        }
        public  event Action<GAMESTATE> OnGameStateChangedAction;
        #endregion

        #region STATS
        public List<Statistics> stats = new List<Statistics>();

        Statistics playerStats;

        public class Completion : IComparer<Statistics>
        {
            public int Compare(Statistics x, Statistics y)
            {
                return x.completion.CompareTo(y.completion);
            }
        }

        void SortStats()
        {
            stats.Sort(new Completion());
            stats.Reverse();
            for (int i = 0; i < stats.Count; i++)
            {
                stats[i].rank = i;
            }
        }
        public void AddToStats(Statistics s)
        {
            stats.Add(s);
            if (FindObjectOfType<UiManager>())
            {
                if (s.isPlayer)
                {
                    FindObjectOfType<UiManager>().mainPlayerStats = s;
                    playerStats = s;
                }
            }
        }
        #endregion

        #region Builtin Methods
        //private void Awake()
        //{
        //    if (PlayerPrefs.GetInt("LEVEL", 0) != SceneManager.GetActiveScene().buildIndex)
        //    {
        //        if (Application.CanStreamedLevelBeLoaded(PlayerPrefs.GetInt("LEVEL",0)))
        //            SceneManager.LoadScene(PlayerPrefs.GetInt("LEVEL", 0));
        //    }
        //}

        void Start()
        {
            CalculateSides();
        }
   
        #endregion

        #region Custom Methods

        public void SetGameState(GAMESTATE gs)
        {
            OnGameStateChangedAction?.Invoke(gs);
            CurrentGameState = gs;

                switch (gs)
                {
                    case GAMESTATE.GAMESTART:
                        InvokeRepeating("SortStats", 0.1f, 0.1f);
                    //TinySauce.OnGameStarted((SceneManager.GetActiveScene().buildIndex+1).ToString());
                    break;
                    case GAMESTATE.RACECOMPLETE:
                        if (playerStats.finalRank == 0)
                        {
                            SetGameState(GAMESTATE.GAMECOMPLETE);
                        }
                        else
                        {
                            SetGameState(GAMESTATE.GAMEOVER);
                        }
                    break;
                case GAMESTATE.GAMEOVER:
                    //TinySauce.OnGameFinished((SceneManager.GetActiveScene().buildIndex + 1).ToString(), false, 0);
                    Taptic.Heavy();
                    break;
                case GAMESTATE.GAMECOMPLETE:
                    //  Taptic.Success();
                    //TinySauce.OnGameFinished((SceneManager.GetActiveScene().buildIndex + 1).ToString(), true,0);
                    PlayerPrefs.SetInt("LEVEL", SceneManager.GetActiveScene().buildIndex + 1);
                    break;
            }
        }

        void CalculateSides()
        {
            engines = FindObjectsOfType<EngineBase>().ToList();

            int randomNum = UnityEngine.Random.Range(0, engines.Count);

            for (int i = 0; i < engines.Count; i++)
            {
                if (i == randomNum)
                {
                    engines[i].Initialize(true);
                }
                else
                {
                    engines[i].Initialize(false);
                }
            }
        }
    
    #endregion

    }
}
