using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TeddyAI : AI {

    private TeddyAnimation anim;
    // Use this for initialization
    void Start () {
        anim = GetComponent<TeddyAnimation>();
        anim.Init(Speed);
        Init();
        idleTime = Random.Range(idleTimeMin, idleTimeMax);
    }


    // Update is called once per frame
    void Update()
    {
        if (agent.hasPath)
        {
            moving = true;
        }
        else
        {
            moving = false;
        }
        StateUpdate();
        anim.moving = moving;

    }

    protected override void Seek()
    {
        RotateTowards(player, RotationSpeed);
        if (obstructed)
        {
            moving = true;
            obstructionTimer += Time.deltaTime;
            if (obstructionTimer > obstructionHandlingTime)
            {
                ObstructionCleared();
            }
            else
            {
                agent.SetDestination(player.position);
            }
        }
        else if (playerDistance > shootdistance)
        {
            float step = Speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, player.position, step);
            moving = true;
        }
        else
        {
            state = AIState.Attack;
        }
    }

    protected override void Idle()
    {
        idleTimer += Time.deltaTime;
        if (idleTimer > idleTime)
        {
            idleTime = Random.Range(idleTimeMin, idleTimeMax);
            idleTimer = 0;
            agent.SetDestination(RndPointInArea(5f, transform.position));
        }
    }

    protected override void Attack()
    {
        if (shootdistance > playerDistance && RotateTowards(player, RotationSpeed))
        {
            weapon.Use();
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
        if (agent.remainingDistance < 1f || !agent.hasPath)
            agent.SetDestination(RndPointInCircle(shootdistance, player.position));


    }
}
