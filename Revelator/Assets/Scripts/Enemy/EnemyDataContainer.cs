using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "MyScripatbleObjects/Enemy")]
public class EnemyDataContainer : ScriptableObject
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
}
