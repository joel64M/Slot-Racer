using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Examples;
using PathCreation;
using UnityEditor;
namespace NameSpaceName {

    public class FollowPath : MonoBehaviour
    {

        #region Variables

        public SidePath sp;
        Rigidbody rb;
        List<newPoints> positions = new List<newPoints>();
        public int currentIndex = 0;
        public Vector3 steerVector;
        public float dir;
        public float motor;
        Quaternion targetRotation;
        public float distFromPath = 10f;
        public float currentSpeed = 10f;
        public float move;
        public float smoothTurnSpeed = 2.5f;
        public float minDistToCrash = 2f;
        float distTravelled = 0;
        public bool crash;
        bool safe;
        public bool isAi;
        public bool isleftPath;
        public Transform p1;
        public Transform p2;
        public Transform p3;
        public Transform p4;

        int crashturn = 1;
        #endregion

        #region Builtin Methods

        void Awake()
        {
            if (isleftPath)
            {
                sp = GameObject.FindGameObjectWithTag("LeftPath").GetComponent<SidePath>();
                transform.position = sp.cubepos[0].point;
            }
            else
            {
                sp = GameObject.FindGameObjectWithTag("RightPath").GetComponent<SidePath>();
                transform.position = sp.cubepos[0].point;


            }
            positions = sp.cubepos;
            rb = GetComponent<Rigidbody>();

        }


        void CalculateBools()
        {
            if (sp.angles.Count - 1 > currentIndex)
                if (sp.angles[currentIndex] < 1.5f)
                {
                    safe = true;
                }
                else
                {
                    tempSafeTimer += Time.deltaTime;
                    if (move < 0.55f)
                    {
                        if (tempSafeTimer > 0.3f)
                        {
                            safe = true;
                            if (tempSafeTimer > 0.33f)
                                tempSafeTimer = 0;
                        }
                    }
                    else
                    {
                        safe = false;
                    }
                }
        }

        float tempSafeTimer = 0;
        public LayerMask layer;

        private void OnDrawGizmos()
        {
            DrawWireCapsule(p1.position, p2.position, 0.25f, 1f, Color.red);
        }

        public static void DrawWireCapsule(Vector3 _pos, Vector3 _pos2, float _radius, float _height, Color _color = default)
        {
            if (_color != default) Handles.color = _color;

            var forward = _pos2 - _pos;
            var _rot = Quaternion.LookRotation(forward);
            var pointOffset = _radius / 2f;
            var length = forward.magnitude;
            var center2 = new Vector3(0f, 0, length);

            Matrix4x4 angleMatrix = Matrix4x4.TRS(_pos, _rot, Handles.matrix.lossyScale);

            using (new Handles.DrawingScope(angleMatrix))
            {
                Handles.DrawWireDisc(Vector3.zero, Vector3.forward, _radius);
                Handles.DrawWireArc(Vector3.zero, Vector3.up, Vector3.left * pointOffset, -180f, _radius);
                Handles.DrawWireArc(Vector3.zero, Vector3.left, Vector3.down * pointOffset, -180f, _radius);
                Handles.DrawWireDisc(center2, Vector3.forward, _radius);
                Handles.DrawWireArc(center2, Vector3.up, Vector3.right * pointOffset, -180f, _radius);
                Handles.DrawWireArc(center2, Vector3.left, Vector3.up * pointOffset, -180f, _radius);

                DrawLine(_radius, 0f, length);
                DrawLine(-_radius, 0f, length);
                DrawLine(0f, _radius, length);
                DrawLine(0f, -_radius, length);
            }
        }

        private static void DrawLine(float arg1, float arg2, float forward)
        {
            Handles.DrawLine(new Vector3(arg1, arg2, 0f), new Vector3(arg1, arg2, forward));
        }

        RaycastHit[] hit = new RaycastHit[2];// = Physics.CapsuleCastAll(p1.position, p2.position, 0.25f, -transform.up, 2f, layer);
        RaycastHit[] hit2 = new RaycastHit[2];
        void Update()
        {
            if (isAi)
                CalculateBools();
            IncrIndex();

          //  RaycastHit[] hit2 = Physics.CapsuleCastAll(p3.position, p3.position, 0.25f, -transform.up, 2f, layer);
       //     Debug.Log(Physics.CapsuleCastNonAlloc(p1.position, p2.position, 0.25f, -transform.up, hit, 2f,layer));
            if (Physics.CapsuleCastNonAlloc(p1.position, p2.position, 0.25f, -transform.up, hit,2f,layer) < 2   && !crash) 
            {
                Debug.Log("crashed 1 init");
                crash = true;
                crashturn = 1;
            }
            if (Physics.CapsuleCastNonAlloc(p3.position, p4.position, 0.25f, -transform.up, hit, 2f, layer) < 2 && !crash)
            {
                Debug.Log("crashed 2init");
                crash = true;
                crashturn = -1;

            }
            if (isAi)
            {
                if (safe)
                {
                    move += Time.deltaTime;
                    move = Mathf.Clamp01(move);
                }
                else
                {
                    move -= Time.deltaTime;
                    move = Mathf.Clamp01(move);

                }

            }
            else
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
            }
      
            motor = Time.deltaTime * move;
            engine = motor * currentSpeed;
            distTravelled += engine;
            CalculateSteering();
     

            // Vector3 vec = pc.path.GetClosestPointOnPath(transform.position);//   pc.path.GetPointAtDistance(distTravelled, EndOfPathInstruction.Stop);
            //  if(Vector3.Distance(vec,transform.position) > minDistToCrash)
            {
                // crash = true;
            }
            if(!donee)
            if (!crash)
            {
                transform.Translate(Vector3.forward * (engine), Space.Self);
                crashSpeed = 1.5f;
                    
               // Debug.DrawLine(vec, transform.position, Color.green, 100f);

            }
            else
            {
                crashSpeed -= Time.deltaTime;
                    crashSpeed2 -= Time.deltaTime;
                    if (crashSpeed > 0)
                    {
                        transform.Translate(Vector3.forward * (crashSpeed), Space.Self);

                    }
                }

        }
        float engine = 0;
        float crashSpeed = 0f;
        float crashSpeed2 = 1f;

        #endregion

        #region Custom Methods
        void CalculateSteering()
        {
            steerVector = transform.InverseTransformPoint(positions[currentIndex].point);
           // Debug.DrawLine(positions[currentIndex].point, transform.position, Color.red, 100f);
            // steer.y =steerAngle* (steerVector.x / steerVector.magnitude);
            //Vector3 steerVector2 = transform.InverseTransformPoint(path[currentIndex]);
            //  this.transform.Rotate(steer*Time.deltaTime );
            dir = (steerVector.x / steerVector.magnitude) * 100;
            Debug.DrawLine(transform.position, positions[currentIndex].point, Color.white, 100f);
            targetRotation = Quaternion.LookRotation(positions[currentIndex].point - transform.position);
            if(!crash )
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime* smoothTurnSpeed);
            else
            {
                if(crashSpeed2>0)
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + ( crashSpeed2) * crashturn, 0);
            }
            //  IncrIndex();


        }
        bool donee;
        void IncrIndex()
        {
            if (steerVector.magnitude <= distFromPath )
            {
                if (currentIndex >= positions.Count-1)
                {
                    distFromPath = 0f;
                    //motor = 0;
                    // currentIndex = 0;
                    donee = true;
                }
                else
                {
                    currentIndex++;

                }
            }
        }
        #endregion

    }
}
