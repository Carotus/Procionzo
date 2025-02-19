using UnityEngine;
using System.Collections;
using Unity.Behavior;

public class EnemyFOV : MonoBehaviour
{
    public GameObject playerRef;

    public SpottedBar spottedBar;

    public BehaviorGraphAgent EnemyGraph;

    public float viewRadius;

[Range(0, 360)]
    public float viewAngle;

    public LayerMask targetMask;

    public LayerMask obstacleMask;

    public bool Spotted;

    public bool IsInVision;

    public float gameOverTimer;

    public float gameOverTimerMax;


    [Header("Audio")]

    public AudioSource audioSource;

    public AudioClip spottedSound, walkSound;
    public bool PlaySpotted;

    public bool isWalkingSound;

    public Vector3 previousPosition;

    public void Start()
    {
        previousPosition = transform.position;
        gameOverTimer = 0;
        playerRef = GameObject.Find("Player");
        spottedBar = FindFirstObjectByType<SpottedBar>();
        spottedBar.SetMaxSpot(gameOverTimerMax);
        StartCoroutine(FOVRoutine());

        
    }

    private IEnumerator FOVRoutine()
    {
        float delay = 0.2f;
        WaitForSeconds wait = new WaitForSeconds(delay);

        while(true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    void FieldOfViewCheck()
    {
        Collider[] rangeCheck = Physics.OverlapSphere (transform.position, viewRadius, targetMask);
        //Debug.Log("rangeCheck length: " + rangeCheck.Length);

       if (rangeCheck.Length != 0)
       {
            //Debug.Log("rangeCheck not 0");
            Transform target = rangeCheck[0].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            if(Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2.0f)
            {
                //Debug.Log("vector less than view angle");
                float distToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask))  //raycast verso il player limitato da distanza e layermask
                {
                    PlaySpottedSound();
                    Debug.Log("Player Spotted");
                    EnemyGraph.SetVariableValue( "Spotted" , true);
                    EnemyGraph.SetVariableValue( "InVision" , true);
                    Spotted = true;
                    IsInVision = true;
                }
                else
                {
                    PlaySpotted = false;
                    EnemyGraph.SetVariableValue( "Spotted" , false);
                    EnemyGraph.SetVariableValue( "InVision" , false);
                    Spotted = false;
                    IsInVision = false;
                }
            }
            else 
            {
                PlaySpotted = false;
                EnemyGraph.SetVariableValue( "Spotted" , false);
                EnemyGraph.SetVariableValue( "InVision" , false);
                IsInVision = false;
            }
       }
       else if (IsInVision)
        {
            PlaySpotted = false;
            EnemyGraph.SetVariableValue( "Spotted" , false);
            EnemyGraph.SetVariableValue( "InVision" , false);
            IsInVision = false;
        }

    }

    public void Update()
    {
        SpottingPlayer();

        if(transform.position != previousPosition)
        {
            PlayWalkingSound();
        }
        else
        {
            StopWalkingSound();
        }

        previousPosition = transform.position;
    }




    void SpottingPlayer()
    {
        if(IsInVision)
        {
            gameOverTimer += Time.deltaTime;
            spottedBar.SetCurrentSpot(gameOverTimer);

            

            if(gameOverTimer >= gameOverTimerMax)
            {
                GameManager gm = FindAnyObjectByType<GameManager>();  
                gm.LoseScreen();
            }
            
        }
        else if (gameOverTimer >= 0)
        {
            gameOverTimer -= Time.deltaTime;
            spottedBar.SetCurrentSpot(gameOverTimer);
            Debug.Log(gameOverTimer.ToString());
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        Vector3 viewAngleA = DirFromAngle(-viewAngle / 2, false);
        Vector3 viewAngleB = DirFromAngle(viewAngle / 2, false);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + viewAngleA * viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + viewAngleB * viewRadius);
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    void PlaySpottedSound()
    {
        if(PlaySpotted == false)
        {
        audioSource.PlayOneShot(spottedSound);
        PlaySpotted = true;
        }
    }

    void PlayWalkingSound()
    {
        if(isWalkingSound == false)
        {

            audioSource.loop = true;
            audioSource.clip = walkSound;
            audioSource.Play();
            isWalkingSound = true;
        }
    }

    void StopWalkingSound()
    {
        if(isWalkingSound == true)
        {

            audioSource.loop = false;
            audioSource.Stop();
            isWalkingSound = false;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            GameManager gm = FindAnyObjectByType<GameManager>();  
            gm.LoseScreen();
        }
    }

    
}
