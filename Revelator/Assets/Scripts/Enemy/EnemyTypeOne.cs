using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class EnemyTypeOne : MonoBehaviour, IEnemy
{
    public EnemyDataContainer dataContainer;            // 拖入引用，存储信息

    public EnemyStructure enemyIdentity;                // 不能直接修改ScriptableObject上的数据
    private CircleCollider2D detectArea;                // 检测（玩家）范围
    private BoxCollider2D attackArea;                   // 攻击距离

    private void Start()
    {
        enemyIdentity.SetEnemyData(dataContainer);
        detectArea = this.GetComponent<CircleCollider2D>();
        detectArea.isTrigger = true;                    // 仅用作检测
        detectArea.radius = dataContainer.detectRadius; // 初始化检测半径
        attackArea = this.GetComponent<BoxCollider2D>();
        attackArea.isTrigger = false;
        attackArea.edgeRadius = dataContainer.attackRadius;
    }

    private void Update()
    {
        Die();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {// 还是得将探测范围或攻击范围作为敌人的子物体才行，不过具体手感等测试吧，现在先完成玩家的攻击，敌人都当靶子用
        if (collision.tag == "Player")
        {
            ChasePlayer();
        }
    }

    public void AttackPlayer()
    {
        throw new System.NotImplementedException();
    }

    public void ChasePlayer()
    {

    }

    public void Die()
    {
        if (enemyIdentity.health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void OnDamage(int damage)
    {
        enemyIdentity.health = enemyIdentity.health - (damage - enemyIdentity.defence);
    }
}
