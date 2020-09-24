using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouch :StateBase
{
    public override void OnEnter()
    {
        player.rb.velocity = Vector2.zero;
        anim.Play("Crouch");
    }
    public override void Excecute()
    {

    }
}
