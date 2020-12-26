using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Examples;
using PathCreation;
using UnityEditor;
namespace NameSpaceName {
    [RequireComponent(typeof(Statistics))]
    [RequireComponent(typeof(Rigidbody))]

    public class FollowPath : MonoBehaviour
    {

        #region Variables

         SidePath sp;
        Rigidbody rb;
        List<newPoints> positions = new List<newPoints>();
        Quaternion targetRotation;
        Vector3 steerVector;

        [SerializeField] int currentIndex = 0;
         float dir;
       float motor;
         float engine = 0;
        [SerializeField] float inputMove;
        bool safe;
        public float completion;
    [SerializeField]    bool pathCompleted;

        [Header("Common Properties")]
        public float distFromPath = 10f;
        public float maxSpeed = 10f;
        public float smoothTurnSpeed = 2.5f;
        public float minDistToCrash = 2f;
         

        [Header("AI Properties")]
        public bool isAi;
        public bool isleftPath;
        public Transform p1;
        public Transform p2;
        public Transform p3;
        public Transform p4;

       [SerializeField] bool crash;
        int crashturn = 1;
        float tempSafeTimer = 0;
        float crashSpeed = 0f;
        float crashSpeed2 = 1f;
        public LayerMask layer;
        RaycastHit[] hit = new RaycastHit[2];
        RaycastHit[] hit2 = new RaycastHit[2];
        GameManager gm;
        AudioSource ads;
        #endregion

        #region Builtin Methods

        private void OnEnable()
        {
            gm = FindObjectOfType<GameManager>();
            ads = GetComponent<AudioSource>();
            ads.volume = 0;

            gm.OnGameStateChangedAction -= OnGameStateChanged;
        }
        private void OnDisable()
        {
            gm.OnGameStateChangedAction += OnGameStateChanged;
        }

        /*
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
        */

        void Update()
        {
            if (gm)
            {
                if (gm.CurrentGameState == GAMESTATE.GAMESTART || gm.CurrentGameState == GAMESTATE.GAMECOMPLETE || gm.CurrentGameState == GAMESTATE.GAMEOVER || gm.CurrentGameState == GAMESTATE.RACECOMPLETE) 
                {
                    GetInput();

                    if (isAi)
                        CalculateSafeZone();
                    CalculateCrashZone();
                    if (!pathCompleted)
                    {
                        IncrIndex();
                        CalculateSteering();
                        CalculateMovement();
                     
                    }
                    else
                    {
                     //   ads.volume = 0;
                    }
                    if (ads)
                        CalculateEngineSound();
                    CalculateCompletion();
                    CalculateRace();
                }
            }
        
        }
  
        #endregion

        #region Custom Methods
       public void Initialize(bool isLeft)
        {
            isleftPath = isLeft;
            if (isLeft)
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
       public bool complete;
        void CalculateEngineSound()
        {
            if (inputMove > 0)
            {
                ads.volume = inputMove;
                ads.pitch = 1 + inputMove/3f;

            }
            else
            {
                ads.volume = 0;
            }
        }
        void CalculateRace()
        {
            if (gm)
            {
                if (!isAi)
                {
                    if (crash)
                    {
                        gm.SetGameState(GAMESTATE.GAMEOVER);
                    }
                  
                }
                if (completion >= 99.5f)
                {
                    if (!complete)
                    {
                        GetComponent<Statistics>().finalRank = gm.reachedDestination;
                        gm.reachedDestination++;
                        complete = true;
                    }
                    if (!isAi)
                    {
                        gm.SetGameState(GAMESTATE.RACECOMPLETE);
                        rb.constraints = RigidbodyConstraints.FreezeAll;
                    }
                }

            }
        }
        void CalculateSteering()
        {
            if(currentIndex < positions.Count)
            {
                steerVector = transform.InverseTransformPoint(positions[currentIndex].point);
                dir = (steerVector.x / steerVector.magnitude) * 100;
                Debug.DrawLine(transform.position, positions[currentIndex].point, Color.white, 100f);
                targetRotation = Quaternion.LookRotation(positions[currentIndex].point - transform.position);
                if (!crash)
                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * smoothTurnSpeed);
                else
                {
                    if (crashSpeed2 > 0)
                        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + (crashSpeed2) * crashturn, 0);
                }
            }
        }

        void IncrIndex()
        {
            if (steerVector.magnitude <= distFromPath )
            {
                if (currentIndex >= positions.Count)
                {
                  //  distFromPath = 1f;
                  // pathCompleted = true;
                }
                else
                {
                    currentIndex++;
                    
                }
            }
            if (currentIndex >= positions.Count)
            {
                if(Vector3.Distance(transform.position,positions[currentIndex-1].point)<2f)
                {
                    pathCompleted = true;
                    currentIndex = positions.Count - 1;

                }
            }

        }

        void CalculateSafeZone()
        {
            if (sp.angles.Count - 1 > currentIndex)
                if (sp.angles[currentIndex] < 1.5f)
                {
                    safe = true;
                }
                else
                {
                    tempSafeTimer += Time.deltaTime;
                    if (inputMove < 0.5f)
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

        void GetInput()
        {
            if (isAi)
            {
                if (safe && !pathCompleted)
                {
                    inputMove += Time.deltaTime/2;
                    inputMove = Mathf.Clamp01(inputMove);
                }
                else
                {
                    inputMove -= Time.deltaTime/2;
                    inputMove = Mathf.Clamp01(inputMove);
                }
            }
            else
            {
                if (Input.GetMouseButton(0))
                {
                    inputMove += Time.deltaTime;
                    inputMove = Mathf.Clamp01(inputMove);
                }
                else
                {
                    inputMove -= Time.deltaTime;
                    inputMove = Mathf.Clamp01(inputMove);
                }
            }
            motor = Time.deltaTime * inputMove;
            engine = motor * maxSpeed;
        }

        void CalculateMovement()
        {
            if (!pathCompleted)
            {
                if (!crash)
                {
                    transform.Translate(Vector3.forward * (engine), Space.Self);
                    crashSpeed = 1.5f;
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
        }

        void CalculateCrashZone()
        {
            if (!pathCompleted)
            {
                if (Physics.CapsuleCastNonAlloc(p1.position, p2.position, 0.25f, -transform.up, hit, 2f, layer) < 2 && !crash)
                {
                    //  Debug.Log("crashed 1 init");
                    crash = true;
                    crashturn = 1;
                }
                if (Physics.CapsuleCastNonAlloc(p3.position, p4.position, 0.25f, -transform.up, hit, 2f, layer) < 2 && !crash)
                {
                    //Debug.Log("crashed 2init");
                    crash = true;
                    crashturn = -1;
                }
            }
        }
         void CalculateCompletion()
        {
            if( currentIndex<positions.Count)
            {
                //completion = (currentIndex - (Vector3.Distance(positions[currentIndex].point, transform.position) / Vector3.Distance(positions[currentIndex].point, positions[currentIndex - 1].point))) / (positions.Count - 1) * 100;
                completion = (currentIndex - (Vector3.Distance(positions[currentIndex].point, transform.position) / Vector3.Distance(positions[currentIndex].point, positions[currentIndex - 1].point))) / (positions.Count - 1) * 100;

            }
        }
        void OnGameStateChanged(GAMESTATE gs)
        {
            if(gs == GAMESTATE.GAMESTART)
            {

            }
        }
        #endregion

    }
}
