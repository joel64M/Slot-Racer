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
        public List<GameObject> prefabs2 = new List<GameObject>();
        public GameObject coinPrefab;

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
            PlantPrefabs2(minGapDist, 10, 15);
            PlantPrefabs(minGapDist, 16, 25);
            PlantPrefabs(minGapDist, 29, 37);

        }

        public void CoinPlacer(SidePath sp)
        {
            GameObject go;
            
            //for (int i = 0; i < sp.pc.path.NumPoints; i++)
            //{
            //   // if (Vector3.Distance(previousPoint, rmc.pathCreator.path.GetPoint(i)) > dist)
            //    {
            //       // previousPoint = rmc.pathCreator.path.GetPoint(i);
            //        go = Instantiate(coinPrefab, parent);
            //        go.transform.position = sp.pc.path.GetPoint(i);// + rmc.pathCreator.path.GetNormal(i) ;
            //    }
            //}
                
            for (int i = 0; i < sp.angles.Count; i++)
            {
                if (sp.angles[i] > 5)
                {
                    //prevs=   sp.pc.path.GetClosestPointOnPath(sp.pos[i].point);
                    go = Instantiate(coinPrefab, parent);
                    go.transform.position = sp.pos[i].point;

                }
            }

        }
        Vector3 prevs;

        void PlantPrefabs2(float dist, float min, float max)
        {
            GameObject go = Instantiate(prefabs2[Random.Range(0, prefabs2.Count)], parent);
            go.transform.position = rmc.pathCreator.path.GetPoint(0) + rmc.pathCreator.path.GetNormal(0) * Random.Range(min, max);
            go = Instantiate(prefabs2[Random.Range(0, prefabs2.Count)], parent);
            go.transform.position = rmc.pathCreator.path.GetPoint(0) - rmc.pathCreator.path.GetNormal(0) * Random.Range(min, max);
            previousPoint = rmc.pathCreator.path.GetPoint(0);

            for (int i = 0; i < rmc.pathCreator.path.NumPoints; i++)
            {
                if (Vector3.Distance(previousPoint, rmc.pathCreator.path.GetPoint(i)) > dist)
                {
                    previousPoint = rmc.pathCreator.path.GetPoint(i);
                    go = Instantiate(prefabs2[Random.Range(0, prefabs2.Count)], parent);
                    go.transform.position = rmc.pathCreator.path.GetPoint(i) + rmc.pathCreator.path.GetNormal(i) * Random.Range(min, max);


                    go = Instantiate(prefabs2[Random.Range(0, prefabs2.Count)], parent);
                    go.transform.position = rmc.pathCreator.path.GetPoint(i) - rmc.pathCreator.path.GetNormal(i) * Random.Range(min, max);
                }



            }
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
