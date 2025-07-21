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
    public Transform uiCharList;           //������
    public GameObject uiCharInfo;       // ��ɫ��Ϣ�б�ģ��
    public List<GameObject> uiChars = new List<GameObject>();   //���ͼ���
    void Start()
    {
        InitCharacterSelct(true);
        DataManager.Instance.Load();    // Ԥ���ر��Json���  
        UserService.Instance.OnCharectorCreate = OnCharacterCreate; //(����)   C5
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
            ChartName[i].gameObject.SetActive(X - 1 == i);          //��ʾ�Ǹ���ɫģ��
            BBName[i].text = DataManager.Instance.Characters[i + 1].Name;

        }
        BDescripute.text = DataManager.Instance.Characters[X].Description;    // �����б��е������Ķ� 

    }
    public void OnClickCreate()
    {
        if (string.IsNullOrEmpty(this.JueSeName.text))
        {
            MessageBox.Show("����������");
            return;
        }
        UserService.Instance.SendCharectorCreate(this.JueSeName.text, this.charclass);   //���������Ϣ�������� C1
    }
    void OnCharacterCreate(Result result, string msg)
    {
        if (result == Result.Success)        //�����ɹ�  C6
        {
            InitCharacterSelct(true);

        }
        else
        {
            MessageBox.Show(msg, "����", MessageBoxType.Error);
        }
    }
    public void back()
    {
        Select1.SetActive(true);
        Select2.SetActive(false);
    }
    public void InitCharacterSelct(bool init)                                                                                        //���ؽ��� C7
    {
        Select1.SetActive(true);
        Select2.SetActive(false);
        if (init)
        {
            foreach (var X in uiChars)                                                                                             //foreach  var X in Y(Y���ֶ�)  ��Y����X��
            {
                Destroy(X);                                                                                                //��֮ǰ�������б�ȫ����ȥ
            }
            uiChars.Clear();                                                                                              //������Ⱦ������;��������
        }
        for (int i = 0; i < User.Instance.Info.Player.Characters.Count; i++)                                                                                       // ȫ����ȡһ��
        {
            Debug.Log("----------------��ȡ��ɫ��Ϣ-------");
            GameObject go = Instantiate(uiCharInfo, this.uiCharList);                                                                                           //������Ϸʵ��(��Ϣ�б�)

            UICharInfo chrInfo = go.GetComponent<UICharInfo>();                                                                                                 //��ȡ�����Ϣ��ģ��
            chrInfo.info = User.Instance.Info.Player.Characters[i];                                                                                           //����ɫ��Ϣ����info

            Button button = go.transform.Find("JueSeList").GetComponent<Button>();
            int idx = i;
            button.onClick.AddListener(() =>
            {
                OnSelectCharcter(idx);
            });                                                                                                                 //���½�ɫִ�н�ɫѡ��

            uiChars.Add(go);
            go.SetActive(true);
        }

    }
    public void OnSelectCharcter(int idx)
    {

        this.selectTID = idx;
        var cha = User.Instance.Info.Player.Characters[idx];                 //��ȡ��X����ɫ��Ϣ
        Debug.LogFormat("Select Char:[{0}]{1}[{2}]", cha.Id, cha.Name, cha.Class);
        User.Instance.CurrentCharacter = cha;
        charcterView.CurrectCharacter = idx;

        for (int i = 0; i < User.Instance.Info.Player.Characters.Count; i++)
        {
            UICharInfo ci = this.uiChars[i].GetComponent<UICharInfo>();
            ci.Selected = idx == i;                                               //�����ͬ�ĵط��Ż���ʾ
        }

    }
    public void OnClickPlay()             //������ȥ������ M1
    {
        if (selectTID >= 0)
        {
           UserService.Instance.SendGameEnter(selectTID);

        }
    
    }
}

