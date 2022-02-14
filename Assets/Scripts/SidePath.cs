using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using UnityEditor;
namespace NameSpaceName {
    [System.Serializable]
    public class newPoints
    {

        public Vector3 point;
        public Vector3 normal;
    }
    public class SidePath : MonoBehaviour
    {

        #region Variables
        public List<newPoints> pos = new List<newPoints>();
        public List<float> angles = new List<float>();
        public PathCreator pc;
        public bool gizmos;

        [Space(5)]
        [Header("Private")]
        [SerializeField] float minDistanceBetweenPoints = 3f;



         Vector3 st;
         Vector3 nd;
        #endregion

        #region Builtin Methods
        /*
        private void OnDrawGizmos()
        {

            if (!gizmos)
                return;
            for (int i = 0; i < cubepos.Count; i++)
            {
                Gizmos.color = Color.red;

                Gizmos.DrawWireCube(cubepos[i].point, Vector3.one);

           
                Gizmos.color = Color.red;
                
                if (cubepos.Count > 3)
                {
                    if (i < cubepos.Count )
        
                    Handles.color = Color.red;
                    GUIStyle guistyle = new GUIStyle();
                    guistyle.fontSize = 20;
                    guistyle.normal.textColor = Color.white;
                    guistyle.alignment = TextAnchor.MiddleCenter;
                    if (i < cubepos.Count)
                    {
                        Handles.Label(cubepos[i].point, (i+1) + " * ", guistyle);
                      //  angles.Add(angle);
                    }
                }
           
            }
        
        }
        */

    #endregion

    #region Custom Methods
        public void PathUpdated(float  minDistBtwnPts)
        {
            minDistanceBetweenPoints = minDistBtwnPts;
            UpdateSidePath(pc);
        }
        void UpdateSidePath(PathCreator pathCreator)
        {
            
                pos.Clear();
                angles.Clear();
                //  cubepos = new Vector3[pathCreator.path.NumPoints];

                int index = 0;
                int i = 0;
                int j = 1;

                if (pathCreator.bezierPath.NumPoints > 8)
                    while (i < pathCreator.path.NumPoints - 1)
                    {
                        while (j < pathCreator.path.NumPoints - 1)
                        {
                            if (j + i < pathCreator.path.NumPoints - 1)
                            {

                                if (Vector3.Distance(pathCreator.path.GetPoint(i), pathCreator.path.GetPoint(i + j)) > minDistanceBetweenPoints)
                                {
                                    // pathCreator.path.GetNormal(0);
                                    newPoints np = new newPoints();
                                    np.point = pathCreator.path.GetPoint(i);
                                    np.normal = pathCreator.path.GetNormal(i);
                                    pos.Add(np);
                                    i = j + i;
                                    j = 1;
                                    break;
                                }
                                else
                                {
                                    j++;
                                }
                            }
                            else
                            {
                                for (int k = 0; k < pos.Count; k++)
                                {

                                    //  Debug.Log("asś");


                                    float angle = 0;

                                    if (pos.Count > 3)
                                    {
                                        if (k < pos.Count - 1)
                                        {
                                            st = new Vector3(pos[k + 1].point.x - pos[k].point.x, 0, pos[k + 1].point.z - pos[k].point.z);
                                        }
                                        if (k < pos.Count - 2)
                                        {
                                            nd = new Vector3(pos[k + 2].point.x - pos[k].point.x, 0, pos[k + 2].point.z - pos[k].point.z);
                                        }

                                        angle = Vector3.Angle(nd, st);

                                        if (k < pos.Count - 1)
                                        {
                                            //  Handles.Label(cubepos[i + 1].point, (angle + " >" + (360 - angle * angle)).ToString(), guistyle);
                                            angles.Add(angle);
                                        }
                                    }

                                }
                                angles.Add(0);
                                newPoints np = new newPoints();
                                np.point = pathCreator.path.GetPoint(pathCreator.path.NumPoints - 1);
                                np.normal = pathCreator.path.GetNormal(pathCreator.path.NumPoints - 1);
                                pos.Add(np);
                                return;
                            }

                        }
                        index++;
                    }


            
        }

        private void OnDrawGizmos()
        {
            foreach (var item in pos)
            {
                Gizmos.DrawCube(item.point, Vector3.one);
            }
        }
        #endregion

    }
}
