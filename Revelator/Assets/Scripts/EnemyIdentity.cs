using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdentity : MonoBehaviour
{
    public static int damaged = 0;          // 由PlayerAttacker传入伤害量

    public EnemyDataContainer enemy;

    private void Update()
    {
        DamagedCaculator();
        Die();
    }

    public void DamagedCaculator()
    {
        enemy.health -= damaged;
    }

    public void Die()
    {
        if (enemy.health <= 0)
        {
            Destroy(gameObject);
        }
    }

}
