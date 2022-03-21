namespace Remoter
{
	public class Service
    {
        public Config.Service Conf;
        
        /// <summary>
        /// IP address to be ised used to connect to the service;
        /// Localhost 127.0.0.1 if the computers is behind a gateway;
        /// Otherwise the IP of the remote computer
        /// </summary>
        public string IP; // IP to use to connect to the service

        /// <summary>
        /// Port to be used to connect to the service;
        /// Local forwarded port if the computers is behind a gateway used with local IP 127.0.0.1:PORT;
        /// otherwise the usual port for that service what port number we will use when connecting via 
        /// </summary>
        public int Port;
        
    }
}
