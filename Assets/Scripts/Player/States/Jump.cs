using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump :StateBase
{
    public float jumpTime;
    private float jumpTimeCounter;
    public bool finished = false;

    public override void OnEnter()
    {
        jumpTimeCounter = 0.0f;
        anim.Play("Jump");
        switch(mgr.prev)
        {
            case State.WalkRight:
                // 在PlayerController、GameManager内部实现延迟切换状态
                if(Mathf.Abs(player.rb.velocity.x) >= 0.1f)
                    player.rb.velocity = new Vector2(player.jumpSpeedX, player.jumpSpeedY + player.rb.velocity.y);
                break;
            case State.WalkLeft:
                if(Mathf.Abs(player.rb.velocity.x) >= 0.1f)
                    player.rb.velocity = new Vector2(-player.jumpSpeedX, player.jumpSpeedY + player.rb.velocity.y);
                break;
            case State.Crouch:
                // jump in situ
                player.rb.velocity = Vector2.up * player.jumpSpeedY;
                break;
            case State.Idle:
                // jump in situ
                player.rb.velocity = Vector2.up * player.jumpSpeedY;
                break;
            default:
                break;
        }
    }

    public override void Excecute()
    {
        // 抛物过程 = 不处理
        jumpTimeCounter += Time.fixedDeltaTime;

        // 自由下落时取消X速度
        if (!player.RoughlyOnGround() && jumpTimeCounter > jumpTime)
        {
            player.rb.velocity = new Vector2(0, player.rb.velocity.y);
        }
        else if(player.RoughlyOnGround() && jumpTimeCounter > jumpTime / 2)
        {
            player.rb.velocity = new Vector2(0, player.rb.velocity.y);
            finished = true;
        }
        
    }

}
