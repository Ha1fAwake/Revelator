using UnityEngine;
public struct EnemyStructure
{
    public Sprite profile;      // 外貌
    public string enemyName;    // 名字
    public int health;          // 血条
    public int attack;          // 攻击力
    public int defence;         // 防御力
    public float moveSpeed;     // 移动速度
    public float detectRadius;  // 探测距离
    public float attackRadius;  // 攻击距离
    public string description;  // 描述信息

    public void SetEnemyData(EnemyDataContainer enemyDataContainer)
    {
        profile = enemyDataContainer.profile;
        enemyName = enemyDataContainer.enemyName;
        health = enemyDataContainer.health;
        attack = enemyDataContainer.attack;
        defence = enemyDataContainer.defence;
        moveSpeed = enemyDataContainer.moveSpeed;
        detectRadius = enemyDataContainer.detectRadius;
        attackRadius = enemyDataContainer.attackRadius;
        description = enemyDataContainer.description;
    }
}
