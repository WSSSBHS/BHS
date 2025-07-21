using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Configuration;

using System.Threading;

using Network;
using GameServer.Services;
using GameServer.Managers;

namespace GameServer
{
    class GameServer
    {
        int X = Properties.Settings.Default.ServerPort;
        Thread thread;
        bool running = false;
        NetService network;
        
        public bool Init()
        {
            network = new NetService();
            network.Init(X);
            DBService.Instance.Init();
            thread = new Thread(new ThreadStart(this.Update));
            DataManager.Instance.Load();
            MapManager.Instance.Init();
            HelloWorldServices.Instance.Init();
            UserService.Instance.Init();

            return true;
        }

        public void Start()
        {
            HelloWorldServices.Instance.Start();



            running = true;
            thread.Start();
            network.Start();
        }


        public void Stop()
        {
            running = false;
            thread.Join();
            network.Stop();
        }

        public void Update()
        {
            while (running)
            {
                Time.Tick();
                Thread.Sleep(100);
                //Console.WriteLine("{0} {1} {2} {3} {4}", Time.deltaTime, Time.frameCount, Time.ticks, Time.time, Time.realtimeSinceStartup);
            }
        }
    }
}
