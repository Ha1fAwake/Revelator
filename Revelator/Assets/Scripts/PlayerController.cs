using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Author: 圈毛君
/// </summary>
public class PlayerController : MonoBehaviour
{
    public static int faceDirection;        // 角色朝向，0左1右

    //public float walkSpeed = 3.0f;         // 步行速度
    //public float runSpeed = 5.0f;          // 跑步速度
    public float pressInterval = 0.5f;      // 双击按键的有效时间间隔
    public float exStepDistance = 3.0f;     // 瞬步距离（速度)
    public float exStepCD = 5.0f;           // 瞬步技能CD
    public float lastExStepTime = 0.0f;     // 记录上一次使用瞬步的时间
    public float jumpForce_y = 5.0f;        // 弹跳力
    public float floatForce_x = 1.0f;       // 空中微调的漂浮力度（速度不能与重力叠加）
    public float walkForce_x = 3.0f;        // 步行力度
    public float runForce_x = 5.0f;         // 跑步力度
    public float maxWalkSpeed = 3.0f;       // 最高步行速度
    public float maxRunSpeed = 5.0f;        // 最高跑步速度
    public float maxFloatSpeed = 2.0f;      // 最大腾空微调速度

    private Rigidbody2D rgb;
    private SpriteRenderer sr;
    private Color originColor;
    private float pressATime;               // 按下A键时间
    private float pressDTime;
    private float releaseATime = .0f;       // 松开A键时间
    private float releaseDTime = .0f;
    private bool exStepEnabled = true;      // 能否使用瞬步
    private bool onFloor = true;            // 角色是否在地上
    private int jumpTimer = 0;              // 跳跃计数器

    private void Awake()
    {
        rgb = this.GetComponent<Rigidbody2D>();
        sr = this.GetComponent<SpriteRenderer>();
        originColor = sr.color;
    }

    private void Update()
    {
        MoveController();                           // 移动控制
        JumpController();                           // 跳跃控制
    }

    public void MoveController()
    {
        if (Input.GetKeyUp(KeyCode.A))              // 不能||D，否则快速AD也算双击有效时间
        {
            releaseATime = Time.time;
            sr.color = originColor;
            rgb.velocity = new Vector2(0, 0);       // Stop
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            releaseDTime = Time.time;
            sr.color = originColor;
            rgb.velocity = new Vector2(0, 0);
        }

        if (Input.GetKey(KeyCode.A))
        {
            faceDirection = 0;  // 朝向左

            if (onFloor)
            {// 地面上才允许步行、跑步和瞬步
                if (Input.GetKeyDown(KeyCode.LeftShift) && exStepEnabled && Time.time - lastExStepTime >= exStepCD)
                {
                    //sr.color = new Color(1, 1, 0);                  // 暂用变色来表示瞬步冷却时间
                    float tempStepDistance = exStepDistance;        // 临时瞬步距离
                    Vector3 temp = transform.position;
                    RaycastHit2D hit = Physics2D.Raycast(new Vector2(temp.x - 0.6f, temp.y), new Vector2(-1, 0), exStepDistance);   // 使用Raycast解决穿墙问题
                    if (hit)
                    {// 仅通过Raycast判断还不能解决贴墙穿越的问题，需要在碰撞事件中进一步处理
                        tempStepDistance = hit.distance;            // 重新赋值瞬步距离
                    }
                    transform.position = new Vector3(-tempStepDistance + temp.x, temp.y, temp.z);    // 虽然可以瞬步，但要解决穿墙的BUG
                    lastExStepTime = Time.time;                     // 重置瞬步使用时间
                }

                pressATime = Time.time;
                if (pressATime - releaseATime <= pressInterval)
                {
                    //rgb.velocity = new Vector2(-runSpeed, 0);       // 跑步
                    if(-rgb.velocity.x < maxRunSpeed)               // 不能超过最大跑步速度
                        rgb.AddForce(new Vector2(-runForce_x, 0));
                    sr.color = new Color(1, 0, 0);                  // 跑步状态（红色）
                    releaseATime = Time.time;                       // 跑步状态中要持续更新松键时间
                }
                else
                {
                    //rgb.velocity = new Vector2(-walkSpeed, 0);      // 步行
                    if(-rgb.velocity.x < maxWalkSpeed)              // 不能超过最大步行速度
                        rgb.AddForce(new Vector2(-walkForce_x, 0));
                    //if (Time.time - lastExStepTime >= exStepCD)
                        sr.color = originColor;                     // 若不在瞬步CD和跑步状态，显示原色
                }
            }

            if (!onFloor && -rgb.velocity.x < maxFloatSpeed)        // 不能超过最大微调速度
            {// 腾空时允许空中左右微调
                rgb.AddForce(new Vector2(-floatForce_x, 0));
            }
        }

        if (Input.GetKey(KeyCode.D))
        {
            faceDirection = 1;  // 朝向右

            if (onFloor)
            {
                if (Input.GetKeyDown(KeyCode.LeftShift) && exStepEnabled && Time.time - lastExStepTime >= exStepCD)
                {
                    //sr.color = new Color(1, 1, 0);
                    float tempStepDistance = exStepDistance;
                    Vector3 temp = transform.position;
                    RaycastHit2D hit = Physics2D.Raycast(new Vector2(temp.x + 0.6f, temp.y), new Vector2(1, 0), exStepDistance);
                    if (hit)
                    {
                        tempStepDistance = hit.distance;
                    }
                    transform.position = new Vector3(tempStepDistance + temp.x, temp.y, temp.z);
                    lastExStepTime = Time.time;
                }

                pressDTime = Time.time;
                if (pressDTime - releaseDTime <= pressInterval)
                {
                    //rgb.velocity = new Vector2(runSpeed, 0);
                    if (rgb.velocity.x < maxRunSpeed)
                        rgb.AddForce(new Vector2(runForce_x, 0));
                    sr.color = new Color(1, 0, 0);
                    releaseDTime = Time.time;
                }
                else
                {
                    //rgb.velocity = new Vector2(walkSpeed, 0);
                    if (rgb.velocity.x < maxWalkSpeed)
                        rgb.AddForce(new Vector2(walkForce_x, 0));
                    //if (Time.time - lastExStepTime >= exStepCD)
                        sr.color = originColor;
                }
            }

            if (!onFloor && rgb.velocity.x < maxFloatSpeed)
            {
                rgb.AddForce(new Vector2(floatForce_x, 0));
            }
        }
    }

    public void JumpController()
    {
        if (Input.GetKeyDown(KeyCode.K) && jumpTimer < 2)
        {// 跳跃及二段跳
            onFloor = false;
            jumpTimer++;
            if(jumpTimer == 2)
            {
                //transform.Rotate(new Vector3(0, 0, 1), 120);    // 前空翻360度（播放前空翻动画）
                sr.color = new Color(0, 1, 0.5f);
            }
            rgb.AddForce(new Vector2(0, jumpForce_y), ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag != "Floor")    // 某些物体的碰撞不可计算在内
        {
            exStepEnabled = false;
        }
        if (collision.gameObject.tag == "Floor")
        {
            onFloor = true;
            jumpTimer = 0;
            sr.color = originColor;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Floor")
        {
            exStepEnabled = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Floor")
        {
            exStepEnabled = true;
        }
    }
}
