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
            navAgent.updateRotation = false;
            HeadTurnAnimation();
        }
        else
        {
            isRotatingHead = false;
            navAgent.updateRotation = true;
        }
    }


    float initialHeadRotY;
    bool turnLeft = false;
    bool isRotatingHead = false;

    Quaternion turnLeftQuat;
    Quaternion turnRightQuat;

    void HeadTurnAnimation()
    {
        if(!isRotatingHead)
        {
            Quaternion curQuat = transform.localRotation;
            turnLeftQuat = Quaternion.Euler(curQuat.eulerAngles + new Vector3(0, headTurnAngle));
            turnRightQuat = Quaternion.Euler(curQuat.eulerAngles + new Vector3(0, -headTurnAngle));
            isRotatingHead = true;
            turnLeft = false;
        }

        transform.localRotation = Quaternion.Slerp(transform.rotation, turnLeft ? turnLeftQuat : turnRightQuat, Time.deltaTime * headTurnSpeed);

        if(transform.localRotation == (turnLeft ? turnLeftQuat : turnRightQuat))
        {
            turnLeft = !turnLeft;
        }
        
        Debug.Log("TeenaTest.GuardAI.HeadTurn.turnLeft: " + turnLeft);
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
            
            Debug.Log("TeenaTest.GuardAI.LookAtTarget: ");
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

    void CheckEnemyToList(Transform target)
    {
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
        {
            float dstToTarget = Vector3.Distance(transform.position, target.position);

            if (Physics.Raycast(transform.position, dirToTarget, dstToTarget, targetMask))
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

                //if player - do LOS
                if (currentTarget.gameObject.layer == targetMask.value)
                {
                    CheckEnemyToList(currentTarget);
                }
                else //if bell - no need line of sight
                {
                    visibleTargets.Add(currentTarget);
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
                Debug.Log("TeenaTest.GuardAI.DetectionFOV.gameObject: " + vt.name + " - status: " + nmp.status + " - pathPending: " + navAgent.pathPending);
                navAgent.ResetPath();
                navAgent.SetPath(nmp);
                SetGuardAIState(GuardAI.AIState.Chase);
                currentTarget = vt;

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

    public void SetCurrentTarget(Transform target)
    {
        currentTarget = target;
    }

    void CheckPatrol()
    {
        switch (patrolType)
        {
            case PatrolType.None:
                break;
            case PatrolType.PatrolPoint:
                DoHeadTurn(true);
                break;
            case PatrolType.RandomPatrol:
                LookAtTarget();
                break;
            default:
                // code block
                break;
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

    // Update is called once per frame
    void Update()
    {
        switch (currentAIState)
        {
            case AIState.Idle:
                DoHeadTurn(true);
                break;
            case AIState.Patrol:
                DoHeadTurn(true);
                break;
            case AIState.Chase:
                LookAtTarget();
                break;
            case AIState.Suspicous:
                // code block
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
