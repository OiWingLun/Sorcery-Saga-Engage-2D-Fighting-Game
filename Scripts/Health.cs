using UnityEngine;

public class Health : MonoBehaviour
{
    public float health;
    public float maxHealth;

    public float block;
    public float maxBlock;
    public float blockRegenRate = 1f;

    private void Awake()
    {
        health = maxHealth;
        block = maxBlock;
    }

    private void Update()
    {

    }

    public void ReduceBlockOverTime(float amount)
    {
        block -= amount * Time.deltaTime;
        block = Mathf.Clamp(block, 0, maxBlock);
    }

    public void ReduceBlockOnHit(float amount)
    {
        block -= amount;
        block = Mathf.Clamp(block, 0, maxBlock);
    }

     public void RegenerateBlock(float amount)
    {
        block += amount;
        block = Mathf.Clamp(block, 0, maxBlock);
    }
}
