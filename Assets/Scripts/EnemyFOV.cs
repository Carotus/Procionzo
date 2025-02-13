using UnityEngine;
using System.Collections;
using Unity.Behavior;

public class EnemyFOV : MonoBehaviour
{
    public GameObject playerRef;

    public BehaviorGraphAgent EnemyGraph;

  // public Blackboard enemyBoard;

    public float viewRadius;

[Range(0, 360)]
    public float viewAngle;

    public LayerMask targetMask;

    public LayerMask obstacleMask;

    public bool Spotted;

    public bool IsInVision;

    public float gameOverTimer;

    private float gameOverTimerMax;

    public void Start()
    {
        gameOverTimerMax = gameOverTimer;
        playerRef = GameObject.Find("Player");
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

       if (rangeCheck.Length != 0)
       {
            Transform target = rangeCheck[0].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            if(Vector3.Angle(transform.position, dirToTarget) < viewAngle / 2.0f)
            {
                float distToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask))  //raycast verso il player limitato da distanza e layermask
                {
                    Spotted = true;
                    IsInVision = true;
                }
                else
                {
                    Spotted = false;
                    IsInVision = false;
                }
            }
            else 
            {
                IsInVision = false;
            }
       }
       else if (IsInVision)
        {
            IsInVision = false;
        }

    }

    public void Update()
    {
        // var enemyBoard = EnemyGraph.GetBlackboard(); //non va una sega
          //  if (enemyBoard != null)
         //   {
          //      IsInVision = enemyBoard.GetValue<bool>("IsSeen");
          //  }

        SpottingPlayer();
    }




    void SpottingPlayer()
    {
        if(IsInVision)
        {
            gameOverTimer -= Time.deltaTime;

            

            if(gameOverTimer <= 0)
            {
                GameManager gm = FindAnyObjectByType<GameManager>();  
                gm.LoseScreen();
            }
            
        }
        else if (gameOverTimer < gameOverTimerMax)
        {
            gameOverTimer = gameOverTimerMax;
            Debug.Log(gameOverTimer.ToString());
        }
    }
}
