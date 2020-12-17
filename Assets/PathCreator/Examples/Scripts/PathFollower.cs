using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace PathCreation.Examples
{
    // Moves along a path at constant speed.
    // Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
    public class PathFollower : MonoBehaviour
    {
        public PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction;
        public float speed = 5;
        float distanceTravelled;
        public float cspeed;
        Rigidbody rb;
      public  RoadMeshCreator rmc;
        public float ang;
        List<float> angles = new List<float>();
        void Start() {
            if (pathCreator != null)
            {
                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
                pathCreator.pathUpdated += OnPathChanged;
                rb = GetComponent<Rigidbody>();
//                angles = rmc.angles;
                StartCoroutine(SpeedReckoner());

            }
        }
        public float topSpeed = 10f;
        public float move;
        public float motor;
        public int index = 0;
        void Update()
        {

            if (Input.GetMouseButton(0))
            {
                move += Time.deltaTime;
                move = Mathf.Clamp01(move);
            }
            else
            {
                move -= Time.deltaTime;
                move = Mathf.Clamp01(move);

            }
            motor =  Time.deltaTime * move;

            if (pathCreator != null)
            {
                distanceTravelled += speed *motor;
                transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                Quaternion  q = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
                q.eulerAngles = new Vector3(0,q.eulerAngles.y,0);
                transform.eulerAngles = q.eulerAngles;
              
            }
//            if (Vector3.Distance(transform.position, rmc.cubepos[index + 1].point) < 3f) 
            {
                index++;

                //if (index > rmc.cubepos.Count)
                {
                    index = 0;
                }

            }
            cspeed = speed * motor;

           // ang = angles[index];
            if (Speed >  (360/3 -  angles[index]*angles[index]))
            {
                Debug.Log("crash");
            }
        }
        Vector3 lastPosition = Vector3.zero;

        private void FixedUpdate()
        {
            //  cspeed = rb.velocity.magnitude*2.347f;
        }
        // If the path changes during the game, update the distance travelled so that the follower's position on the new path
        // is as close as possible to its position on the old path
        void OnPathChanged() {
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }

        void OnEnabled()
        {
          //  Debug.Log("dssdssdsd");
        }

        public float Speed;
        public float UpdateDelay;
        YieldInstruction timedWait = new WaitForSeconds(0.2f);

        private IEnumerator SpeedReckoner()
        {

            Vector3 lastPosition = transform.position;
            float lastTimestamp = Time.time;

            while (enabled)
            {
                yield return timedWait;

                var deltaPosition = (transform.position - lastPosition).magnitude;
                var deltaTime = Time.time - lastTimestamp;

                if (Mathf.Approximately(deltaPosition, 0f)) // Clean up "near-zero" displacement
                    deltaPosition = 0f;

                Speed = deltaPosition / deltaTime;


                lastPosition = transform.position;
                lastTimestamp = Time.time;
            }
        }
    }


}