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
        switch (occupation)
        {
            case Occupation.Player:
            {
                // @todo find monster => attack monster => find town => tribute loot => buy weapon/armor => repeat
                switch (currentTask)
                {
                    case TaskType.Idle:
                    {
                        // @todo find new task
                        FindTask();
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

                        transform.position = Vector3.MoveTowards(transform.position,
                            currentTarget.transform.position, GetMoveSpeed());
                    }
                        break;
                    case TaskType.AttackTarget:
                    {
                    }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
                break;
            case Occupation.Monster:
            {
                // @todo find player => attack player => find town => attack town
                switch (currentTask)
                {
                    case TaskType.Idle:
                    {
                        // @todo find new task
                        FindTask();
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

                        transform.position = Vector3.MoveTowards(transform.position,
                            currentTarget.transform.position, GetMoveSpeed());
                    }
                        break;
                    case TaskType.AttackTarget:
                    {
                    }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
                break;
            case Occupation.Worker:
            {
                // @todo find resource => pick resource => back to town => repeat
                switch (currentTask)
                {
                    case TaskType.Idle:
                    {
                        // @todo find new task
                        FindTask();
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

                        transform.position = Vector3.MoveTowards(transform.position,
                            currentTarget.transform.position, GetMoveSpeed());
                    }
                        break;
                    case TaskType.AttackTarget:
                    {
                    }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
                break;
            case Occupation.Resource:
            {
                // @todo wait to be killed
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
            {
                if (false) // @todo town in range
                {
                    currentTask = TaskType.Idle;
                }
                else if (resource > 0)
                {
                    currentTask = TaskType.GotoTown;
                }
                else if (false) // @todo has monster in range
                {
                    currentTask = TaskType.AttackTarget;
                }
                else
                {
                    currentTask = TaskType.GotoTarget;
                }
            }
                break;
            case Occupation.Monster:
            {
                if (false) // @todo player in range
                {
                    currentTask = TaskType.AttackTarget;
                }
                else if (false) // @todo town is closer
                {
                    currentTask = TaskType.GotoTown;
                }
                else // @todo player is closer
                {
                    currentTask = TaskType.GotoTarget;
                }
            }
                break;
            case Occupation.Worker:
            {
                if (false) // @todo town in range
                {
                    currentTask = TaskType.Idle;
                }
                else if (resource > 0)
                {
                    currentTask = TaskType.GotoTown;
                }
                else if (false) // @todo resource in range
                {
                    currentTask = TaskType.AttackTarget;
                }
                else
                {
                    currentTask = TaskType.GotoTarget;
                }
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

    private void FindTarget()
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

        currentTarget = GameObject.FindGameObjectsWithTag(targetTag)
            .OrderBy(o => (o.transform.position - position).sqrMagnitude)
            .FirstOrDefault();
    }
}