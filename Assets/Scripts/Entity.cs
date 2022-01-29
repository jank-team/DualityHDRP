using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class Entity : MonoBehaviour, IObservable<EntityEvent>
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
    public float lastHealth = 0;
    public float maxHealth = 100;
    public float baseAttack = 10;
    public float baseDefence = 0;
    public float baseMoveSpeed = 500.0f;
    public int baseAttackCooldown = 20;
    public int currentAttackCooldown = 0;
    public Weapon Weapon;
    public Armor Armor;
    public int resource = 0;
    public float balance = 100;
    public TaskType currentTask = TaskType.Idle;
    private Entity _currentTargetEntity;
    public GameObject currentTarget;

    public AudioClip attackClip;
    private AudioSource _audioSource;

    private Texture2D _healthBarTexture;

    private void Awake()
    {
        _healthBarTexture = new Texture2D(100, 20);
        GetComponentInChildren<Canvas>().GetComponentInChildren<RawImage>().texture = _healthBarTexture;
    }

    // Start is called before the first frame update
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        InvokeRepeating(nameof(TimedUpdate), 0, 0.1f);
        var healthPercent = (int) (currentHealth / maxHealth * _healthBarTexture.width);

        for (var y = 0; y < _healthBarTexture.height; ++y)
        {
            for (var x = 0; x < healthPercent; ++x)
            {
                _healthBarTexture.SetPixel(x, y, Color.green);
            }
        }

        for (var y = 0; y < _healthBarTexture.height; ++y)
        {
            for (var x = healthPercent; x < _healthBarTexture.width; ++x)
            {
                _healthBarTexture.SetPixel(x, y, Color.grey);
            }
        }

        _healthBarTexture.Apply();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Math.Abs(currentHealth - lastHealth) > float.Epsilon)
        {
            var healthPercent = (int) (currentHealth / maxHealth * _healthBarTexture.width);

            for (var y = 0; y < _healthBarTexture.height; ++y)
            {
                for (var x = 0; x < healthPercent; ++x)
                {
                    _healthBarTexture.SetPixel(x, y, Color.green);
                }
            }

            for (var y = 0; y < _healthBarTexture.height; ++y)
            {
                for (var x = healthPercent; x < _healthBarTexture.width; ++x)
                {
                    _healthBarTexture.SetPixel(x, y, Color.grey);
                }
            }

            _healthBarTexture.Apply();
            lastHealth = currentHealth;
        }
    }

    private void FixedUpdate()
    {
        DoTask();
    }

    private void TimedUpdate()
    {
        if (currentAttackCooldown > 0)
        {
            currentAttackCooldown--;
        }
    }

    private float GetAttack()
    {
        return baseAttack + (Weapon?.Attack ?? 0);
    }

    private float GetDefence()
    {
        return baseDefence + (Armor?.Defence ?? 0);
    }

    private float GetMoveSpeed()
    {
        return baseMoveSpeed;
    }

    private void Attack()
    {
        if (!currentTarget) return;
        if (currentAttackCooldown != 0) return;

        _audioSource.PlayOneShot(attackClip);

        // if (!((currentTarget.transform.position - transform.position).sqrMagnitude <= Weapon.AttackRange)) return;
        currentAttackCooldown = baseAttackCooldown + (Weapon?.AttackCooldown ?? 0);
        _currentTargetEntity.currentHealth -= GetAttack() + _currentTargetEntity.GetDefence();

        if (_currentTargetEntity.occupation == Occupation.Resource)
        {
            resource = Convert.ToInt32(GetAttack());
        }

        if (_currentTargetEntity.currentHealth <= 0)
        {
            currentTarget = null;
            _currentTargetEntity.Kill();
        }
    }

    private void Kill()
    {
        NotifyObservers(new EntityEvent()
        {
            EventType = EntityEventType.Death,
            Occupation = occupation
        });
        Destroy(gameObject);
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
                // transform.position = Vector3.MoveTowards(transform.position,
                //     GameManager.Instance.Town.transform.position, GetMoveSpeed());
                var direction = (GameManager.Instance.Town.transform.position - transform.position).normalized;
                GetComponent<Rigidbody>()
                    .MovePosition(transform.position + direction * Time.deltaTime * GetMoveSpeed());
                switch (occupation)
                {
                    case Occupation.Player:
                    case Occupation.Worker:
                    {
                        GameManager.Instance.Town.GetComponent<Town>().balance += resource;
                        resource = 0;

                        if (occupation == Occupation.Player){
                            UpgradeItems();
                        }

                    }
                        break;
                    case Occupation.Monster:
                    {
                        // @todo attack
                    }
                        break;
                    case Occupation.Resource:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
                break;
            case TaskType.GotoTarget:
            {
                if (!currentTarget)
                {
                    FindTarget();
                }

                if (!currentTarget)
                {
                    currentTask = TaskType.Idle;
                    break;
                }

                var targetPosition = currentTarget.transform.position;

                // transform.position = Vector3.MoveTowards(transform.position,
                //     targetPosition, GetMoveSpeed());
                var direction = (targetPosition - transform.position).normalized;
                GetComponent<Rigidbody>()
                    .MovePosition(transform.position + direction * Time.deltaTime * GetMoveSpeed());

                var distance = Vector3.Distance(transform.position, targetPosition);
                if (distance < 2)
                {
                    currentTask = TaskType.AttackTarget;
                }
            }
                break;
            case TaskType.AttackTarget:
            {
                if (!currentTarget)
                {
                    currentTask = TaskType.Idle;
                    break;
                }

                var targetPosition = currentTarget.transform.position;
                var distance = Vector3.Distance(transform.position, targetPosition);
                if (distance > 2)
                {
                    currentTask = TaskType.GotoTarget;
                }

                Attack();
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
                var nearestTarget = FindNearestTarget();
                var targetDistance = nearestTarget
                    ? Vector3.Distance(position, nearestTarget.transform.position)
                    : float.MaxValue;
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
        _currentTargetEntity = currentTarget.GetComponent<Entity>();
    }

    private void UpgradeItems(){

        Building weaponSmith = GameManager.Instance.WeaponSmith.GetComponent<Building>();
        
        Building armorSmith = GameManager.Instance.ArmorSmith.GetComponent<Building>();

        Town town = GameManager.Instance.Town.GetComponent<Town>();

        Weapon = (Weapon)weaponSmith
            .GetItems().Where(arg => arg.Value < town.balance/2)
            .OrderByDescending(item => item.Value).First();

        Armor = (Armor)armorSmith.GetItems()
            .Where(arg => arg.Value < town.balance/2)
            .OrderByDescending(item => item.Value).First();
        
        town.balance = town.balance - Weapon.Value - Armor.Value;
        
    }

    private List<IObserver<EntityEvent>> _observers = new List<IObserver<EntityEvent>>();

    private void NotifyObservers(EntityEvent entityEvent)
    {
        _observers.ForEach(observer => observer.OnNext(entityEvent));
    }
    
    public IDisposable Subscribe(IObserver<EntityEvent> observer)
    {
        _observers.Add(observer);
        return new Unsubscriber(_observers, observer);
    }

    public string GetDisplayName()
    {
        switch (occupation)
        {
            case Occupation.Player:
                return "Adventurer";
            default:
                return occupation.ToString();
        }
    }

    public string GetDescription()
    {
        switch (currentTask)
        {
            case TaskType.Idle:
                return "Idle";
            case TaskType.GotoTown:
                if (occupation == Occupation.Player) return "Returning to town";
                return "Attacking town";
            case TaskType.GotoTarget:
                return $"Engaging {_currentTargetEntity.GetDisplayName()}";
            case TaskType.AttackTarget:
                return $"Attacking {_currentTargetEntity.GetDisplayName()}";
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    private class Unsubscriber : IDisposable
    {
        private List<IObserver<EntityEvent>>_observers;
        private IObserver<EntityEvent> _observer;

        public Unsubscriber(List<IObserver<EntityEvent>> observers, IObserver<EntityEvent> observer)
        {
            _observers = observers;
            _observer = observer;
        }

        public void Dispose()
        {
            if (_observer != null && _observers.Contains(_observer))
                _observers.Remove(_observer);
        }
    }
}

public class EntityEvent
{
    public EntityEventType EventType { get; set; }
    public Occupation Occupation { get; set; }
}

public enum EntityEventType
{
    Death
}