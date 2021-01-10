using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
namespace NameSpaceName {

    public class UiManager : MonoBehaviour
    {

        #region Variables
        GameManager gm;
        
      public  Statistics mainPlayerStats;

        [Header("Panel Properties")]
        [SerializeField] GameObject beforeGameplayPanel;
        [SerializeField] GameObject gameplayPanel;
        [SerializeField] GameObject gameCompletePanel;
        [SerializeField] GameObject gameOverPanel;
    //    [SerializeField] Slider progressionSlider;
        [SerializeField] Image progressionSliderImage;
        [SerializeField] TextMeshProUGUI positionText;
        [SerializeField] TextMeshProUGUI levelText;

        #endregion

        #region Builtin Methods

        void Awake()
        {

        }

        void OnEnable()
        {
            gm = FindObjectOfType<GameManager>();
            gm.OnGameStateChangedAction += OnGameStateChanged;
        }
        private void Start()
        {
            levelText.text = (PlayerPrefs.GetInt("LEVEL", 0) + 1).ToString();
        }
        private void OnDisable()
        {
            gm.OnGameStateChangedAction -= OnGameStateChanged;
        }
        private void Update()
        {
            if (mainPlayerStats)
            {
                progressionSliderImage.fillAmount = mainPlayerStats.completion / 100;
                if (mainPlayerStats.rank == 0)
                {
                    positionText.text = (mainPlayerStats.rank + 1).ToString() + " st";
                }
                if (mainPlayerStats.rank == 1)
                {
                    positionText.text = (mainPlayerStats.rank + 1).ToString() + " nd";
                }
                if (mainPlayerStats.rank == 2)
                {
                    positionText.text = (mainPlayerStats.rank + 1).ToString() + " rd";
                }
            }
        }
        #endregion

        #region Custom Methods
        void OnGameStateChanged(GAMESTATE gs)
        {
         
            switch (gs)
            {
                case GAMESTATE.GAMESTART:
                    break;
                case GAMESTATE.RACECOMPLETE:
                    if (mainPlayerStats.finalRank == 0)
                    {
                      //  GameCompleteUI();

                        gm.SetGameState(GAMESTATE.GAMECOMPLETE);
                    }
                    else
                    {
                       // GameOverUI();

                        gm.SetGameState(GAMESTATE.GAMEOVER);
                    }
                    break;
                case GAMESTATE.GAMEOVER:
                    //  gm.SetGameState(GAMESTATE.GAMEOVER);
                    GameOverUI();
                    break;
                case GAMESTATE.GAMECOMPLETE:
                    GameCompleteUI();
                    break;
            }
        }


        public void _StartGameButton()
        {
            gm.SetGameState(GAMESTATE.GAMESTART);
            //change ui to gameplay
            beforeGameplayPanel.SetActive(false);
            gameplayPanel.SetActive(true);
        }
        public void _RestartGameButton()
        {
            Time.timeScale = 1;
          // Debug.Log(SceneManager.GetActiveScene().buildIndex);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        }
        public void _NextLevelButton()
        {
          Time.timeScale = 1;
            if (Application.CanStreamedLevelBeLoaded(SceneManager.GetActiveScene().buildIndex + 1))
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        }
        public void _LoadScene(string scene)
        {
            Time.timeScale = 1;

            SceneManager.LoadScene(scene);
        }
        public void _PauseButton()
        {
            //change ui to pause
            
            Time.timeScale = 0;

            gm.SetGameState(GAMESTATE.PAUSE);

        }
        public void _ResumeButton()
        {
            //change ui to gameplay
            //timer countdown 
            Time.timeScale = 1;

            gm.SetGameState(GAMESTATE.RESUME);
        }
        void GameOverUI()
        {
            //GameOverUI
            gameplayPanel.SetActive(false);
            gameOverPanel.SetActive(true);
        }
        void GameCompleteUI()
        {
            gameplayPanel.SetActive(false);
            gameCompletePanel.SetActive(true);
        }

        #endregion

    }
}
