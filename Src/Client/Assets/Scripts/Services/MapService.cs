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
            foreach (var cha in response.Characters)  //response.Characters�����������ͻ��˵ĵ�ͼ���߽�ɫ�б�
            {
                if (User.Instance.CurrentCharacter.Id == cha.Id)
                { 
                User.Instance.CurrentCharacter = cha;     //Сˢ��
                
                }
            CharacterManager.Instance.AddCharacter(cha); //�������������ͼ�е����߽�ɫ��ӽ����صĽ�ɫ������ ��������ڿͻ��˿�����Щ��ɫ
            }
            if (CurrentMapId != response.mapId)        //�����µ�ͼ
            {
                this.EnterMap(response.mapId);             //�����µ�ͼ�ķ���
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
                MapDefine map = DataManager.Instance.Maps[mapId];   //ȡ����Դ������������Ҫ�ĵ�ͼ���������Ϣ
                SceneManager.Instance.LoadScene(map.Resource);     //��ת����
            }
            else
                Debug.LogErrorFormat("�����µ�ͼ����{0}", mapId);
        }
    }
}
