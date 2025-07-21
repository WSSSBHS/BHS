using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Entities;
using Services;
using SkillBridge.Message;
using Managers;

public class GameObjectManager : MonoBehaviour
{

    Dictionary<int, GameObject> Characters = new Dictionary<int, GameObject>();
    // Use this for initialization
    void Start()
    {
        StartCoroutine(InitGameObjects());    //通过携程查找所有在地图中的角色,然后创建相应的对象
        CharacterManager.Instance.OnCharacterEnter = OnCharacterEnter;  //进去时执行角色进入的事件
    }

    private void OnDestroy()
    {
        CharacterManager.Instance.OnCharacterEnter = null;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCharacterEnter(Character cha)
    {
        CreateCharacterObject(cha);     //创建对象
    }

    IEnumerator InitGameObjects()
    {
        foreach (var cha in CharacterManager.Instance.Characters.Values)
        {
            CreateCharacterObject(cha);
            yield return null;
        }
    }

    private void CreateCharacterObject(Character character)
    {
        if (!Characters.ContainsKey(character.Info.Id) || Characters[character.Info.Id] == null)//判断角色在不在地图中
        {
            Object obj = Resloader.Load<Object>(character.Define.Resource);                                 //加载资源
            if(obj == null)
            {
                Debug.LogErrorFormat("Character[{0}] Resource[{1}] not existed.",character.Define.TID, character.Define.Resource);
                return;
            }
            GameObject go = (GameObject)Instantiate(obj);                                     //根据游戏资源实例化自己
            go.name = "Character_" + character.Info.Id + "_" + character.Info.Name;

            go.transform.position = GameObjectTool.LogicToWorld(character.position);        //将服务器的坐标返回到地图上
            go.transform.forward = GameObjectTool.LogicToWorld(character.direction);        //将服务器的朝向返回到地图上
            Characters[character.Info.Id] = go;

            EntityController ec = go.GetComponent<EntityController>();
            if (ec != null)
            {
                ec.entity = character;
                ec.isPlayer = character.IsPlayer;
            }
            
            PlayerInputController pc = go.GetComponent<PlayerInputController>();
            if (pc != null)                                                              
            {
                if (character.Info.Id == Models.User.Instance.CurrentCharacter.Id)            //如果这个角色是本机玩家控制的将启用玩家控制的逻辑 以及相机的设置更换
                {
                    MainPlayerCamera.Instance.player = go;
                    pc.enabled = true;
                    pc.character = character;
                    pc.entityController = ec;
                }
                else
                {
                    pc.enabled = false;
                }
            }
            UIWorldElementManger.Instance.AddCharcterNameBar(go.transform, character);
        }
    }
}

