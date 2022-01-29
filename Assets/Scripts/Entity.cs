using System;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public enum TaskType
    {
        Idle,
        GotoTown,
        GotoTarget,
        AttackTarget,
    }

    public Occupation occupation;
    public float currentHealth = 50;
    public float maxHealth = 100;
    public float baseAttack = 10;
    public float baseDefence = 0;
    public float baseMoveSpeed = 0.05f;
    public Weapon Weapon;
    public Armor Armor;
    public int resource = 0;
    public float balance = 100;
    public TaskType currentTask = TaskType.Idle;
    public GameObject currentTarget;

    // Start is called before the first frame update
    private void Start()
    {
        InvokeRepeating(nameof(TimedUpdate), 0, 1.0f);
    }

    // Update is called once per frame
    private void Update()
    {
        DoTask();
    }

    private void TimedUpdate()
    {
    }

    private float GetAttack()
    {
        return baseAttack + Weapon?.Attack ?? 0;
    }

    private float GetDefence()
    {
        return baseDefence + Armor?.Defence ?? 0;
    }

    private float GetMoveSpeed()
    {
        return baseMoveSpeed;
    }

    private void Attack(Entity target)
    {
        float nextDamageEvent = 0.0f;

        if (((target.transform.position - this.transform.position).sqrMagnitude) <= this.Weapon.AttackRange){

            if(Time.time >= nextDamageEvent){
                nextDamageEvent = Time.time + this.Weapon.AttackSpeed;
                target.currentHealth -= GetAttack() + target.GetDefence();

                if (target.currentHealth <= 0)
                {
                    target.Kill();
                }
            }else{
                nextDamageEvent = Time.time + this.Weapon.AttackSpeed;
            }          
        }
    }

    private void Kill()
    {
    }

    private void DoTask()
    {
        // @todo player: find monster => attack monster => find town => tribute loot => buy weapon/armor => repeat
        // @todo monster: find player => attack player => find town => attack town
        // @todo worker: find resource => pick resource => back to town => repeat
        // @todo resource: wait to be killed
        switch (currentTask)
        {
            case TaskType.Idle:
            {
                // @todo find new task
                if (occupation != Occupation.Resource)
                {
                    FindTask();
                }
            }
                break;
            case TaskType.GotoTown:
            {
                transform.position = Vector3.MoveTowards(transform.position,
                    GameManager.Instance.Town.transform.position, GetMoveSpeed());
            }
                break;
            case TaskType.GotoTarget:
            {
                if (!currentTarget)
                {
                    FindTarget();
                }

                var targetPosition = currentTarget.transform.position;

                transform.position = Vector3.MoveTowards(transform.position,
                    targetPosition, GetMoveSpeed());

                var distance = Vector3.Distance(transform.position, targetPosition);
                if (distance < 2)
                {
                    currentTask = TaskType.AttackTarget;
                }
            }
                break;
            case TaskType.AttackTarget:
            {
                var targetPosition = currentTarget.transform.position;
                var distance = Vector3.Distance(transform.position, targetPosition);
                if (distance > 2)
                {
                    currentTask = TaskType.GotoTarget;
                }
                // @todo attack
            }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void FindTask()
    {
        switch (occupation)
        {
            case Occupation.Player:
            case Occupation.Worker:
            {
                currentTask = resource > 0 ? TaskType.GotoTown : TaskType.GotoTarget;
            }
                break;
            case Occupation.Monster:
            {
                var position = transform.position;
                var townDistance = Vector3.Distance(position, GameManager.Instance.Town.transform.position);
                var targetDistance = Vector3.Distance(position, FindNearestTarget().transform.position);
                currentTask = townDistance < targetDistance ? TaskType.GotoTown : TaskType.GotoTarget;
            }
                break;
            case Occupation.Resource:
            {
            }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private GameObject FindNearestTarget()
    {
        var position = transform.position;
        string targetTag;
        switch (occupation)
        {
            case Occupation.Player:
                targetTag = "Monster";
                break;
            case Occupation.Monster:
                targetTag = "Player";
                break;
            case Occupation.Worker:
                targetTag = "Resource";
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return GameObject.FindGameObjectsWithTag(targetTag)
            .OrderBy(o => (o.transform.position - position).sqrMagnitude)
            .FirstOrDefault();
    }

    private void FindTarget()
    {
        currentTarget = FindNearestTarget();
    }
}