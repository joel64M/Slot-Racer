using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NameSpaceName {

    public class FinalPod : MonoBehaviour
    {

        #region Variables
        GameManager gm;

        [SerializeField] GameObject[] celebrations;
        #endregion

        #region Builtin Methods

        void Awake()
        {
            foreach (var item in celebrations)
            {
                item.SetActive(false);
            }
        }


        void OnEnable()
        {
            gm = FindObjectOfType<GameManager>();
            gm.OnGameStateChangedAction += OnGameStateChanged;
        }

        void Start()
        {

        }

        void Update()
        {

        }

        void FixedUpdate()
        {

        }

        void LateUpdate()
        {

        }

        private void OnDisable()
        {
            gm.OnGameStateChangedAction -= OnGameStateChanged;
        }

        void Destroy()
        {

        }

        #endregion

    
        void OnGameStateChanged(GAMESTATE gs)
        {

            switch (gs)
            {
                case GAMESTATE.GAMESTART:
                    break;
                case GAMESTATE.RACECOMPLETE:

                    break;
                case GAMESTATE.GAMEOVER:
                    //  gm.SetGameState(GAMESTATE.GAMEOVER);
                    break;
                case GAMESTATE.GAMECOMPLETE:
                    CelebrateVictory();
                    break;
            }
        }
        void CelebrateVictory()
        {
            foreach (var item in celebrations)
            {
                item.SetActive(false);
            }
            foreach (var item in celebrations)
            {
                item.SetActive(true);
            }
            Taptic.Light();
            Invoke("CelebrateVictory", 2f);
        }

    }
}