using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class WalkLeft: StateBase
{
    bool paradolaFalling = false;

    public override void OnEnter()
    {
        anim.Play("WalkLeft");
        switch (mgr.prev)
        {
            case State.Jump:
                player.rb.velocity = new Vector2(-player.jumpSpeedX, player.rb.velocity.y);
                paradolaFalling = true;
                break;
            case State.Crouch:
                player.rb.velocity = Vector2.left* player.walkSpeed;
                break;
            case State.WalkRight:
                player.rb.velocity = Vector2.left* player.walkSpeed;
                break;
            default:
                break;
        }

    }

    public override void Excecute()
    {
        if (player.RoughlyOnGround() && !player.LeftBocked())
        {
            paradolaFalling = false;
            player.rb.velocity = new Vector2(-player.walkSpeed, player.rb.velocity.y);

        }
        else if (!paradolaFalling)
        {
            player.rb.velocity = new Vector2(0, player.rb.velocity.y);
        }
    }
}
