A computer provides one or more network services on some ports.
For each configured sevice on each omputer the Remoter estabilishes port forwarding via SSH server running on the gateway computer.

For a computer there can be zero or more consumer applications defined, easily startable by clicking their icon presented as part of the computer record.
The consumer aplication is supposed to use one of the services.

Computer
   IP
   Service1         <---------------  ConsumerApp1, Consumer App2
	 Port	
	 Credentials
   Service2        
	 Port	
	 Credentials
   Service3
	 Port	
	 Credentials
   Service4         <---------------  Consumer App 3
	 Port	
	 Credentials


Remoter knows nothing about the service but its name, port and credentials. These attributes can be used by the apps consuming that service.


Grid - one column per service consumer app instance.
There can be more apps tied to the same service
App represented with an icon.
Icon tooltip shows
  - app name (service name)
  - local port and remote port
  - user name
  - password

  TightVNC Viewer : VNC : 7150 => 5900 : student : Zaq1Xsw2

Clicking the icon launches the app (or puts it to the foreground if already running.)

ComputerName  |  Service consumer apps  
----------------------------------------------------
IOS_1			 [TVNC] [WinSCP] [SSH Client] [RDP Client]