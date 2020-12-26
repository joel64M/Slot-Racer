using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Examples;
namespace NameSpaceName
{
    public class Statistics : MonoBehaviour
    {
        public string playerName;
        public int rank;
        public int finalRank;
        public float completion;
        public bool isPlayer;
        FollowPath pf;
        GameManager gm;
        bool complete;
        void Start()
        {
            pf = GetComponent<FollowPath>();
            gm = FindObjectOfType<GameManager>();
            if (!pf.isAi)
            {
                isPlayer = true;
            }
            gm.AddToStats(this);

        }

        // Update is called once per frame
        void Update()
        {
            if (pf)
            {
                completion = pf.completion;
             
            }

        }
    }
}