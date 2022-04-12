using FirstXamarinProject.DarkRift;

using DarkRift;
using DarkRift.Client;
using DarkRift.Dispatching;
using System;

using System.Net;
using System.Net.Sockets;

namespace FirstXamarinProject.DarkRift
{
    public sealed class Darkrift_Client
    {
        /// <summary>
        ///     The IP address this client connects to.
        /// </summary>
        public IPAddress Address
        {
            get { return IPAddress.Parse(address); }
            set { address = value.ToString(); }
        }


        // The address of the server to connect to.
        //private string address = IPAddress.Loopback.ToString();                 // Unity requires a serializable backing field so use string
        private String address = "157.245.92.93";


        /// <summary>
        ///     The port this client connects to.
        /// </summary>
        public ushort Port
        {
            get { return port; }
            set { port = value; }
        }

        // The port on the server the client will connect to.
        private ushort port = 4296;

        // Whether to disable Nagel's algorithm or not.
#pragma warning disable IDE0044 // Add readonly modifier, Unity can't serialize readonly fields
        private bool noDelay = false;

        // Indicates whether the client will connect to the server in the Start method.
        private bool autoConnect = true;

        // Specifies whether DarkRift should log all data to the console.
        private volatile bool sniffData = false;
#pragma warning restore IDE0044 // Add readonly modifier
        #region Cache settings
        #region Legacy
        /// <summary>
        ///     The maximum number of <see cref="DarkRiftWriter"/> instances stored per thread.
        /// </summary>
        [Obsolete("Use the ObjectCacheSettings property instead.")]
        public int MaxCachedWriters
        {
            get
            {
                return ObjectCacheSettings.MaxWriters;
            }
        }

        /// <summary>
        ///     The maximum number of <see cref="DarkRiftReader"/> instances stored per thread.
        /// </summary>
        [Obsolete("Use the ObjectCacheSettings property instead.")]
        public int MaxCachedReaders
        {
            get
            {
                return ObjectCacheSettings.MaxReaders;
            }
        }

        /// <summary>
        ///     The maximum number of <see cref="Message"/> instances stored per thread.
        /// </summary>
        [Obsolete("Use the ObjectCacheSettings property instead.")]
        public int MaxCachedMessages
        {
            get
            {
                return ObjectCacheSettings.MaxMessages;
            }
        }

        /// <summary>
        ///     The maximum number of <see cref="System.Net.Sockets.SocketAsyncEventArgs"/> instances stored per thread.
        /// </summary>
        [Obsolete("Use the ObjectCacheSettings property instead.")]
        public int MaxCachedSocketAsyncEventArgs
        {
            get
            {
                return ObjectCacheSettings.MaxSocketAsyncEventArgs;
            }
        }

        /// <summary>
        ///     The maximum number of <see cref="ActionDispatcherTask"/> instances stored per thread.
        /// </summary>
        [Obsolete("Use the ObjectCacheSettings property instead.")]
        public int MaxCachedActionDispatcherTasks
        {
            get
            {
                return ObjectCacheSettings.MaxActionDispatcherTasks;
            }
        }
        #endregion Legacy

        /// <summary>
        ///     The object cache settings in use.
        /// </summary>
        public ObjectCacheSettings ObjectCacheSettings { get; set; }

        /// <summary>
        ///     Serialisable version of the object cache settings for Unity.
        /// </summary>

#pragma warning disable IDE0044 // Add readonly modifier, Unity can't serialize readonly fields
        private DarkRift.SerializableObjectCacheSettings objectCacheSettings = new SerializableObjectCacheSettings();
#pragma warning restore IDE0044 // Add readonly modifier, Unity can't serialize readonly fields
        #endregion

        /// <summary>
        ///     Event fired when a message is received.
        /// </summary>
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        /// <summary>
        ///     Event fired when we disconnect form the server.
        /// </summary>
        public event EventHandler<DisconnectedEventArgs> Disconnected;

        /// <summary>
        ///     The ID the client has been assigned.
        /// </summary>
        public ushort ID
        {
            get
            {
                return Client.ID;
            }
        }

        /// <summary>
        ///     Returns whether or not this client is connected to the server.
        /// </summary>
        [Obsolete("User ConnectionState instead.")]
        public bool Connected
        {
            get
            {
                return Client.Connected;
            }
        }


        /// <summary>
        ///     Returns the state of the connection with the server.
        /// </summary>
        public ConnectionState ConnectionState
        {
            get
            {
                return Client.ConnectionState;
            }
        }

        /// <summary>
        /// 	The actual client connecting to the server.
        /// </summary>
        /// <value>The client.</value>
        public DarkRiftClient Client { get; private set; }

        /// <summary>
        ///     The dispatcher for moving work to the main thread.
        /// </summary>
        public Dispatcher Dispatcher { get; private set; }

        public Darkrift_Client()
        {
            //ObjectCacheSettings = objectCacheSettings.ToObjectCacheSettings();

            Client = new DarkRiftClient(2, 2, 4, 32, 16);

            //Setup dispatcher
            Dispatcher = new Dispatcher(true);

            //Setup routing for events
            Client.MessageReceived += Client_MessageReceived;
            Client.Disconnected += Client_Disconnected;
        }

        public void Start()
        {
            //If auto connect is true then connect to the server
            if (autoConnect)
                Connect(Address, port, noDelay);
        }

        public void Update()
        {
            //Execute all the queued dispatcher tasks
            Dispatcher.ExecuteDispatcherTasks();
        }

        private void OnDestroy()
        {
            //Remove resources
            Close();
        }

        private void OnApplicationQuit()
        {
            //Remove resources
            Close();
        }

        /// <summary>
        ///     Connects to a remote server.
        /// </summary>
        /// <param name="ip">The IP address of the server.</param>
        /// <param name="port">The port of the server.</param>
        [Obsolete("Use other Connect overloads that automatically detect the IP version.")]
        public void Connect(IPAddress ip, int port, IPVersion ipVersion)
        {
            Client.Connect(ip, port, ipVersion);

            if (ConnectionState == ConnectionState.Connected)
                Console.WriteLine("Connected to " + ip + " on port " + port + " using " + ipVersion + ".");
            else
                Console.WriteLine("Connection failed to " + ip + " on port " + port + " using " + ipVersion + ".");
        }

        /// <summary>
        ///     Connects to a remote server.
        /// </summary>
        /// <param name="ip">The IP address of the server.</param>
        /// <param name="port">The port of the server.</param>
        /// <param name="noDelay">Whether to disable Nagel's algorithm or not.</param>
        public void Connect(IPAddress ip, int port, bool noDelay)
        {
            if(Client != null)
                Client.Connect(IPAddress.Parse(address), 4296, false);

            if (ConnectionState == ConnectionState.Connected)
                Console.WriteLine("Connected to " + ip + " on port " + port + ".");
            else
                Console.WriteLine("Connection failed to " + ip + " on port " + port + ".");
        }

        /// <summary>
        ///     Connects to a remote server.
        /// </summary>
        /// <param name="ip">The IP address of the server.</param>
        /// <param name="tcpPort">The port the server is listening on for TCP.</param>
        /// <param name="udpPort">The port the server is listening on for UDP.</param>
        /// <param name="noDelay">Whether to disable Nagel's algorithm or not.</param>
        public void Connect(IPAddress ip, int tcpPort, int udpPort, bool noDelay)
        {
            Client.Connect(ip, tcpPort, udpPort, noDelay);

            if (ConnectionState == ConnectionState.Connected)
                Console.WriteLine("Connected to " + ip + " on port " + tcpPort + "|" + udpPort + ".");
            else
                Console.WriteLine("Connection failed to " + ip + " on port " + tcpPort + "|" + udpPort + ".");
        }

        /// <summary>
        ///     Connects to a remote asynchronously.
        /// </summary>
        /// <param name="ip">The IP address of the server.</param>
        /// <param name="port">The port of the server.</param>
        /// <param name="callback">The callback to make when the connection attempt completes.</param>
        [Obsolete("Use other ConnectInBackground overloads that automatically detect the IP version.")]
        public void ConnectInBackground(IPAddress ip, int port, IPVersion ipVersion, DarkRiftClient.ConnectCompleteHandler callback = null)
        {
            Client.ConnectInBackground(
                ip,
                port,
                ipVersion,
                delegate (Exception e)
                {
                    if (callback != null)
                        callback.Invoke(e);

                    if (ConnectionState == ConnectionState.Connected)
                        Console.WriteLine("Connected to " + ip + " on port " + port + " using " + ipVersion + ".");
                    else
                        Console.WriteLine("Connection failed to " + ip + " on port " + port + " using " + ipVersion + ".");
                }
            );
        }

        /// <summary>
        ///     Connects to a remote asynchronously.
        /// </summary>
        /// <param name="ip">The IP address of the server.</param>
        /// <param name="port">The port of the server.</param>
        /// <param name="noDelay">Whether to disable Nagel's algorithm or not.</param>
        /// <param name="callback">The callback to make when the connection attempt completes.</param>
        public void ConnectInBackground(IPAddress ip, int port, bool noDelay, DarkRiftClient.ConnectCompleteHandler callback = null)
        {
            Client.ConnectInBackground(
                ip,
                port,
                noDelay,
                delegate (Exception e)
                {
                    if (callback != null)
                        callback.Invoke(e);

                    if (ConnectionState == ConnectionState.Connected)
                        Console.WriteLine("Connected to " + ip + " on port " + port + ".");
                    else
                        Console.WriteLine("Connection failed to " + ip + " on port " + port + ".");
                }
            );
        }

        /// <summary>
        ///     Connects to a remote asynchronously.
        /// </summary>
        /// <param name="ip">The IP address of the server.</param>
        /// <param name="tcpPort">The port the server is listening on for TCP.</param>
        /// <param name="udpPort">The port the server is listening on for UDP.</param>
        /// <param name="noDelay">Whether to disable Nagel's algorithm or not.</param>
        /// <param name="callback">The callback to make when the connection attempt completes.</param>
        public void ConnectInBackground(IPAddress ip, int tcpPort, int udpPort, bool noDelay, DarkRiftClient.ConnectCompleteHandler callback = null)
        {
            Client.ConnectInBackground(
                ip,
                tcpPort,
                udpPort,
                noDelay,
                delegate (Exception e)
                {
                    if (callback != null)
                        callback.Invoke(e);

                    if (ConnectionState == ConnectionState.Connected)
                        Console.WriteLine("Connected to " + ip + " on port " + tcpPort + "|" + udpPort + ".");
                    else
                        Console.WriteLine("Connection failed to " + ip + " on port " + tcpPort + "|" + udpPort + ".");
                }
            );
        }

        /// <summary>
        ///     Sends a message to the server.
        /// </summary>
        /// <param name="message">The message template to send.</param>
        /// <returns>Whether the send was successful.</returns>
        public bool SendMessage(Message message, SendMode sendMode)
        {
            return Client.SendMessage(message, sendMode);
        }

        /// <summary>
        ///     Invoked when DarkRift receives a message from the server.
        /// </summary>
        /// <param name="sender">THe client that received the message.</param>
        /// <param name="e">The arguments for the event.</param>
        private void Client_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            if (sniffData)
                Console.WriteLine("Message Received");      //TODO more information!

            EventHandler<MessageReceivedEventArgs> handler = MessageReceived;
            if (handler != null)
            {
                handler.Invoke(sender, e);
            }
        }

        private void Client_Disconnected(object sender, DisconnectedEventArgs e)
        {
            if (!e.LocalDisconnect)
                Console.WriteLine("Disconnected from server, error: " + e.Error);

            EventHandler<DisconnectedEventArgs> handler = Disconnected;
            if (handler != null)
            {
                handler.Invoke(sender, e);
            }
        }

        /// <summary>
        ///     Disconnects this client from the server.
        /// </summary>
        /// <returns>Whether the disconnect was successful.</returns>
        public bool Disconnect()
        {
            return Client.Disconnect();
        }

        /// <summary>
        ///     Closes this client.
        /// </summary>
        public void Close()
        {
            Client.MessageReceived -= Client_MessageReceived;
            Client.Disconnected -= Client_Disconnected;

            Client.Dispose();
            Dispatcher.Dispose();
        }
    }
}
