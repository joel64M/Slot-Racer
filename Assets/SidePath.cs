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
        public float minDistanceBetweenPoints = 3f;
        public List<newPoints> cubepos = new List<newPoints>();
        public List<float> angles = new List<float>();
        public PathCreator pc;
         Vector3 st;
         Vector3 nd;
        public bool gizmos;
        #endregion

        #region Builtin Methods

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
                    if (i < cubepos.Count - 1)
        
                    Handles.color = Color.red;
                    GUIStyle guistyle = new GUIStyle();
                    guistyle.fontSize = 20;
                    guistyle.normal.textColor = Color.white;
                    guistyle.alignment = TextAnchor.MiddleCenter;
                    if (i < cubepos.Count - 1)
                    {
                        Handles.Label(cubepos[i].point, i + " * " + (angles[i].ToString("F2") + " >" + (360/3-  angles[i] * angles[i]).ToString("F2")), guistyle);
                      //  angles.Add(angle);
                    }
                }
           
            }
        
        }


    #endregion

    #region Custom Methods
        public void PathUpdated()
        {
            UpdateSidePath(pc);
        }
        void UpdateSidePath(PathCreator pathCreator)
        {
            
                cubepos.Clear();
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
                                    cubepos.Add(np);
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
                                for (int k = 0; k < cubepos.Count; k++)
                                {

                                    //  Debug.Log("asś");


                                    float angle = 0;

                                    if (cubepos.Count > 3)
                                    {
                                        if (k < cubepos.Count - 1)
                                        {
                                            st = new Vector3(cubepos[k + 1].point.x - cubepos[k].point.x, 0, cubepos[k + 1].point.z - cubepos[k].point.z);
                                        }
                                        if (k < cubepos.Count - 2)
                                        {
                                            nd = new Vector3(cubepos[k + 2].point.x - cubepos[k].point.x, 0, cubepos[k + 2].point.z - cubepos[k].point.z);
                                        }

                                        angle = Vector3.Angle(nd, st);

                                        if (k < cubepos.Count - 1)
                                        {
                                            //  Handles.Label(cubepos[i + 1].point, (angle + " >" + (360 - angle * angle)).ToString(), guistyle);
                                            angles.Add(angle);
                                        }
                                    }

                                }
                                angles.Add(0);
                                angles.Add(0);
                                newPoints np = new newPoints();
                                np.point = pathCreator.path.GetPoint(pathCreator.path.NumPoints - 1);
                                np.normal = pathCreator.path.GetNormal(pathCreator.path.NumPoints - 1);
                                cubepos.Add(np);
                                cubepos.Add(np);
                                return;
                            }

                        }
                        index++;
                    }


            
        }
    #endregion

    }
}
