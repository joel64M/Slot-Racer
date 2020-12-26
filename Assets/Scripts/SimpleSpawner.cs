using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Examples;
using PathCreation;
namespace NameSpaceName {

    public class SimpleSpawner : MonoBehaviour
    {

        #region Variables
        public RoadMeshCreator rmc;
        public List<GameObject> prefabs = new List<GameObject>();
        //  List<newPoints> positions = new List<newPoints>();
        public Transform parent;
        public float minGapDist = 5f;
        Vector3 previousPoint;
        Vector3 nextPoint;
        #endregion

        #region Builtin Methods

        private void Start()
        {
            //   positions = rmc.pathCreator.path.GetPoint;

            rmc = FindObjectOfType<RoadMeshCreator>();
            PlantPrefabs(minGapDist, 11, 13);
            PlantPrefabs(minGapDist, 15, 20);
            PlantPrefabs(minGapDist, 22, 27);

        }

        void PlantPrefabs(float dist,float min, float max)
        {
            GameObject go = Instantiate(prefabs[Random.Range(0, prefabs.Count)], parent);
            go.transform.position = rmc.pathCreator.path.GetPoint(0) + rmc.pathCreator.path.GetNormal(0) * Random.Range(min, max);
            go = Instantiate(prefabs[Random.Range(0, prefabs.Count)], parent);
            go.transform.position = rmc.pathCreator.path.GetPoint(0) - rmc.pathCreator.path.GetNormal(0) * Random.Range(min, max);
            previousPoint = rmc.pathCreator.path.GetPoint(0);

            for (int i = 0; i < rmc.pathCreator.path.NumPoints; i++)
            {
                if (Vector3.Distance(previousPoint, rmc.pathCreator.path.GetPoint(i)) > dist)
                {
                    previousPoint = rmc.pathCreator.path.GetPoint(i);
                    go = Instantiate(prefabs[Random.Range(0, prefabs.Count)], parent);
                    go.transform.position = rmc.pathCreator.path.GetPoint(i) + rmc.pathCreator.path.GetNormal(i) * Random.Range(min, max);


                    go = Instantiate(prefabs[Random.Range(0, prefabs.Count)], parent);
                    go.transform.position = rmc.pathCreator.path.GetPoint(i) - rmc.pathCreator.path.GetNormal(i) * Random.Range(min, max);
                }



            }
        }

        #endregion

        #region Custom Methods

        #endregion

    }
}
