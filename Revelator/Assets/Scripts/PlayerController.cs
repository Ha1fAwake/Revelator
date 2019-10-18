using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Author：圈毛君
/// MoveController：2019/10/18 20:50
/// </summary>
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 3.0f;         // 步行速度
    public float runSpeed = 5.0f;          // 跑步速度
    public float pressInterval = 0.5f;     // 双击按键的有效时间间隔
    public float exStepDistance = 3.0f;    // 瞬步距离（速度)
    public float jumpForce_y = 5.0f;       // 弹跳力

    private Rigidbody2D rgb;
    private SpriteRenderer sr;
    private Color originColor;
    private float pressATime;              // 按下A键时间
    private float pressDTime;
    private float releaseATime = .0f;      // 松开A键时间
    private float releaseDTime = .0f;
    private bool exStepEnabled = true;     // 能否使用瞬步

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
            if (Input.GetKeyDown(KeyCode.LeftShift) && exStepEnabled)
            {
                float tempStepDistance = exStepDistance;        // 临时瞬步距离
                Vector3 temp = transform.position;
                RaycastHit2D hit = Physics2D.Raycast(new Vector2(temp.x - 0.6f, temp.y), new Vector2(-1, 0), exStepDistance);   // 使用Raycast解决穿墙问题
                if (hit)
                {// 仅通过Raycast判断还不能解决贴墙穿越的问题，需要在碰撞事件中进一步处理
                    tempStepDistance = hit.distance;            // 重新赋值瞬步距离
                }
                transform.position = new Vector3(-tempStepDistance + temp.x, temp.y, temp.z);    // 虽然可以瞬步，但要解决穿墙的BUG
            }
            pressATime = Time.time;
            if(pressATime - releaseATime <= pressInterval)
            {
                rgb.velocity = new Vector2(-runSpeed, 0);       // 跑步
                sr.color = new Color(1, 0, 0);
                releaseATime = Time.time;                       // 跑步状态中要持续更新松键时间
            }
            else
                rgb.velocity = new Vector2(-walkSpeed, 0);      // 步行
        }

        if (Input.GetKey(KeyCode.D))
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && exStepEnabled)
            {
                float tempStepDistance = exStepDistance;
                Vector3 temp = transform.position;
                RaycastHit2D hit = Physics2D.Raycast(new Vector2(temp.x + 0.6f, temp.y), new Vector2(1, 0), exStepDistance);
                if (hit)
                {
                    tempStepDistance = hit.distance;
                }
                transform.position = new Vector3(tempStepDistance + temp.x, temp.y, temp.z);
            }
            pressDTime = Time.time;
            if (pressDTime - releaseDTime <= pressInterval)
            {
                rgb.velocity = new Vector2(runSpeed, 0);
                sr.color = new Color(1, 0, 0);
                releaseDTime = Time.time;
            }
            else
                rgb.velocity = new Vector2(walkSpeed, 0);
        }
    }

    public void JumpController()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name != "Floor")    // 某些物体的碰撞不可计算在内
        {
            exStepEnabled = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name != "Floor")
        {
            exStepEnabled = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name != "Floor")
        {
            exStepEnabled = true;
        }
    }
}
