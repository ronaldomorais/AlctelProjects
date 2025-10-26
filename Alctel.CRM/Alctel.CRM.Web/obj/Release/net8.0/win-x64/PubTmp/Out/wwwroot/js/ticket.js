$(function () {
    tools.Init.ComboBox()
    tools.Init.ParentInput()
    //tools.Init.IsInteractionWithMe()
    tools.Methods.LoadJourneyTableHover()

    //screenAutoSaveData.Init.Fields();
    screenAutoSaveData.Init.StartAutoSaveTimer();
    //screenAutoSaveData.Methods.OnLoad();
    classification.Events.OnChangeOptions()


    //$("#Files").on("change", function () {
    //    var files = Array.from(this.files)
    //    //var files_allowed = []

    //    //Array.from(files).forEach(file => {
    //    //    console.log(file)
    //    //    const filename = file.name
    //    //    const filesize = file.size

    //    //    if (filename.includes(".PDF") || filename.includes(".DOC") || filename.includes(".JPG") || filename.includes(".PNG") || filename.includes(".DOCX")) {

    //    //        if (filesize <= 25000000) {
    //    //            files_allowed
    //    //        }
    //    //        else {
    //    //            alert('Tamanho máximo para anexar: 25MB')
    //    //        }
    //    //    }
    //    //    else {
    //    //        alert('Arquivo Não Permitido')
    //    //    }
    //    //})

    //    var myFile = $('#Files').prop('files');
    //    console.log(myFile)

    //    $('#Files').val('');

    //    Array.from(files).forEach(file => {
    //        const filename = file.name
    //        const filesize = file.size

    //        if (filename.toUpperCase().includes(".PDF") || filename.toUpperCase().includes(".DOC") || filename.toUpperCase().includes(".JPG") || filename.toUpperCase().includes(".PNG") || filename.toUpperCase().includes(".DOCX")) {

    //            if (filesize > 25000000) {
    //                $('#attachmentMessageId').empty();
    //                $('#attachmentMessageId').append('<div>Tamanho máximo permitido 25MB</div>');
    //                $('#attachmentMessageId').show();

    //                setTimeout(() => {
    //                    $('#attachmentMessageId').hide();
    //                }, 5000);
    //            }
    //            else {
    //                if (attached_files.filter(file => file.name.toUpperCase() === filename.toUpperCase()).length === 0) {
    //                    attached_files.push(file)
    //                    $('#filelist').append(`<div class="alert alert-success col-md-1" role="alert" style="display: inline">${filename}<button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close" onclick="tools.Methods.DetachFile('${filename}')"></button></div>&nbsp;`)
    //                }
    //            }
    //        }
    //        else {
    //            $('#attachmentMessageId').empty();
    //            $('#attachmentMessageId').append('<div>Extensão de arquivo não permitido</div>');
    //            $('#attachmentMessageId').show();

    //            setTimeout(() => {
    //                $('#attachmentMessageId').hide();
    //            }, 5000);
    //        }
    //    });

    //    if (attached_files.length > 0) {
    //        const dataTransfer = new DataTransfer();

    //        Array.from(attached_files).forEach(f => {
    //            dataTransfer.items.add(f);
    //        });

    //        console.log(attached_files);
    //        $("#Files").prop("files", dataTransfer.files);
    //    }
    //});
})

var screenAutoSaveData = {
    Init: {
        Fields: function () {
            const fieldsArray = ['DemandTypeId', 'TicketCriticalityId', 'TicketStatusId', 'AnySolution', 'DemandObservation'];

            fieldsArray.forEach(field => {

                $(`#${field}`).on('change', function (e) {
                    const type = e.target.id;
                    const value = e.target.value;
                    //screenAutoSaveData.Methods.OnChange(type, value)
                })
            })

        },
        StartAutoSaveTimer: function () {
            const autoSaveTimer = setInterval(() => {
                screenAutoSaveData.Methods.SendDataToAutoSave();
            }, 2000);
        }
    },
    Methods: {
        GetConversationId: function () {
            const queryString = window.location.search;
            let qs_array = queryString.split('&');

            if (qs_array.length >= 6) {
                return qs_array[1].replace("conversastionid=", "")
            }
            return ''
        },
        Session: function (action, key, value) {
            switch (action) {
                case "SET":
                    sessionStorage.setItem(key, JSON.stringify(value));
                    break;
                case "GET":
                    return sessionStorage.getItem(key);
                case "REMOVE":
                    sessionStorage.removeItem(key);
                    break;
            }
        },
        //OnChange: function (type, value) {
        //    const convid = screenAutoSaveData.Methods.GetConversationId();
        //    console.log(convid)
        //    //screenAutoSaveData.Methods.AutoSaveData(type, value, convid);
        //},
        AutoSaveData: function (type, value, convid) {
            if (convid !== '') {
                let dataInSession = screenAutoSaveData.Methods.Session('GET', `AUTOSAVE_${convid}`)
                console.log('dataInSession', dataInSession)

                if (dataInSession == null) {
                    let dataArray = [];
                    let data = {}

                    data.type = type;
                    data.value = value;
                    dataArray.push(data)
                    screenAutoSaveData.Methods.Session('SET', `AUTOSAVE_${convid}`, dataArray)
                }
                else {
                    let dataArray = JSON.parse(dataInSession);

                    const hasData = dataArray.filter(d => d.type == type).length > 0;

                    if (hasData) {
                        dataArray.forEach(d => {
                            if (d.type === type) {
                                d.value = value;
                            }
                        })
                    }
                    else {
                        let data = {}

                        data.type = type;
                        data.value = value;
                        dataArray.push(data);
                    }

                    screenAutoSaveData.Methods.Session('REMOVE', `AUTOSAVE_${convid}`)
                    screenAutoSaveData.Methods.Session('SET', `AUTOSAVE_${convid}`, dataArray)
                }
            }
        },
        SendDataToAutoSave: function () {
            //let autoSaveData = {};
            const formData = new FormData();
            const conversationid = screenAutoSaveData.Methods.GetConversationId();
            formData.append('ConversationId', conversationid);

            const ticketCriticalityId = $('#TicketCriticalityId').val();
            if (ticketCriticalityId !== null && ticketCriticalityId !== '') {
                //autoSaveData.TicketCriticalityId = ticketCriticalityId;
                formData.append('TicketCriticalityId', ticketCriticalityId);
            }

            const ticketStatusId = $('#TicketStatusId').val();
            if (ticketStatusId !== null && ticketStatusId !== '') {
                //autoSaveData.TicketStatusId = ticketStatusId;
                formData.append('TicketStatusId', ticketStatusId);
            }

            const anySolution = $('#AnySolution').val();
            if (anySolution !== null && anySolution !== '') {
                //autoSaveData.AnySolution = anySolution;
                formData.append('AnySolution', anySolution);
            }

            const demandObservation = $('#DemandObservation').val();
            if (demandObservation !== null && demandObservation !== '') {
                //autoSaveData.DemandObservation = demandObservation;
                formData.append('DemandObservation', demandObservation);
            }

            const parentTicket = $('#ParentTicket').val();
            if (parentTicket !== null && parentTicket !== '') {
                //autoSaveData.ParentTicket = parentTicket;
                formData.append('ParentTicket', parentTicket);
            }

            if (attached_files.length > 0) {
                console.log(attached_files)
                Array.from(attached_files).forEach(file => {
                    formData.append('Files', file, file.name);
                });
            }

            let autoSaveTable = $('#autoSaveTable').val();

            if (autoSaveTable !== '') {
                const ticketClassificationAutoSaveArray = JSON.parse(autoSaveTable);

                ticketClassificationAutoSaveArray.forEach((obj, index) => {
                    console.log(obj);
                    formData.append(`TicketClassification[${index}][ManifestationTypeId]`, obj.ManifestationTypeId);
                    formData.append(`TicketClassification[${index}][ManifestationTypeName]`, obj.ManifestationTypeName);
                    formData.append(`TicketClassification[${index}][ServiceUnitId]`, obj.ServiceUnitId);
                    formData.append(`TicketClassification[${index}][ServiceUnitName]`, obj.ServiceUnitName);
                    formData.append(`TicketClassification[${index}][ServiceId]`, obj.ServiceId);
                    formData.append(`TicketClassification[${index}][ServiceName]`, obj.ServiceName);
                    formData.append(`TicketClassification[${index}][Reason01Id]`, obj.Reason01Id);
                    formData.append(`TicketClassification[${index}][Reason01ListItemName]`, obj.Reason01ListItemName);
                    formData.append(`TicketClassification[${index}][Reason01ListItemId]`, obj.Reason01ListItemId);
                    formData.append(`TicketClassification[${index}][Reason02Id]`, obj.Reason02Id);
                    formData.append(`TicketClassification[${index}][Reason02ListItemName]`, obj.Reason02ListItemName);
                    formData.append(`TicketClassification[${index}][Reason02ListItemId]`, obj.Reason02ListItemId);
                });

                console.log(formData.get('TicketClassification[0]'));
            }

            $.ajax({
                type: 'POST',
                url: `${origin_url}/Ticket/AutoSaveData`,
                data: formData,
                processData: false, // Important: prevent jQuery from processing the data
                contentType: false, // Important: prevent jQuery from setting the content type
                success: function (response) {
                    console.log('Upload successful:', response);
                },
                error: function (xhr, status, error) {
                    console.error('Upload failed:', xhr.responseText);
                }
            });
        },
        RemoveDataFromAutoSave: function (conid, type) {
            if (conid !== null) {
                let dataInSession = screenAutoSaveData.Methods.Session('GET', `AUTOSAVE_${conid}`);

                if (dataInSession !== null) {
                    let dataArray = JSON.parse(dataInSession);

                    if (dataArray !== null) {
                        const newDataArray = dataArray.filter(item => item.type !== type);
                        screenAutoSaveData.Methods.Session('REMOVE', `AUTOSAVE_${conid}`)
                        screenAutoSaveData.Methods.Session('SET', `AUTOSAVE_${conid}`, newDataArray)
                    }
                }
            }
        },
        OnLoad: function () {

            const convid = screenAutoSaveData.Methods.GetConversationId();
            console.log(convid)
            if (convid !== '') {
                $.get(`${origin_url}/Ticket/OnInteraction`, function (data, status) {
                    if (status == 'success') {
                        const dataLen = data.length;
                        console.log(data)
                        if (dataLen > 0) {
                            const hasConversationId = data.filter(d => d.conversationId == convid).length > 0;
                            //console.log('hasConversationId', hasConversationId)
                            if (hasConversationId === false) {
                                console.log('REMOVED')
                                screenAutoSaveData.Methods.Session('REMOVE', `AUTOSAVE_${convid}`)
                            }
                        }
                    }

                    let dataInSession = screenAutoSaveData.Methods.Session('GET', `AUTOSAVE_${convid}`);

                    console.log('dataInSession', dataInSession)

                    if (dataInSession !== null) {
                        let dataArray = JSON.parse(dataInSession);

                        dataArray.forEach(d => {
                            console.log(d.type, d.value)
                            $(`#${d.type}`).val(d.value).change();
                            //if (d.type === 'alertInfoId') {
                            //    $('#ParentTicket').val(d.value)
                            //    $('#alertInfoId').append(`<div>Protocolo ${d.value} associado com sucesso!&nbsp;&nbsp;&nbsp;<button type="button" class="btn btn-outline-success" data-bs-dismiss="alert" aria-label="Close" onclick="tools.Events.OnUnbindTicket()"><span aria-hidden="true">Desvincular</span></button></div>`)
                            //    $('#alertInfoId').show()
                            //}
                            //else {
                            //    $(`#${d.type}`).val(d.value).change();
                            //}
                        })
                    }
                    else {
                        //console.log('Session Removed')
                    }
                })
            }
        }
    }

}

var tools = {
    Init: {
        ComboBox: function () {
            $(".ticket_init").select2ToTree({
                // treeData: {
                //     dataArr:mydata
                // },
                // maximumSelectionLength: 3,
                placeholder: "Opções...",
            });
        },
        ParentInput: function () {
            const parentTicket = $('#ParentTicket').val()

            if (parentTicket !== undefined && parentTicket !== '') {
                //$('#alertInfoId').text(`Protocolo ${parentTicket} associado com sucesso!`)

                const ticketSaved = $('#TicketSaved').val()
                console.log(ticketSaved)
                if (ticketSaved == "False") {
                    $('#alertInfoId').append(`<div class="alert alert-success" role="alert">Protocolo ${parentTicket} associado com sucesso!&nbsp;&nbsp;&nbsp;<button type="button" class="btn btn-outline-success" data-bs-dismiss="alert" aria-label="Close" onclick="tools.Events.OnUnbindTicket()"><span aria-hidden="true">Desvincular</span></button></div>`)
                }
                else {
                    $('#alertInfoId').append(`<div class="alert alert-success" role="alert">Protocolo ${parentTicket} associado com sucesso!&nbsp;&nbsp;&nbsp;</div>`)
                }

                //const ticketSaved = $('#TicketSaved').val()
                //console.log(ticketSaved)
                //if (ticketSaved == false) {
                //    /*                    $('#alertInfoId').append('<button type="button" data-bs-dismiss="alert" aria-label="Close" onclick="tools.Events.OnUnbindTicket()"></button>')*/
                //    $('#alertInfoId').append(`<div>Protocolo ${value} associado com sucesso!&nbsp;&nbsp;&nbsp;<button type="button" class="btn btn-outline-success" data-bs-dismiss="alert" aria-label="Close" onclick="tools.Events.OnUnbindTicket()"><span aria-hidden="true">Desvincular</span></button></div>`)
                //}

                $('#alertInfoId').show()
            }
        },

        TicketTransfer: function () {
            $("#TransferQueueGTId").select2ToTree({
                dropdownParent: $('#transferModal'),
                // treeData: {
                //     dataArr:mydata
                // },
                // maximumSelectionLength: 3,
                placeholder: "Opções...",
            });

            $("#TransferAreaId").select2ToTree({
                dropdownParent: $('#transferModal'),
                // treeData: {
                //     dataArr:mydata
                // },
                // maximumSelectionLength: 3,
                placeholder: "Opções...",
            });

            $("#TransferDemandTypeId").select2ToTree({
                dropdownParent: $('#transferModal'),
                // treeData: {
                //     dataArr:mydata
                // },
                // maximumSelectionLength: 3,
                placeholder: "Opções...",
            });

            $("#TransferServiceUnitId").select2ToTree({
                dropdownParent: $('#transferModal'),
                // treeData: {
                //     dataArr:mydata
                // },
                // maximumSelectionLength: 3,
                placeholder: "Opções...",
            });
        },
        IsInteractionWithMe: function () {
            const protocol = $('#Protocol').val();
            const user = $('#User').val();

            setInterval(() => {
                $.get(`${origin_url}/Ticket/IsInteractionWithMe/?protocol=${protocol}&email=${user}`, function (data, status) {
                    if (status == 'success') {
                        console.log(data)

                        if (data.isInteractionWithMe === false) {
                            window.location.href = `${origin_url}/Ticket/Index`
                        }
                    }
                })
            }, 5000);
        }
    },
    Methods: {
        LoadJourneyTableHover: function () {
            $(".journey-link").mouseover(function () {
                $(this).addClass("hover");
                $(this).css('cursor', 'pointer');
            });

            $(".journey-link").mouseleave(function () {
                $(this).removeClass("hover");
                $(this).css('cursor', 'default');
            });
        },
        //DetachFile: function (filename) {
        //    attached_files = attached_files.filter(f => f.name !== filename);
        //    $('#Files').val('');

        //    const dataTransfer = new DataTransfer();
        //    attached_files.map(f => {
        //        dataTransfer.items.add(f);
        //    });

        //    $("#Files").prop("files", dataTransfer.files);
        //}
    },

    Events: {

        OnJourneyLine: function (ticketId, childProtocol, childTicketId) {
            $.get(`${origin_url}/Ticket/BindTicket/?id=${ticketId}&childProtocol=${childProtocol}&childTicketId=${childTicketId}`, function (data, status) {
                if (status === 'success') {
                    //$('.modal-body').html(data)
                    $('#bindTicketBody').html(data)
                    $(`#journeyBtnId`).trigger('click')
                }
            })
        },
        OnBindTicket: function (value) {
            console.log(value)

            if (value !== undefined && value !== '') {
                $('#ParentTicket').val(value)
                $('#alertInfoId').empty();

                $('#alertInfoId').append(`<div class="alert alert-success" role="alert">Protocolo ${value} associado com sucesso!&nbsp;&nbsp;&nbsp;<button type="button" class="btn btn-outline-success" data-bs-dismiss="alert" aria-label="Close" onclick="tools.Events.OnUnbindTicket()"><span aria-hidden="true">Desvincular</span></button></div>`)
                //$('#protocolTextId').val(value);
                $('#alertInfoId').show()

                //const conversationid = screenAutoSaveData.Methods.GetConversationId();

                //if (conversationid !== '') {
                //    screenAutoSaveData.Methods.AutoSaveData('alertInfoId', value, conversationid);
                //}
            }

            // setTimeout(function() {
            // 	$('#alertInfoId').hide()
            // }, 5000);
        },
        OnUnbindTicket: function () {
            $('#ParentTicket').val('')
            $('#alertInfoId').hide();
            //const conversationid = screenAutoSaveData.Methods.GetConversationId();
            //screenAutoSaveData.Methods.RemoveDataFromAutoSave(conversationid, 'alertInfoId')
        },
        OnTransferModelOpen: function (ticketid, protocol, userid) {
            console.log(ticketid, protocol, userid)

            if (userid === undefined) {
                userid = 0;
            }

            const queuegt = $('#QueueGT').val()

            $.get(`${origin_url}/TicketTransfer/Create/?ticketid=${ticketid}&protocol=${protocol}&userid=${userid}&queuegt=${queuegt}`, function (data, status) {
                if (status === 'success') {
                    $('#transferTicketOpenId').html(data)
                    tools.Init.TicketTransfer()
                    tools.Events.OnTransferQueueGTChanged()
                    tools.Events.OnTransferAreaChange()
                    tools.Events.OnTransferDemandAreaChanged()
                    //tools.Events.OnTransferSubmit()
                }
            })
        },
        OnTransferQueueGTChanged: function () {
            $('#TransferQueueGTId').on('change', function () {
                var selectedValue = $(this).val();
                console.log(selectedValue)

                $('#transferServiceUnitDivId').hide()
                $('#transferAreaDivId').hide()
                $('#transferDemandTypeDivId').hide()
                $('#transferDemandObservationDivId').hide()
                $('#TransferDemandObservation').val('')
                $('#buttonTransferModal').prop('disabled', true)

                $.get(`${origin_url}/TicketTransfer/GetAreaByQueue/${selectedValue}`, function (data, status) {
                    if (status === 'success') {
                        console.log(data)
                        $('#TransferServiceUnitId').empty()
                        $('#TransferAreaId').empty()

                        switch (selectedValue) {
                            case "2":
                                $('#TransferAreaId').append(
                                    $('<option>', {
                                        value: '',
                                        text: 'Opções',
                                    })
                                )

                                if (data.length > 0) {
                                    $.each(data, function (index, value) {
                                        $('#TransferAreaId').append(
                                            $('<option>', {
                                                value: value.areaId,
                                                text: value.areaName,
                                            })
                                        )
                                    })
                                }

                                $('#transferAreaDivId').show()
                                break;
                            case "3":
                                $('#TransferServiceUnitId').append(
                                    $('<option>', {
                                        value: '',
                                        text: 'Opções',
                                    })
                                )

                                if (data.length > 0) {
                                    $.each(data[0].listItemAPICollection, function (index, value) {
                                        $('#TransferServiceUnitId').append(
                                            $('<option>', {
                                                value: value.listItemId,
                                                text: value.listItemName,
                                            })
                                        )
                                    })
                                    console.log('IdLista', data[0].listId)
                                    $('#ListId').val(data[0].listId)
                                }


                                $('#transferServiceUnitDivId').show()

                                $('#buttonTransferModal').prop('disabled', false)
                                break;
                            case "4":
                                $('#buttonTransferModal').prop('disabled', false)
                                break;
                        }
                    }
                })
            })
        },
        OnTransferAreaChange: function () {
            $('#TransferAreaId').on('change', function () {
                var selectedValue = $(this).val();
                console.log(selectedValue)

                $('#transferDemandObservationDivId').hide()
                $('#transferDemandTypeDivId').hide()
                $('#TransferDemandTypeId').empty()
                $('#TransferDemandObservation').val('')
                $('#buttonTransferModal').prop('disabled', true)

                $.get(`${origin_url}/TicketTransfer/GetDemandAreaByArea/${selectedValue}`, function (data, status) {
                    if (status === 'success') {
                        console.log(data)

                        if (data.length > 0) {

                            $('#TransferDemandTypeId').append(
                                $('<option>', {
                                    value: '',
                                    text: 'Opções',
                                })
                            )

                            $.each(data, function (index, value) {
                                $('#TransferDemandTypeId').append(
                                    $('<option>', {
                                        value: `${value.demandAreaId}:${value.formId}`,
                                        text: value.demandAreaName,
                                    })
                                )
                            })

                            $('#transferDemandTypeDivId').show()
                        }
                        else {
                            $('#buttonTransferModal').prop('disabled', false)
                        }
                    }
                })
            })
        },
        OnTransferDemandAreaChanged: function () {
            $('#TransferDemandTypeId').on('change', function () {
                var selectedValue = $(this).val();
                console.log(selectedValue)

                if (selectedValue.includes(':')) {
                    let selectedValueArray = selectedValue.split(':')
                    const transferFormId = selectedValueArray[1]
                    $.get(`${origin_url}/TicketTransfer/GetTransferForm/${transferFormId}`, function (data, status) {
                        if (status === 'success') {
                            console.log(data)

                            if (data.length > 0) {
                                const formInfo = data[0]
                                $('#transferDemandObservationDivId').show()

                                let formBodyArray = formInfo.formBody.split('\\r\\n')
                                let formBody = ''

                                if (formBodyArray.length > 0) {
                                    $.each(formBodyArray, function (index, value) {
                                        let line = value.replace('\n', '')
                                        console.log(line)
                                        formBody = `${formBody}${line}\r\n`
                                    })
                                }
                                console.log(formBody)

                                $('#TransferDemandObservation').val(formBody)
                            }

                        }
                    })
                }
                $('#buttonTransferModal').prop('disabled', false)
            })
        },
        //OnTransferSubmit: function () {
        //    $('#ticketTransferForm').on('submit', function (e) {
        //        e.preventDefault();
        //        var formData = $(this).serialize();
        //        console.log(formData)

        //        $.post(`${origin_url}/TicketTransfer/SendTransfer`, formData,
        //            function (data, status) {
        //                if (status === 'success') {
        //                    if (data.isValid) {

        //                        $('#transferSuccessMessage').show()

        //                        setTimeout(function () {
        //                            $('#transferSuccessMessage').hide()
        //                        }, 7000);

        //                        //$('#alertModalBody').html('<div class="alert alert-success" role="alert">Chamado Transferido com Sucesso!</div >')
        //                        //$(`#alertBtnModalId`).trigger('click')
        //                        $('#requestTransfer').prop('disabled', true)
        //                    }
        //                    else {
        //                        $('#transferErrorMessage').show()

        //                        //$('#alertModalBody').html('<div class="alert alert-danger" role="alert">Erro Transferindo Chamado!</div >')
        //                        //$(`#alertBtnModalId`).trigger('click')
        //                        setTimeout(function () {
        //                            $('#transferErrorMessage').hide()
        //                        }, 7000);
        //                    }
        //                }
        //                else {
        //                    $('#transferErrorMessage').show()

        //                    setTimeout(function () {
        //                        $('#transferErrorMessage').hide()
        //                    }, 7000);
        //                    //$('#alertModalBody').html('<div class="alert alert-danger" role="alert">Erro Transferindo Chamado!</div >')
        //                    //$(`#alertBtnModalId`).trigger('click')
        //                }
        //            },
        //            null,
        //            "json"
        //        );

        //    })
        //}
        OnTransferSubmit: function () {
            console.log('OnTransferSubmit');

            let ticketTransferModel = {};

            ticketTransferModel.TicketId = $('#TicketId').val();
            ticketTransferModel.UserId = $('#UserId').val();
            ticketTransferModel.QueueGT = $('#QueueGT').val();
            ticketTransferModel.TransferReason = $('#TransferReason').val();
            ticketTransferModel.TransferQueueGTId = $('#TransferQueueGTId').val();
            ticketTransferModel.TransferServiceUnitId = $('#TransferServiceUnitId').val();
            ticketTransferModel.TransferAreaId = $('#TransferAreaId').val();
            ticketTransferModel.TransferDemandTypeId = $('#TransferDemandTypeId').val();
            ticketTransferModel.TransferDemandObservation = $('#TransferDemandObservation').val();
            ticketTransferModel.TransferDemandObservation = $('#TransferDemandObservation').val();

            console.log(ticketTransferModel);
            
            $.post(`${origin_url}/TicketTransfer/SendTransfer`, ticketTransferModel,
                function (data, status) {
                    if (status === 'success') {
                        if (data.isValid) {

                            $('#transferSuccessMessage').show()

                            setTimeout(function () {
                                $('#transferSuccessMessage').hide()
                            }, 7000);

                            //$('#alertModalBody').html('<div class="alert alert-success" role="alert">Chamado Transferido com Sucesso!</div >')
                            //$(`#alertBtnModalId`).trigger('click')
                            $('#requestTransfer').prop('disabled', true)
                        }
                        else {
                            $('#transferErrorMessage').show()

                            //$('#alertModalBody').html('<div class="alert alert-danger" role="alert">Erro Transferindo Chamado!</div >')
                            //$(`#alertBtnModalId`).trigger('click')
                            setTimeout(function () {
                                $('#transferErrorMessage').hide()
                            }, 7000);
                        }
                    }
                    else {
                        $('#transferErrorMessage').show()

                        setTimeout(function () {
                            $('#transferErrorMessage').hide()
                        }, 7000);
                        //$('#alertModalBody').html('<div class="alert alert-danger" role="alert">Erro Transferindo Chamado!</div >')
                        //$(`#alertBtnModalId`).trigger('click')
                    }
                },
                null,
                "json"
            );

        }
    }
}

var classification = {
    Events: {
        OnChangeOptions: function () {
            $('.ticket_classification').on('change', function (e) {
                const elementid = e.target.id
                var selectedValue = $(this).val();

                if (elementid.includes('ClassificationDemandId')) {

                    $('.ticket_types').empty()
                    $('.ticket_reasons').empty()
                    $('.ticket_reasons_listitem').empty()
                    $('.ticket_submotive01').empty()
                    $('.ticket_submotive01list').empty()
                    $('.ticket_submotive02').empty()
                    $('.ticket_submotive02list').empty()

                    classification.Triggers.ClassificationRequest('DEMAND', selectedValue, classification.Callbacks.ClassificationCb)
                }
                else if (elementid.includes('ClassificationDemandTypeId')) {

                    $('.ticket_reasons').empty()
                    $('.ticket_reasons_listitem').empty()
                    $('.ticket_submotive01').empty()
                    $('.ticket_submotive01list').empty()
                    $('.ticket_submotive02').empty()
                    $('.ticket_submotive02list').empty()

                    classification.Triggers.ClassificationRequest('TYPE', selectedValue, classification.Callbacks.ClassificationCb)
                }
                else if (elementid.includes('ClassificationReasonId')) {
                    $('.ticket_reasons_listitem').empty()
                    $('.ticket_submotive01').empty()
                    $('.ticket_submotive01list').empty()
                    $('.ticket_submotive02').empty()
                    $('.ticket_submotive02list').empty()

                    var values = selectedValue.split(":")
                    var reasonid = values[0]
                    var listid = values[1]

                    classification.Triggers.ClassificationRequest('REASON', listid, classification.Callbacks.ClassificationCb)

                    classification.Triggers.ClassificationRequest('REASON_SUBMOTIVE01', reasonid, classification.Callbacks.ClassificationCb)
                }
                else if (elementid.includes('ClassificationSubmotive01Id')) {

                    $('.ticket_submotive01list').empty()
                    $('.ticket_submotive02').empty()
                    $('.ticket_submotive02list').empty()

                    var values = selectedValue.split(":")
                    var reasonid = values[0]
                    var listid = values[1]

                    classification.Triggers.ClassificationRequest('REASON_SUBMOTIVE01_LIST', listid, classification.Callbacks.ClassificationCb)

                    classification.Triggers.ClassificationRequest('REASON_SUBMOTIVE02', reasonid, classification.Callbacks.ClassificationCb)
                }
                else if (elementid.includes('ClassificationSubmotive02Id')) {
                    classification.Triggers.ClassificationRequest('REASON_SUBMOTIVE02_LIST', selectedValue, classification.Callbacks.ClassificationCb)
                }
            })
        }
    },

    Triggers: {
        ClassificationRequest: function (eventFromCb, selectedValue, callback) {
            let target_url = ''
            let target_class = ''

            console.log(selectedValue)

            if (eventFromCb === 'DEMAND') {
                target_url = `${origin_url}/ClassificationDemandType/GetClassificationDemandTypeList/${selectedValue}`

                target_class = 'ticket_types'
            }
            else if (eventFromCb === 'TYPE') {
                target_url = `${origin_url}/ClassificationListItems/GetClassificationReasonList/${selectedValue}`

                target_class = 'ticket_reasons'
            }
            else if (eventFromCb === 'REASON') {
                target_url = `${origin_url}/ClassificationListItems/GetClassificationListItemsActive/${selectedValue}`

                target_class = 'ticket_reasons_listitem'
            }
            else if (eventFromCb === 'REASON_SUBMOTIVE01') {
                target_url = `${origin_url}/ClassificationListItems/ClassificationReasonChildren/${selectedValue}`

                target_class = 'ticket_submotive01'
            }
            else if (eventFromCb === 'REASON_SUBMOTIVE01_LIST') {
                target_url = `${origin_url}/ClassificationListItems/GetClassificationListItemsActive/${selectedValue}`

                target_class = 'ticket_submotive01list'
            }
            else if (eventFromCb === 'REASON_SUBMOTIVE02') {
                target_url = `${origin_url}/ClassificationListItems/ClassificationReasonChildren/${selectedValue}`

                target_class = 'ticket_submotive02'
            }
            else if (eventFromCb === 'REASON_SUBMOTIVE02_LIST') {
                target_url = `${origin_url}/ClassificationListItems/GetClassificationListItemsActive/${selectedValue}`

                target_class = 'ticket_submotive02list'
            }

            $.get(target_url, function (data, status) {
                if (status === 'success') {
                    callback(target_class, data)
                }
            })
        }
    },

    Callbacks: {
        ClassificationCb: function (target_class, data) {
            console.log(target_class, data)
            $(`.${target_class}`).empty()

            $(`.${target_class}`).append(
                $('<option>', {
                    value: '',
                    text: 'Opções',
                })
            )

            $.each(data, function (index, value) {
                if (value.active) {
                    let id = value.id

                    if (target_class === 'ticket_reasons') {
                        id = `${value.id}:${value.classificationListReasonId}`
                    }
                    else if (target_class === 'ticket_submotive01') {
                        id = `${value.id}:${value.classificationListReasonId}`
                    }
                    else if (target_class === 'ticket_submotive02') {
                        // id = `${value.id}:${value.classificationListReasonId}`
                        id = value.classificationListReasonId
                    }

                    $(`.${target_class}`).append(
                        $('<option>', {
                            value: id,
                            text: value.name,
                        })
                    )
                }
            })
        }
    }
}
