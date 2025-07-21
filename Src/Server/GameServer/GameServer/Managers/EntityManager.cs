using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Entities;

namespace GameServer.Managers
{
    internal class EntityManager :Singleton<EntityManager>
    {
        private int idx = 0;
        public List<Entity> AllEnities = new List<Entity>();
        public Dictionary <int , List <Entity>> MapEntities = new Dictionary<int , List <Entity>>();


        public void AddEntity(int mapId, Entity entity)
        { 
        AllEnities.Add(entity);
            //加入管理器生成唯一项
            entity.EntityData.Id = ++this.idx;

            List <Entity> entities = null;
            if (!MapEntities.TryGetValue(mapId, out entities))
            { 
                entities = new List<Entity>();
                MapEntities[mapId] = entities;
            }
            entities.Add(entity);
        
        }
        public void RemoveEntity(int mapId, Entity entity)
        {
            AllEnities.Remove(entity);
            MapEntities[mapId].Remove(entity);
        }
    }

}
