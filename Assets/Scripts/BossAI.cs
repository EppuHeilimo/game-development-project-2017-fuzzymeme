using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Script;
using Assets.Scripts.Weapon_Inventary;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{
    private BossAnimation animation;
    public Transform CenterOfZone;
    enum MovementState
    {
        Idle,
        Follow,
        FindMiddle
    }

    enum AttackMode
    {
        Homing,
        AimAndShoot,
        BulletStorm
    }

    private Stats stats;
    private AttackMode aMode = AttackMode.AimAndShoot;
    private MovementState mMode = MovementState.Idle;
    private NavMeshAgent agent;

    
    private int stage = 1;
    

    private float attackModeTime = 0f;
    private float bulletSpeedModifier = 1f;


    public float BulletSpeedAdditionPerStage = 1f;
    public int StageCount = 3;

    // Use this for initialization
    void Start ()
	{
        animation = transform.FindDeepChild("Azazel").GetComponent<BossAnimation>();
        stats = GetComponent<Stats>();
        agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (stats.CurrentLifeEnergy < Double.Epsilon)
	    {
	        animation.Die();
            agent.Stop();
            agent.ResetPath();
	    }
	    else if (stats.CurrentLifeEnergy < stats.LifeEnergy - stats.LifeEnergy / StageCount * stage)
	    {
	        NextStage();
	    }
	}

    void NextStage()
    {
        stage++;
        bulletSpeedModifier += BulletSpeedAdditionPerStage;
    }
}
