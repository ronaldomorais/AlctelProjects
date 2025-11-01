
var ticket_transfer = {
    Init: {
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
    },
    Events: {
        OnTransferModelOpen: function (ticketid, protocol, userid) {
            console.log(ticketid, protocol, userid)

            if (userid === undefined) {
                userid = 0;
            }

            const queuegt = $('#QueueGT').val()

            $.get(`${origin_url}/TicketTransfer/Create/?ticketid=${ticketid}&protocol=${protocol}&userid=${userid}&queuegt=${queuegt}&screenorigin=Edit`, function (data, status) {
                if (status === 'success') {
                    $('#transferTicketOpenId').html(data)
                    ticket_transfer.Init.TicketTransfer()
                    ticket_transfer.Events.OnTransferQueueGTChanged()
                    ticket_transfer.Events.OnTransferAreaChange()
                    ticket_transfer.Events.OnTransferDemandAreaChanged()
                    //ticket_transfer.Events.OnTransferSubmit()
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