using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Internal;

public class PlayerController : MonoBehaviour
{

    [Header("行走速度")]
    public float walkSpeed = 3.0f;

    [Header("跳跃的纵向初始速度")]
    public float jumpSpeedY = 10.0f ;
    [Header("跳跃的横向初始速度")]
    public float jumpSpeedX = 5.0f;



    [Header("当前为第几个行为，游戏开始前请设置为-1")]
    public int actIndex = -1;

    [Header("测试序列，需手动修改")]
    public List<string> tempActs = new List<string>();

    [HideInInspector]
    [Header("最终使用的序列，暂为空")]
    public List<string> stageActs = new List<string>();

    [Header("设置为true时游戏运行")]
    public bool isRunning = false;

    [Header("当前玩家是否能切换到下一个动作，由脚本控制")]
    public bool canSwitch = true;

    [Header("粗检测地面的射线长度")]
    public float roughRayLenY = 1.0f;
    [Header("检测地面的射线长度")]
    public float shortRayLenY = 0.1f;
    [Header("检测墙面的射线长度")]
    public float rayLenX = 0.1f;

    [Header("连通盒的尺寸")]
    public float boxLength = 18.0f;
    public float boxHeight = 18.0f;

    [Header("策划不用管的区域")]
    public LayerMask groundLayer;
    public Animator animator;
    public StateManager mgr;

    // 与角色物理相关的变量
    public Rigidbody2D rb;
    [HideInInspector]
    public Vector2 boxSize;
    [HideInInspector]
    public Vector2 boxOffset;
    [HideInInspector]
    public Vector2 leftFoot;
    [HideInInspector]
    public Vector2 rightFoot;
    [HideInInspector]
    public Vector2 leftHead;
    [HideInInspector]
    public Vector2 rightHead;
    [HideInInspector]
    public Vector2 modBox;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play("Idle");

        // 计算碰撞盒偏移与尺寸，用于确定发射射线的位置
        rb = GetComponent<Rigidbody2D>();
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        float scale = transform.localScale.x;
        boxOffset = box.offset * scale;
        boxSize = box.size * scale;
        leftFoot = new Vector2(-boxSize.x / 2, -boxSize.y * 0.9f / 2) + boxOffset;
        rightFoot = new Vector2(boxSize.x / 2, -boxSize.y * 0.9f / 2) + boxOffset;

        leftHead = new Vector2(-boxSize.x / 2, boxSize.y * 0.9f / 2) + boxOffset;
        rightHead = new Vector2(boxSize.x / 2, boxSize.y * 0.9f / 2) + boxOffset;

       

        modBox = boxSize + new Vector2(boxLength, boxHeight);

        mgr = GetComponent<StateManager>();
        mgr.InitMgr();
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning)
        {
            HandleInput();
            ModPosition();
            //UIController.uIController.HighLight((actIndex) % stageActs.Count);
        }

    }

    private void FixedUpdate()
    {
        if(actIndex >= 0)
        {
            mgr.ExecuteState();
        }
        
    }

    void HandleInput()
    {
        if (canSwitch && Input.GetKeyDown(KeyCode.Space))
        {
            //actIndex = (actIndex + 1) % stageActs.Count;
            //string actName = stageActs[actIndex];

            // 使用测试序列的临时代码
            actIndex = (actIndex + 1) % tempActs.Count;
            string actName = tempActs[actIndex];

            Debug.Log(actName);
            State result;
            Enum.TryParse(actName, out result);
            mgr.UpdateState(result);
        }
    }

    public IEnumerator WaitForSwitch(State next)
    {
        Jump jp = (Jump)mgr.states[State.Jump];
        while(jp.finished == false || !RoughlyOnGround())
        {
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
        // 完成跳跃后脱离检测的循环，并初始化表征跳跃结束的字段
        jp.finished = false;
        // 允许玩家输入
        canSwitch = true;
        // 执行切换
        mgr.prev = mgr.cur;
        mgr.cur = next;
        mgr.states[mgr.cur].OnEnter();
    }


    public bool RoughlyOnGround()
    {
        bool left = Raycast(leftFoot, Vector2.down, roughRayLenY, groundLayer, Color.cyan);
        bool right = Raycast(rightFoot, Vector2.down, roughRayLenY, groundLayer, Color.cyan);
        //Debug.Log("Rough:" + left + "," + right);
        return left || right;
    }
 public bool CloselyOnGround()
    {
        bool left = Raycast(leftFoot, Vector2.down, shortRayLenY, groundLayer, Color.red);
        bool right = Raycast(rightFoot, Vector2.down, shortRayLenY, groundLayer, Color.red);
        //Debug.Log("Close:" + left + "," + right);
        return left || right;
    }

    public bool LeftBocked()
    {
        bool top = Raycast(leftHead, Vector2.left, rayLenX, groundLayer, Color.cyan);
        bool bottom = Raycast(leftFoot, Vector2.left, rayLenX, groundLayer, Color.cyan);
        return top || bottom;
    }

    public bool RightBlocked()
    {
        bool top = Raycast(rightHead, Vector2.right, rayLenX, groundLayer, Color.cyan);
        bool bottom = Raycast(rightFoot, Vector2.right, rayLenX, groundLayer, Color.cyan);
        return top || bottom;
    }

    // 绘制射线，并返回是否有集中碰撞盒
    public bool Raycast(Vector2 offset, Vector2 dir, float len, LayerMask layer, Color color)
    {
        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, dir, len, layer);
        Debug.DrawRay(new Vector3(pos.x + offset.x, pos.y + offset.y, 0), new Vector3(dir.x, dir.y, 0.0f) * len, color);
        return hit;
    }

    void ModPosition()
    {
        Vector3 pos = transform.position;
        pos.x = (boxSize.x / 3 + pos.x + modBox.x) % modBox.x - boxSize.x / 3;
        pos.y = (boxSize.y / 3 + pos.y + modBox.y) % modBox.y - boxSize.y / 3;
        transform.position = pos;
    }

    public bool dead = false;
    public void deadSign()
    {
        dead = true;
        Debug.Log("CHANGE");
    }
    public void TakeDamage()
    {
        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        gm.LoadScence(gm.level);
    }

    // trash functions
    IEnumerator StackVelocity()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
        while (true)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            if (RoughlyOnGround())
            {
                rb.velocity = Vector2.right * walkSpeed;
                break;
            }
        }

    }

    //void DoAction(string actName)
    //{
    //    MethodInfo mt = t.GetMethod(actName);
    //    mt.Invoke(acts, new object[] { rb });
    //}

}
