$(function () {
    ticketClassification.Init.select2ToTree();
    ticketClassification.Events.OnTicketCriticalityChanged();
});


var ticketClassification = {
    Init: {
        select2ToTree: function () {
            const selectsArray = ['ManifestationTypeId', 'ServiceUnitId', 'ServiceId', 'Reason01Id', 'Reason02Id', 'TicketCriticalityId'];

            Array.from(selectsArray).forEach(item => {

                if (item === 'TicketCriticalityId') {
                    $(`#${item}`).select2ToTree({
                        placeholder: "Opções...",
                    });

                }
                else {
                    $(`#${item}`).select2ToTree({
                        dropdownParent: $('#ticketClassificationModal'),
                        placeholder: "Opções...",
                    });

                }

            });

        },
    },

    Events: {
        OnTicketCriticalityChanged: function () {
            const selectedValue = $('#TicketCriticalityId option:selected').text();

            switch (selectedValue.toUpperCase()) {
                case "ALTA":
                    $('#TicketCriticalityId').siblings('.select2-container').css('border', '3px solid red');
                    break;
                case "MÉDIA":
                    $('#TicketCriticalityId').siblings('.select2-container').css('border', '3px solid yellow');
                    break;
                case "BAIXA":
                    $('#TicketCriticalityId').siblings('.select2-container').css('border', '3px solid green');
                    break;
            }
        },

        OnTicketClassificationListChanged: function () {
            var selectedValue = $('#Listname').val();

            $.get(`${origin_url}/TicketClassification/TicketClassificationListItemsIndex/?id=${selectedValue}`, function (data, success) {
                if (success === 'success') {
                    if (data != null) {
                        console.log(data);

                        $('#ticketClassificationListItemsId').html(data);

                        //$('#ListItemName').append(
                        //    $('<option>', {
                        //        value: "0",
                        //        text: "Opções",
                        //    })
                        //)

                        //$.each(data, function (index, value) {
                        //    $('#ListItemName').append(
                        //        $('<option>', {
                        //            value: value.listItemId,
                        //            text: value.name,
                        //        })
                        //    )
                        //})
                    }
                }
            });
        },

        OnTicketClassificationListItemEdit: function (id) {
            $.get(`${origin_url}/TicketClassification/EditListItem/?id=${id}`, function (data, success) {
                if (success === 'success') {
                    if (data != null) {
                        console.log(data);

                        $('#ticketClassificationListItemsId').html(data);

                        //$('#ListItemName').append(
                        //    $('<option>', {
                        //        value: "0",
                        //        text: "Opções",
                        //    })
                        //)

                        //$.each(data, function (index, value) {
                        //    $('#ListItemName').append(
                        //        $('<option>', {
                        //            value: value.listItemId,
                        //            text: value.name,
                        //        })
                        //    )
                        //})
                    }
                }
            });
        },

        OnAddClassificationClicked: function () {
            $.get(`${origin_url}/TicketClassification/ScreenPopup`, function (data, success) {

                if (success === 'success') {
                    $('#ticketClassificationOpenId').html(data);
                    ticketClassification.Init.select2ToTree();
                }
            });
        },

        OnManifestationTypeChanged: function () {
            var selectedValue = $('#ManifestationTypeId').val();
            
            $('#ServiceUnitId').val("0");
            $('#ServiceId').val("0");
            $('#Reason01Id').val("0");
            $('#Reason02Id').val("0");

            $('#serviceUnitDivId').hide();
            $('#serviceDivId').hide();
            $('#programDivId').hide();
            $('#reason01DivId').hide();
            $('#reason02DivId').hide();

            $('#btnAddClassificationId').prop('disabled', true); 

            $('#ServiceId').empty();

            if (selectedValue != "0") {

                $.get(`${origin_url}/TicketClassification/GetTicketClassificationServiceByManifestation/?id=${selectedValue}`, function (data, success) {

                    if (success === 'success') {
                        if (data !== null) {

                            $('#ServiceId').append(
                                $('<option>', {
                                    value: '',
                                    text: 'Opções',
                                })
                            )

                            if (data.length > 0) {
                                $.each(data, function (index, value) {
                                    $('#ServiceId').append(
                                        $('<option>', {
                                            value: value.id,
                                            text: value.name,
                                        })
                                    )
                                })
                            }

                            $('#serviceUnitDivId').show();
                        }
                    }
                });

            }
        },

        OnServiceUnitChanged: function () {
            var selectedValue = $('#ServiceUnitId').val();

            $('#ServiceId').val("0");
            $('#Reason01Id').val("0");
            $('#Reason02Id').val("0");

            $('#serviceDivId').hide();
            $('#programDivId').hide();
            $('#reason01DivId').hide();
            $('#reason02DivId').hide();

            $('#btnAddClassificationId').prop('disabled', true); 

            if (selectedValue != "0") {
                $('#serviceDivId').show();
            }
        },

        OnServiceChanged: function () {
            const serviceId = $('#ServiceId').val();

            $('#Reason01Id').val("0");
            $('#Reason02Id').val("0");
            $('#ProgramId').val('');
            $('#ProgramName').val('');

            $('#programDivId').hide();
            $('#reason01DivId').hide();
            $('#reason02DivId').hide();
            $('#btnAddClassificationId').prop('disabled', true); 

            $('#Reason01Id').empty();

            if (serviceId != "0") {

                $.get(`${origin_url}/TicketClassification/GetTicketClassificationProgramByService/?id=${serviceId}`, function (data, success) {

                    if (success === 'success') {
                        if (data !== null && data.length > 0) {

                            $('#ProgramId').val(data[0].id);
                            $('#ProgramName').val(data[0].name);
                            $('#programDivId').show();

                            const manifestationid = $('#ManifestationTypeId').val();

                            $('#btnAddClassificationId').prop('disabled', false); 

                            $.get(`${origin_url}/TicketClassification/GetTicketClassificationReasonListItems/?manifestationid=${manifestationid}&serviceid=${serviceId}`, function (data, success) {
                                if (success === 'success') {
                                    if (data !== null && data.length > 0) {
                                        console.log(data);

                                        $('#Reason01Id').append(
                                            $('<option>', {
                                                value: '',
                                                text: 'Opções',
                                            })
                                        )

                                        if (data.length > 0) {
                                            $.each(data, function (index, value) {
                                                $('#Reason01Id').append(
                                                    $('<option>', {
                                                        value: value.id,
                                                        text: value.listItemName,
                                                    })
                                                )
                                            })
                                        }

                                        console.log(data);
                                        console.log(data[0].idLista);
                                        $('#Reason01ListId').val(data[0].listId);

                                        $('#reason01DivId').show();
                                        $('#btnAddClassificationId').prop('disabled', false); 
                                    }
                                }
                            });
                        }
                    }
                });

            }
        },

        OnReason01Changed: function () {
            var selectedValue = $('#Reason01Id').val();

            $('#Reason02Id').val("0");

            $('#reason02DivId').hide();

            $('#Reason02Id').empty();

            const manifestationid = $('#ManifestationTypeId').val();
            const serviceId = $('#ServiceId').val();
            const reason01ListId = $('#Reason01ListId').val();

            if (selectedValue != "0") {
                $.get(`${origin_url}/TicketClassification/GetTicketClassificationReasonListItems/?manifestationid=${manifestationid}&serviceid=${serviceId}&parentId=${reason01ListId}`, function (data, success) {

                    if (success === 'success') {
                        if (data !== null && data.length > 0) {
                            
                            $('#Reason02Id').append(
                                $('<option>', {
                                    value: '',
                                    text: 'Opções',
                                })
                            )

                            if (data.length > 0) {
                                $.each(data, function (index, value) {
                                    $('#Reason02Id').append(
                                        $('<option>', {
                                            value: value.Id,
                                            text: value.Name,
                                        })
                                    )
                                })
                            }

                            $('#Reason02ListId').val(data[0].idLista);

                            $('#reason02DivId').show();
                        }
                    }
                });
            }
        },

        OnAddClassification: function () {
            const ticketClassificationCounter = $('#ticketClassificationTableId tbody tr').length;

            if (ticketClassificationCounter >= 6) {
                $('#btnTicketClassificationClose').trigger('click');
                $('#ticketClassificationMessage').show();
                setTimeout(() => {
                    $('#ticketClassificationMessage').hide();
                }, 5000);
                return;
            }


            const manifestationTypeId = $('#ManifestationTypeId').val();
            let manifestationTypeName = $('#ManifestationTypeId option:selected').text();

            const serviceUnitId = $('#ServiceUnitId').val();
            let serviceUnitName = $('#ServiceUnitId option:selected').text();

            const serviceId = $('#ServiceId').val();
            let serviceName = $('#ServiceId option:selected').text();

            let reason01Id = $('#Reason01Id').val();
            let reason01Name = $('#Reason01Id option:selected').text();
            let reason01ListId = $('#Reason01ListId').val();

            let reason02Id = $('#Reason02Id').val();
            let reason02Name = $('#Reason02Id option:selected').text();
            let reason02ListId = $('#Reason01ListId').val();
            
            if (manifestationTypeName === 'Opções') {
                $('#btnTicketClassificationClose').trigger('click');
                return;
            }

            if (reason01Id === null) {
                reason01Id = '0';
            }

            if (reason02Id === null) {
                reason02Id = '0';
                reason02ListId = '0'
            }

            if (serviceUnitName === "Opções") {
                serviceUnitName = '';
            }

            if (serviceName === "Opções") {
                serviceName = '';
            }

            if (reason01Name === "Opções") {
                reason01Name = '';
            }

            if (reason02Name === "Opções") {
                reason02Name = '';
            }


            $('#ticketClassificationDivId').show();

            let classificationTable = $('#ticketClassificationTableId tbody');

            let row = `
            <tr>
                <td>
                    <input type="hidden" name="TicketClassification[${ticketClassificationCounter}].ManifestationTypeId" value="${manifestationTypeId}" />
                    <input type="hidden" name="TicketClassification[${ticketClassificationCounter}].ManifestationTypeName" value="${manifestationTypeName}" />
                    ${manifestationTypeName}
                </td>
                <td>
                    <input type="hidden" name="TicketClassification[${ticketClassificationCounter}].ServiceUnitId" value="${serviceUnitId}" />
                    <input type="hidden" name="TicketClassification[${ticketClassificationCounter}].ServiceUnitName" value="${serviceUnitName}" />
                    ${serviceUnitName}
                </td>
                <td>
                    <input type="hidden" name="TicketClassification[${ticketClassificationCounter}].ServiceId" value="${serviceId}" />                     
                    <input type="hidden" name="TicketClassification[${ticketClassificationCounter}].ServiceName" value="${serviceName}" /> ${serviceName}
                </td>
                <td>
                    <input type="hidden" name="TicketClassification[${ticketClassificationCounter}].Reason01Id" value="${reason01Id}" />                    
                    <input type="hidden" name="TicketClassification[${ticketClassificationCounter}].Reason01Name" value="${reason01Name}" />${reason01Name}
                    <input type="hidden" name="TicketClassification[${ticketClassificationCounter}].Reason01ListId" value="${reason01ListId}" />  
                </td>
                <td>
                    <input type="hidden" name="TicketClassification[${ticketClassificationCounter}].Reason02Id" value="${reason02Id}" />                     
                    <input type="hidden" name="TicketClassification[${ticketClassificationCounter}].Reason02Name" value="${reason02Name}" /> ${reason02Name}
                    <input type="hidden" name="TicketClassification[${ticketClassificationCounter}].Reason02ListId" value="${reason02ListId}" />  
                </td>
            </tr>`

            classificationTable.append(row);

            $('#btnTicketClassificationClose').trigger('click');
        }
    }
}