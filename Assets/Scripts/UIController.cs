using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIController : MonoBehaviour
{
    public GameObject endPanel;
    private Vector2 endPanelPosition = new Vector2(488, 1076);
    public Button startButton;
    public Button[] skillButtons;
    public Button[] selectedButtonPrefabs;
    private Button[] selectedButtons;
    public GameObject selectedPanel;
    /// <summary>
    /// 不同SelectedButton数量时，每个按钮的位置
    /// </summary>
    private List<Vector2>[] positionOfSelectedButton = new List<Vector2>[GameManager.levelNum];
    /// <summary>
    /// 存储目前可以使用的技能
    /// </summary>
    public GameManager.Skill[] enableSkills;
    private int[] matchedSkillButtons;
    /// <summary>
    /// 指向将要被决定的selectButton位置
    /// </summary>
    private int selectButtonPoint;
    private bool[] isSelectButtonEmpty;
    public GameObject levelPanel;
    /// <summary>
    /// 单例模式
    /// </summary>
    private static UIController _intance;
    public static UIController uIController
    {
        get
        {
            return _intance;
        }
    }
    private void Awake()
    {
        _intance = this;
        positionOfSelectedButton[0] = new List<Vector2> { new Vector2(491.6f, 391.8f) };
        positionOfSelectedButton[1] = new List<Vector2> { new Vector2(491.6f, 451.5f), new Vector2(491.6f, 332) };
        positionOfSelectedButton[2] = new List<Vector2> { new Vector2(491.6f, 451.5f), new Vector2(431.8f, 332), new Vector2(551.3f, 332) };
        //todo
    }
    private void Start()
    {
        ResetUI();
    }
    public void ResetUI()
    {
        InstantiateSelectButtons();
        selectButtonPoint = 0;
        isSelectButtonEmpty = new bool[selectedButtons.Length];
        for (int i = 0; i < isSelectButtonEmpty.Length; i++)
        {
            isSelectButtonEmpty[i] = true;
        }
        enableSkills = new GameManager.Skill[selectedButtons.Length];
        matchedSkillButtons = new int[selectedButtons.Length];
        for (int i = 0; i < matchedSkillButtons.Length; i++)
        {
            matchedSkillButtons[i] = -1;
        }
        List<GameManager.Skill> temp = GameManager.gameManager.GetSkillsForThisLevel();
        int len = temp.Count;
        for (int i = 0; i < len; i++)
        {
            skillButtons[i].image.sprite = GameManager.gameManager.skillSprites[(int)temp[i]];
        }
        for (int i = len; i < 4; i++)
        {
            //skillButtons[i].enabled = false;
            DisableButton(skillButtons[i]);
        }
        // SelectButton初始化不可以点
        for (int i = 0; i < selectedButtons.Length; i++)
        {
            //selectedButtons[i].enabled = false;
            DisableButton(selectedButtons[i]);
        }
    } 
    private void InstantiateSelectButtons()
    {
        int len = GameManager.gameManager.GetSelectNum();
        selectedButtons = new Button[len];
        for (int i = 0; i < len; i++)
        {
            Debug.Log("i1:" + i);
            selectedButtons[i] = Instantiate(selectedButtonPrefabs[i], positionOfSelectedButton[len - 1][i], Quaternion.identity, selectedPanel.transform);
        }
        if (selectedButtons.Length>0)
        {
            selectedButtons[0].onClick.AddListener(delegate ()
            {
                Unselect(0);
            });
        }
        if (selectedButtons.Length > 1)
        {
            selectedButtons[1].onClick.AddListener(delegate ()
            {
                Unselect(1);
            });
        }
        if (selectedButtons.Length > 2)
        {
            selectedButtons[2].onClick.AddListener(delegate ()
            {
                Unselect(2);
            });
        }
    }
    public void Select(int num)
    {
        selectedButtons[selectButtonPoint].image.sprite = skillButtons[num].image.sprite;
        enableSkills[selectButtonPoint] = GameManager.gameManager.skillForEachLevel[GameManager.gameManager.level][num];
        matchedSkillButtons[selectButtonPoint] = num;
        isSelectButtonEmpty[selectButtonPoint] = false;
        //selectedButtons[selectButtonPoint].enabled = true;
        EnableButton(selectedButtons[selectButtonPoint]);
        NextPoint();
        //skillButtons[num].enabled = false;
        DisableButton(skillButtons[num]);
    }
    public void Unselect(int num)
    {
        Debug.Log(num);
        Debug.Log(selectedButtons.Length);
        selectedButtons[num].image.sprite = null;
        isSelectButtonEmpty[num] = true;
        NextPoint();
        //skillButtons[matchedSkillButtons[num]].enabled = true;
        EnableButton(skillButtons[matchedSkillButtons[num]]);
        //selectedButtons[num].enabled = false;
        DisableButton(selectedButtons[num]);
        matchedSkillButtons[num] = -1;
    }
    private void Update()
    {
        if (selectButtonPoint == selectedButtons.Length)
        {
            for (int i = 0; i < skillButtons.Length; i++)
            {
                //skillButtons[i].enabled = false;
                DisableButton(skillButtons[i]);
            }
            //startButton.enabled = true;
            EnableButton(startButton);
        }
        else
        {
            List<GameManager.Skill> temp = GameManager.gameManager.GetSkillsForThisLevel();
            int len = temp.Count;
            for (int i = 0; i < len; i++)
            {
                //skillButtons[i].enabled = true;
                EnableButton(skillButtons[i]);
            }
            for (int i = 0; i < matchedSkillButtons.Length; i++)
            {
                if (matchedSkillButtons[i]!=-1)
                {
                    //skillButtons[matchedSkillButtons[i]].enabled = false;
                    DisableButton(skillButtons[matchedSkillButtons[i]]);
                }
            }
            //startButton.enabled = false;
            DisableButton(startButton);
        }
        //for (int i = 0; i < isSelectButtonEmpty.Length; i++)
        //{
        //    Debug.Log(i+":"+isSelectButtonEmpty[i]);
        //}
    }
    public void Determine()
    {
        for (int i = 0; i < enableSkills.Length; i++)
        {
            Debug.Log(enableSkills[i]);
        }
        for (int i = 0; i < selectedButtons.Length; i++)
        {
            //selectedButtons[i].enabled = false;
            DisableButton(selectedButtons[i]);
        }
        //startButton.enabled = false;
        DisableButton(startButton);
        GameManager.gameManager.StartTheGame();
    }
    private void NextPoint()
    {
        for (int i = 0; i < isSelectButtonEmpty.Length; i++)
        {
            if (isSelectButtonEmpty[i])
            {
                selectButtonPoint = i;
                return;
            }
        }
        selectButtonPoint = isSelectButtonEmpty.Length;
    }

    public void Restart()
    {
        GameManager.gameManager.LoadScence(GameManager.gameManager.level);

    }
    public void Next()
    {
        GameManager.gameManager.NextLevel();
    }
    public void ShowEndPanel()
    {
        Instantiate(endPanel, endPanelPosition, Quaternion.identity, GameObject.Find("Canvas").transform);
    }
    public void EnableButton(Button button)
    {
        button.interactable = true;
        button.GetComponentInChildren<Button>().image.color = new Color(255, 255, 255, 255);
    }
    public void DisableButton(Button button)
    {
        button.interactable = false;
        button.GetComponentInChildren<Button>().image.color = new Color(255, 255, 255, 100);
    }
    public void HighLight(int index)
    {
        for (int i = 0; i < selectedButtons.Length; i++)
        {
            selectedButtons[i].interactable = false;
        }
        selectedButtons[index].interactable = true;
        selectedButtons[index].enabled = false;
    }
    public void ChooseLevel(int level)
    {
        GameManager.gameManager.level= level;
        GameManager.gameManager.LoadScence(GameManager.gameManager.level);
        ShowOrHideLevelPanel(false);
        //hack：查一下
    }
    public void ShowOrHideLevelPanel(bool show)
    {
        if (show)
        {
            levelPanel.SetActive(true);
        }
        else
        {
            levelPanel.SetActive(false);
        }
    }
}

