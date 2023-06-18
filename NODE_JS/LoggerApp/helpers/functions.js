const fs = require('fs');
const { Observable } = require('rxjs');
const path = require('path');


function createPipe(operatorFn) {
    return function(source) {
        return new Observable(subscriber => {
            const sub = operatorFn(subscriber)
            source.subscribe({
                next: sub.next,
                error: sub.error || (e => subscriber.error(e)),
                complete: sub.complete || (e => subscriber.complete(e))
            })
        })
    }
}

function checkRootLogPath(rootLogPath) {
    return new Observable(subscriber => {
        try {
            if (!fs.existsSync(rootLogPath)) {
                fs.mkdirSync(rootLogPath, { recursive: true });
            }   
            subscriber.next(rootLogPath);
            subscriber.complete();                
        }
        catch (e) {
            subscriber.error(e);
        }
    });
}

function generateCurrentLogDirIfNotExists(customDateDAO) {
    return createPipe(subscriber => ({
        next(rootLogPath) {
            const fullLogPath = `${rootLogPath}\\${customDateDAO.getStringFullYear()}-${customDateDAO.getStringMonth()}-${customDateDAO.getStringDate()}`;
            try {
                if (!fs.existsSync(fullLogPath)) {
                    fs.mkdirSync(fullLogPath, { recursive: true });
                }   
                subscriber.next(fullLogPath);
                subscriber.complete();                
            }
            catch (e) {
                subscriber.error(e);
            }
        }
    }));
}

function checkLogFile(partialLogFilename) {
    return createPipe(subscriber => ({
        next(currentLogDir) {
            try {                
                let array = [];
                fs
                    .readdirSync(currentLogDir)
                    .sort((a, b) => fs.statSync(currentLogDir +"/"+ a).mtime.getTime() - fs.statSync(currentLogDir +"/"+ b).mtime.getTime())
                    .forEach(logFilename => {
                        if( fs.lstatSync(currentLogDir+"/"+logFilename).isFile() ){
                            if (logFilename.toLocaleLowerCase().startsWith(partialLogFilename.toLocaleLowerCase())) {
                                array.push(logFilename);
                            }
                        }
                    })
                
                const lastLogFilename = array.pop();
                const currentLogFilename = lastLogFilename === undefined ? `${partialLogFilename}_1.log` : lastLogFilename;
                const fullLogFilename = path.join(currentLogDir, currentLogFilename);
                                
                subscriber.next(fullLogFilename);
                subscriber.complete();
            }
            catch(e) {
                subscriber.error(e);
            }
        }
    }));
}

function checkLogFileSize(maxLogFileSizeMB) {
    return createPipe(subscriber => ({
        next(fullLogFilename) {
            const logFileSizeMB = getLogFileSize(fullLogFilename, 'MB');

            if (maxLogFileSizeMB < 5) {
                maxLogFileSizeMB = 5;
            }         
                    
            if (logFileSizeMB > maxLogFileSizeMB) {
                for(let i = 1; i < 200; i++) {
                    let finalWords = `_${i}.log`;
                    if (fullLogFilename.toLocaleLowerCase().endsWith(finalWords)) {
                        let index = i + 1;
                        fullLogFilename = fullLogFilename.replace(finalWords, `_${index}.log`);
                        break;
                    }
                }
            }

            subscriber.next(fullLogFilename);
            subscriber.complete();
        }
    })); 
}

function getLogFileSize(fullfilename, sizein = '') {
    let fileSize = 0;
    if (fs.existsSync(fullfilename)) {
        var stats = fs.statSync(fullfilename)
        fileSize = stats["size"]
    
        if (sizein == 'KB') {
            fileSize = fileSize / 1000.0
        }
        else if (sizein == 'MB') {
            fileSize = fileSize / 1000000.0
        }
        else if(sizein == 'GB') {
            fileSize = fileSize / 1000000000.0
        }
    }

    return fileSize
}

function writeInLogFile(fullLogFilename) {
    return createPipe(subscriber => ({
        next(jsonData) {
            try {  
                fs.appendFile(fullLogFilename, `${jsonData}\r\n`, function (err) {
                    if (err) return console.log(err);                    
                });
                subscriber.next('Log escrito com sucesso!');
                subscriber.complete();                
            }
            catch (e) {
                subscriber.error(e);
            }
        }
    }));
}

function treateDataReceived(fullLogFilename) {
    var result = '{"estatistica":[';
    return createPipe(subscriber => ({
        next(jsonData) {
            jsonData.estatistica.map(function(data) {
                if (data.dbid_skill > 0) {
                    result += JSON.stringify(data) + ',';
                }                
            });
        },
        complete() {
            result += ']}';
            try {  
                fs.appendFile(fullLogFilename, `${result}\r\n`, function (err) {
                    if (err) return console.log(err);                    
                });
                subscriber.next('Log escrito com sucesso!');
                subscriber.complete();                
            }
            catch (e) {
                subscriber.error(e);
            }

        }
    }));
}

function getLogsDirs(rootLogPath) {
    return new Observable(subscriber => {
        try {                
            if (fs.existsSync(rootLogPath)) {
                let array = [];
    
                fs
                    .readdirSync(rootLogPath)
                    .sort((a, b) => fs.statSync(rootLogPath +"/"+ a).mtime.getTime() - fs.statSync(rootLogPath +"/"+ b).mtime.getTime())
                    .forEach(logFilename => {
                        array.push(path.join(rootLogPath, logFilename));
                    });
    
                subscriber.next(array);
                subscriber.complete();
            }
        }
        catch(e) {
            subscriber.error(e);
        }
    });
}

function purgeLogDirs(numberOfDirsToKeep) {
    return createPipe(subscriber => ({
        next(logDir) {
            try {
                const qtDirs = logDir.length - (numberOfDirsToKeep - 1);

                if (qtDirs > 0) {
                    for(let i = 0; i < qtDirs; i++) {
                        fs.rmSync(logDir[i], { recursive: true, force: true });
                    }
    
                    subscriber.next(`Logs apagados com sucesso!`);
                }
                subscriber.complete();
            }
            catch(e) {
                subscriber.error(e);
            }            
        }
    }));
}

module.exports = {
    checkRootLogPath,
    generateCurrentLogDirIfNotExists,
    checkLogFile,
    checkLogFileSize,
    writeInLogFile,
    getLogsDirs,
    purgeLogDirs,
    treateDataReceived
}