using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Author: 圈毛君
/// </summary>
public class PlayerAttacker : MonoBehaviour
{
    //public PlayerController playerController;
    public int normalDamage = 5;           // 普通攻击伤害，每段攻击伤害按照不同百分比
    public int specialDamage = 8;          // 特殊攻击伤害
    public float noramlDmgDistance = 1.0f;      // 普通攻击距离，暂且只设置一个距离，用于测试
    public float specialDmgDistance = 5.0f;     // 特殊攻击——瞬斩
    public float normalAtkInterval = 0.1f;      // 普攻衔接最大时间间隔
    public float lastNormalAtkTime = 0.0f;      // 上一次普攻时间

    private int normalAtkCounter = 0;             // 普攻计数器
    private int specialAtkCounter = 0;            // 特攻计数器

    private void Start()
    {
        
    }

    private void Update()
    {
        NormalAttack();
        SpecialAttack();

        //if (playerController.FaceDirection())
        //{
        //    Debug.Log("right");
        //}
        //else
        //{
        //    Debug.Log("left");
        //}
    }

    private void NormalAttack()
    {// 普通攻击
        if (Input.GetKeyDown(KeyCode.J))
        {
            normalAtkCounter++;
        }
        OnceAttack();
        TwiceAttack();
        EndAttack();
    }

    private void OnceAttack()
    {// 第一段普攻
        if (normalAtkCounter == 1)
        {
            lastNormalAtkTime = Time.time;
        }
    }

    private void TwiceAttack()
    {// 第二段普攻
        if (normalAtkCounter == 2 && Time.time - lastNormalAtkTime <= normalAtkInterval)
        {

        }
    }

    private void EndAttack()
    {// 第三段普攻
        if (normalAtkCounter == 3 && Time.time - lastNormalAtkTime <= normalAtkInterval)
        {

        }
    }

    private void SpecialAttack()
    {// 特殊攻击
        if (Input.GetKeyDown(KeyCode.U))
        {

        }
    }
}
