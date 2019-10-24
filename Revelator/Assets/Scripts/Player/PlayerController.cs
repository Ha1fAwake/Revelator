using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Author: 圈毛君
/// </summary>
public class PlayerController : MonoBehaviour
{
    #region 移动控制变量
    public float pressInterval = 0.5f;      // 双击按键的有效时间间隔
    public float exStepDistance = 3.0f;     // 瞬步距离（速度)
    public float exStepCD = 5.0f;           // 瞬步技能CD
    public float jumpForce_y = 5.0f;        // 弹跳力
    public float floatForce_x = 1.0f;       // 空中微调的漂浮力度（速度不能与重力叠加）
    public float walkForce_x = 3.0f;        // 步行力度
    public float runForce_x = 5.0f;         // 跑步力度
    public float maxWalkSpeed = 3.0f;       // 最高步行速度
    public float maxRunSpeed = 5.0f;        // 最高跑步速度
    public float maxFloatSpeed = 2.0f;      // 最大腾空微调速度

    private bool positiveFace;              // 是否朝着正向（右为正）
    private Rigidbody2D rgb;
    private Animator anim;
    private SpriteRenderer sr;
    private Color originColor;
    private float pressATime;               // 按下A键时间
    private float pressDTime;
    private float releaseATime = .0f;       // 松开A键时间
    private float releaseDTime = .0f;
    private bool exStepEnabled = true;      // 能否使用瞬步
    private float lastExStepTime = 0.0f;     // 记录上一次使用瞬步的时间
    private bool onFloor = true;            // 角色是否在地上
    private int jumpTimer = 0;              // 跳跃计数器
    #endregion

    enum STATE      // 角色的运动状态
    {
        Idle,
        WALK,
        RUN,
        EXSTEP,
        JUMP,
        DOUBLEJUMP
    }

    STATE characterState = STATE.Idle;

    private void Start()
    {
        rgb = this.GetComponent<Rigidbody2D>();
        anim = this.GetComponentInChildren<Animator>();         // 图片动画相关在子物体上
        sr = this.GetComponentInChildren<SpriteRenderer>();
        originColor = sr.color;
    }

    private void Update()
    {
        MoveController();                           // 移动控制
        JumpController();                           // 跳跃控制
    }

    private void MoveController()
    {
        if (Input.GetKeyUp(KeyCode.A))              // 不能||D，否则快速AD也算双击有效时间
        {
            releaseATime = Time.time;
            Stop();
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            releaseDTime = Time.time;
            Stop();
        }

        if (Input.GetKey(KeyCode.A))
        {
            positiveFace = false;

            if (onFloor)
            {// 地面上才允许步行、跑步和瞬步
                pressATime = Time.time;

                if (Input.GetKeyDown(KeyCode.LeftShift) && exStepEnabled && Time.time - lastExStepTime >= exStepCD)
                    ExStep();       // 瞬步
                if (pressATime - releaseATime <= pressInterval)
                    Run();
                else
                    Walk();
            }

            if (!onFloor && -rgb.velocity.x < maxFloatSpeed)        // 不能超过最大微调速度
            {// 腾空时允许空中左右微调
                rgb.AddForce(new Vector2(-floatForce_x, 0));
            }
        }

        if (Input.GetKey(KeyCode.D))
        {
            positiveFace = true;

            if (onFloor)
            {
                pressDTime = Time.time;

                if (Input.GetKeyDown(KeyCode.LeftShift) && exStepEnabled && Time.time - lastExStepTime >= exStepCD)
                    ExStep();
                if (pressDTime - releaseDTime <= pressInterval)
                    Run();
                else
                    Walk();
            }

            if (!onFloor && rgb.velocity.x < maxFloatSpeed)
            {
                rgb.AddForce(new Vector2(floatForce_x, 0));
            }
        }
    }

    private void JumpController()
    {
        if (Input.GetKeyDown(KeyCode.K) && jumpTimer < 2)
        {// 跳跃及二段跳
            onFloor = false;
            jumpTimer++;
            if(jumpTimer == 2)
            {
                //transform.Rotate(new Vector3(0, 0, 1), 120);    // 前空翻360度（播放前空翻动画）
                sr.color = new Color(0, 1, 0.5f);
                anim.SetBool("doubleJump", true);
            }
            rgb.AddForce(new Vector2(0, jumpForce_y), ForceMode2D.Impulse);
        }
    }

    private void Stop()
    {// 松开A/D键
        sr.color = originColor;
        rgb.velocity = new Vector2(0, 0);
    }

    private void ExStep()
    {
            //sr.color = new Color(1, 1, 0);                  // 暂用变色来表示瞬步冷却时间
            float tempStepDistance = exStepDistance;        // 临时瞬步距离
            Vector3 temp = transform.position;
            RaycastHit2D hit = new RaycastHit2D();
            if (!positiveFace)
                hit = Physics2D.Raycast(new Vector2(temp.x - 0.6f, temp.y), new Vector2(-1, 0), exStepDistance);   // 使用Raycast解决穿墙问题
            else
                hit = Physics2D.Raycast(new Vector2(temp.x + 0.6f, temp.y), new Vector2(1, 0), exStepDistance);
            if (hit)
            {// 仅通过Raycast判断还不能解决贴墙穿越的问题，需要在碰撞事件中进一步处理
                tempStepDistance = hit.distance;            // 重新赋值瞬步距离
            }
            if (!positiveFace)
                transform.position = new Vector3(-tempStepDistance + temp.x, temp.y, temp.z);    // 虽然可以瞬步，但要解决穿墙的BUG
            else
                transform.position = new Vector3(tempStepDistance + temp.x, temp.y, temp.z);
            lastExStepTime = Time.time;                     // 重置瞬步使用时间
    }

    private void Run()
    {
        if (-rgb.velocity.x < maxRunSpeed && !positiveFace)    // 不能超过最大跑步速度
            rgb.AddForce(new Vector2(-runForce_x, 0));

        if (rgb.velocity.x < maxRunSpeed && positiveFace)
            rgb.AddForce(new Vector2(runForce_x, 0));

        sr.color = new Color(1, 0, 0);
        releaseATime = Time.time;
        releaseDTime = Time.time;
    }

    private void Walk()
    {
        if (-rgb.velocity.x < maxWalkSpeed && !positiveFace)              // 不能超过最大步行速度
            rgb.AddForce(new Vector2(-walkForce_x, 0));

        if (rgb.velocity.x < maxWalkSpeed && positiveFace)
            rgb.AddForce(new Vector2(walkForce_x, 0));
        sr.color = originColor;                     // 若不在瞬步CD和跑步状态，显示原色
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Floor")    // 某些物体的碰撞不可计算在内
        {
            exStepEnabled = false;
        }
        if (collision.gameObject.tag == "Floor")    // 进行各种落地初始化
        {
            onFloor = true;
            jumpTimer = 0;
            sr.color = originColor;
            anim.SetBool("doubleJump", false);
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
