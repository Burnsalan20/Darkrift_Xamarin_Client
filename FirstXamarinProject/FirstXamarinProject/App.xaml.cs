using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FirstXamarinProject.Services;
using FirstXamarinProject.Views;
using FirstXamarinProject.DarkRift;
using System.Threading.Tasks;

using DarkRift.Client;
using DarkRift;
using FirstXamarinProject.ViewModels;

namespace FirstXamarinProject
{
    public partial class App : Application
    {

        public bool Running { get; private set; }
        private Darkrift_Client client;
        bool client_Started = false;

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new MainPage();

            client = new Darkrift_Client();
        //    client.Awake();
        //    client.Start();
              
            client.MessageReceived += Received;
            Start();
        }

        public async void Start()
        {
            // Set gameloop state
            Running = true;

            // Set previous game time
            DateTime _previousGameTime = DateTime.Now;

            while (Running)
            {
                // Calculate the time elapsed since the last game loop cycle
                TimeSpan GameTime = DateTime.Now - _previousGameTime;
                // Update the current previous game time
                _previousGameTime = _previousGameTime + GameTime;
                // Update the game
                client.Update();
                if (!client_Started)
                {
                    client.Start();
                    client_Started = true;
                }

                // Update Game at 60fps
                await Task.Delay(8);
            }
        }
        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        void Received(object sender, MessageReceivedEventArgs e)
        {
            AboutViewModel.instance.SetTitle("79");
            using (Message message = e.GetMessage())
            using (DarkRiftReader reader = message.GetReader())
            {
                AboutViewModel.instance.SetTitle("83");
                if (message.Tag == 0)
                {
                    AboutViewModel.instance.SetTitle("86");
                   // if (reader.Length % 17 != 0)
                    //{
                      //  Debug.LogWarning("Received malformed spawn packet.");
                      //  return;
                    //}

                    while (reader.Position < reader.Length)
                    {
                        String r = reader.ReadString();

                        AboutViewModel.instance.SetTitle(r);
                    }
                }
            }
        }
    }
}
