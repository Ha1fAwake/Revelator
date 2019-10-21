public interface IEnemy
{
    void ChasePlayer();             // 追击玩家
    void AttackPlayer();            // 攻击玩家
    void OnDamage(int damage);      // 受伤
    void Die();                     // 死亡
}
