using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;


public class GuardAI : MonoBehaviour
{
    public enum AIState
    {
        Idle,
        Patrol,
        Chase,
        Suspicous,//or Lost. Do we need this
        Distracted //Do we really need this - can piggy back on chase
    }

    public enum PatrolType
    {
        None,
        PatrolPoint,
        RandomPatrol
    }


    [SerializeField]
    NavMeshAgent navAgent;

    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public LayerMask targetMask;

    [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();

    public Transform currentTarget;

    public AIState currentAIState = AIState.Idle;
    
    public float headTurnAngle;
    public float headTurnSpeed;

    //Teena - if patrol 
    public PatrolType patrolType;

    [Tooltip("Fixed patrol point but will skip points if the path is incomplete")]
    public List<Transform> patrolPoint;

    [Tooltip("PatrolRadius is for random patrol use only")]
    public float patrolRadius;

    public float patrolIdleTimerMin = 2.0f;
    float patrolIdleTimer = 0.0f;
    
    public float suspicionMeter = 0.0f;
    [Tooltip("TimeLimit in seconds")]
    public float suspicionMeterTimeLimit = 5.0f;
    public float suspicionMeterIncrease = 1.0f;

    int targetLayer = 7; //hard coded 7 as player layer

    protected void Start()
    {
        if (navAgent == null) navAgent = GetComponent<NavMeshAgent>();

        if (navAgent)
        {
            StartCoroutine("FindTargetsWithDelay", 0.2f);
        }
    }

    
    void DoHeadTurn(bool doHeadTurn = false)
    {
        if(doHeadTurn)
        {
            HeadTurnAnimation();
        }
        else
        {
            isRotatingHead = false;
        }
    }


    float initialHeadRotY;
    bool turnLeft = false;
    bool isRotatingHead = false;

    Quaternion turnLeftQuat;
    Quaternion turnRightQuat;

    Vector3 turnLeftVector3;
    Vector3 turnRightVector3;

    void HeadTurnAnimation()
    {
        if(!isRotatingHead)
        {
            Quaternion curQuat = transform.localRotation;

            turnLeftVector3 = curQuat.eulerAngles + new Vector3(0, headTurnAngle);
            turnRightVector3 = curQuat.eulerAngles + new Vector3(0, -headTurnAngle);

            turnLeftQuat = Quaternion.Euler(turnLeftVector3);
            turnRightQuat = Quaternion.Euler(turnRightVector3);

            isRotatingHead = true;
            turnLeft = false;
        }

        transform.localRotation = Quaternion.RotateTowards(transform.rotation, turnLeft ? turnLeftQuat : turnRightQuat, Time.deltaTime * headTurnSpeed);

        //transform.localRotation = Quaternion.Slerp(transform.rotation, turnLeft ? turnLeftQuat : turnRightQuat, Time.deltaTime * headTurnSpeed);

        if(transform.localRotation == (turnLeft ? turnLeftQuat : turnRightQuat))
        {
            turnLeft = !turnLeft;
        }
        
        //Debug.Log("TeenaTest.GuardAI.HeadTurn.turnLeft: " + turnLeft);
    }

    void LookAtTarget()
    {
        navAgent.updateRotation = true;
        if (currentTarget)
        {
            Vector3 posDelta = currentTarget.position - transform.position;
            Quaternion lookAtQuat = Quaternion.LookRotation(posDelta);

            //Smooth rotate towards target
            transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, lookAtQuat, Time.deltaTime * headTurnSpeed);
            
            //Debug.Log("TeenaTest.GuardAI.LookAtTarget: ");
        }
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    RaycastHit[] enemyHit = new RaycastHit[1];

    void CheckEnemyToList(Transform target)
    {
        float dstToTarget = Vector3.Distance(transform.position, target.position);
        Vector3 dirToTarget = (target.position - transform.position).normalized;

        //Debug.Log("TeenaTest.GuardAI.CheckEnemyToList.dstToTarget: " + dstToTarget + " - navAgent.radius: " + navAgent.radius);
        /*
        if (dstToTarget > navAgent.radius && dstToTarget < navAgent.radius * 2)
        {
            int hits = Physics.RaycastNonAlloc(transform.position, dirToTarget, enemyHit, dstToTarget);

            //if (Physics.Raycast(transform.position, dirToTarget, dstToTarget))
            if (hits > 0 && (enemyHit[0].collider.gameObject.layer == targetLayer || enemyHit[0].collider.GetComponent<BasePlayer>() != null))
            {
                visibleTargets.Add(target);
            }
        }
        else
        */

        if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
        {
            int hits = Physics.RaycastNonAlloc(transform.position, dirToTarget, enemyHit, dstToTarget);

            //if (Physics.Raycast(transform.position, dirToTarget, dstToTarget))
            if (hits > 0 && (enemyHit[0].collider.gameObject.layer == targetLayer || enemyHit[0].collider.GetComponent<BasePlayer>() != null))
            {
                visibleTargets.Add(target);
            }
        }
    }

    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        
        //Check nearby radius + fov angle first 
        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            CheckEnemyToList(target);
        }

        if (visibleTargets.Count <= 0)
        {
            if (currentTarget != null)
            {
                //if cannot find any enemy, then check last known target if available in line of sight

                if(currentTarget.gameObject != null)
                {
                    //if player - do LOS
                    if (currentTarget.gameObject.layer == targetLayer || (enemyHit[0].collider && enemyHit[0].collider.GetComponent<BasePlayer>() != null))
                    {
                        CheckEnemyToList(currentTarget);
                    }
                    /*
                    else //if bell - no need line of sight
                    {
                        visibleTargets.Add(currentTarget);
                    }
                    */
                }
            }
        }

        if (visibleTargets.Count > 0)
        {
            visibleTargets = visibleTargets.OrderBy(x => Vector3.Distance(this.transform.position, x.transform.position)).ToList();

            foreach (var vt in visibleTargets)
            {
                NavMeshPath nmp = new NavMeshPath();
                navAgent.CalculatePath(vt.position, nmp);
                //Debug.Log("TeenaTest.GuardAI.DetectionFOV.gameObject: " + vt.name + " - status: " + nmp.status + " - pathPending: " + navAgent.pathPending + " - navAgent.path: " + navAgent.path);
                navAgent.ResetPath();
                navAgent.SetPath(nmp);
                SetGuardAIState(GuardAI.AIState.Chase);
                currentTarget = vt;
                break;
                /*
                if (nmp.status == NavMeshPathStatus.PathComplete)
                {
                    
                    //Teena - can be used for moving backwards a bit before attacking the player again
                    //navAgent.Move()
                    //navAgent.isPathStale;
                    //navAgent.speed;

                    break;
                }
                */
            }
        }
        else
        {
            //Teena - we want enemy to look at player when they are visible only but we will still follow last known path, so no path reset
            currentTarget = null;
        }
    }

    void CheckForChaseEndedState()
    {
        if(suspicionMeter > 0)
        {
            SetGuardAIState(GuardAI.AIState.Suspicous);
        }
        else
        {
            if(patrolType == PatrolType.None)
            {
                SetGuardAIState(GuardAI.AIState.Idle);
            }
            else
            {
                SetGuardAIState(GuardAI.AIState.Patrol);
            }
        }

        if(currentAIState != AIState.Patrol)
        {
            returnToPatrolPath = false;
        }
    }

    public void SetCurrentTarget(Transform target)
    {
        currentTarget = target;
    }

    bool returnToPatrolPath = false;
    int patrolPointIndex = 0;

    NavMeshPathStatus CalculatePatrolPath(Vector3 dest)
    {
        NavMeshPath navPath = new NavMeshPath();
        navAgent.CalculatePath(dest, navPath);

        if (navPath.status == NavMeshPathStatus.PathComplete)
        {
            navAgent.ResetPath();
            navAgent.SetPath(navPath);
        }

        return navPath.status;
    }

    void PatrolBehaviour()
    {
        DoHeadTurn(true);
        if (returnToPatrolPath == false)
        {
            navAgent.ResetPath();
            patrolPointIndex = -1;
            returnToPatrolPath = true;
            //Debug.Log("TeenaTest.GuardAI.PatrolBehaviour.V1");
        }

        if (navAgent.hasPath && navAgent.remainingDistance != 0)
        {
            patrolIdleTimer = patrolIdleTimerMin;
            //Debug.Log("TeenaTest.GuardAI.PatrolBehaviour.V2.patrolIdleTimer: " + patrolIdleTimer);
            return;
        }
        else
        {
            patrolIdleTimer -= Time.deltaTime;

            if (patrolIdleTimer > 0.0f)
            {
                //Debug.Log("TeenaTest.GuardAI.PatrolBehaviour.V3.patrolIdleTimer: " + patrolIdleTimer);
                return;
            }
        }

        if (navAgent.hasPath && navAgent.remainingDistance == 0)
        {
            
        }

        if (patrolType == PatrolType.PatrolPoint)
        {
            patrolPointIndex++;
            if (patrolPointIndex > patrolPoint.Count - 1) patrolPointIndex = 0;
            else if (patrolPointIndex < 0) patrolPointIndex = patrolPoint.Count - 1;

            NavMeshPathStatus status = CalculatePatrolPath(patrolPoint[patrolPointIndex].position);
            //Debug.Log("TeenaTest.GuardAI.PatrolBehaviour.V4.status: " + status);
            if (status != NavMeshPathStatus.PathComplete)
            {
                for (int i = 0; i < patrolPoint.Count; i++)
                {
                    NavMeshPathStatus statusTemp = CalculatePatrolPath(patrolPoint[i].position);

                    if (statusTemp == NavMeshPathStatus.PathComplete)
                    {
                        //Debug.Log("TeenaTest.GuardAI.PatrolBehaviour.V5");
                        patrolPointIndex = i;
                        break;
                    }
                }
            }
        }
        else if (patrolType == PatrolType.RandomPatrol)
        {
            
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    void SetTarget(Vector3 targetPosition)
    {
        if (navAgent == null)
        {
            Debug.LogError("Missing NavAgent for Guard AI");
            return;
        }

        NavMeshPath path = new NavMeshPath();
        navAgent.CalculatePath(targetPosition, path);
        if (path.status == NavMeshPathStatus.PathPartial)
        {
        }

        navAgent.destination = targetPosition;
    }
    
    public void SetGuardAIState(AIState InCurrentAIState)
    {
        currentAIState = InCurrentAIState;
    }

    void SuspicionBehaviour()
    {
        DoHeadTurn(true);
        if (suspicionMeter >= 0.0f)
        {
            suspicionMeter -= suspicionMeterIncrease * Time.deltaTime;
            if (suspicionMeter < 0.0f)
            {
                suspicionMeter = 0.0f;
            }
        }
    }

    void ChaseBehaviour()
    {
        DoHeadTurn(false);
        LookAtTarget();
        if (suspicionMeter <= suspicionMeterTimeLimit)
        {
            suspicionMeter += suspicionMeterIncrease * Time.deltaTime;
            if (suspicionMeter > suspicionMeterTimeLimit) suspicionMeter = suspicionMeterTimeLimit;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        BasePlayer ply = other.gameObject.GetComponent<BasePlayer>();
        //Debug.Log("TeenaTest.GuardAI.KillCollidedPlayer.V1");
        if (ply != null || other.gameObject.layer == targetLayer)
        {
            //Debug.Log("TeenaTest.GuardAI.KillCollidedPlayer.V2");
            if (ply != null)
            {
                //Debug.Log("TeenaTest.GuardAI.KillCollidedPlayer.V3");
                ply.Damage();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTarget == null)
        {
            CheckForChaseEndedState();
        }

        switch (currentAIState)
        {
            case AIState.Idle:
                DoHeadTurn(true);
                break;
            case AIState.Patrol:
                PatrolBehaviour();
                break;
            case AIState.Chase:
                ChaseBehaviour();
                break;
            case AIState.Suspicous:
                SuspicionBehaviour();
                break;
            case AIState.Distracted:
                // code block
                break;
            default:
                // code block
                break;
        }
    }
    
    
}
