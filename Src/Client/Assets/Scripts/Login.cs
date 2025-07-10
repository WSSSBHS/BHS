using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Login : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Network.NetClient.Instance.Init("127.0.0.1", 8000);   // 客户端初始化
        Network.NetClient.Instance.Connect();       // 客户端链接


        SkillBridge.Message.NetMessage msg = new SkillBridge.Message.NetMessage();        //客户端发送消息到服务端
        msg.Request = new SkillBridge.Message.NetMessageRequest();
        msg.Request.firstTextRequest = new SkillBridge.Message.FirstTextRequest();
        msg.Request.firstTextRequest.Helloworld = "HelloWorld";

         
        Network.NetClient.Instance.SendMessage(msg);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
