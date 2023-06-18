// //APVS
// const settings = {
//     request_interval_in_seconds: 5,
//     endpoints: [
//         {
//             name: 'Cti',
//             api: {
//                 url: 'http://172.20.1.44:7399/?funcao=estagentes',
//                 method: 'GET' 
//             }
//         },
//         {
//             name: 'Cti_Backup',
//             api: {
//                 url: 'http://172.20.1.45:7399/?funcao=estagentes',
//                 method: 'GET' 
//             }
//         },
//         {
//             name: 'Filas',
//             api: {
//                 url: 'http://172.20.1.44:7399/?funcao=estfilas', 
//                 method: 'GET' 
//             },
//         },
//         {
//             name: 'Filas_Backup',
//             api: {
//                 url: 'http://172.20.1.45:7399/?funcao=estfilas', 
//                 method: 'GET' 
//             },
//         },
//         {
//             name: 'EServices',
//             api: {
//                 url: 'http://172.20.1.44:5799/?funcao=estagentes',
//                 method: 'GET'
//             }
//         },
//         {
//             name: 'EServices_Backup',
//             api: {
//                 url: 'http://172.20.1.45:5799/?funcao=estagentes',
//                 method: 'GET'
//             }
//         },
//         {
//             name: 'Estatistica',
//             api: {
//                 url: 'http://172.20.1.44:5799/?funcao=estatistica',
//                 method: 'GET'
//             }
//         },
//         {
//             name: 'Estatistica_Backup',
//             api: {
//                 url: 'http://172.20.1.44:5799/?funcao=estatistica',
//                 method: 'GET'
//             }
//         },
//         {
//             name: 'AlcPAAE_TempoRealFilas',
//             api: {
//                 url: 'http://172.20.1.44/Alc_PAAE/Home/fnTempoRealFila',
//                 method: 'POST',
//                 body: {
//                     icId: 1,
//                     parUser: 'ursamaior',
//                     parAplicacao: 4,
//                     parReport: 'trfilas',
//                     parSite: 0,
//                     acao: 1,
//                     sessionId: '046eb603-af27-42d1-972a-9cc82d0bf7bb'
//                 }                
//             }
//         },
//         {
//             name: 'AlcPAAE_TempoRealFilasChat',
//             api: {
//                 url: 'http://172.20.1.44/Alc_PAAE/Home/fnTempoRealFila',
//                 method: 'POST',
//                 body: {
//                     icId: 1,
//                     parUser: 'ursamaior',
//                     parAplicacao: 4,
//                     parReport: 'trfilaschat',
//                     parSite: 0,
//                     acao: 1,
//                     sessionId: '046eb603-af27-42d1-972a-9cc82d0bf7bb'
//                 }                
//             }
//         },
//         {
//             name: 'AlcPAAE_TempoRealAgentes',
//             api: {
//                 url: 'http://172.20.1.44/Alc_PAAE/Home/fnTempoRealAgentes',
//                 method: 'POST',
//                 body: {
//                     icId: 1,
//                     parUser: 'ursamaior',
//                     parAplicacao: 4,
//                     parReport: 'tragentessites',
//                     parSite: 0,
//                     acao: 1,
//                     sessionId: '046eb603-af27-42d1-972a-9cc82d0bf7bb'
//                 }                
//             }
//         }  

//     ],
//     log: {
//         log_location: 'C:\\_PROJECTS\\_DEVELOP\\NodeJS\\LoggerApp',
//         maximum_log_file_sizeMB: 100,
//         keep_last_log_directories: 1
//     },
// }

// //DMCard
// const settings = {
//     request_interval_in_seconds: 5,
//     endpoints: [
//          {
//             name: 'Cti',
//             api: {
//                 url: 'http://10.50.5.46:7399/?funcao=estagentes',
//                 method: 'GET' 
//             }
//         },
//         {
//             name: 'EServices',
//             api: {
//                 url: 'http://10.50.5.46:5799/?funcao=estagentes', 
//                 method: 'GET' 
//             },
//         },
//         {
//             name: 'AlcPAAE_TempoRealAgentes',
//             api: {
//                 url: 'http://10.50.5.50/Alc_PAAE/Home/fnTempoRealAgentes',
//                 method: 'POST',
//                 body: {
//                     icId: 1,
//                     parUser: 'UrsaMaior',
//                     parAplicacao: 4,
//                     parReport: 'tragentes',
//                     parSite: 1,
//                     acao: 1,
//                     sessionId: '679f17fa-9796-411c-8b89-df70becc2f16'
//                 }                
//             }
//         }  
//     ],
//     log: {
//         log_location: 'C:\\_PROJECTS\\_DEVELOP\\NodeJS\\LoggerApp',
//         maximum_log_file_sizeMB: 15,
//         keep_last_log_directories: 1
//     },
// }

//MOVIDA
const settings = {
    request_interval_in_seconds: 5,
    endpoints: [
        // {
        //     name: 'Cti',
        //     api: {
        //         url: 'http://10.219.1.173:7399/?funcao=estagentes',
        //         method: 'GET' 
        //     }
        // },
        {
            name: 'Estatisticas',
            api: {
                url: 'http://10.219.1.173:7399/?funcao=estatistica',
                method: 'GET' 
            }
        },
        // {
        //     name: 'Filas',
        //     api: {
        //         url: 'http://10.219.1.173:7399/?funcao=estfilas', 
        //         method: 'GET' 
        //     },
        // },
        // {
        //     name: 'AlcPAAE_TempoRealFilas',
        //     api: {
        //         url: 'http://10.219.1.74/Alc_PAAE/Home/fnTempoRealFila',
        //         method: 'POST',
        //         body: {
        //             icId: 1,
        //             parUser: 'ronaldo.morais',
        //             parAplicacao: 4,
        //             parReport: 'trfilas',
        //             parSite: 20,
        //             acao: 1,
        //             sessionId: '679f17fa-9796-411c-8b89-df70becc2f16'
        //         }                
        //     }
        // }  
    ],
    log: {
        log_location: 'C:\\_PROJECTS\\_DEVELOP\\NodeJS\\LoggerApp',
        maximum_log_file_sizeMB: 100,
        keep_last_log_directories: 1
    },
}

module.exports.getSettings = function() {
    return settings;
}