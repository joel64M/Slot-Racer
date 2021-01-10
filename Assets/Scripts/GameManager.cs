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
        public List<FollowPath> followPaths =  new List<FollowPath>();
        public GAMESTATE CurrentGameState
        {
           private set;
            get;
        }
       public  event Action<GAMESTATE> OnGameStateChangedAction;
        #endregion


        public List<Statistics> stats = new List<Statistics>();

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


        #region Builtin Methods
        private void Awake()
        {
            if (PlayerPrefs.GetInt("LEVEL", 0) != SceneManager.GetActiveScene().buildIndex)
            {
                if (Application.CanStreamedLevelBeLoaded(PlayerPrefs.GetInt("LEVEL",0)))
                    SceneManager.LoadScene(PlayerPrefs.GetInt("LEVEL", 0));
            }
        }
        void OnEnable()
        {

        }
        private void OnDisable()
        {
            
        }
        void Start()
        {
            followPaths = FindObjectsOfType<FollowPath>().ToList();
            CalculateSides();
        }



        #endregion

        #region Custom Methods
        public void AddToStats(Statistics s)
        {
            stats.Add(s);
            if (FindObjectOfType<UiManager>())
            {
                if (s.isPlayer)
                {
                    FindObjectOfType<UiManager>().mainPlayerStats = s;
                }
            }
        }
        public void SetGameState(GAMESTATE gs)
        {
            OnGameStateChangedAction?.Invoke(gs);
            CurrentGameState = gs;

                switch (gs)
                {
                    case GAMESTATE.GAMESTART:
                        InvokeRepeating("SortStats", 0.1f, 0.1f);
                    TinySauce.OnGameStarted((SceneManager.GetActiveScene().buildIndex+1).ToString());

                    break;
                    case GAMESTATE.RACECOMPLETE:
                        break;
                case GAMESTATE.GAMEOVER:
                    TinySauce.OnGameFinished((SceneManager.GetActiveScene().buildIndex + 1).ToString(), false, 0);
                    Taptic.Heavy();
                    break;
                case GAMESTATE.GAMECOMPLETE:
                    //  Taptic.Success();
                    TinySauce.OnGameFinished((SceneManager.GetActiveScene().buildIndex + 1).ToString(), true,0);
                    PlayerPrefs.SetInt("LEVEL", SceneManager.GetActiveScene().buildIndex + 1);
                    break;
            }
        }

        void CalculateSides()
        {
            int randomNum = UnityEngine.Random.Range(0, followPaths.Count);

            for (int i = 0; i < followPaths.Count; i++)
            {
                if (i == randomNum)
                {
                    followPaths[i].Initialize(true);
                }
                else
                {
                    followPaths[i].Initialize(false);
                }
            }

        }
    
    #endregion

    }
}
