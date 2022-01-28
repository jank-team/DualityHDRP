using UnityEngine;

public class Entity : MonoBehaviour
{
    public double currentHealth = 50;
    public double maxHealth = 100;
    public double baseAttack = 10;
    public double baseDefence = 0;

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
        return baseAttack;
    }

    private double GetDefence()
    {
        return baseDefence;
    }

    private void Attack(Entity target)
    {
        target.currentHealth -= GetAttack() + target.GetDefence();
    }
}