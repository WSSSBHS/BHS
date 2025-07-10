using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using Common;
using Network;
using SkillBridge.Message;

namespace GameServer.Services
{
    internal class HelloWorldServices:Singleton<HelloWorldServices>
    {
        public void Init()
        {
           
        }
        public void Start()
        {

            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<FirstTextRequest>(this.OnFirstTextRequest);
        
        }
        void OnFirstTextRequest(NetConnection<NetSession>sender, FirstTextRequest request)
        {

            Log.InfoFormat("OnFirstTextRequest:HelloWorld:{0}", request.Helloworld);

        }
        public void Stop()
        {


        }

    }
}
