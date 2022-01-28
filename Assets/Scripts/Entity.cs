using System;
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
    public double currentHealth = 50;
    public double maxHealth = 100;
    public double baseAttack = 10;
    public double baseDefence = 0;
    [CanBeNull] public Weapon Weapon;
    [CanBeNull] public Armor Armor;
    public int resource = 0;
    public double balance = 100;
    public TaskType currentTask = TaskType.Idle;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private double GetAttack()
    {
        return baseAttack + Weapon?.Attack ?? 0;
    }

    private double GetDefence()
    {
        return baseDefence + Armor?.Defence ?? 0;
    }

    private void Attack(Entity target)
    {
        target.currentHealth -= GetAttack() + target.GetDefence();
        if (target.currentHealth <= 0)
        {
            target.Kill();
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
                    }
                        break;
                    case TaskType.GotoTarget:
                    {
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
                    }
                        break;
                    case TaskType.GotoTarget:
                    {
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
                    }
                        break;
                    case TaskType.GotoTarget:
                    {
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
}