//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class NormalAttack : MonoBehaviour
//{
//    public int atkDamage = 1;

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (collision.tag == "Enemy")
//        {
//            if(GetComponentInParent<PlayerController>().normalAtkCounter == 1)
//                collision.GetComponent<EnemyTypeOne>().OnDamage(atkDamage);     // 普攻一下
//            if(GetComponentInParent<PlayerController>().normalAtkCounter == 2)
//                collision.GetComponent<EnemyTypeOne>().OnDamage(atkDamage + 1);     // 普攻两下
//            if (GetComponentInParent<PlayerController>().normalAtkCounter == 3)
//                collision.GetComponent<EnemyTypeOne>().OnDamage(atkDamage + 2);     // 普攻三下
//            Debug.Log(collision.GetComponent<EnemyTypeOne>().enemyIdentity.health);
//        }
//    }
//}
