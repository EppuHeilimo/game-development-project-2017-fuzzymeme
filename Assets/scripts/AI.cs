using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Assets.Scripts.Weapon_Inventary;

public class AI : MonoBehaviour
{
    private Transform player;
    private float sightDistance = 12f;
    private Vector3 playerDirection;
    private float playerDistance;
    private NavMeshAgent agent;
    private float speed = 3f;
    private float shootdistance = 5f;
    private float giveupdistance = 10f;
    private bool obstructed = false;
    private float reactionTime = 0.5f;
    private float obstructionHandlingTime = 1f;
    private float idleTime = 1.5f;
    
    private float reactionTimer = 0f;
    private float obstructionTimer = 0f;
    private float idleTimer = 0f;
    private Weapon weapon;

    private bool moving;

    private TeddyAnimation anim;
    enum AIState
    {
        Idle,
        Attack
    }

    private AIState state;
	// Use this for initialization
	void Start ()
	{
	    anim = GetComponent<TeddyAnimation>();
	    agent = GetComponent<NavMeshAgent>();
	    player = GameObject.FindGameObjectWithTag("Player").transform;
        state = AIState.Idle;
        playerDistance = Vector3.Distance(transform.position, player.position);
        playerDirection = (transform.position - player.position) / playerDistance;
        weapon = GetComponent<Weapon>();
	    shootdistance = weapon.BulletPrefab.GetComponent<Bullet>().Distance;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (agent.hasPath)
	    {
            moving = true;
        }
	    else
	    {
            moving = false;
        }
	    
        switch (state)
	    {
	        case AIState.Idle:
                Idle();
	        break;

            case AIState.Attack:
                Attack();
            break;
	    }
	    anim.moving = moving;
	}


    void FixedUpdate()
    {
        playerDistance = Vector3.Distance(transform.position, player.position);
        reactionTimer += Time.fixedDeltaTime;
        if (reactionTimer > reactionTime && playerDistance <= sightDistance)
        {
            reactionTimer = 0;
            RaycastHit hit;
            playerDirection = (player.position - transform.position ) / playerDistance;
            //Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), playerDirection, Color.blue, 5f);
            if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), playerDirection, out hit, sightDistance))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    if (state == AIState.Idle)
                    {
                        agent.ResetPath();
                        state = AIState.Attack;
                    }
                    if (obstructed && state == AIState.Attack)
                    {
                        obstructionCleared();
                        //Debug.Log("Obstructed: " + obstructed);
                    }
                }
                else
                {
                    if (state == AIState.Attack)
                    {
                        obstructed = true;
                        //Debug.Log("Obstructed: " + obstructed);
                    }
                }
            }
        }
        
    }

    void Idle()
    {
        idleTimer += Time.deltaTime;
        if (idleTimer > idleTime)
        {
            idleTimer = 0;
            agent.SetDestination(RndPointInArea(5f, NavMesh.AllAreas));
        }
    }

    Vector3 RndPointInArea(float walkradius, int areaindex)
    {
        Vector3 direction = Random.insideUnitSphere* walkradius;
        direction += transform.position;
        NavMeshHit hit;
        int areamask = 1 << areaindex; 
        NavMesh.SamplePosition(direction, out hit, walkradius, areamask);
        return hit.position;
    }

    void obstructionCleared()
    {
        agent.ResetPath();
        obstructed = false;
        obstructionTimer = 0;
    }

    void Attack()
    {
        
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        if (obstructed)
        {
            moving = true;
            obstructionTimer += Time.deltaTime;
            if (obstructionTimer > obstructionHandlingTime)
            {
                obstructionCleared();
            }
            else
            {
                agent.SetDestination(player.position);
            }
        }
        else if (playerDistance > shootdistance)
        {
            
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, player.position, step);
            moving = true;
        }
        if(shootdistance > playerDistance)
        {
            weapon.Use();
        }
 
    }

    void OnCollisionEnter(Collision collision)
    {   
        if (state == AIState.Attack)
        {
            if (!collision.transform.CompareTag("Player") && !collision.transform.CompareTag("IsHit"))
            {
                obstructed = true;
                Debug.Log("Obstructed: " + obstructed);
            }
        }     
    }
}
