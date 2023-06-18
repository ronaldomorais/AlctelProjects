//Utilizar os comandos abaixo para instalar a aplicação node como service no windows:
//	
//    npm install -g node-windows
//    
//    npm link node-windows

var Service = require('node-windows').Service;

// Create a new service object
var svc = new Service({
  name:'Node application as Windows Service',
  description: 'Node application as Windows Service',
  script: 'C:\\_PROJECTS\\DEMO_PROJETOS\\NODE_JS\\LoggerApp\\app.js'
});

// Listen for the "install" event, which indicates the
// process is available as a service.
svc.on('install',function(){
  svc.start();
});

svc.install();