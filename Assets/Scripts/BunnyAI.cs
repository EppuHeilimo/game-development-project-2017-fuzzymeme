using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BunnyAI : AI {


    private BunnyAnimation anim;
    private bool Standing = false;
    private bool Jump = false;
    private BunnyAnimation.StandingState standingState = BunnyAnimation.StandingState.Idle;
    public bool locked = false;
    // Use this for initialization
    void Start()
    {
        anim = GetComponent<BunnyAnimation>();
        anim.Init(Speed, this);
        Init();
        idleTime = Random.Range(idleTimeMin, idleTimeMax);
    }

    // Update is called once per frame
    void Update()
    {
        if (!locked)
        {
            StateUpdate();
        }

        anim.Standing = Standing;
        anim.Jump = Jump;
        anim.StandState = standingState;
    }

    protected override void Seek()
    {
        Standing = true;
        standingState = BunnyAnimation.StandingState.RunForward;
        RotateTowards(player, RotationSpeed);
        if (obstructed)
        {
            obstructionTimer += Time.deltaTime;
            if (obstructionTimer > obstructionHandlingTime)
            {
                ObstructionCleared();
            }
            else
            {
                agent.SetDestination(player.position);
                agent.speed = 3.5f;
            }
        }
        else if (playerDistance > shootdistance)
        {
            float step = Speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, player.position, step);
        }
        else
        {
            state = AIState.Attack;
        }
    }

    protected override void Idle()
    {
        Standing = false;
        idleTimer += Time.deltaTime;
        if (idleTimer > idleTime)
        {
            idleTimer = 0;
            agent.SetDestination(RndPointInArea(5f, transform.position));
            Jump = true;
            idleTime = Random.Range(idleTimeMin, idleTimeMax);
            agent.speed = 5f;
        }
        else if (agent.pathStatus == NavMeshPathStatus.PathComplete || agent.remainingDistance < 1f)
        {
            Jump = false;
            agent.speed = 3.5f;
        }
    }

    protected override void Attack()
    {
        if (shootdistance > playerDistance &&  RotateTowards(player, RotationSpeed))
        {
            weapon.Use();
            //standingState = BunnyAnimation.StandingState.Idle;
            CirclePlayer();
        }
        else
        {
            state = AIState.Seek;
            agent.updateRotation = true;
        }

    }

    private void CirclePlayer()
    {
        agent.updateRotation = false;
        if (agent.remainingDistance < 0.3f || !agent.hasPath)
            agent.SetDestination(RndPointInCircle(shootdistance, player.position));


    }
}
