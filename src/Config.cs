using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remoter
{
    namespace Config
    {
        public class Main
        {
            public string Gateway;
            public string UserName;
            public string Password;
            public List<Computer> Computers;
        }

        
        public class Service
        {
            public int Port;
        }

        public class AuthenticatedService : Service
        {
            public string UserName;
            public string Password;
        }

        public class VNC : AuthenticatedService
        {
        }

        public class RDP : AuthenticatedService
        {
        }

        public class SSH : AuthenticatedService
        {
        }

        public class SCP : AuthenticatedService
        {
        }

        public class Services
        {
            public VNC VNC;
            public RDP RDP;
            public SSH SSH;
            public SCP SCP;

            public Service ByName(string name)
            {
                if( name=="VNC" ) return VNC;
                if( name=="RDP" ) return RDP;
                if( name=="SSH" ) return SSH;
                if( name=="SCP" ) return SCP;
                return null;
            }
        }



        public class Computer
        {
            public string Label;
            public string IP;
            public Services Services;
        }

    }
}
