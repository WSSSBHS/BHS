using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Network;
using SkillBridge.Message;
using GameServer.Entities;
using GameServer.Managers;

namespace GameServer.Services
{
    class UserService : Singleton<UserService>
    {

        public UserService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserLoginRequest>(this.OnLogin);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserRegisterRequest>(this.OnRegister);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserCreateCharacterRequest>(this.OnCreateCharacter); // 接受并处理客户端发过来的消息
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserGameEnterRequest>(this.OnGameEnter);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserGameLeaveRequest>(this.OnGameLeave);
           
        }

       
        public void Init()
        {

        }
        
        void OnLogin(NetConnection<NetSession> sender, UserLoginRequest request)
        {
            Log.InfoFormat("UserLoginRequest: User:{0}  Pass:{1}", request.User, request.Passward);

            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();
            message.Response.userLogin = new UserLoginResponse();


            TUser user = DBService.Instance.Entities.Users.Where(u => u.Username == request.User).FirstOrDefault();
            if (user == null)
            {
                message.Response.userLogin.Result = Result.Failed;
                message.Response.userLogin.Errormsg = "用户不存在";
            }
            else if (user.Password != request.Passward)
            {
                message.Response.userLogin.Result = Result.Failed;
                message.Response.userLogin.Errormsg = "密码错误";
            }
            else
            {
                sender.Session.User = user;      //   登录成功获取到user信息,与Session对照找到数据库中的user信息

                message.Response.userLogin.Result = Result.Success;
                message.Response.userLogin.Errormsg = "None";
                message.Response.userLogin.Userinfo = new NUserInfo();
                message.Response.userLogin.Userinfo.Id = 1;
                message.Response.userLogin.Userinfo.Player = new NPlayerInfo();
                message.Response.userLogin.Userinfo.Player.Id = user.Player.ID;
                foreach (var c in user.Player.Characters)
                {
                    NCharacterInfo info = new NCharacterInfo();
                    info.Id = c.ID;
                    info.Name = c.Name;
                    info.Class = (CharacterClass)c.Class;
                    message.Response.userLogin.Userinfo.Player.Characters.Add(info);
                }

            }
            byte[] data = PackageHandler.PackMessage(message);
            sender.SendData(data, 0, data.Length);
        }

        void OnRegister(NetConnection<NetSession> sender, UserRegisterRequest request)
        {
            Log.InfoFormat("UserRegisterRequest: User:{0}  Pass:{1}", request.User, request.Passward);

            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();
            message.Response.userRegister = new UserRegisterResponse();


            TUser user = DBService.Instance.Entities.Users.Where(u => u.Username == request.User).FirstOrDefault();
            if (user != null)
            {
                message.Response.userRegister.Result = Result.Failed;
                message.Response.userRegister.Errormsg = "用户已存在.";
            }
            else
            {
                TPlayer player = DBService.Instance.Entities.Players.Add(new TPlayer());
                DBService.Instance.Entities.Users.Add(new TUser() { Username = request.User, Password = request.Passward, Player = player });
                DBService.Instance.Entities.SaveChanges();
                message.Response.userRegister.Result = Result.Success;
                message.Response.userRegister.Errormsg = "None";
            }

            byte[] data = PackageHandler.PackMessage(message);
            sender.SendData(data, 0, data.Length);
        }
        void OnCreateCharacter(NetConnection<NetSession> sender, UserCreateCharacterRequest request) 
        {
            Log.InfoFormat("UserCreateCharacterRequest: User:{0}  Pass:{1}", request.Name, request.Class);

         
            TCharacter character = new TCharacter();
            {
                character.Name = request.Name;                       //将客户端输入的信息放入表中  C3
                character.Class = (int)request.Class;
                character.TID = (int)request.Class;
                character.MapID = 1;
                character.MapPosX = 5000;
                character.MapPosY = 4000; 
                character.MapPosZ = 820;
       
            }
            character = DBService.Instance.Entities.Characters.Add(character);  // 将刚才构建的表放入数据库
            sender.Session.User.Player.Characters.Add(character);  // 将信息填入到User中去,       session用于区分客户端信息 ,需要在登录时确定session(session中存储了用户的信息)
            DBService.Instance.Entities.SaveChanges();           //保持 任何修改都要加这一个栏

            NetMessage message = new NetMessage();                      //发送消息到客户端
            message.Response = new NetMessageResponse();
            message.Response.createChar = new UserCreateCharacterResponse();               
            message.Response.createChar.Result = Result.Success;       //创建成功的消息
            message.Response.createChar.Errormsg = "None";          // 没有发生错误的消息

            foreach (var c in sender.Session.User.Player.Characters)
            { 
            NCharacterInfo info = new NCharacterInfo();
                info.Name = c.Name;
                info.Tid =  c.TID;
                info.Id = c.ID;
                info.Class = (CharacterClass)c.Class;
            message.Response.createChar.Characters.Add(info);
            
            }
            byte[] data = PackageHandler.PackMessage(message);
            sender.SendData(data, 0, data.Length);
        }
        private void OnGameEnter(NetConnection<NetSession> sender, UserGameEnterRequest request)
        {
            TCharacter dbchar = sender.Session.User.Player.Characters.ElementAt(request.characterIdx);  //获取选择的具体角色信息
            Log.InfoFormat("UserCreateCharacterRequest: character:{0} [1] map:{2}", dbchar.ID,dbchar.Name,dbchar.MapID);
            Character character = CharacterManager.Instance.AddCharacter(dbchar);        //将角色添加进入角色管理器             M3

            NetMessage message = new NetMessage();                      //发送响应消息到客户端
            message.Response = new NetMessageResponse();
            message.Response.gameEnter = new UserGameEnterResponse();
            message.Response.gameEnter.Result = Result.Success;       
            message.Response.gameEnter.Errormsg = "None";

            byte[] data = PackageHandler.PackMessage(message);
            sender.SendData(data, 0, data.Length);
            sender.Session.Character = character;
            MapManager.Instance[dbchar.MapID].CharacterEnter(sender, character); //将角色传入地图管理器        M4
        }
        private void OnGameLeave(NetConnection<NetSession> sender, UserGameLeaveRequest message)
        {
            throw new NotImplementedException();
        }

    }
}
