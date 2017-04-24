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
        public float reloadSpeed;
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
        public float LaserRotationSpeed;
        public float LaserEyeAngleAdjustSpeed;
        public int Minions;
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
        DeathWave,
        LaserEyes,
        SpawnMinions,
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
    private bool lasersOn = false;
    /* Find middle */
    public Transform CenterOfZone;
   

    /* BulletStorm */
    public float currRotation = 0f;
    public float Speed = 3f;
    private float rotationTarget = 360f;   
    private int turn = -1;

    public MinionSpawn[] MinionSpawns;
    public GameObject MinionPrefab;

    /* mode timer */
    public float modeTime = 10f;
    public float modeTimer = 0f;

    private GameObject player;
    private int currentCombination = 0;
    private int stageCombinationCount = 0;
    private bool dead = false;
    private float deathDestroyTimer = 0;
    private float deathDestroyTime = 3f;
    private float minionSpawnIdleTimer = 0f;
    private float minionSpawnIdleTime = 5f;
    private bool minionsSpawned = false;

    private LaserEyes eyes;

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
        stageCombinationCount = Stages[0].ModeCombinations.Count;
        eyes = transform.FindDeepChild("LaserEyes").GetComponent<LaserEyes>();

        GameObject area = GameObject.FindGameObjectWithTag("BossArea");
        GameObject[] MinionSpawnPoints = GameObject.FindGameObjectsWithTag("MinionSpawn");
        MinionSpawns = new MinionSpawn[MinionSpawnPoints.Length];
        int count = 0;
        foreach (GameObject go in MinionSpawnPoints)
        {
            MinionSpawns[count] = go.GetComponent<MinionSpawn>();
            count++;
        }

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
                    Follow();
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

	    if (dead && transform.root.CompareTag("Minion"))
	    {
	        deathDestroyTimer += Time.deltaTime;
	        if (deathDestroyTimer > deathDestroyTime)
	        {
                Transform t = transform;
                foreach (Transform tr in t)
                {
                    if (tr.tag == "BloodScript")
                    {
                        var bloodParticles = tr.GetComponent<ParticleSystem>();
                        bloodParticles.transform.parent = null;

                    }
                }
                Destroy(gameObject);
	        }
	    }

	}

    void Follow()
    {
        if (Vector3.Distance(player.transform.position, transform.position) > 5f)
        {
            float step = Speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
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
                animation.ChangeUpperState(BossAnimation.UBodyAnimationState.OneHanded);
                BulletStorm();
                break;
            case AttackMode.Homing:
                ModeTime();
                animation.ChangeUpperState(BossAnimation.UBodyAnimationState.OneHanded);
                Homing();
                break;
            case AttackMode.Idle:
                if (Vector3.Distance(transform.position, player.transform.position) < 10f)
                {
                    ChangeState(Stages[stage].ModeCombinations[currentCombination].aState, Stages[stage].ModeCombinations[currentCombination].mState);
                }
                break;
            case AttackMode.DeathWave:
                animation.ChangeUpperState(BossAnimation.UBodyAnimationState.TwoHanded);
                DeathWave();
                ModeTime();
                break;
            case AttackMode.LaserEyes:
                animation.ChangeUpperState(BossAnimation.UBodyAnimationState.Idle);
                Laser();
                break;
            case AttackMode.SpawnMinions:
                SpawnMinions();
                break;
        }
    }

    private void SpawnMinions()
    {
        if (!minionsSpawned)
        {
            int count = 0;
            for (int i = 0; i < Stages[stage].Minions; i++)
            {
                MinionSpawns[i].Spawn(MinionPrefab);
                count++;
                if (count > MinionSpawns.Length)
                {
                    break;
                }
            }
            minionsSpawned = true;
        }
        else
        {
            minionSpawnIdleTimer += Time.deltaTime;
            if (minionSpawnIdleTimer > minionSpawnIdleTime)
            {
                modeComplete = true;
                minionSpawnIdleTimer = 0f;
                minionsSpawned = false;
            }
        }
        
    }

    private void Laser()
    {
        if (!lasersOn)
        {
            transform.GetComponentInChildren<LaserEyes>().OpenEyes();
            lasersOn = false;
            if (MMode == MovementState.Follow)
            {
                agent.updateRotation = false;
            }
        }
        if (MMode == MovementState.FindMiddle)
        {
            Turn(Stages[stage].LaserRotationSpeed);
            if (turn > 3)
            {
                transform.GetComponentInChildren<LaserEyes>().CloseEyes();
                modeComplete = true;
            }
        }
        else
        {
            ModeTime();
        }
    }

    private void DeathWave()
    {
        if (RotateTowards(player.transform, Stages[stage].RotationSpeed))
        {
            weapon.Use();
        }
    }

    void ModeTime()
    {
        modeTimer += Time.deltaTime;
        if (modeTimer > modeTime)
        {
            modeComplete = true;
            modeTimer = 0f;
        }
            
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
            rotationTarget = transform.rotation.eulerAngles.y;
            return true;
        }
        return false;
    }

    public void ChangeState(AttackMode aState, MovementState mState)
    {
        if(aState == AttackMode.LaserEyes)
            eyes.speed = Stages[stage].LaserEyeAngleAdjustSpeed;
        modeChanged = true;
        AMode = aState;
        MMode = mState;
        turn = -1;
        agent.updateRotation = true;

        foreach (ModeBullet modeBullet in ModeBullets)
        {
            if (modeBullet.mode == AMode)
            {
                modeBullet.bullet.GetComponent<Bullet>().Speed *= Stages[stage].BulletSpeedModifier;
                weapon.ChangeBullet(modeBullet.bullet); 

                weapon2.AngleBetweenBullets = Stages[stage].BulletStormBulletAngle;
                weapon2.Number = Stages[stage].BulletStormBulletCount;
                if (Mathf.Approximately(modeBullet.reloadSpeed,-1))
                {
                    weapon.ReloadTime = Stages[stage].ReloadSpeed;
                    weapon2.ReloadTime = Stages[stage].ReloadSpeed;
                }
                else
                {
                    weapon.ReloadTime = modeBullet.reloadSpeed;
                    weapon2.ReloadTime = modeBullet.reloadSpeed;
                }
            }
        }
    }

    void Turn(float rotationspeed)
    {
        float rotation = Time.deltaTime*rotationspeed; 
        currRotation += rotation;
        transform.Rotate(Vector3.up, rotation);
        if (currRotation >= 360f)
        {
            turn++;
            currRotation = 0f;
        }
    }

    void BulletStorm()
    {
        if (MMode == MovementState.Follow)
        {
            agent.updateRotation = false;
        }
        Turn(Stages[stage].BulletStormRotationSpeed);
        weapon.Use();
            
        if (turn >= Stages[stage].BulletStormTurns)
        {
            modeComplete = true;
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
            weapon.Use();
        }
    }



    void CheckStats()
    {
        if (stats.CurrentLifeEnergy < Double.Epsilon)
        {
            animation.Die();
            agent.Stop();
            agent.ResetPath();
            if(AMode == AttackMode.LaserEyes)
            {
                eyes.CloseEyes();
            }
            dead = true;
            if(transform.root.CompareTag("Boss"))
                GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().GameWin();
        }
        else if (stats.CurrentLifeEnergy < stats.LifeEnergy - stats.LifeEnergy / Stages.Count * (stage + 1))
        {
            if(transform.root.CompareTag("Boss"))
                NextStage();
        }
    }

    void NextStage()
    {
        
        stage++;
        stageCombinationCount = Stages[stage].ModeCombinations.Count;
        currentCombination = 0;
        agent.speed *= Stages[stage].SpeedModifier;
        Speed *= Stages[stage].SpeedModifier;
        ChangeState(Stages[stage].ModeCombinations[currentCombination].aState, Stages[stage].ModeCombinations[currentCombination].mState);

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
