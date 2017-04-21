using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Script;
using Assets.Scripts.Weapon_Inventary;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{
    [Serializable]
    public struct ModeCombination
    {
        public MovementState mState;
        public AttackMode aState;
    }

    [Serializable]
    public struct ModeBullet
    {
        public AttackMode mode;
        public GameObject bullet;
    }

    [Serializable]
    public struct StageSettings
    {
        public float RotationSpeed;
        public float BulletSpeedModifier;
        public float SpeedModifier;
        public float ReloadSpeed;
        public int BulletStormTurns;
        public float BulletStormRotationSpeed;
        public float BulletStormBulletAngle;
        public int BulletStormBulletCount;

        public List<ModeCombination> ModeCombinations;
    }

    public List<StageSettings> Stages;
    public List<ModeBullet> ModeBullets;
    private BossAnimation animation;

    public enum MovementState
    {
        Idle,
        Follow,
        FindMiddle
    }

    public enum AttackMode
    {
        Homing,
        AimAndShoot,
        BulletStorm,
        Idle
    }
    public AttackMode AMode = AttackMode.AimAndShoot;
    public MovementState MMode = MovementState.Idle;

    private Stats stats;
    private NavMeshAgent agent;
    private Weapon weapon;
    private MultiBulletWeapon weapon2;

    private bool modeChanged = false;
    private bool modeComplete = false;
    private int stage = 0;

    private bool lockedToMoving = false;

    /* Find middle */
    public Transform CenterOfZone;
   

    /* BulletStorm */
    private float currRotation = 0f;
    private float rotationTarget = 360f;   
    private int turn = 0;

    /* mode timer */
    public float modeTime = 10f;
    public float modeTimer = 0f;

    private GameObject player;
    private int currentCombination = 0;
    private int stageCombinationCount = 0;
    private bool dead = false;

    // Use this for initialization
    void Start ()
	{
        animation = transform.FindDeepChild("Azazel").GetComponent<BossAnimation>();
        stats = GetComponent<Stats>();
        agent = GetComponent<NavMeshAgent>();
        weapon = GetComponent<Weapon>();
        weapon2 = GetComponent<MultiBulletWeapon>();
        player = GameObject.FindGameObjectWithTag("Player");
        ChangeState(AttackMode.Idle, MovementState.Idle);
	}
	
	// Update is called once per frame
	void Update ()
	{
        if(!dead)
            CheckStats();
        if (!dead)
        {
            if (modeComplete)
            {
                currentCombination++;
                if (currentCombination >= stageCombinationCount)
                    currentCombination = 0;
                ChangeState(Stages[stage].ModeCombinations[currentCombination].aState, Stages[stage].ModeCombinations[currentCombination].mState);
                modeComplete = false;
            }
            switch (MMode)
            {
                case MovementState.FindMiddle:
                    if (FindMiddle())
                    {
                        Attack();
                    }
                    else
                    {
                        if (modeChanged)
                        {
                            modeChanged = false;
                            animation.ChangeLowerState(BossAnimation.LBodyAnimationState.RunForward);
                        }
                    }
                    break;
                case MovementState.Follow:
                    if (modeChanged)
                    {
                        modeChanged = false;
                        animation.ChangeLowerState(BossAnimation.LBodyAnimationState.RunForward);
                    }
                    Attack();
                    break;
                case MovementState.Idle:
                    if (modeChanged)
                    {
                        modeChanged = false;
                        animation.ChangeUpperState(BossAnimation.UBodyAnimationState.Idle);
                        animation.ChangeLowerState(BossAnimation.LBodyAnimationState.Idle);
                    }
                    Attack();
                    break;
            }
        }

	}

    void Attack()
    {
        switch (AMode)
        {
            case AttackMode.AimAndShoot:
                ModeTime();
                animation.ChangeUpperState(BossAnimation.UBodyAnimationState.OneHanded);
                AimAndShoot();
                break;
            case AttackMode.BulletStorm:
                animation.ChangeUpperState(BossAnimation.UBodyAnimationState.TwoHanded);
                BulletStorm(UnityEngine.Random.Range(0, 2));
                break;
            case AttackMode.Homing:
                ModeTime();
                animation.ChangeUpperState(BossAnimation.UBodyAnimationState.OneHanded);
                Homing();
                break;
            case AttackMode.Idle:
                if (Vector3.Distance(transform.position, player.transform.position) < 10f)
                {
                    modeComplete = true;
                }
                break;
        }
    }

    void ModeTime()
    {
        modeTimer += Time.deltaTime;
        if (modeTimer > modeTime)
            modeComplete = true;
    }

    bool FindMiddle()
    {
        if (!lockedToMoving)
        {
            agent.SetDestination(CenterOfZone.position);
            lockedToMoving = true;
        }
        else if (agent.pathStatus == NavMeshPathStatus.PathComplete || agent.remainingDistance < 1f)
        {
            lockedToMoving = false;
            return true;
        }
        return false;
    }

    public void ChangeState(AttackMode aState, MovementState mState)
    {
        modeChanged = true;
        AMode = aState;
        MMode = mState;

        foreach (ModeBullet modeBullet in ModeBullets)
        {
            if (modeBullet.mode == AMode)
            {
                weapon.BulletPrefab = modeBullet.bullet;
                weapon.BulletPrefab.GetComponent<Bullet>().Speed *= Stages[stage].BulletSpeedModifier;
                weapon2.BulletPrefab = modeBullet.bullet;
                weapon2.BulletPrefab.GetComponent<Bullet>().Speed *= Stages[stage].BulletSpeedModifier;
                weapon2.AngleBetweenBullets = Stages[stage].BulletStormBulletAngle;
                weapon2.Number = Stages[stage].BulletStormBulletCount;
            }
        }
    }

    void BulletStorm(int mode)
    {
        if (mode == 0)
        {
            transform.Rotate(Vector3.up, Time.deltaTime * Stages[stage].BulletStormRotationSpeed);
            turn++;
            weapon.Use();
            if (turn >= Stages[stage].BulletStormTurns)
            {
                modeComplete = true;
            }
        }
        else if (mode == 1)
        {
            transform.Rotate(Vector3.up, Time.deltaTime * Stages[stage].BulletStormRotationSpeed);
            turn++;
            weapon2.Use();
            if (turn >= Stages[stage].BulletStormTurns)
            {
                modeComplete = true;
            }
        }
    }

    private void AimAndShoot()
    {
        if(RotateTowards(player.transform, Stages[stage].RotationSpeed))
        {
            weapon.Use();
        }
    }

    private void Homing()
    {
        if (RotateTowards(player.transform, Stages[stage].RotationSpeed))
        {
            weapon2.Use();
        }
    }



    void CheckStats()
    {
        if (stats.CurrentLifeEnergy < Double.Epsilon)
        {
            animation.Die();
            agent.Stop();
            agent.ResetPath();
            dead = true;
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().GameWin();
        }
        else if (stats.CurrentLifeEnergy < stats.LifeEnergy - stats.LifeEnergy / Stages.Count * (stage + 1))
        {
            NextStage();
        }
    }

    void NextStage()
    {
        
        stage++;
        stageCombinationCount = Stages[stage].ModeCombinations.Count;
        currentCombination = 0;
        ChangeState(Stages[stage].ModeCombinations[currentCombination].aState, Stages[stage].ModeCombinations[currentCombination].mState);
        agent.speed *= Stages[stage].SpeedModifier;
        weapon.ReloadTime = Stages[stage].ReloadSpeed;
        weapon2.ReloadTime = Stages[stage].ReloadSpeed;
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
        if (approx(transformy, looky, 10f))
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
}
