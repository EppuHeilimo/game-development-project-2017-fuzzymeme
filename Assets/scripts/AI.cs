using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Assets.Scripts.Weapon_Inventary;

public abstract class AI : MonoBehaviour
{
    protected Transform player;
    protected Vector3 playerDirection;
    protected float playerDistance;
    protected NavMeshAgent agent;
    protected float idleTimeMax = 8.5f;
    protected float idleTimeMin = 3f;
    protected float idleTime = 2f;
    protected float shootdistance = 5f;
    protected bool obstructed = false;
    protected float obstructionHandlingTime = 1f;
    protected float reactionTimer = 0f;
    protected float obstructionTimer = 0f;
    protected float idleTimer = 0f;
    protected Weapon weapon;
    protected bool moving;
    protected Transform ThisTransform;
    public AIState state;


    public float RotationSpeed = 5.0f;
    public float SightDistance = 12f;
    public float Speed = 3f;
    public float GiveupDistance = 10f;
    public float ReactionTime = 0.5f;

    public enum AIState
    {
        Idle,
        Seek,
        Attack
    }

    
	// Use this for initialization
	public void Init()
	{
	    agent = GetComponent<NavMeshAgent>();
	    player = GameObject.FindGameObjectWithTag("Player").transform;
        state = AIState.Idle;
        playerDistance = Vector3.Distance(transform.position, player.position);
        playerDirection = (transform.position - player.position) / playerDistance;
        weapon = GetComponent<Weapon>();
	    shootdistance = weapon.BulletPrefab.GetComponent<Bullet>().Distance * 0.8f;


       
	}
	
	// Update is called once per frame
	protected void StateUpdate()
	{
	    FindPlayer();
        switch (state)
	    {
	        case AIState.Idle:
                Idle();
	        break;
            case AIState.Attack:
                Attack();
            break;
            case AIState.Seek:
                Seek();
	        break;
	    }
	}

    public void GotAttacked()
    {
        if (state == AIState.Idle)
        {
            state = AIState.Seek;
        }
    }

    void FindPlayer()
    {
        playerDistance = Vector3.Distance(transform.position, player.position);

        reactionTimer += Time.fixedDeltaTime;
        if (reactionTimer > ReactionTime && playerDistance <= SightDistance)
        {
            reactionTimer = 0;
            RaycastHit hit;
            playerDirection = (player.position - transform.position ) / playerDistance;
            //Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), playerDirection, Color.blue, 5f);
            if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), playerDirection, out hit, SightDistance))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    if (state == AIState.Idle)
                    {
                        agent.ResetPath();
                        state = AIState.Seek;
                    }
                    if (obstructed && state == AIState.Seek || state == AIState.Attack)
                    {
                        ObstructionCleared();
                    }
                }
                else
                {
                    if (state == AIState.Seek || state == AIState.Attack)
                    {
                        obstructed = true;
                    }
                }
            }
        }
        if (playerDistance > GiveupDistance)
        {
            state = AIState.Idle;
        }
    }

    protected Vector3 RndPointInArea(float walkradius, Vector3 position)
    {
        Vector3 temp;
        Vector3 direction;
        NavMeshHit hit;
        temp = Random.insideUnitSphere * .5f * walkradius;
        direction = temp + position;
        direction.y = 0;
        if(NavMesh.SamplePosition(direction, out hit, walkradius, NavMesh.AllAreas))
            return hit.position;
        else
        {
            return direction += temp * -2;
        }

    }

    protected Vector3 RndPointInCircle(float walkradius, Vector3 center)
    {
        Vector3 temp;
        Vector3 direction;
        NavMeshHit hit;
        temp = Random.insideUnitSphere * walkradius / 2;
        direction = temp + center;
        direction += temp * (walkradius / 3);
        direction.y = 0;
        if (NavMesh.SamplePosition(direction, out hit, walkradius, NavMesh.AllAreas))
            return hit.position;
        else
        {
            return center - (transform.rotation*new Vector3(walkradius, 0, walkradius));
        }

    }

    protected void ObstructionCleared()
    {
        agent.ResetPath();
        obstructed = false;
        obstructionTimer = 0;
    }

    public bool RotateTowards(Transform target, float speed)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, speed);
        float transformy = Mathf.Abs(transform.rotation.eulerAngles.y);
        float looky = Mathf.Abs(lookRotation.eulerAngles.y);
        if (approx(transformy, looky, 10f))
        {
            return true;
        }
        return false;
    }

    public bool RotateAwayFrom(Transform target, float speed)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(-direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, speed);
        float transformy = Mathf.Abs(transform.rotation.eulerAngles.y);
        float looky = Mathf.Abs(lookRotation.eulerAngles.y);
        if (approx(transformy, looky, 0.1f))
        {
            return true;
        }
        return false;
    }

    private bool approx(float a, float b, float accuracy)
    {
        float sub = a - b;
        if (Mathf.Abs(sub) < accuracy)
        {
            return true;
        }
        return false;

    }

    protected abstract void Attack();
    protected abstract void Idle();
    protected abstract void Seek();
}
