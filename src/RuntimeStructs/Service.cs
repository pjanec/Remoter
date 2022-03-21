namespace Remoter
{
	public class Service
    {
        public Service(
            Computer computer,
            string localIP,
            int localPort,
            string remoteIP,
            int remotePort )
        {
            _Computer = computer;
        
            NativeIP = localIP;
            NativePort = localPort;

            FwdIP = remoteIP;
            FwdPort = remotePort;
        }




        public Config.Service Conf;

        public string Name => Conf.Name;
        
        /// <summary>
        /// IP address to be ised used to connect to the service;
        /// Localhost 127.0.0.1 if the computers is behind a gateway;
        /// Otherwise the IP of the remote computer
        /// </summary>
        public string IP => _Computer.IsRemote ? FwdIP :NativeIP;

        /// <summary>
        /// Port to be used to connect to the service;
        /// Local forwarded port if the computers is behind a gateway used with local IP 127.0.0.1:PORT;
        /// otherwise the usual port for that service what port number we will use when connecting via 
        /// </summary>
        public int Port => _Computer.IsRemote ? FwdPort :NativePort;

        public string UserName;

        public string Password;

        
        Computer _Computer;
        
        public string NativeIP; // non-forwarded
        public int NativePort; // non-forwarded

        public string FwdIP;
        public int FwdPort;
        
    }
}
