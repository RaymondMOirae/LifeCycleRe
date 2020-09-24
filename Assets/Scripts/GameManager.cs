using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// 单例模式
    /// </summary>
    private static GameManager _intance;
    public static GameManager gameManager
    {
        get
        {
            return _intance;
        }
    }
    public bool IsRunning;
    /// <summary>
    /// 关卡总数
    /// </summary>
    public static int levelNum = 4;
    /// <summary>
    /// 当前关卡
    /// </summary>
    public int level;
    /// <summary>
    /// 主角
    /// </summary>
    public GameObject player;
    /// <summary>
    /// 技能（总）
    /// </summary>
    public enum Skill
    {
        WalkRight,
        WalkLeft,
        Jump,
        Crouch
    }
    /// <summary>
    /// 每一关对应可使用技能
    /// </summary>
    public List<Skill>[] skillForEachLevel = new List<Skill>[levelNum];
    /// <summary>
    /// 返回关卡对应可使用的技能
    /// </summary>
    /// <returns></returns>
    public List<Skill> GetSkillsForThisLevel()
    {
        return skillForEachLevel[level - 1];
    }
    /// <summary>
    /// 技能贴图
    /// </summary>
    public Sprite[] skillSprites;
    /// <summary>
    /// 每轮可选择的技能数量
    /// </summary>
    private int[] selectNums = new int[levelNum];
    public int GetSelectNum()
    {
        return selectNums[level - 1];
    }


    public void LoadScence(int l)
    {//todo:更新level？
        SceneManager.LoadScene(l - 1);
        level = l;
    }
    private void Awake()
    {
        _intance = this;
        //设置selectNums
        selectNums[0] = 2;
        selectNums[1] = 2;
        selectNums[2] = 2;
        selectNums[3] = 3;
        //selectNums[4] = 3;
        //selectNums[5] = 3;
        //设置skillForEachLevel
        skillForEachLevel[0] = new List<Skill> { Skill.WalkRight, Skill.WalkLeft };
        skillForEachLevel[1] = new List<Skill> { Skill.WalkRight, Skill.WalkLeft };
        skillForEachLevel[2] = new List<Skill> { Skill.WalkRight, Skill.WalkLeft, Skill.Jump };
        skillForEachLevel[3] = new List<Skill> { Skill.WalkRight, Skill.WalkLeft, Skill.Jump };
        //skillForEachLevel[4] = new List<Skill> { Skill.WalkRight, Skill.WalkLeft, Skill.Jump, Skill.Crouch };
        //skillForEachLevel[5] = new List<Skill> { Skill.WalkRight, Skill.WalkLeft, Skill.Jump, Skill.Crouch };
        IsRunning = false;
    }
    /// <summary>
    /// 使player开始游戏
    /// </summary>
    public void StartTheGame()
    {
        IsRunning = true;
        player.GetComponent<PlayerController>().stageActs.Clear();
        player.GetComponent<PlayerController>().isRunning = true;
        for (int i = 0; i < UIController.uIController.enableSkills.Length; i++)
        {
            player.GetComponent<PlayerController>().stageActs.Add(UIController.uIController.enableSkills[i].ToString());
        }
    }
    public void NextLevel()
    {
        if (level < levelNum)
        {
            level++;
            Debug.Log("nextlevel:" + level);
            LoadScence(level);
        }
        else
        {
            UIController.uIController.ShowEndPanel();
        }
    }
    public GameObject map;
    public void RotateMap(int angle)
    {
        map = GameObject.Find("Stage");
        map.transform.RotateAround(new Vector3(9, 9, 0), new Vector3(0, 0, 1), angle);
    }
}

