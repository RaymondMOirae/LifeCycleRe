using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum State
{
    WalkRight,
    WalkLeft,
    Jump,
    Crouch,
    Idle
}

public class StateManager : MonoBehaviour
{
    public Dictionary<State, StateBase> states = new Dictionary<State, StateBase>();

    public PlayerController player;
    public Animator anim;

    public State prev;
    public State cur;

    // get all states in Start
    public void InitMgr()
    {
        player = GetComponent<PlayerController>();

        StateBase.player = player;
        StateBase.anim = player.animator;
        StateBase.mgr = this;
        states.Add(State.WalkRight, new WalkRight());
        states.Add(State.WalkLeft, new WalkLeft());
        states.Add(State.Jump, new Jump());
        states.Add(State.Crouch, new Crouch());

        Jump jp = (Jump)states[State.Jump];
        jp.jumpTime = - 2.0f * player.jumpSpeedY / (player.rb.gravityScale * Physics2D.gravity.y);
        Debug.Log(jp.jumpTime);
        //states.Add(State.Idle , new Idle());
        prev = State.Idle;
        cur = State.Idle;

    }

    public void ExecuteState()
    {
        states[cur].Excecute();
    }

    public void UpdateState(State next)
    {

        if ((cur == State.Jump && next == State.Crouch) || // 从任何状态的跳跃进入下蹲
            (cur == State.Jump && (prev == State.WalkLeft || prev == State.WalkRight) && !player.CloselyOnGround()) || // 从抛物的跳跃状态进入其他状态
            ((cur == State.WalkLeft || cur == State.WalkRight) && !player.CloselyOnGround()) // 从正在下落的行走状态进入任何状态
            )
        {
            Debug.Log("Start Coroutine Once");
            // 开始携程检测，在下一次落地时切换状态
            player.StartCoroutine("WaitForSwitch", next);
            // 在“延后的切换”执行完毕之前，玩家不能操作
            player.canSwitch = false;
        }
        else
        {
            player.canSwitch = true;
            prev = cur;
            cur = next;
            states[cur].OnEnter();
        }

    }
    
}
