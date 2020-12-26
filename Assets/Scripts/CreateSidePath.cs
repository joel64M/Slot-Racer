using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using PathCreation.Utility;
namespace NameSpaceName {

    public class CreateSidePath : MonoBehaviour
    {

        #region Variables
      public  PathCreator pc;
    #endregion

    #region Builtin Methods

        void Awake()
        {
          //  pc = GetComponent<PathCreator>();

        }

        private void Start()
        {
           // Debug.Log(pc.path.localPoints[0]);
           // Debug.Log(pc.path.GetPoint(1));
            Debug.Log(pc.path.NumPoints);
            Debug.Log(pc.bezierPath.NumPoints);

            Debug.Log(pc.path.GetClosestPointOnPath(new Vector3(5f, 0,10f)));
           // Debug.Log(pc.path.get)
            //Debug.Log(pc.bezierPath.GetPoint(3));
            //Debug.Log(pc.bezierPath.GetPoint(6));

            // Debug.Log(pc.bezierPath.GetPoint(15));

            //    BezierPath bezierPath = new BezierPath(waypoints, false, PathSpace.xyz);
            //   GetComponent<PathCreator> ().bezierPath = bezierPath;
          
            Vector3[] pns = new Vector3[((pc.bezierPath.NumPoints-1)/3) + 1];
            int indx = 0; 
            for (int i = 0; i < pc.bezierPath.NumPoints; i+=3)
            {
               //Vector3 localUp = Vector3.Cross(pc.path.GetTangent(i), pc.path.GetNormal(i));//  pc.path.up;
                Vector3 localRight = pc.path.GetNormalAtWorldpoint(pc.bezierPath.GetPoint(i));// Vector3.Cross(localUp, pc.bezierPath.GetTangent(i)); //pc.path.GetNormal(i) :
                pns[indx] = pc.bezierPath.GetPoint(i) - localRight * 1.5f;
                indx++;
            }
          
           
            BezierPath shs = new BezierPath(pns, false, PathSpace.xyz);

            this.gameObject.AddComponent<PathCreator>().bezierPath = shs;
            this.gameObject.GetComponent<PathCreator>().bezierPath.ControlPointMode = BezierPath.ControlMode.Automatic;
            this.gameObject.GetComponent<PathCreator>().bezierPath.IsClosed = false;

            Vector3[] pns2 = new Vector3[((pc.bezierPath.NumPoints - 1) / 3) + 1];
            int indx2 = 0;
            for (int i = 0; i < pc.bezierPath.NumPoints; i += 3)
            {
                Vector3 localRight = pc.path.GetNormalAtWorldpoint(pc.bezierPath.GetPoint(i));// Vector3.Cross(localUp, pc.bezierPath.GetTangent(i)); //pc.path.GetNormal(i) :
                pns2[indx2] = pc.bezierPath.GetPoint(i) + localRight * 1.5f;
                indx2++;
            }


            BezierPath shs2 = new BezierPath(pns2, false, PathSpace.xyz);

            this.gameObject.AddComponent<PathCreator>().bezierPath = shs2;
            this.gameObject.GetComponent<PathCreator>().bezierPath.ControlPointMode = BezierPath.ControlMode.Automatic;
            this.gameObject.GetComponent<PathCreator>().bezierPath.IsClosed = false;
        }

        #endregion

        #region Custom Methods

        #endregion

    }
}
