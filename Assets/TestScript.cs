using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace NameSpaceName {

    public class TestScript : MonoBehaviour
    {

        #region Variables
        public Transform t1;
        public Transform t2;
        public Transform t3;
        public Vector3 st;
        public Vector3 nd;
        #endregion

        #region Builtin Methods


        private void OnDrawGizmos()
        {

            float angle = 0;
            float angle2 = 0;
            //  angle   = Mathf.Abs( Vector3.SignedAngle(t1.forward, t2.position,transform.up));
            // angle2 =   Mathf.Atan2( t1.position.z - t2.position.z, t1.position.x - t2.position.x) * Mathf.Rad2Deg;
            //  angle = t1.eulerAngles.y - angle2; //-180
              st = new Vector3(t2.position.x - t1.position.x, 0, t2.position.z - t1.position.z);
            Gizmos.DrawLine(t1.position, t2.position);
            Gizmos.DrawLine(t1.position, t3.position);
             nd = new Vector3(t3.position.x - t1.position.x, 0, t3.position.z - t1.position.z);
            angle = Vector3.Angle(nd, st);
            GUIStyle guistyle = new GUIStyle();
            guistyle.fontSize = 20;
            guistyle.normal.textColor = Color.white;
            guistyle.alignment = TextAnchor.MiddleCenter;

            Handles.Label(transform.position, (angle + " >" + angle2).ToString(), guistyle);
        }

        

    #endregion

    #region Custom Methods

    #endregion

    }
}
