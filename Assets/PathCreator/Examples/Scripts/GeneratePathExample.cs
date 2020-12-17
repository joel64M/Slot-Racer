using UnityEngine;

namespace PathCreation.Examples {
    // Example of creating a path at runtime from a set of points.

   // [RequireComponent(typeof(PathCreator))]
    public class GeneratePathExample : MonoBehaviour {

        public bool closedLoop = true;
        public Transform[] waypoints;

        public Vector3[] positions;
        public Vector3[] rotations;

        void Start () {
            //  if (waypoints.Length > 0) {
            // Create a new bezier path from the waypoints.
            for (int i = 0; i < waypoints.Length; i++)
            {
                GameObject go = new GameObject();
                waypoints[i] = go.transform;
                waypoints[i].position = positions[i];
                waypoints[i].rotation = Quaternion.Euler( rotations[i]) ;
            }
   
                BezierPath bezierPath = new BezierPath (waypoints, closedLoop, PathSpace.xyz);
                //   GetComponent<PathCreator> ().bezierPath = bezierPath;
                this.gameObject.AddComponent<PathCreator>().bezierPath = bezierPath;
            this.gameObject.AddComponent<PathCreator>().bezierPath.ControlPointMode = BezierPath.ControlMode.Automatic;
            this.gameObject.GetComponent<PathCreator>().bezierPath.IsClosed = false;
         //   }
        }
    }
}