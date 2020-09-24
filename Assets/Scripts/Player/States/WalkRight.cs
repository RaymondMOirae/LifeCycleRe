using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class WalkRight :StateBase
{
    bool paradolaFalling = false;
    Vector2 walkVector = new Vector2(player.walkSpeed, player.rb.velocity.y);
    Vector2 fallVector = new Vector2(0, player.rb.velocity.y);

    public override void OnEnter()
    {
        anim.Play("WalkRight");
        switch (mgr.prev)
        {
            case State.Jump:
                player.rb.velocity = new Vector2(player.jumpSpeedX, player.rb.velocity.y);
                paradolaFalling = true;
                break;
            case State.Crouch:
                player.rb.velocity = Vector2.right * player.walkSpeed;
                paradolaFalling = false;
                break;
            case State.WalkLeft:
                player.rb.velocity = Vector2.right * player.walkSpeed;
                paradolaFalling = false;
                break;
            default:
                break;
        }

    }

    public override void Excecute()
    {
        if (player.RoughlyOnGround()) // 站在地上且右方无阻挡
        {
            paradolaFalling = false;
            walkVector.y = player.rb.velocity.y;
            player.rb.velocity = walkVector;
            //Debug.Log("Walking");
            //Debug.Log(walkVector);
        }
        else if (!player.RoughlyOnGround() && !paradolaFalling) // 滞空且不是“跳跃→行走”，说明在下落
        {
            fallVector.y = player.rb.velocity.y;
            player.rb.velocity = fallVector;
            //Debug.Log("EdgeFalling");
        }
        else
        {
            Debug.Log("out of situs");
        }

        // 设定对应的动画
        if (player.name != "Baby" && 
            !player.RoughlyOnGround() &&
            !anim.GetCurrentAnimatorStateInfo(0).IsName("FallRight"))
        {
            anim.Play("FallRight");
        }else if (player.RoughlyOnGround())
        {
            anim.Play("WalkRight");
        }
    }
}
