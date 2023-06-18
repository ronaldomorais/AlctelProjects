const { interval } = require('rxjs');
const { map } = require('rxjs/operators');

module.exports = function(application) {
    const server_settings = application.config.appsettings.getSettings();
    const logsDeleteIntervalInSeconds = 20;
    const logsDeleteInterval = interval(logsDeleteIntervalInSeconds * 1000);
    
    const sub2 = logsDeleteInterval.subscribe(() => {
        const customDateDAO = new application.api.models.CustomDateDAO();
        const currentTime = customDateDAO.getNowTimeHHMMSS();
        const rootLogPath = `${server_settings.log.log_location}\\logs`;
        const currentLogDir = customDateDAO.getYYYYMMDD();   
        
        application.helpers.functions.getLogsDirs(rootLogPath)
            .pipe(
                map(dirs => dirs.filter(d => !d.endsWith(currentLogDir))),
                application.helpers.functions.purgeLogDirs(server_settings.log.keep_last_log_directories),
            )
            .subscribe(result => console.log(`${currentTime} -> ${result}`));
    });

    // sub2.unsubscribe();
}