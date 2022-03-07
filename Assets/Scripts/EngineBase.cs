using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NameSpaceName;
using UnityEditor;

[RequireComponent(typeof(Statistics))]
[RequireComponent(typeof(Rigidbody))]
public class EngineBase : MonoBehaviour
{

   public SidePath sp;  //class
    Rigidbody rb;
    List<newPoints> positions = new List<newPoints>();
    Quaternion targetRotation;
    Vector3 steerVector;
    public int currentIndex = 0; //class
   public float motor;
    float engine = 0;
   public float inputMove; //class

    public float completion;
    [SerializeField] bool pathCompleted;

    [Header("Common Properties")]
    public float distFromPath = 10f;
    public float maxSpeed = 100f;
    public float smoothTurnSpeed = 4f;
    public float minDistToCrash = 1.5f;

    [Header("SideCapsules")]
    public Transform p1;
    public Transform p2;
    public Transform p3;
    public Transform p4;

    [SerializeField] bool crash;
    int crashturn = 1;
    float crashSpeed = 0f;
    float crashSpeed2 = 1f;
    public LayerMask layer;
    RaycastHit[] hit = new RaycastHit[2];
    RaycastHit[] hit2 = new RaycastHit[2];
   public GameManager gm; //class
    AudioSource ads;
    public bool complete;
    bool isPlaying = false;
    private void OnEnable()
    {
        gm = FindObjectOfType<GameManager>();
        ads = GetComponent<AudioSource>();
        ads.volume = 0;

        //gm.OnGameStateChangedAction -= OnGameStateChanged;
    }
    private void OnDisable()
    {
        //gm.OnGameStateChangedAction += OnGameStateChanged;
    }

    public void Initialize(bool isLeft)
    {
        if (isLeft)
        {
            sp = GameObject.FindGameObjectWithTag("LeftPath").GetComponent<SidePath>();
            transform.position = sp.pos[0].point;
        }
        else
        {
            sp = GameObject.FindGameObjectWithTag("RightPath").GetComponent<SidePath>();
            transform.position = sp.pos[0].point;
        }
        positions = sp.pos;
        rb = GetComponent<Rigidbody>();
    }

    public virtual void GetInput()
    {
        motor = Time.deltaTime * inputMove;
        engine = motor * maxSpeed;
    }

    public virtual void CalculateCrashZone()
    {
        if (!complete)
        {
            if (Physics.CapsuleCastNonAlloc(p1.position, p2.position, 0.25f, -transform.up, hit, 2f, layer) < 2 && !crash)
            {
                crash = true;
                crashturn = 1;
            }
            if (Physics.CapsuleCastNonAlloc(p3.position, p4.position, 0.25f, -transform.up, hit, 2f, layer) < 2 && !crash)
            {
                crash = true;
                crashturn = -1;
            }
        }
    }

    public virtual void IncrIndex()
    {
        if (steerVector.magnitude <= distFromPath)
        {
            if (currentIndex < positions.Count)
            {
                currentIndex++;
            }
        }
        //if (currentIndex >= positions.Count)
        //{
        //    if (Vector3.Distance(transform.position, positions[currentIndex - 1].point) < 2f)
        //    {
        //        pathCompleted = true;
        //        if (!complete)
        //        {
        //            RaceComplete();
        //        }
        //        currentIndex = positions.Count - 1;

        //    }
        //}

    }

    public virtual void CalculateSteering()
    {
        if (currentIndex < positions.Count)
        {
            steerVector = transform.InverseTransformPoint(positions[currentIndex].point);
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

    public virtual void CalculateMovement()
    {
        if (!complete)
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
                else
                {
                    ads.volume = 0;
                    ads.Stop();
                }
            }
        }
    }

     public  void CalculateEngineSound()
    {
        if (!isPlaying)
        {
            isPlaying = true;
            ads.Play();
        }
        if (inputMove > 0)
        {
            ads.volume = inputMove;
            ads.pitch = 1 + inputMove / 3f;

        }
        else
        {
            ads.volume = 0;
        }
    }

    public void CalculateRace()
    {
      
     
            if (crash && gm.CurrentGameState != GAMESTATE.GAMEOVER)
            {
                gm.SetGameState(GAMESTATE.GAMEOVER);
            }

     
        //if (completion >= 99.9f)
        //{
        //    if (!complete)
        //    {
        //        RaceComplete();
        //    }
        //}
    }

   public void CalculateCompletion()
    {
        if (currentIndex < positions.Count)
        {
            //completion = (currentIndex - (Vector3.Distance(positions[currentIndex].point, transform.position) / Vector3.Distance(positions[currentIndex].point, positions[currentIndex - 1].point))) / (positions.Count - 1) * 100;
            completion = (currentIndex - (Vector3.Distance(positions[currentIndex].point, transform.position) / Vector3.Distance(positions[currentIndex].point, positions[currentIndex - 1].point))) / (positions.Count - 1) * 100;

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal"))
        {
            if (!complete)
            {
                pathCompleted = true;
                RaceComplete();
            }
        }

        if (other.CompareTag("Coin"))
        {
            gm.CoinCollected();
        }
    }

    void RaceComplete()
    {
        //    GetComponent<Statistics>().finalRank = gm.reachedDestination;
        //    gm.reachedDestination++;
        GetComponent<Statistics>().ReachedGoal();
        complete = true;
        ads.volume = 0;
        ads.Stop();
        //if (!isAi)
        //{
        //    gm.SetGameState(GAMESTATE.RACECOMPLETE);
        //    rb.constraints = RigidbodyConstraints.FreezeAll;
        //}
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }


    //#region Gizmos
    //private void OnDrawGizmos()
    //{
    //    DrawWireCapsule(p1.position, p2.position, 0.25f, 1f, Color.red);
    //    DrawWireCapsule(p3.position, p4.position, 0.25f, 1f, Color.red);
    //}
    //public static void DrawWireCapsule(Vector3 _pos, Vector3 _pos2, float _radius, float _height, Color _color = default)
    //{
    //    if (_color != default) Handles.color = _color;

    //    var forward = _pos2 - _pos;
    //    var _rot = Quaternion.LookRotation(forward);
    //    var pointOffset = _radius / 2f;
    //    var length = forward.magnitude;
    //    var center2 = new Vector3(0f, 0, length);

    //    Matrix4x4 angleMatrix = Matrix4x4.TRS(_pos, _rot, Handles.matrix.lossyScale);

    //    using (new Handles.DrawingScope(angleMatrix))
    //    {
    //        Handles.DrawWireDisc(Vector3.zero, Vector3.forward, _radius);
    //        Handles.DrawWireArc(Vector3.zero, Vector3.up, Vector3.left * pointOffset, -180f, _radius);
    //        Handles.DrawWireArc(Vector3.zero, Vector3.left, Vector3.down * pointOffset, -180f, _radius);
    //        Handles.DrawWireDisc(center2, Vector3.forward, _radius);
    //        Handles.DrawWireArc(center2, Vector3.up, Vector3.right * pointOffset, -180f, _radius);
    //        Handles.DrawWireArc(center2, Vector3.left, Vector3.up * pointOffset, -180f, _radius);

    //        DrawLine(_radius, 0f, length);
    //        DrawLine(-_radius, 0f, length);
    //        DrawLine(0f, _radius, length);
    //        DrawLine(0f, -_radius, length);
    //    }
    //}

    //private static void DrawLine(float arg1, float arg2, float forward)
    //{
    //    Handles.DrawLine(new Vector3(arg1, arg2, 0f), new Vector3(arg1, arg2, forward));
    //}

    //#endregion

}
