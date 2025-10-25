const origin_url = 'https://localhost:7011'
//const origin_url = 'https://services-prd.contactfy.cloud/GestaoTicketSESC'
//const origin_url = 'https://sesc.contactfy.cloud/GestaoTicketHML'
//const origin_url = 'https://contactfydevelop.alctel.com.br/GestaoTicketSesc'
//const origin_url = 'https://contactfydevelop.alctel.com.br/GestaoTicketDEV'
//const origin_url = 'https://localhost:5240'
//const origin_url = 'https://localhost:7011'

var attached_files = []
let checkOnInteractionAlertInfoTimer = undefined;

var tools = {
    HelpMethods: {

        EnableFormCancelButton: function () {
            $(".cancel-form").on('click', function () {

                const href = window.location.href;
                let conversationid = '';
                if (href.includes('#conversation_id=')) {

                    const hrefArray = href.split('#');
                    const conversationidParam = hrefArray[1];
                    conversationid = conversationidParam.replace('conversation_id=', '')
                }

                let origin = window.location.origin
                let pathname = window.location.pathname

                var pathnameSplitted = pathname.split('/')

                var redirectTo = "";
                if (pathnameSplitted.length == 3 || pathnameSplitted.length == 4)
                    redirectTo = `${origin}/${pathnameSplitted[1]}/Index`
                else
                    redirectTo = `${origin}/${pathnameSplitted[1]}/${pathnameSplitted[2]}/Index`

                if (conversationid !== '') {
                    redirectTo = `${redirectTo}#conversation_id=${conversationid}`
                }

                window.location.href = redirectTo
            })
        },

        EnableViewPasswordButton: function () {

            $('#showPwdId').on('click', function () {

                let openeyedsp = $('#openeyeId').css('display');

                if (openeyedsp == 'none') {
                    $('#openeyeId').show()
                }
                else {
                    $('#openeyeId').hide()
                }

                let closedeyedsp = $('#closedeyeId').css('display');

                if (closedeyedsp == 'none') {
                    $('#closedeyeId').show()
                }
                else {
                    $('#closedeyeId').hide()
                }

                //alert(openeyedsp + ' - ' + closedeyedsp)

                //$('#openeyeId').toggle()
                //$('#closedeyeId').toggle()

                let passwordEl = $('#Password').attr('type');

                if (passwordEl === 'password') {
                    $('#Password').attr('type', 'text');
                }
                else {
                    $('#Password').attr('type', 'password');
                }
            })
        },

        FormatField: function (mask, element) {
            let value = element.value
            let length = value.length
            let lastvalue = value.substring(length - 1, length)
            let maskchar = mask.substring(length - 1, length)
            console.log(length, lastvalue, maskchar, value)

            if (isNaN(lastvalue)) {
                element.value = value.substring(0, length - 1)
                return
            }

            if (maskchar != '#') {
                let newvalue = `${value.substring(0, length - 1)}${maskchar}${lastvalue}`
                element.value = newvalue
            }

            //else {
            //    console.log('test2')
            //    element.value = value.substring(0, length - 1)
            //}

            //if (length > 14) {
            //    return
            //}

            //let maskchar = mask.substring(length - 1, length)
            //let valuechar = value.substring(length - 1, length)

            //console.log(length, maskchar, valuechar, value)

            //if (maskchar == '#') {
            //    element.value = value
            //}
            //else {
            //    var newvalue = ''
            //    let i = 0
            //    Array.from(value).forEach(char => {
            //        i++;
            //        if (i == length) {
            //            if (maskchar != char) {
            //                newvalue = `${newvalue}${maskchar}${char}`
            //            }
            //        }
            //        else {
            //            newvalue = `${newvalue}${char}`
            //        }
            //    })
            //    element.value = newvalue
            //}
        },

        SubmitPageControlForm: function () {

            $(".btnpagecontrol").on('click', function (event) {
                event.preventDefault();
                $(".btnpagecontrol").prop('disabled', true)
                const inputAttr = event.target.getAttribute('name');
                console.log(inputAttr)

                $('#' + inputAttr).submit();
            })

            $(".pagecontrollink").on('click', function (event) {
                $(".pagecontrollink").prop('disabled', true)
            })
        },

        ChangeStatusSendForm: function sendForm(element) {
            let url = `${window.location.href}/Edit`

            $.post(url, { Id: element.id, Name: element.name, Active: $('#' + element.id).is(':checked') },
                function (data, status) {
                },
                null,
                "json"
            );
        },

        InputLetterOnly: function (evt) {
            evt = (evt) ? evt : event;
            var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
                ((evt.which) ? evt.which : 0));

            if (charCode != 32 && charCode > 31 && (charCode < 65 || charCode > 90) &&
                (charCode < 97 || charCode > 122)) {
                //alert("Enter letters only.");
                return false;
            }
            return true;
        },

        OnInteracion: function () {
            console.log('teste')
            $.get(`${origin_url}/Ticket/OnInteraction`, function (data, status) {
                if (status == 'success') {
                    console.log(data)

                    //if (data.OnInteraction) {
                    //    $('#OnInteractionLink').attr('href', `${origin_url}/Ticket/GenesysInteractionEvent/?nomeFila=${data.QueueName}&conversastionid=${data.ConversationId}&email=&cpf=${data.Cpf}&protocolo=${data.Protocol}&navegacao=${data.CustomerNavigation}&reload=true&cancel=false`)
                    //    $('#OnInteractionAlert').show()
                    //}
                }
            })
        },

        CallTicketFromCustomerScreen: function (id) {
            console.log(id, conversationid_ative);

            window.location.href = `${origin_url}/Ticket/Edit/?id=${id}`
        }
    }
}

var select2treeHandler = {
    Init: {
        ClassificationDemandLoad: function () {
            $("#ClassificationDemand_ClassificationDemandName").select2ToTree({
                // treeData: {
                //     dataArr:mydata
                // },
                // maximumSelectionLength: 3,
                placeholder: "Opções...",
            });
        },

        ClassificationTypeLoad: function () {
            $("#ClassificationType_ClassificationTypeName").select2ToTree({
                // treeData: {
                //     dataArr:mydata
                // },
                // maximumSelectionLength: 3,
                placeholder: "Opções...",
            });
        },

        ClassificationReasonLoad: function () {
            $("#ClassificationReason_ClassificationReasonName").select2ToTree({
                // treeData: {
                //     dataArr:mydata
                // },
                // maximumSelectionLength: 3,
                placeholder: "Opções...",
            });
        },

        ClassificationSubReason01Load: function () {
            $("#ClassificationSubReason01_ClassificationSubReason01Name").select2ToTree({
                // treeData: {
                //     dataArr:mydata
                // },
                // maximumSelectionLength: 3,
                placeholder: "Opções...",
            });
        },

        ClassificationSubReason02Load: function () {
            $("#ClassificationSubReason02_ClassificationSubReason02Name").select2ToTree({
                // treeData: {
                //     dataArr:mydata
                // },
                // maximumSelectionLength: 3,
                placeholder: "Opções...",
            });
        },
    }
}

var dataTablesHandler = {
    Init: {
        UserDataTableLoad: function () {
            DataTable.ext.errMode = 'none';
            $('#userDataTable').DataTable({


                /*
                
                dom: PBlftipr
                    f - adiciona pesquisa
                    t - posiciona paginação abaixo da tabela
                    i - adiciona texto de quantidade de páginas e registros
                    p - adiciona paginação
                    l - adiciona select para quantidade de registro por página
                */
                pageLength: 100,
                dom: 'PB',
                //layout: {
                //    topStart: 'pageLength',
                //    topEnd: 'search',
                //    bottomStart: 'info',
                //    bottomEnd: 'paging'
                //},
                //searchPanes: {
                //    cascadePanes: true,
                //    initCollapsed: true,
                //    show: true,
                //    dtOpts: {
                //        dom: 'tp',
                //        paging: 'true',
                //        pagingType: 'simple',
                //        searching: true
                //    }
                //},
                language: {
                    //url: "//cdn.datatables.net/plug-ins/1.11.3/i18n/pt_br.json"
                    url: "lib/datatable/css/pt_br.json"
                },
                //buttons: [
                //    {
                //        extend: 'copyHtml5',
                //        text: 'Copia',
                //        titleAttr: 'Copia',
                //        className: 'btn'
                //    },
                //    {
                //        extend: 'csvHtml5',
                //        text: 'CSV',
                //        titleAttr: 'CSV',
                //        className: 'btn '
                //    },
                //    {
                //        extend: 'excelHtml5',
                //        text: 'Excel',
                //        titleAttr: 'Excel',
                //        className: 'btn '
                //    },
                //    {
                //        extend: 'pdfHtml5',
                //        orientation: 'landscape',
                //        pageSize: 'LEGAL',
                //        text: 'PDF',
                //        titleAttr: 'PDF',
                //        className: 'btn '
                //    },
                //    {
                //        extend: 'print',
                //        text: 'Stampa',
                //        titleAttr: 'Stampa',
                //        className: 'btn '
                //    },
                //    {
                //        extend: "colvis",
                //        postfixButtons: ["colvisRestore"],
                //    }
                //],
                //deferRender: true,
                responsive: true,
                //lengthChange: true,
                //orderCellsTop: true,
                //fixedHeader: true,
                //select: true,
                //processing: true,
                //serverSide: true,
                //stateSave: true,
                //ajax: '{{ route('tickets.index') }}',
                //columns: [
                //    {
                //        data: 'id',
                //        searchPanes: {
                //            show: true
                //        }
                //    },
                //    {
                //        data: 'time',
                //        searchPanes: {
                //            show: true
                //        }
                //    },
                //    {
                //        data: 'priority',
                //        searchPanes: {
                //            show: true
                //        }
                //    },
                //    {
                //        data: 'code',
                //        searchPanes: {
                //            show: true
                //        }
                //    },
                //    {
                //        data: 'category',
                //        searchPanes: {
                //            show: true
                //        }
                //    },
                //    {
                //        data: 'action', name: 'Azioni', orderable: false, searchable: false,
                //        searchPanes: {
                //            show: false
                //        }
                //    },
                //],
                columnDefs: [
                    {
                        searchPanes: {
                            show: true
                        },
                        targets: [0, 1, 2, 3, 4, 5]
                    }
                ]
            });
        },

        CustomerDataTableLoad: function () {
            $('#customerDataTable').DataTable({
                pageLength: 100,
                dom: 'PB',
                //layout: {
                //    topStart: 'pageLength',
                //    topEnd: 'search',
                //    bottomStart: 'info',
                //    bottomEnd: 'paging'
                //},
                //searchPanes: {
                //    cascadePanes: true,
                //    initCollapsed: true,
                //    show: true,
                //    dtOpts: {
                //        dom: 'tp',
                //        paging: 'true',
                //        pagingType: 'simple',
                //        searching: true
                //    }
                //},
                language: {
                    url: "//cdn.datatables.net/plug-ins/1.11.3/i18n/pt_br.json"
                    //url: "lib/datatable/css/pt_br.json"
                },
                //buttons: [
                //    {
                //        extend: 'copyHtml5',
                //        text: 'Copia',
                //        titleAttr: 'Copia',
                //        className: 'btn'
                //    },
                //    {
                //        extend: 'csvHtml5',
                //        text: 'CSV',
                //        titleAttr: 'CSV',
                //        className: 'btn '
                //    },
                //    {
                //        extend: 'excelHtml5',
                //        text: 'Excel',
                //        titleAttr: 'Excel',
                //        className: 'btn '
                //    },
                //    {
                //        extend: 'pdfHtml5',
                //        orientation: 'landscape',
                //        pageSize: 'LEGAL',
                //        text: 'PDF',
                //        titleAttr: 'PDF',
                //        className: 'btn '
                //    },
                //    {
                //        extend: 'print',
                //        text: 'Stampa',
                //        titleAttr: 'Stampa',
                //        className: 'btn '
                //    },
                //    {
                //        extend: "colvis",
                //        postfixButtons: ["colvisRestore"],
                //    }
                //],
                //deferRender: true,
                responsive: true,
                //lengthChange: true,
                //orderCellsTop: true,
                //fixedHeader: true,
                //select: true,
                //processing: true,
                //serverSide: true,
                //stateSave: true,
                //ajax: '{{ route('tickets.index') }}',
                //columns: [
                //    {
                //        data: 'id',
                //        searchPanes: {
                //            show: true
                //        }
                //    },
                //    {
                //        data: 'time',
                //        searchPanes: {
                //            show: true
                //        }
                //    },
                //    {
                //        data: 'priority',
                //        searchPanes: {
                //            show: true
                //        }
                //    },
                //    {
                //        data: 'code',
                //        searchPanes: {
                //            show: true
                //        }
                //    },
                //    {
                //        data: 'category',
                //        searchPanes: {
                //            show: true
                //        }
                //    },
                //    {
                //        data: 'action', name: 'Azioni', orderable: false, searchable: false,
                //        searchPanes: {
                //            show: false
                //        }
                //    },
                //],
                columnDefs: [
                    {
                        searchPanes: {
                            show: true
                        },
                        targets: [0, 1, 2, 3, 4, 5]
                    }
                ]
            });
        },

        AreaDataTableLoad: function () {
            $('#areaDataTable').DataTable({
                pageLength: 100,
                dom: 'PB',
                //layout: {
                //    topStart: 'pageLength',
                //    topEnd: 'search',
                //    bottomStart: 'info',
                //    bottomEnd: 'paging'
                //},
                //searchPanes: {
                //    cascadePanes: true,
                //    initCollapsed: true,
                //    show: true,
                //    dtOpts: {
                //        dom: 'tp',
                //        paging: 'true',
                //        pagingType: 'simple',
                //        searching: true
                //    }
                //},
                language: {
                    url: "//cdn.datatables.net/plug-ins/1.11.3/i18n/pt_br.json"
                    //url: "lib/datatable/css/pt_br.json"
                },
                //buttons: [
                //    {
                //        extend: 'copyHtml5',
                //        text: 'Copia',
                //        titleAttr: 'Copia',
                //        className: 'btn'
                //    },
                //    {
                //        extend: 'csvHtml5',
                //        text: 'CSV',
                //        titleAttr: 'CSV',
                //        className: 'btn '
                //    },
                //    {
                //        extend: 'excelHtml5',
                //        text: 'Excel',
                //        titleAttr: 'Excel',
                //        className: 'btn '
                //    },
                //    {
                //        extend: 'pdfHtml5',
                //        orientation: 'landscape',
                //        pageSize: 'LEGAL',
                //        text: 'PDF',
                //        titleAttr: 'PDF',
                //        className: 'btn '
                //    },
                //    {
                //        extend: 'print',
                //        text: 'Stampa',
                //        titleAttr: 'Stampa',
                //        className: 'btn '
                //    },
                //    {
                //        extend: "colvis",
                //        postfixButtons: ["colvisRestore"],
                //    }
                //],
                //deferRender: true,
                responsive: true,
                //lengthChange: true,
                //orderCellsTop: true,
                //fixedHeader: true,
                //select: true,
                //processing: true,
                //serverSide: true,
                //stateSave: true,
                //ajax: '{{ route('tickets.index') }}',
                //columns: [
                //    {
                //        data: 'id',
                //        searchPanes: {
                //            show: true
                //        }
                //    },
                //    {
                //        data: 'time',
                //        searchPanes: {
                //            show: true
                //        }
                //    },
                //    {
                //        data: 'priority',
                //        searchPanes: {
                //            show: true
                //        }
                //    },
                //    {
                //        data: 'code',
                //        searchPanes: {
                //            show: true
                //        }
                //    },
                //    {
                //        data: 'category',
                //        searchPanes: {
                //            show: true
                //        }
                //    },
                //    {
                //        data: 'action', name: 'Azioni', orderable: false, searchable: false,
                //        searchPanes: {
                //            show: false
                //        }
                //    },
                //],
                //columnDefs: [
                //    {
                //        searchPanes: {
                //            show: true
                //        },
                //        targets: [0, 1, 2, 3, 4, 5]
                //    }
                //]
            });
        },

        ServiceLevelDataTableLoad: function () {
            $('#serviceLevelDataTable').DataTable({
                pageLength: 100,
                dom: 'PB',
                //layout: {
                //    topStart: 'pageLength',
                //    topEnd: 'search',
                //    bottomStart: 'info',
                //    bottomEnd: 'paging'
                //},
                //searchPanes: {
                //    cascadePanes: true,
                //    initCollapsed: true,
                //    show: true,
                //    dtOpts: {
                //        dom: 'tp',
                //        paging: 'true',
                //        pagingType: 'simple',
                //        searching: true
                //    }
                //},
                language: {
                    url: "//cdn.datatables.net/plug-ins/1.11.3/i18n/pt_br.json"
                    //url: "lib/datatable/css/pt_br.json"
                },
                //buttons: [
                //    {
                //        extend: 'copyHtml5',
                //        text: 'Copia',
                //        titleAttr: 'Copia',
                //        className: 'btn'
                //    },
                //    {
                //        extend: 'csvHtml5',
                //        text: 'CSV',
                //        titleAttr: 'CSV',
                //        className: 'btn '
                //    },
                //    {
                //        extend: 'excelHtml5',
                //        text: 'Excel',
                //        titleAttr: 'Excel',
                //        className: 'btn '
                //    },
                //    {
                //        extend: 'pdfHtml5',
                //        orientation: 'landscape',
                //        pageSize: 'LEGAL',
                //        text: 'PDF',
                //        titleAttr: 'PDF',
                //        className: 'btn '
                //    },
                //    {
                //        extend: 'print',
                //        text: 'Stampa',
                //        titleAttr: 'Stampa',
                //        className: 'btn '
                //    },
                //    {
                //        extend: "colvis",
                //        postfixButtons: ["colvisRestore"],
                //    }
                //],
                //deferRender: true,
                responsive: true,
                //lengthChange: true,
                //orderCellsTop: true,
                //fixedHeader: true,
                //select: true,
                //processing: true,
                //serverSide: true,
                //stateSave: true,
                //ajax: '{{ route('tickets.index') }}',
                //columns: [
                //    {
                //        data: 'id',
                //        searchPanes: {
                //            show: true
                //        }
                //    },
                //    {
                //        data: 'time',
                //        searchPanes: {
                //            show: true
                //        }
                //    },
                //    {
                //        data: 'priority',
                //        searchPanes: {
                //            show: true
                //        }
                //    },
                //    {
                //        data: 'code',
                //        searchPanes: {
                //            show: true
                //        }
                //    },
                //    {
                //        data: 'category',
                //        searchPanes: {
                //            show: true
                //        }
                //    },
                //    {
                //        data: 'action', name: 'Azioni', orderable: false, searchable: false,
                //        searchPanes: {
                //            show: false
                //        }
                //    },
                //],
                //columnDefs: [
                //    {
                //        searchPanes: {
                //            show: true
                //        },
                //        targets: [0, 1, 2, 3, 4, 5]
                //    }
                //]
            });
        },

        DemandTypeDataTableLoad: function () {
            $('#demandTypeDataTable').DataTable({
                pageLength: 100,
                dom: 'PB',
                //layout: {
                //    topStart: 'pageLength',
                //    topEnd: 'search',
                //    bottomStart: 'info',
                //    bottomEnd: 'paging'
                //},
                //searchPanes: {
                //    cascadePanes: true,
                //    initCollapsed: true,
                //    show: true,
                //    dtOpts: {
                //        dom: 'tp',
                //        paging: 'true',
                //        pagingType: 'simple',
                //        searching: true
                //    }
                //},
                language: {
                    url: "//cdn.datatables.net/plug-ins/1.11.3/i18n/pt_br.json"
                    //url: "lib/datatable/css/pt_br.json"
                },
                //buttons: [
                //    {
                //        extend: 'copyHtml5',
                //        text: 'Copia',
                //        titleAttr: 'Copia',
                //        className: 'btn'
                //    },
                //    {
                //        extend: 'csvHtml5',
                //        text: 'CSV',
                //        titleAttr: 'CSV',
                //        className: 'btn '
                //    },
                //    {
                //        extend: 'excelHtml5',
                //        text: 'Excel',
                //        titleAttr: 'Excel',
                //        className: 'btn '
                //    },
                //    {
                //        extend: 'pdfHtml5',
                //        orientation: 'landscape',
                //        pageSize: 'LEGAL',
                //        text: 'PDF',
                //        titleAttr: 'PDF',
                //        className: 'btn '
                //    },
                //    {
                //        extend: 'print',
                //        text: 'Stampa',
                //        titleAttr: 'Stampa',
                //        className: 'btn '
                //    },
                //    {
                //        extend: "colvis",
                //        postfixButtons: ["colvisRestore"],
                //    }
                //],
                //deferRender: true,
                responsive: true,
                //lengthChange: true,
                //orderCellsTop: true,
                //fixedHeader: true,
                //select: true,
                //processing: true,
                //serverSide: true,
                //stateSave: true,
                //ajax: '{{ route('tickets.index') }}',
                //columns: [
                //    {
                //        data: 'id',
                //        searchPanes: {
                //            show: true
                //        }
                //    },
                //    {
                //        data: 'time',
                //        searchPanes: {
                //            show: true
                //        }
                //    },
                //    {
                //        data: 'priority',
                //        searchPanes: {
                //            show: true
                //        }
                //    },
                //    {
                //        data: 'code',
                //        searchPanes: {
                //            show: true
                //        }
                //    },
                //    {
                //        data: 'category',
                //        searchPanes: {
                //            show: true
                //        }
                //    },
                //    {
                //        data: 'action', name: 'Azioni', orderable: false, searchable: false,
                //        searchPanes: {
                //            show: false
                //        }
                //    },
                //],
                //columnDefs: [
                //    {
                //        searchPanes: {
                //            show: true
                //        },
                //        targets: [0, 1, 2, 3, 4, 5]
                //    }
                //]
            });
        },

        ServiceUnitDataTableLoad: function () {
            $('#serviceUnitDataTable').DataTable({
                pageLength: 100,
                dom: 'PB',
                //layout: {
                //    topStart: 'pageLength',
                //    topEnd: 'search',
                //    bottomStart: 'info',
                //    bottomEnd: 'paging'
                //},
                //searchPanes: {
                //    cascadePanes: true,
                //    initCollapsed: true,
                //    show: true,
                //    dtOpts: {
                //        dom: 'tp',
                //        paging: 'true',
                //        pagingType: 'simple',
                //        searching: true
                //    }
                //},
                language: {
                    url: "//cdn.datatables.net/plug-ins/1.11.3/i18n/pt_br.json"
                    //url: "lib/datatable/css/pt_br.json"
                },
                //buttons: [
                //    {
                //        extend: 'copyHtml5',
                //        text: 'Copia',
                //        titleAttr: 'Copia',
                //        className: 'btn'
                //    },
                //    {
                //        extend: 'csvHtml5',
                //        text: 'CSV',
                //        titleAttr: 'CSV',
                //        className: 'btn '
                //    },
                //    {
                //        extend: 'excelHtml5',
                //        text: 'Excel',
                //        titleAttr: 'Excel',
                //        className: 'btn '
                //    },
                //    {
                //        extend: 'pdfHtml5',
                //        orientation: 'landscape',
                //        pageSize: 'LEGAL',
                //        text: 'PDF',
                //        titleAttr: 'PDF',
                //        className: 'btn '
                //    },
                //    {
                //        extend: 'print',
                //        text: 'Stampa',
                //        titleAttr: 'Stampa',
                //        className: 'btn '
                //    },
                //    {
                //        extend: "colvis",
                //        postfixButtons: ["colvisRestore"],
                //    }
                //],
                //deferRender: true,
                responsive: true,
                //lengthChange: true,
                //orderCellsTop: true,
                //fixedHeader: true,
                //select: true,
                //processing: true,
                //serverSide: true,
                //stateSave: true,
                //ajax: '{{ route('tickets.index') }}',
                //columns: [
                //    {
                //        data: 'id',
                //        searchPanes: {
                //            show: true
                //        }
                //    },
                //    {
                //        data: 'time',
                //        searchPanes: {
                //            show: true
                //        }
                //    },
                //    {
                //        data: 'priority',
                //        searchPanes: {
                //            show: true
                //        }
                //    },
                //    {
                //        data: 'code',
                //        searchPanes: {
                //            show: true
                //        }
                //    },
                //    {
                //        data: 'category',
                //        searchPanes: {
                //            show: true
                //        }
                //    },
                //    {
                //        data: 'action', name: 'Azioni', orderable: false, searchable: false,
                //        searchPanes: {
                //            show: false
                //        }
                //    },
                //],
                //columnDefs: [
                //    {
                //        searchPanes: {
                //            show: true
                //        },
                //        targets: [0, 1, 2, 3, 4, 5]
                //    }
                //]
            });
        },

        TicketDataTableLoad: function () {
            $('#ticketDataTable').DataTable({
                pageLength: 1000,
                dom: 'PB',
                //layout: {
                //    topStart: 'pageLength',
                //    topEnd: 'search',
                //    bottomStart: 'info',
                //    bottomEnd: 'paging'
                //},
                //searchPanes: {
                //    cascadePanes: true,
                //    initCollapsed: true,
                //    show: true,
                //    dtOpts: {
                //        dom: 'tp',
                //        paging: 'true',
                //        pagingType: 'simple',
                //        searching: true
                //    }
                //},
                language: {
                    url: "//cdn.datatables.net/plug-ins/1.11.3/i18n/pt_br.json"
                    //url: "lib/datatable/css/pt_br.json"
                },
                //buttons: [
                //    {
                //        extend: 'copyHtml5',
                //        text: 'Copia',
                //        titleAttr: 'Copia',
                //        className: 'btn'
                //    },
                //    {
                //        extend: 'csvHtml5',
                //        text: 'CSV',
                //        titleAttr: 'CSV',
                //        className: 'btn '
                //    },
                //    {
                //        extend: 'excelHtml5',
                //        text: 'Excel',
                //        titleAttr: 'Excel',
                //        className: 'btn '
                //    },
                //    {
                //        extend: 'pdfHtml5',
                //        orientation: 'landscape',
                //        pageSize: 'LEGAL',
                //        text: 'PDF',
                //        titleAttr: 'PDF',
                //        className: 'btn '
                //    },
                //    {
                //        extend: 'print',
                //        text: 'Stampa',
                //        titleAttr: 'Stampa',
                //        className: 'btn '
                //    },
                //    {
                //        extend: "colvis",
                //        postfixButtons: ["colvisRestore"],
                //    }
                //],
                //deferRender: true,
                responsive: true,
                //lengthChange: true,
                //orderCellsTop: true,
                //fixedHeader: true,
                //select: true,
                //processing: true,
                //serverSide: true,
                //stateSave: true,
                //ajax: '{{ route('tickets.index') }}',
                //columns: [
                //    {
                //        data: 'id',
                //        searchPanes: {
                //            show: true
                //        }
                //    },
                //    {
                //        data: 'time',
                //        searchPanes: {
                //            show: true
                //        }
                //    },
                //    {
                //        data: 'priority',
                //        searchPanes: {
                //            show: true
                //        }
                //    },
                //    {
                //        data: 'code',
                //        searchPanes: {
                //            show: true
                //        }
                //    },
                //    {
                //        data: 'category',
                //        searchPanes: {
                //            show: true
                //        }
                //    },
                //    {
                //        data: 'action', name: 'Azioni', orderable: false, searchable: false,
                //        searchPanes: {
                //            show: false
                //        }
                //    },
                //],
                //columnDefs: [
                //    {
                //        searchPanes: {
                //            show: true
                //        },
                //        targets: [0, 1, 2, 3, 4, 5]
                //    }
                //]
            });
        },
    }
}

var genesysCloud = {
    Methods: {
        GetAccessToken: function (url) {

            if (url.includes('#access_token')) {
                let indice = url.indexOf("#")

                if (indice >= 0) {
                    let partial_token = url.substring(indice).replace('#access_token=', '')

                    indice = partial_token.indexOf('&expires_in')

                    if (indice >= 0) {
                        let token = partial_token.substring(0, indice)
                        return token
                    }
                }
            }

            return ''
        },
    },
    Events: {
        //OnLoginImplicit: function () {
        //    const href = window.location.href;

        //    $.get(`${origin_url}/Login/GetSessionInfo`, function (data, status) {
        //        const platformClient = require('platformClient');
        //        var client = platformClient.ApiClient.instance;
        //        client.setEnvironment('sae1.pure.cloud');
        //        const usersApi = new platformClient.UsersApi();

        //        if (status === 'success') {
        //            const logininfo = data

        //            if (logininfo.IsAuthenticated) {

        //                const access_token = logininfo.AccessToken
        //                if (access_token !== null) {

        //                    let opts = {}
        //                    client.setAccessToken(access_token);
        //                    usersApi.getUsersMe(opts)
        //                        .then((data) => {
        //                            console.log(`getUsersMe success! data: ${JSON.stringify(data, null, 2)}`);

        //                            const usernameGC = data.email
        //                            const usernameGT = logininfo.Username

        //                            console.log(usernameGC, logininfo.Username)

        //                            if (usernameGC != usernameGT) {
        //                                window.location.href = `${origin_url}/Login/Logout`
        //                            }
        //                        })
        //                        .catch((err) => {
        //                            console.log("There was a failure calling getUsersMe");
        //                            console.error(err);
        //                            window.location.href = `${origin_url}/Login/Logout`
        //                        });
        //                }
        //                else {
        //                    $.get(`${origin_url}/Config/GetGenesysClientId`, function (clientid, status) {
        //                        if (status === 'success') {

        //                            client.loginImplicitGrant(clientid, `${origin_url}/Login/Create`)
        //                                .then(() => usersApi.getUsersMe())
        //                                .then((me) => {
        //                                    console.log(me)
        //                                    const token = genesysCloud.Methods.GetAccessToken(href)

        //                                    if (token !== '') {

        //                                        $.post(`${origin_url}/Login/ImplicitLogin`, { Username: me.email, Password: token },
        //                                            function (data, status) {
        //                                                let isAuthenticated = false;

        //                                                if (status === "success") {
        //                                                    if (data !== undefined) {
        //                                                        isAuthenticated = data.isAuthenticated

        //                                                        if (isAuthenticated) {
        //                                                            window.location.href = `${origin_url}${data.RedirectTo}`
        //                                                        }
        //                                                    }
        //                                                }

        //                                                if (isAuthenticated == false) {
        //                                                    alert('Falha de login')
        //                                                }
        //                                            },
        //                                            null,
        //                                            "json"
        //                                        );


        //                                    }
        //                                })
        //                        }
        //                    })
        //                }
        //            }
        //            else {
        //                $.get(`${origin_url}/Config/GetGenesysClientId`, function (data, status) {

        //                    if (status === 'success') {
        //                        const client_id = data

        //                        client.loginImplicitGrant(client_id, `${origin_url}/Login/Create`)
        //                            .then(() => usersApi.getUsersMe())
        //                            .then((me) => {
        //                                console.log(me)

        //                                const token = genesysCloud.Methods.GetAccessToken(href)

        //                                if (token !== '') {

        //                                    $.post(`${origin_url}/Login/ImplicitLogin`, { Username: me.email, Password: token },
        //                                        function (data, status) {
        //                                            let isAuthenticated = false;
        //                                            if (status === "success") {
        //                                                if (data !== undefined) {
        //                                                    isAuthenticated = data.isAuthenticated

        //                                                    if (isAuthenticated) {
        //                                                        window.location.href = `${origin_url}/Home/Index`
        //                                                    }
        //                                                }
        //                                            }

        //                                            if (isAuthenticated == false) {
        //                                                alert('Falha de login')
        //                                            }
        //                                        },
        //                                        null,
        //                                        "json"
        //                                    );


        //                                }
        //                            })
        //                    }
        //                })
        //            }
        //        }

        //    })
        //}

        OnLoginImplicit: function () {
            const href = window.location.href;

            $.get(`${origin_url}/Login/GetSessionInfo`, function (data, status) {
                const platformClient = require('platformClient');
                var client = platformClient.ApiClient.instance;
                client.setEnvironment('sae1.pure.cloud');
                const usersApi = new platformClient.UsersApi();

                if (status === 'success') {
                    const logininfo = data

                    if (logininfo.IsAuthenticated) {

                        const access_token = logininfo.AccessToken
                        if (access_token !== null) {

                            let opts = {}
                            client.setAccessToken(access_token);
                            usersApi.getUsersMe(opts)
                                .then((data) => {
                                    console.log(`getUsersMe success! data: ${JSON.stringify(data, null, 2)}`);

                                    const usernameGC = data.email
                                    const usernameGT = logininfo.Username

                                    console.log(usernameGC, logininfo.Username)

                                    if (usernameGC != usernameGT) {
                                        window.location.href = `${origin_url}/Login/Logout`
                                    }
                                })
                                .catch((err) => {
                                    console.log("There was a failure calling getUsersMe");
                                    console.error(err);
                                    window.location.href = `${origin_url}/Login/Logout`
                                });
                        }
                        else {
                            $.get(`${origin_url}/Config/GetGenesysClientId`, function (clientid, status) {
                                if (status === 'success') {

                                    client.loginImplicitGrant(clientid, `${origin_url}/Login/Create`)
                                        .then(() => usersApi.getUsersMe())
                                        .then((me) => {
                                            console.log(me)
                                            const token = genesysCloud.Methods.GetAccessToken(href)

                                            if (token !== '') {

                                                $.post(`${origin_url}/Login/ImplicitLogin`, { Username: me.email, Password: token },
                                                    function (data, status) {
                                                        let isAuthenticated = false;

                                                        if (status === "success") {
                                                            if (data !== undefined) {
                                                                isAuthenticated = data.isAuthenticated

                                                                if (isAuthenticated) {

                                                                    $.get(`${origin_url}/Ticket/GetInteractionByUser/?user=${me.email}`, function (interaction_info, status) {

                                                                        if (status === 'success') {
                                                                            console.log(interaction_info);


                                                                            if (interaction_info !== null) {
                                                                                const href_link = `${origin_url}/Ticket/GenesysInteractionEvent/?nomeFila=${interaction_info.queueName}&conversastionid=${interaction_info.conversationid}&email=${interaction_info.email}&cpf=${interaction_info.cpf}&protocolo=${interaction_info.protocol}&navegacao=${interaction_info.customerNavigation}&emailCliente=${interaction_info.customerEmail}&reload=true&cancel=false`

                                                                                console.log(href_link);

                                                                                window.location.href = href_link;
                                                                            }
                                                                        }
                                                                        else {
                                                                            window.location.href = `${origin_url}${data.RedirectTo}`
                                                                        }
                                                                    });
                                                                }
                                                            }
                                                        }

                                                        if (isAuthenticated == false) {
                                                            alert('Falha de login')
                                                        }
                                                    },
                                                    null,
                                                    "json"
                                                );


                                            }
                                        })
                                }
                            })
                        }
                    }
                    else {
                        $.get(`${origin_url}/Config/GetGenesysClientId`, function (data, status) {

                            if (status === 'success') {
                                const client_id = data

                                client.loginImplicitGrant(client_id, `${origin_url}/Login/Create`)
                                    .then(() => usersApi.getUsersMe())
                                    .then((me) => {
                                        console.log(me)

                                        const token = genesysCloud.Methods.GetAccessToken(href)

                                        if (token !== '') {

                                            $.post(`${origin_url}/Login/ImplicitLogin`, { Username: me.email, Password: token },
                                                function (data, status) {
                                                    let isAuthenticated = false;
                                                    if (status === "success") {
                                                        if (data !== undefined) {
                                                            isAuthenticated = data.isAuthenticated

                                                            if (isAuthenticated) {
                                                                window.location.href = `${origin_url}/Home/Index`
                                                            }
                                                        }
                                                    }

                                                    if (isAuthenticated == false) {
                                                        alert('Falha de login')
                                                    }
                                                },
                                                null,
                                                "json"
                                            );


                                        }
                                    })
                            }
                        })
                    }
                }

            })
        }
    }
}

var sessionHandler = {
    Methods: {
        RemoveConversationDataDisconnected: function () {
            if (sessionStorage.length > 0) {
                $.get(`${origin_url}/Ticket/OnInteraction`, function (data, status) {
                    if (status === 'success') {
                        for (const [key, value] of Object.entries(sessionStorage)) {
                            console.log(key, value)

                            if (key.includes('CONVERSATIONID_')) {
                                const conversationid = key.replace('CONVERSATIONID_', '');
                                const hasData = data.filter(d => d.conversationId == conversationid).length > 0;

                                if (hasData === false) {
                                    sessionStorage.removeItem(key);
                                }
                            }

                            if (key.includes('AUTOSAVE_')) {
                                const conversationid = key.replace('AUTOSAVE_', '');
                                const hasData = data.filter(d => d.conversationId == conversationid).length > 0;

                                if (hasData === false) {
                                    sessionStorage.removeItem(key);
                                }
                            }
                        }
                    }
                });
            }
        },
        //        CreateLinkRestoreInteractionScreen: function () {
        //            const menuItemId = ['customerMenuId', 'userMenuId', 'ticketMenuId', 'serviceUnitMenuId', 'areaMenuId', 'serviceLevelMenuId', 'demandTypeMenuId', 'classificationListMenuId', 'classificationTreeMenuId'];

        //            const href = window.location.href;
        //            const queryString = window.location.search;
        //            let qs_array = queryString.split('&');

        //            console.log('CreateLinkRestoreInteractionScreen', href)

        //            if (qs_array.length >= 6) {
        //                //conversationid = qs_array[1].replace("conversastionid=", "")
        //                let interaction_info = {};
        //                interaction_info.queueName = qs_array[0].replace("?nomeFila=", "")
        //                interaction_info.conversationid = qs_array[1].replace("conversastionid=", "")
        //                interaction_info.email = qs_array[2].replace("email=", "")
        //                interaction_info.cpf = qs_array[3].replace("cpf=", "")
        //                interaction_info.protocol = qs_array[4].replace("protocolo=", "")
        //                interaction_info.customerNavigation = qs_array[5].replace("navegacao=", "")
        //                interaction_info.customerEmail = qs_array[6].replace("emailCliente=", "")

        //                if (interaction_info.conversationid !== '') {
        //                    menuItemId.forEach(item => {
        //                        let hrefattr = $(`#${item}`).attr('href');
        //                        $(`#${item}`).removeAttr('href')
        //                        $(`#${item}`).attr('href', `${hrefattr}#conversation_id=${interaction_info.conversationid}`)
        //                    });

        //                    const searchTicketId = $('#searchTicketId');
        //                    console.log(searchTicketId.attr('action'));
        //                }

        ///*                const href_link = `${origin_url}/Ticket/GenesysInteractionEvent/?nomeFila=${interaction_info.queueName}&conversastionid=${interaction_info.conversationid}&email=${interaction_info.email}&cpf=${interaction_info.cpf}&protocolo=${interaction_info.protocol}&navegacao=${interaction_info.customerNavigation}&TipoMidia=${interaction_info.mediaType}&reload=true&cancel=false`*/

        //                const href_link = `${origin_url}/Ticket/GenesysInteractionEvent/?nomeFila=${interaction_info.queueName}&conversastionid=${interaction_info.conversationid}&email=${interaction_info.email}&cpf=${interaction_info.cpf}&protocolo=${interaction_info.protocol}&navegacao=${interaction_info.customerNavigation}&emailCliente=${interaction_info.customerEmail}&reload=true&cancel=false`

        //                $('#OnInteractionLink').attr('href', href_link)
        //                $('#OnInteractionAlert').show()

        //                if (sessionStorage.getItem(`CONVERSATIONID_${interaction_info.conversationid}`) == null) {
        //                    sessionStorage.setItem(`CONVERSATIONID_${interaction_info.conversationid}`, JSON.stringify(interaction_info));
        //                }
        //            }

        //            if (href.includes('#conversation_id=')) {
        //                const hrefArray = href.split('#');
        //                const conversationidParam = hrefArray[1];
        //                const conversationid = conversationidParam.replace('conversation_id=', '')

        //                menuItemId.forEach(item => {
        //                    let hrefattr = $(`#${item}`).attr('href');
        //                    $(`#${item}`).removeAttr('href')
        //                    $(`#${item}`).attr('href', `${hrefattr}#${conversationidParam}`)
        //                });

        //                const searchTicketId = $('#searchTicketId');
        //                console.log(searchTicketId.attr('action'));


        //                const interactionSession = sessionStorage.getItem(`CONVERSATIONID_${conversationid}`);

        //                if (interactionSession !== null) {
        //                    const interaction_info = JSON.parse(interactionSession);
        //                    console.log(interaction_info)
        ///*                    const href_link = `${origin_url}/Ticket/GenesysInteractionEvent/?nomeFila=${interaction_info.queueName}&conversastionid=${interaction_info.conversationid}&email=${interaction_info.email}&cpf=${interaction_info.cpf}&protocolo=${interaction_info.protocol}&navegacao=${interaction_info.customerNavigation}&TipoMidia=${interaction_info.mediaType}&reload=true&cancel=false`*/
        //                    const href_link = `${origin_url}/Ticket/GenesysInteractionEvent/?nomeFila=${interaction_info.queueName}&conversastionid=${interaction_info.conversationid}&email=${interaction_info.email}&cpf=${interaction_info.cpf}&protocolo=${interaction_info.protocol}&navegacao=${interaction_info.customerNavigation}&emailCliente=${interaction_info.customerEmail}&reload=true&cancel=false`

        //                    $('#OnInteractionLink').attr('href', href_link)
        //                    $('#OnInteractionAlert').show()
        //                }
        //            }
        //        }
        CreateLinkRestoreInteractionScreen: function () {
            const href = window.location.href;
            const queryString = window.location.search;
            let qs_array = queryString.split('&');

            console.log('CreateLinkRestoreInteractionScreen', href)

            if (qs_array.length >= 6) {
                //conversationid = qs_array[1].replace("conversastionid=", "")
                let interaction_info = {};
                interaction_info.queueName = qs_array[0].replace("?nomeFila=", "")
                interaction_info.conversationid = qs_array[1].replace("conversastionid=", "")
                interaction_info.email = qs_array[2].replace("email=", "")
                interaction_info.cpf = qs_array[3].replace("cpf=", "")
                interaction_info.protocol = qs_array[4].replace("protocolo=", "")
                interaction_info.customerNavigation = qs_array[5].replace("navegacao=", "")
                interaction_info.customerEmail = qs_array[6].replace("emailCliente=", "")

                if (interaction_info.conversationid !== null) {
                    var interaction_infoStr = JSON.stringify(interaction_info)
                    sessionStorage.setItem(`CONVERSATIONSESSION_${interaction_info.conversationid}`, interaction_infoStr)
                }
            }

        }
    }
}

var fileHelper = {
    Init: {
        OnLoadFile: function () {
            $("#Files").on("change", function () {
                var files = Array.from(this.files)
                //var files_allowed = []

                //Array.from(files).forEach(file => {
                //    console.log(file)
                //    const filename = file.name
                //    const filesize = file.size

                //    if (filename.includes(".PDF") || filename.includes(".DOC") || filename.includes(".JPG") || filename.includes(".PNG") || filename.includes(".DOCX")) {

                //        if (filesize <= 25000000) {
                //            files_allowed
                //        }
                //        else {
                //            alert('Tamanho máximo para anexar: 25MB')
                //        }
                //    }
                //    else {
                //        alert('Arquivo Não Permitido')
                //    }
                //})

                var myFile = $('#Files').prop('files');
                console.log(myFile)

                $('#Files').val('');

                Array.from(files).forEach(file => {
                    const filename = file.name
                    const filesize = file.size

                    if (filename.toUpperCase().includes(".PDF") || filename.toUpperCase().includes(".DOC") || filename.toUpperCase().includes(".JPG") || filename.toUpperCase().includes(".PNG") || filename.toUpperCase().includes(".DOCX")) {

                        if (filesize > 25000000) {
                            $('#attachmentMessageId').empty();
                            $('#attachmentMessageId').append('<div>Tamanho máximo permitido 25MB</div>');
                            $('#attachmentMessageId').show();

                            setTimeout(() => {
                                $('#attachmentMessageId').hide();
                            }, 5000);
                        }
                        else {
                            if (attached_files.filter(file => file.name.toUpperCase() === filename.toUpperCase()).length === 0) {
                                attached_files.push(file)
                                $('#filelist').append(`<div class="alert alert-success col-md-1" role="alert" style="display: inline">${filename}<button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close" onclick="fileHelper.Methods.DetachFile('${filename}')"></button></div>&nbsp;`)
                            }
                        }
                    }
                    else {
                        $('#attachmentMessageId').empty();
                        $('#attachmentMessageId').append('<div>Extensão de arquivo não permitido</div>');
                        $('#attachmentMessageId').show();

                        setTimeout(() => {
                            $('#attachmentMessageId').hide();
                        }, 5000);
                    }
                });

                if (attached_files.length > 0) {
                    const dataTransfer = new DataTransfer();

                    Array.from(attached_files).forEach(f => {
                        dataTransfer.items.add(f);
                    });

                    console.log(attached_files);
                    $("#Files").prop("files", dataTransfer.files);
                }
            });
        }
    },
    Methods: {
        DetachFile: function (filename) {
            attached_files = attached_files.filter(f => f.name !== filename);
            $('#Files').val('');

            const dataTransfer = new DataTransfer();
            attached_files.map(f => {
                dataTransfer.items.add(f);
            });

            $("#Files").prop("files", dataTransfer.files);
        },

        ReloadAttachedFiles: function () {
            const queryString = window.location.search;
            let qs_array = queryString.split('&');

            if (qs_array.length >= 6) {
                const conversationid = qs_array[1].replace("conversastionid=", "")
                console.log(conversationid)
                $.get(`${origin_url}/Ticket/GetAttachments/?conversationid=${conversationid}`, function (data, status) {

                    if (status === 'success') {

                        console.log('From Server', data)

                        if (data.success) {

                            const dataTransfer = new DataTransfer();
                            Array.from(data.attachments).forEach(f => {
                                console.log(f);

                                //const base64Data = f.fileContent.replace(/^data:.+;base64,/, '');
                                const byteCharacters = atob(f.fileContent); // Decode Base64 string
                                const byteNumbers = new Array(byteCharacters.length);

                                for (let i = 0; i < byteCharacters.length; i++) {
                                    byteNumbers[i] = byteCharacters.charCodeAt(i);
                                }

                                /*                        const blob = new Blob([f.fileBytes], { type: 'application/pdf', contentDisposition: f.contentDisposition });*/
                                const byteArray = new Uint8Array(byteNumbers);

                                const blob = new Blob([byteArray], { type: 'application/pdf', contentDisposition: f.contentDisposition });

                                const attachfile = new File([blob], f.fileName, {
                                    type: f.contentType, lastModified: Date.now(), contentDisposition: f.contentDisposition
                                })

                                dataTransfer.items.add(attachfile);
                                attached_files.push(attachfile);

                                $('#filelist').append(`<div class="alert alert-success col-md-1" role="alert" style="display: inline">${f.fileName}<button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close" onclick="fileHelper.Methods.DetachFile('${f.fileName}')"></button></div>&nbsp;`)
                            });
                            console.log('dataTransfer', dataTransfer.files)
                            $("#Files").prop("files", dataTransfer.files);
                        }
                    }
                })
            }
        }
    }
}

var urlManager = {
    Methods: {
        //LoadConversationIdInUrl: function () {
        //    const useremail = $('#userEmailId').val();
        //    const queryString = window.location.search;
        //    let qs_array = queryString.split('&');
        //    let conversationid_ative = '';
        //    const ticketSaved = $('#TicketSaved').val();

        //    if (qs_array.length >= 6 && ticketSaved !== 'True') {
        //        let interaction_info = {};
        //        interaction_info.queueName = qs_array[0] !== undefined ? qs_array[0].replace("?nomeFila=", "") : '';
        //        interaction_info.conversationid = qs_array[1] !== undefined ? qs_array[1].replace("conversastionid=", "") : '';
        //        interaction_info.email = qs_array[2] !== undefined ? qs_array[2].replace("email=", "") : '';
        //        interaction_info.cpf = qs_array[3] !== undefined ? qs_array[3].replace("cpf=", "") : '';
        //        interaction_info.protocol = qs_array[4] !== undefined ? qs_array[4].replace("protocolo=", "") : '';
        //        interaction_info.customerNavigation = qs_array[5] !== undefined ? qs_array[5].replace("navegacao=", "") : '';
        //        interaction_info.customerEmail = qs_array[6] !== undefined ? qs_array[6].replace("emailCliente=", "") : '';
        //        interaction_info.parentTicket = qs_array[7] !== undefined ? qs_array[6].replace("protocolo_pai=", "") : '';

        //        if (interaction_info.conversationid !== null && interaction_info.conversationid !== 'undefined') {
        //            conversationid_ative = interaction_info.conversationid;
        //            const backToTicketScreenLink = `${origin_url}/Ticket/GenesysInteractionEvent/?nomeFila=${interaction_info.queueName}&conversastionid=${interaction_info.conversationid}&email=${interaction_info.email}&cpf=${interaction_info.cpf}&protocolo=${interaction_info.protocol}&navegacao=${interaction_info.customerNavigation}&emailCliente=${interaction_info.customerEmail}&protocolo_pai=${interaction_info.parentTicket}&reload=true&cancel=false`

        //            $('#OnInteractionLink').attr('href', backToTicketScreenLink)
        //            $('#OnInteractionAlert').show()
        //        }
        //    }

        //    const href = window.location.href;
        //    if (href.includes('#conversation_id=') && ticketSaved !== 'True') {
        //        console.log('OK')
        //        const hrefArray = href.split('#');
        //        const conversationidParam = hrefArray[1];
        //        const conversationid = conversationidParam.replace('conversation_id=', '')
        //        const useremail = $('#userEmailId').val();
        //        conversationid_ative = conversationid;

        //        $.get(`${origin_url}/Ticket/MyInteraction/?email=${useremail}&conversationid=${conversationid}`, function (data, status) {
        //            if (status == 'success') {
        //                console.log(data)

        //                if (data !== null) {
        //                    //conversationid_ative = conversationid;
        //                    const interaction_info = data;
        //                    console.log(interaction_info.conversationId)
        //                    const backToTicketScreenLink = `${origin_url}/Ticket/GenesysInteractionEvent/?nomeFila=${interaction_info.queueName}&conversastionid=${interaction_info.conversationId}&email=${interaction_info.userEmail}&cpf=${interaction_info.cpf}&protocolo=${interaction_info.protocol}&navegacao=${interaction_info.customerNavigation}&emailCliente=${interaction_info.customerEmail}&protocolo_pai=${interaction_info.parentTicket}&reload=true&cancel=false`

        //                    $('#OnInteractionLink').attr('href', backToTicketScreenLink)
        //                    $('#OnInteractionAlert').show()
        //                }
        //            }
        //        })
        //    }

        //    if (conversationid_ative !== '') {
        //        document.addEventListener('click', event => {

        //            const tagName = event.target.tagName;
        //            let target_href = event.target.getAttribute('href');

        //            if (tagName !== 'BUTTON' && tagName !== 'LABEL' && tagName !== 'INPUT') {
        //                if (target_href !== '#') {
        //                    event.preventDefault();

        //                    if (tagName === 'A') {

        //                        console.log('target_href', target_href);

        //                        if (target_href.includes('amazonaws.com') || target_href.includes('apps.sae1.pure.cloud')) {
        //                            console.log('REDIRECT');
        //                            window.open(target_href, "_blank");
        //                        }
        //                        else {
        //                            const origin_url_array = origin_url.split('/')

        //                            Array.from(origin_url_array).forEach(item => {
        //                                if (target_href.includes(item)) {
        //                                    target_href = target_href.replace(item, '');

        //                                    if (target_href.startsWith('//')) {
        //                                        target_href = target_href.replace('//', '/');
        //                                    }
        //                                }
        //                            });

        //                            const target_url = `${origin_url}${target_href}#conversation_id=${conversationid_ative}`;
        //                            window.location.replace(target_url);


        //                            //window.location.reload();
        //                            //if (window.location.href.includes(target_href)) {
        //                            //    window.location.reload();

        //                            //}
        //                        }
        //                    }
        //                }
        //            }
        //        });
        //    }

        //    $('form').submit(function (event) {
        //        event.preventDefault();

        //        let formData = $(this).serialize();

        //        console.log("Form submitted:", formData);

        //        const originalAction = $(this).attr('action')
        //        $(this).attr('action', `${originalAction}#conversation_id=${conversationid_ative}`)
        //        this.submit();
        //    });

        //}
        LoadConversationIdInUrl: function () {
            const useremail = $('#userEmailId').val();
            const queryString = window.location.search;
            let qs_array = queryString.split('&');
            let conversationid_ative = '';
            const ticketSaved = $('#TicketSaved').val();

            if (qs_array.length >= 6 && ticketSaved !== 'True') {
                let interaction_info = {};
                interaction_info.queueName = qs_array[0] !== undefined ? qs_array[0].replace("?nomeFila=", "") : '';
                interaction_info.conversationid = qs_array[1] !== undefined ? qs_array[1].replace("conversastionid=", "") : '';
                interaction_info.email = qs_array[2] !== undefined ? qs_array[2].replace("email=", "") : '';
                interaction_info.cpf = qs_array[3] !== undefined ? qs_array[3].replace("cpf=", "") : '';
                interaction_info.protocol = qs_array[4] !== undefined ? qs_array[4].replace("protocolo=", "") : '';
                interaction_info.customerNavigation = qs_array[5] !== undefined ? qs_array[5].replace("navegacao=", "") : '';
                interaction_info.customerEmail = qs_array[6] !== undefined ? qs_array[6].replace("emailCliente=", "") : '';
                interaction_info.parentTicket = qs_array[7] !== undefined ? qs_array[6].replace("protocolo_pai=", "") : '';

                if (interaction_info.conversationid !== null && interaction_info.conversationid !== 'undefined') {
                    conversationid_ative = interaction_info.conversationid;
                    const backToTicketScreenLink = `${origin_url}/Ticket/GenesysInteractionEvent/?nomeFila=${interaction_info.queueName}&conversastionid=${interaction_info.conversationid}&email=${interaction_info.email}&cpf=${interaction_info.cpf}&protocolo=${interaction_info.protocol}&navegacao=${interaction_info.customerNavigation}&emailCliente=${interaction_info.customerEmail}&protocolo_pai=${interaction_info.parentTicket}&reload=true&cancel=false`

                    $('#OnInteractionLink').attr('href', backToTicketScreenLink)
                    $('#OnInteractionAlert').show()

                    urlManager.Events.LoadEventsForActiveConversation(conversationid_ative);                    
                    urlManager.Events.CheckOnInteractionAlertInfo(interaction_info.email, conversationid_ative);
                }
            }

            const href = window.location.href;
            if (href.includes('#conversation_id=') && ticketSaved !== 'True') {
                const hrefArray = href.split('#');
                const conversationidParam = hrefArray[1];
                const conversationid = conversationidParam.replace('conversation_id=', '')
                const useremail = $('#userEmailId').val();
                //conversationid_ative = conversationid;
                urlManager.Methods.GetMyInteraction(useremail, conversationid);
                urlManager.Events.CheckOnInteractionAlertInfo(useremail, conversationid);
            }
        },
        GetMyInteraction: function (useremail, conversationid) {

            $.get(`${origin_url}/Ticket/MyInteraction/?email=${useremail}&conversationid=${conversationid}`, function (data, status) {
                if (status == 'success') {
                    console.log(data)

                    if (data !== null) {
                        conversationid_ative = conversationid;
                        const interaction_info = data;
                        console.log(interaction_info.conversationId)
                        const backToTicketScreenLink = `${origin_url}/Ticket/GenesysInteractionEvent/?nomeFila=${interaction_info.queueName}&conversastionid=${interaction_info.conversationId}&email=${interaction_info.userEmail}&cpf=${interaction_info.cpf}&protocolo=${interaction_info.protocol}&navegacao=${interaction_info.customerNavigation}&emailCliente=${interaction_info.customerEmail}&protocolo_pai=${interaction_info.parentTicket}&reload=true&cancel=false`

                        $('#OnInteractionLink').attr('href', backToTicketScreenLink)
                        $('#OnInteractionAlert').show()

                        urlManager.Events.LoadEventsForActiveConversation(conversationid_ative);
                    }
                    else {
                        if (checkOnInteractionAlertInfoTimer !== undefined) {
                            clearInterval(checkOnInteractionAlertInfoTimer);
                            checkOnInteractionAlertInfoTimer = undefined;
                        }
                    }
                }
            })
        }
    },
    Events: {

        LoadEventsForActiveConversation: function (conversationid_ative) {

            if (conversationid_ative !== '') {
                document.addEventListener('click', event => {

                    const tagName = event.target.tagName;
                    let target_href = event.target.getAttribute('href');

                    if (tagName !== 'BUTTON' && tagName !== 'LABEL' && tagName !== 'INPUT') {
                        if (target_href !== '#') {
                            event.preventDefault();

                            if (tagName === 'A') {

                                console.log('target_href', target_href);

                                if (target_href.includes('amazonaws.com') || target_href.includes('apps.sae1.pure.cloud')) {
                                    console.log('REDIRECT');
                                    window.open(target_href, "_blank");
                                }
                                else {
                                    const origin_url_array = origin_url.split('/')

                                    Array.from(origin_url_array).forEach(item => {
                                        if (target_href.includes(item)) {
                                            target_href = target_href.replace(item, '');

                                            if (target_href.startsWith('//')) {
                                                target_href = target_href.replace('//', '/');
                                            }
                                        }
                                    });

                                    const target_url = `${origin_url}${target_href}#conversation_id=${conversationid_ative}`;
                                    window.location.replace(target_url);


                                    //window.location.reload();
                                    //if (window.location.href.includes(target_href)) {
                                    //    window.location.reload();

                                    //}
                                }
                            }
                        }
                    }
                });
            }

            $('form').submit(function (event) {
                event.preventDefault();

                let formData = $(this).serialize();

                console.log("Form submitted:", formData);

                const originalAction = $(this).attr('action')
                $(this).attr('action', `${originalAction}#conversation_id=${conversationid_ative}`)
                this.submit();
            });
        },

        CheckOnInteractionAlertInfo: function (username, conversationid) {

            if (checkOnInteractionAlertInfoTimer === undefined) {
                checkOnInteractionAlertInfoTimer = setInterval(() => {
                    const username_timer = username;
                    const conversationid_timer = conversationid;

                    console.log('Conversation_Id', conversationid_timer);
                    if ($('#OnInteractionAlert').is(":hidden")) {
                        console.log('OnInteractionAlert: hidden')
                        urlManager.Methods.GetMyInteraction(username_timer, conversationid_timer);
                    }
                    else {
                        console.log('OnInteractionAlert: visible')
                    }

                }, 3000);

            }
        }
    }
}


$(function () {

    if (tools.HelpMethods !== undefined) {
        tools.HelpMethods.EnableFormCancelButton()
        tools.HelpMethods.EnableViewPasswordButton()
        tools.HelpMethods.SubmitPageControlForm()
    }


    //dataTablesHandler.Init.UserDataTableLoad()
    dataTablesHandler.Init.CustomerDataTableLoad()
    dataTablesHandler.Init.AreaDataTableLoad()
    dataTablesHandler.Init.ServiceLevelDataTableLoad()
    dataTablesHandler.Init.DemandTypeDataTableLoad()
    dataTablesHandler.Init.ServiceUnitDataTableLoad()
    dataTablesHandler.Init.TicketDataTableLoad()

    select2treeHandler.Init.ClassificationDemandLoad()
    select2treeHandler.Init.ClassificationTypeLoad()
    select2treeHandler.Init.ClassificationReasonLoad()
    select2treeHandler.Init.ClassificationSubReason01Load()
    select2treeHandler.Init.ClassificationSubReason02Load()

    fileHelper.Init.OnLoadFile();

    //genesysCloud.Events.OnLoginImplicit()

    sessionHandler.Methods.RemoveConversationDataDisconnected();
    //sessionHandler.Methods.CreateLinkRestoreInteractionScreen();
    
    urlManager.Methods.LoadConversationIdInUrl();
    fileHelper.Methods.ReloadAttachedFiles();
});





//$(function () {
//    //const screenuuid = $('#screenuuid').val();
//    //console.log(screenuuid)
//    //if (screenuuid === '') {
//    //    $('#screenuuid').val(crypto.randomUUID())
//    //}



//    //$('.click-event-detect').on('click', function () {
//    //    const href = window.location.href;
//    //    console.log(href);
//    //    const customerhref = $('#customerMenuId').attr('href');
//    //    console.log(customerhref);
//    //    $('#customerMenuId').removeAttr('href')
//    //    $('#customerMenuId').attr('href', `${customerhref}#teste`)


//    //    const userhref = $('#userMenuId').attr('href');
//    //    console.log(userhref);
//    //    $('#userMenuId').removeAttr('href')
//    //    $('#userMenuId').attr('href', `${userhref}#teste`)


//    //});

//    //var mydata = [
//    //    {
//    //        id: 1, text: "USA", inc: [
//    //            {
//    //                text: "west", inc: [
//    //                    {
//    //                        id: 111, text: "California", inc: [
//    //                            {
//    //                                id: 1111, text: "Los Angeles", inc: [
//    //                                    { id: 11111, text: "Hollywood" }
//    //                                ]
//    //                            },
//    //                            // {id:1112, text:"San Diego", selected:"true"}
//    //                        ]
//    //                    },
//    //                    { id: 112, text: "Oregon" }
//    //                ]
//    //            }
//    //        ]
//    //    },
//    //    { id: 2, text: "India" },
//    //    { id: 3, text: "中国" }
//    //];


//    //let path = window.location.pathname

//    //if (path.toLowerCase().includes("/login/create")) {

//    //    let href = window.location.href;
//    //    let url = href.replace("Login/Create", "Login/CheckIfIsAuthenticated")

//    //    $.get(url, function (data, stauts) {
//    //        if (!data) {
//    //            genesysCloud.Events.OnLoginImplicit()
//    //        }
//    //        else {
//    //            let href = window.location.href;
//    //            window.location.href = href.replace("Login/Create", "Home/Index")
//    //        }
//    //    })
//    //}

//    //if (path.includes("/Login/Create")) {
//    //    $.get('/Login/CheckIfIsAuthenticated', function (data, stauts) {
//    //        if (!data) {
//    //            $.post(window.location.href, { Username: 'ronaldo.morais@alctel.com.br', Password: '1234' },
//    //                function (data) {
//    //                    let href = window.location.href;
//    //                    window.location.href = href.replace("Login/Create", "Home/Index")

//    //                    if (status === "success") {
//    //                    }
//    //                    else {
//    //                    }
//    //                },
//    //                null,
//    //                "json"
//    //            );
//    //        }
//    //        else {
//    //            let href = window.location.href;
//    //            window.location.href = href.replace("Login/Create", "Home/Index")
//    //        }
//    //    })
//    //}




//    //$.get(`${origin_url}/Ticket/OnInteraction`, function (data, status) {
//    //    if (status == 'success') {
//    //        const dataLen = data.length;

//    //        if (dataLen == 1) {
//    //            const conversastionid = data[0].conversationId;
//    //            const cpf = data[0].cpf;
//    //            const customerNavigation = data[0].customerNavigation;
//    //            const protocol = data[0].protocol;
//    //            const queueName = data[0].queueName;

//    //            const href = `${origin_url}/Ticket/GenesysInteractionEvent/?nomeFila=${queueName}&conversastionid=${conversastionid}&email=&cpf=${cpf}&protocolo=${protocol}&navegacao=${customerNavigation}&reload=true&cancel=false`

//    //            $('#OnInteractionLink').attr('href', href)
//    //            $('#OnInteractionAlert').show()
//    //        }
//    //        else {
//    //            $('#OnInteractionAlert').hide()
//    //        }
//    //    }
//    //})





//    // console.log(window.location.search)
//    // let conversationInfoSession = sessionStorage.getItem("CONVERSATION_INFO");

//    // if (conversationInfoSession == null) {
//    // const queryString = window.location.search;
//    // let qs_array = queryString.split('&')
//    // console.log(qs_array)

//    // let interaction_info = {}
//    // if (qs_array.length == 6) {
//    // interaction_info.queueName = qs_array[0].replace("?nomeFila=", "")
//    // interaction_info.conversationid = qs_array[1].replace("conversastionid=", "")
//    // interaction_info.email = qs_array[2].replace("email=", "")
//    // interaction_info.cpf = qs_array[3].replace("cpf=", "")
//    // interaction_info.protocol = qs_array[4].replace("protocolo=", "")
//    // interaction_info.customerNavigation = qs_array[5].replace("navegacao=", "")

//    // console.log(interaction_info)

//    // var interaction_infoStr = JSON.stringify(interaction_info)
//    // sessionStorage.setItem("CONVERSATION_INFO", interaction_infoStr)

//    // const href = `${origin_url}/Ticket/GenesysInteractionEvent/?nomeFila=${interaction_info.queueName}&conversastionid=${interaction_info.conversationid}&email=${interaction_info.email}&cpf=${interaction_info.cpf}&protocolo=${interaction_info.protocol}&navegacao=${interaction_info.customerNavigation}&reload=true&cancel=false`
//    // console.log(interaction_info.conversationid)
//    // console.log('href', href)
//    // $('#OnInteractionLink').attr('href', href)
//    // $('#OnInteractionAlert').show()
//    // }
//    // }
//    // else {
//    // $.get(`${origin_url}/Ticket/GetOnInteractions`, function (data, status) {
//    // if (status == 'success') {
//    // console.log(data)

//    // var interaction_info = JSON.parse(conversationInfoSession);

//    // if (interaction_info != null) {

//    // if (data.includes(interaction_info.conversationid)) {
//    // const href = `${origin_url}/Ticket/GenesysInteractionEvent/?nomeFila=${interaction_info.queueName}&conversastionid=${interaction_info.conversationid}&email=${interaction_info.email}&cpf=${interaction_info.cpf}&protocolo=${interaction_info.protocol}&navegacao=${interaction_info.customerNavigation}&reload=true&cancel=false`
//    // console.log(interaction_info.conversationid)
//    // console.log('href', href)
//    // $('#OnInteractionLink').attr('href', href)
//    // $('#OnInteractionAlert').show()
//    // }
//    // }
//    // else {
//    // sessionStorage.removeItem("CONVERSATION_INFO")
//    // }
//    // }
//    // })

//    // //const interaction_info = JSON.parse(conversationInfoSession)

//    // //const href = `${origin_url}/Ticket/GenesysInteractionEvent/?nomeFila=${interaction_info.queueName}&conversastionid=${interaction_info.conversationid}&email=${interaction_info.email}&cpf=${interaction_info.cpf}&protocolo=${interaction_info.protocol}&navegacao=${interaction_info.customerNavigation}&reload=true&cancel=false`
//    // //console.log('href', href)
//    // //$('#OnInteractionLink').attr('href', href)
//    // //$('#OnInteractionAlert').show()
//    // }



//    //if (conversationid == null) {
//    //    console.log(conversationid)
//    //    const queryString = window.location.search;

//    //    let qs_array = queryString.split('&')
//    //    console.log(qs_array);
//    //    if (qs_array.length > 2) {
//    //        const conversationParam = qs_array[1]
//    //        let conversation_array = conversationParam.split('=')
//    //        console.log(conversation_array[1]);
//    //        sessionStorage.setItem("conversationid", conversation_array[1])

//    //        const href = `${origin_url}/Ticket/GenesysInteractionEvent/?nomeFila=&conversastionid=${conversationid}&email=&cpf=&protocolo=&navegacao=&reload=true&cancel=false`
//    //        console.log(href)
//    //        $('#OnInteractionLink').attr('href', href)
//    //        $('#OnInteractionAlert').show()
//    //    }
//    //}
//    //else {
//    //    console.log(conversationid)

//    //    const href = `${origin_url}/Ticket/GenesysInteractionEvent/?nomeFila=&conversastionid=${conversationid}&email=&cpf=&protocolo=&navegacao=&reload=true&cancel=false`
//    //    console.log(href)
//    //    $('#OnInteractionLink').attr('href', href)
//    //    $('#OnInteractionAlert').show()
//    //}

//    if (tools.HelpMethods !== undefined) {
//        tools.HelpMethods.EnableFormCancelButton()
//        tools.HelpMethods.EnableViewPasswordButton()
//        tools.HelpMethods.SubmitPageControlForm()
//    }

//    //$.get(`${origin_url}/Ticket/OnInteraction`, function (data, status) {
//    //    if (status == 'success') {
//    //        console.log(data)

//    //        if (data.OnInteraction) {
//    //            const conversationid = sessionStorage.getItem("conversationid");
//    //            console.log('ConversationId', conversationid)
//    //            const href = `${origin_url}/Ticket/GenesysInteractionEvent/?nomeFila=${data.QueueName}&conversastionid=${conversationid}&email=&cpf=${data.Cpf}&protocolo=${data.Protocol}&navegacao=${data.CustomerNavigation}&reload=true&cancel=false`
//    //            console.log(href)
//    //            $('#OnInteractionLink').attr('href', href)
//    //            $('#OnInteractionAlert').show()
//    //        }
//    //    }
//    //})

//    dataTablesHandler.Init.UserDataTableLoad()
//    dataTablesHandler.Init.CustomerDataTableLoad()
//    dataTablesHandler.Init.AreaDataTableLoad()
//    dataTablesHandler.Init.ServiceLevelDataTableLoad()
//    dataTablesHandler.Init.DemandTypeDataTableLoad()
//    dataTablesHandler.Init.ServiceUnitDataTableLoad()
//    dataTablesHandler.Init.TicketDataTableLoad()

//    select2treeHandler.Init.ClassificationDemandLoad()
//    select2treeHandler.Init.ClassificationTypeLoad()
//    select2treeHandler.Init.ClassificationReasonLoad()
//    select2treeHandler.Init.ClassificationSubReason01Load()
//    select2treeHandler.Init.ClassificationSubReason02Load()

//    fileHelper.Init.OnLoadFile();

//    //genesysCloud.Events.OnLoginImplicit()

//    sessionHandler.Methods.RemoveConversationDataDisconnected();
//    //sessionHandler.Methods.CreateLinkRestoreInteractionScreen();

//    //setInterval(() => {
//    //    tools.HelpMethods.OnInteracion();
//    //}, 2000);

//    //tools.HelpMethods.OnInteracion();

//    const useremail = $('#userEmailId').val();
//    console.log(useremail)
//    //$.get(`${origin_url}/Ticket/MyInteractions/?email=${useremail}`, function (data, status) {
//    //    if (status == 'success') {
//    //        console.log(data)

//    //        if (data !== null) {
//    //            const myInteractions = data.myInteractions;
//    //            if (myInteractions !== null && myInteractions.length == 1) {
//    //                console.log(myInteractions[0].conversationId)

//    //                const href_link = `${origin_url}/Ticket/GenesysInteractionEvent/?nomeFila=${myInteractions[0].queueName}&conversastionid=${myInteractions[0].conversationid}&email=${myInteractions[0].userEmail}&cpf=${myInteractions[0].cpf}&protocolo=${myInteractions[0].protocol}&navegacao=${myInteractions[0].customerNavigation}&emailCliente=${myInteractions[0].customerEmail}&reload=true&cancel=false`

//    //                $('#OnInteractionLink').attr('href', href_link)
//    //                $('#OnInteractionAlert').show()

//    //            }
//    //            else {
//    //                $('#OnInteractionAlert').hide()
//    //            }
//    //        }
//    //        else {
//    //            $('#OnInteractionAlert').hide()
//    //        }
//    //    }
//    //    else {
//    //        $('#OnInteractionAlert').hide()
//    //    }
//    //})


//    //setInterval(() => {
//    //    const useremail = $('#userEmailId').val();
//    //    $.get(`${origin_url}/Ticket/MyInteractions/?email=${useremail}`, function (data, status) {
//    //        if (status == 'success') {
//    //            console.log(data)

//    //            if (data !== null) {
//    //                const myInteractions = data.myInteractions;
//    //                if (myInteractions !== null && myInteractions.length == 1) {
//    //                    console.log(myInteractions[0].conversationId)

//    //                    const href_link = `${origin_url}/Ticket/GenesysInteractionEvent/?nomeFila=${myInteractions[0].queueName}&conversastionid=${myInteractions[0].conversationId}&email=${myInteractions[0].userEmail}&cpf=${myInteractions[0].cpf}&protocolo=${myInteractions[0].protocol}&navegacao=${myInteractions[0].customerNavigation}&emailCliente=${myInteractions[0].customerEmail}&reload=true&cancel=false`

//    //                    $('#OnInteractionLink').attr('href', href_link)
//    //                    $('#OnInteractionAlert').show()

//    //                }
//    //                else {
//    //                    $('#OnInteractionAlert').hide()
//    //                }
//    //            }
//    //            else {
//    //                $('#OnInteractionAlert').hide()
//    //            }
//    //        }
//    //        else {
//    //            $('#OnInteractionAlert').hide()
//    //        }
//    //    })

//    //}, 1000);

//    //setTimeout(() => {
//    //    navigation.addEventListener('navigate', (e) => {
//    //        e.stopPropagation();
//    //        console.log('page changed');
//    //        window.location.href = window.location.href;
//    //        /*$.get(`${origin_url}/Ticket/OnInteraction`, function (data, status) { })*/
//    //    });
//    //}, 1000);


//    //navigation.addEventListener('navigate', (e) => {
//    //    e.preventDefault();
//    //    console.log('page changed');
//    //    //window.location.href = window.location.href;
//    //    //setTimeout(() => { window.location.href = window.location.href; }, 0);
//    //    /*$.get(`${origin_url}/Ticket/OnInteraction`, function (data, status) { })*/
//    //    const nextURL = 'https://localhost:7011/Ticket/Index';
//    //    window.location.assign(nextURL);
//    //});



//    fileHelper.Methods.ReloadAttachedFiles();
//});




function isNumber(value) {
    return typeof value === 'number'
}

