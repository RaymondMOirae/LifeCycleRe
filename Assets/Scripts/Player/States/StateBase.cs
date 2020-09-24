using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;


public abstract class StateBase
{
    public static PlayerController player;
    public static StateManager mgr;
    public static Animator anim;

    public abstract void OnEnter();

    public abstract void Excecute();

}
