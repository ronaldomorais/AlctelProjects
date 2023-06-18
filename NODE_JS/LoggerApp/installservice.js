//Utilizar os comandos abaixo para instalar a aplicação node como service no windows:
//	
//    Antes era usadi o node-windows
//    npm install -g node-windows
//    
//    npm link node-windows
//
//    Agora usamos o qckwinsvc
//    npm install -g qckwinsvc
//
//    qckwinsvc

//    prompt: Service name: [name for your service]
//    prompt: Service description: [description for it]
//    prompt: Node script path: [path of your node script]
//    Service installed



// Exemplo real:

// > qckwinsvc
// prompt: Service name: Hello
// prompt: Service description: Greets the world
// prompt: Node script path: C:\my\folder\hello.js
// prompt: Should the service get started immediately? (y/n): y
// Service installed.
// Service started.


// 	qckwinsvc --uninstall
	
// 	prompt: Service name: [name of your service]
// 	prompt: Node script path: [path of your node script]
// 	Service stopped
// 	Service uninstalled

var Service = require('node-windows').Service;

// Create a new service object
var svc = new Service({
  name:'LoggerApp',
  description: 'Logger Application',
  script: 'C:\\_PROJECTS\\DEMO_PROJETOS\\NODE_JS\\LoggerApp\\app.js'
});

// Listen for the "install" event, which indicates the
// process is available as a service.
svc.on('install',function(){
  svc.start();
});

svc.install();