using System;
using Common;
using Network;
using UnityEngine;
using Models;
using SkillBridge.Message;
using Managers;
using Common.Data;

namespace Services
{

    class MapService : Singleton<MapService>, IDisposable
    {
        public MapService()
        {
            MessageDistributer.Instance.Subscribe<SkillBridge.Message.MapCharacterEnterResponse>(this.OnMapCharacterEnter);
            MessageDistributer.Instance.Subscribe<SkillBridge.Message.MapCharacterLeaveResponse>(this.OnMapCharacterLeave);
        }

        public int CurrentMapId { get; private set; }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<SkillBridge.Message.MapCharacterEnterResponse>(this.OnMapCharacterEnter);
            MessageDistributer.Instance.Unsubscribe<SkillBridge.Message.MapCharacterLeaveResponse>(this.OnMapCharacterLeave);
        }
        public void Init()
        { 
        
        
        }
   

        private void OnMapCharacterEnter(object sender, MapCharacterEnterResponse response)
        {
            Debug.LogFormat("OnMapCharacterLeave", response.mapId, response.Characters.Count);
            foreach (var cha in response.Characters)  //response.Characters服务器传给客户端的地图在线角色列表
            {
                if (User.Instance.CurrentCharacter.Id == cha.Id)
                { 
                User.Instance.CurrentCharacter = cha;     //小刷新
                
                }
            CharacterManager.Instance.AddCharacter(cha); //将所有在这个地图中的在线角色添加进本地的角色管理器 让你可以在客户端看见这些角色
            }
            if (CurrentMapId != response.mapId)        //进入新地图
            {
                this.EnterMap(response.mapId);             //进入新地图的方法
                this.CurrentMapId = response.mapId;
            }

        }

        private void OnMapCharacterLeave(object sender, MapCharacterLeaveResponse response)
        {
          
        }
        private void EnterMap(int mapId)
        {
            if (DataManager.Instance.Maps.ContainsKey(mapId))
            {
                MapDefine map = DataManager.Instance.Maps[mapId];   //取得资源管理器中所需要的地图定义相关消息
                SceneManager.Instance.LoadScene(map.Resource);     //跳转场景
            }
            else
                Debug.LogErrorFormat("进入新地图错误{0}", mapId);
        }
    }
}
