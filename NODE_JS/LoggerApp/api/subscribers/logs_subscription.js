const { request } = require('universal-rxjs-ajax');
const { interval, of } = require('rxjs');
const { map, catchError } = require('rxjs/operators');

let logs_events_subscribers = {};

module.exports = function(application) {    
    const server_settings = application.config.appsettings.getSettings();
    const currentIntervalInSec = server_settings.request_interval_in_seconds * 1000;
    const requestInterval = interval(currentIntervalInSec);

    server_settings.endpoints.forEach(function(endpoint) {
        logs_events_subscribers[endpoint.name] = requestInterval.subscribe(() => {             
            const customDateDAO = new application.api.models.CustomDateDAO();
            const currentTime = customDateDAO.getNowTimeHHMMSS();
            const rootLogPath = `${server_settings.log.log_location}\\logs`;
      
            application.helpers.functions.checkRootLogPath(rootLogPath)
                .pipe(
                    application.helpers.functions.generateCurrentLogDirIfNotExists(customDateDAO),
                    application.helpers.functions.checkLogFile(endpoint.name),
                    application.helpers.functions.checkLogFileSize(server_settings.log.maximum_log_file_sizeMB)
                )
                .subscribe(logFilename => {
                    const method = endpoint.api.method.toLocaleLowerCase();

                    try {
                        if (method == 'get') {
                            request(endpoint.api)
                                .pipe(
                                    catchError(
                                        error => of(error)
                                            .pipe(
                                                map(e => `[${currentTime}] => ${e}`),
                                                application.helpers.functions.writeInLogFile(logFilename),               
                                            )
                                    ),
                                    map(resp => `[${currentTime}] => ${JSON.stringify(resp.response)}`),
                                    application.helpers.functions.writeInLogFile(logFilename),
                                    // map(resp => resp.response),
                                    // application.helpers.functions.treateDataReceived(logFilename),               
                                )
                                .subscribe({
                                    next(result) {
                                        console.log(`${currentTime} -> ${endpoint.name}: ${result} - Intervalo: ${server_settings.request_interval_in_seconds} segundos`);
                                    },
                                    complete() {
                                        // console.log('');
                                    },
                                    error(msg) {
                                        // console.log(msg);
                                    }
                                });
                        }
                        else if (method == 'post') {
                            request(endpoint.api)
                                .pipe(
                                    catchError(
                                        error => of(error)
                                            .pipe(
                                                map(e => `[${currentTime}] => ${e}`),
                                                application.helpers.functions.writeInLogFile(logFilename),               
                                            )
                                    ),
                                    map(resp => `[${currentTime}] => ${JSON.stringify(resp.response)}`),
                                    application.helpers.functions.writeInLogFile(logFilename),                      
                                )                    
                                .subscribe({
                                    next(result) {
                                        console.log(`${currentTime} -> ${endpoint.name}: ${result} - Intervalo: ${server_settings.request_interval_in_seconds} segundos`);
                                    },
                                    complete() {
                                        // console.log('');
                                    },
                                    error(msg) {
                                        // console.log(msg);
                                    }
                                });
                        }
                    }
                    catch(e) {
                        console.log(e);
                    }
                });
        });
    });

    
    // setTimeout(() => { 
    //     console.log('Terminado');
    //     logs_events_subscribers['Cti'].unsubscribe();
    // }, 10000);
}

// module.exports.stop = function(sub) {
//     sub.unsubscribe();
// }