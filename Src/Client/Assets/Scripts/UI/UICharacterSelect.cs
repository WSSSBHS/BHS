using System.Collections;
using System.Collections.Generic;
using Services;
using SkillBridge.Message;
using UnityEngine;
using UnityEngine.UI;
using Models;
public class UICharacterSelect : MonoBehaviour
{
    public GameObject Select1;
    public GameObject Select2;
    public CharcterView charcterView;

    public Text[] ChartName;
    public Text[] BBName;
    public Text BDescripute;
    public Text JueSeName;
    private int selectTID = -1;
    public CharacterClass charclass;
    public Transform uiCharList;           //滚动条
    public GameObject uiCharInfo;       // 角色信息列表模板
    public List<GameObject> uiChars = new List<GameObject>();   //泛型集合
    void Start()
    {
        InitCharacterSelct(true);
        DataManager.Instance.Load();    // 预加载表格Json组件  
        UserService.Instance.OnCharectorCreate = OnCharacterCreate; //(订阅)   C5
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void OnSelectClass(int X)
    {

        this.charclass = (CharacterClass)X;

        charcterView.BIANHAO = X - 1;
        for (int i = 0; i < 3; i++)
        {
            ChartName[i].gameObject.SetActive(X - 1 == i);          //显示那个角色模型
            BBName[i].text = DataManager.Instance.Characters[i + 1].Name;

        }
        BDescripute.text = DataManager.Instance.Characters[X].Description;    // 载入列表中的描述文段 

    }
    public void OnClickCreate()
    {
        if (string.IsNullOrEmpty(this.JueSeName.text))
        {
            MessageBox.Show("请输入名字");
            return;
        }
        UserService.Instance.SendCharectorCreate(this.JueSeName.text, this.charclass);   //将输入的信息传入服务层 C1
    }
    void OnCharacterCreate(Result result, string msg)
    {
        if (result == Result.Success)        //创建成功  C6
        {
            InitCharacterSelct(true);

        }
        else
        {
            MessageBox.Show(msg, "错误", MessageBoxType.Error);
        }
    }
    public void back()
    {
        Select1.SetActive(true);
        Select2.SetActive(false);
    }
    public void InitCharacterSelct(bool init)                                                                                        //返回界面 C7
    {
        Select1.SetActive(true);
        Select2.SetActive(false);
        if (init)
        {
            foreach (var X in uiChars)                                                                                             //foreach  var X in Y(Y是字段)  将Y放入X中
            {
                Destroy(X);                                                                                                //把之前创建的列表全部除去
            }
            uiChars.Clear();                                                                                              //消除渲染缓冲区;彻底消除
        }
        for (int i = 0; i < User.Instance.Info.Player.Characters.Count; i++)                                                                                       // 全部获取一遍
        {
            Debug.Log("----------------获取角色信息-------");
            GameObject go = Instantiate(uiCharInfo, this.uiCharList);                                                                                           //创建游戏实体(信息列表)

            UICharInfo chrInfo = go.GetComponent<UICharInfo>();                                                                                                 //获取相关信息的模板
            chrInfo.info = User.Instance.Info.Player.Characters[i];                                                                                           //将角色信息填入info

            Button button = go.transform.Find("JueSeList").GetComponent<Button>();
            int idx = i;
            button.onClick.AddListener(() =>
            {
                OnSelectCharcter(idx);
            });                                                                                                                 //按下角色执行角色选择

            uiChars.Add(go);
            go.SetActive(true);
        }

    }
    public void OnSelectCharcter(int idx)
    {

        this.selectTID = idx;
        var cha = User.Instance.Info.Player.Characters[idx];                 //获取第X个角色信息
        Debug.LogFormat("Select Char:[{0}]{1}[{2}]", cha.Id, cha.Name, cha.Class);
        User.Instance.CurrentCharacter = cha;
        charcterView.CurrectCharacter = idx;

        for (int i = 0; i < User.Instance.Info.Player.Characters.Count; i++)
        {
            UICharInfo ci = this.uiChars[i].GetComponent<UICharInfo>();
            ci.Selected = idx == i;                                               //点击相同的地方才会显示
        }

    }
    public void OnClickPlay()             //发出进去的请求 M1
    {
        if (selectTID >= 0)
        {
           UserService.Instance.SendGameEnter(selectTID);

        }
    
    }
}

