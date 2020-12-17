using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Examples;
namespace NameSpaceName {

    public class SimpleSpawner : MonoBehaviour
    {

        #region Variables
        public RoadMeshCreator rmc;
        public List<GameObject> prefabs = new List<GameObject>();
        List<newPoints> positions = new List<newPoints>();
        public Transform parent;

        #endregion

        #region Builtin Methods

        private void Start()
        {
           // positions = rmc.cubepos;

            for (int i = 0; i < positions.Count; i++)
            {
             GameObject go =    Instantiate(prefabs[Random.Range(0, prefabs.Count)], parent);
                go.transform.position = positions[i].point + positions[i].normal * Random.Range(8f, 13f);


                 go = Instantiate(prefabs[Random.Range(0, prefabs.Count)], parent);
                go.transform.position = positions[i].point - positions[i].normal * Random.Range(8f,13f);
            }
        }

        #endregion

        #region Custom Methods

        #endregion

    }
}
