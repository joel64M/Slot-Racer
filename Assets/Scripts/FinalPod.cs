using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NameSpaceName {

    public class FinalPod : MonoBehaviour
    {
        #region Variables
        [SerializeField] GameObject[] celebrations;
        GameManager gm;
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

        private void OnDisable()
        {
            gm.OnGameStateChangedAction -= OnGameStateChanged;
        }

        #endregion
    
        void OnGameStateChanged(GAMESTATE gs)
        {
            switch (gs)
            {
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