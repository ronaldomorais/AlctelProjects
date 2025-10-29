$(function () {
    ticketClassification.Init.select2ToTree();
    ticketClassification.Events.OnTicketCriticalityChanged();
});


var ticketClassification = {
    Init: {
        select2ToTree: function () {
            const selectsArray = ['ManifestationTypeId', 'ServiceUnitId', 'ServiceId', 'Reason01ListItemId', 'Reason02ListItemId', 'TicketCriticalityId'];

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

            $('#Reason01ListItemId').empty();

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

                                        $('#Reason01ListItemId').append(
                                            $('<option>', {
                                                value: '',
                                                text: 'Opções',
                                            })
                                        )

                                        if (data.length > 0) {
                                            $.each(data, function (index, value) {
                                                $('#Reason01ListItemId').append(
                                                    $('<option>', {
                                                        value: value.listItemId,
                                                        text: value.listItemName,
                                                    })
                                                )
                                            })
                                        }

                                        //$('#Reason01ListItemId').prop('required', true);
                                        $('#Reason01Id').val(data[0].id);
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

            $('#Reason02ListItemId').val("0");

            $('#reason02DivId').hide();

            $('#Reason02ListItemId').empty();

            const manifestationid = $('#ManifestationTypeId').val();
            const serviceId = $('#ServiceId').val();
            //const reason01ListId = $('#Reason01ListId').val();
            const reason01ListId = $('#Reason01Id').val();

            if (selectedValue != "0") {
                $.get(`${origin_url}/TicketClassification/GetTicketClassificationReasonListItems/?manifestationid=${manifestationid}&serviceid=${serviceId}&parentId=${reason01ListId}`, function (data, success) {

                    if (success === 'success') {
                        if (data !== null && data.length > 0) {
                            console.log('Carregar Reason 2', data);
                            $('#Reason02ListItemId').append(
                                $('<option>', {
                                    value: '',
                                    text: 'Opções',
                                })
                            )

                            if (data.length > 0) {
                                $.each(data, function (index, value) {
                                    console.log(value);
                                    $('#Reason02ListItemId').append(
                                        $('<option>', {
                                            value: value.listItemId,
                                            text: value.listItemName,
                                        })
                                    )
                                })
                            }

                            //$('#Reason02ListItemId').prop('required', true);
                            $('#Reason02Id').val(data[0].id);
                            $('#reason02DivId').show();
                        }
                    }
                });
            }
        },

        OnAddClassification: function () {
            const currentUserId = $('#currentUserId').val();
            const currentUsername = $('#currentUsername').val();
            const ticketClassificationCounter = $('#ticketClassificationTableId tbody tr').length;

            let classificationByUserCounter = 1;
            $('#ticketClassificationTableId tbody tr').each(function () {
                const row = $(this);
                const target_td = row.find("td:nth-child(6)");
                const userid_td = target_td.find('input[type="hidden"]').val();

                if (userid_td === userid_td) {
                    classificationByUserCounter++;
                }
            });


            console.log(currentUserId, currentUsername, classificationByUserCounter)

            //if (ticketClassificationCounter >= 6) {
            if (classificationByUserCounter >= 6) {
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
            let reason01ListItemName = $('#Reason01ListItemId option:selected').text();
            let reason01ListItemId = $('#Reason01ListItemId').val();

            let reason02Id = $('#Reason02Id').val();
            let reason02ListItemName = $('#Reason02ListItemId option:selected').text();
            let reason02ListItemId = $('#Reason02ListItemId').val();

            const reason01DivId = $('#reason01DivId').is(':visible');
            const reason02DivId = $('#reason02DivId').is(':visible');

            if (reason01DivId && (reason01ListItemId === '' || reason01ListItemId === undefined || reason01ListItemId === null)) {
                $('#tickektClassificationMessage').show();

                setTimeout(() => {
                    $('#tickektClassificationMessage').hide();
                }, 5000);

                return;
            }

            
            if (reason02DivId && (reason02ListItemId === '' || reason02ListItemId === undefined || reason02ListItemId === null)) {
                $('#tickektClassificationMessage').show();

                setTimeout(() => {
                    $('#tickektClassificationMessage').hide();
                }, 5000);

                return;
            }
            
            if (manifestationTypeName === 'Opções') {
                $('#btnTicketClassificationClose').trigger('click');
                return;
            }

            if (reason01Id === null || reason01Id === undefined || reason01Id === null) {
                reason01Id = '0';
            }

            if (reason01ListItemId === null || reason01ListItemId === undefined || reason01ListItemId === null) {
                reason01ListItemId = '0'
            }

            if (reason02Id === null || reason02Id === undefined || reason02Id === null) {
                reason02Id = '0';
            }

            if (reason02ListItemId === null || reason02ListItemId === undefined || reason02ListItemId === null) {
                reason02ListItemId = '0'
            }

            if (serviceUnitName === "Opções" || serviceUnitName === undefined || serviceUnitName === null) {
                serviceUnitName = '';
            }

            if (serviceName === "Opções" || serviceName === undefined || serviceName === null) {
                serviceName = '';
            }

            if (reason01ListItemName === "Opções" || reason01ListItemName === undefined || reason01ListItemName === null) {
                reason01ListItemName = '';
            }

            if (reason02ListItemName === "Opções" || reason02ListItemName === undefined || reason02ListItemName === null) {
                reason02ListItemName = '';
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
                    <input type="hidden" name="TicketClassification[${ticketClassificationCounter}].Reason01ListItemName" value="${reason01ListItemName}" />${reason01ListItemName}
                    <input type="hidden" name="TicketClassification[${ticketClassificationCounter}].Reason01ListItemId" value="${reason01ListItemId}" />  
                </td>
                <td>
                    <input type="hidden" name="TicketClassification[${ticketClassificationCounter}].Reason02Id" value="${reason02Id}" />                     
                    <input type="hidden" name="TicketClassification[${ticketClassificationCounter}].Reason02ListItemName" value="${reason02ListItemName}" /> ${reason02ListItemName}
                    <input type="hidden" name="TicketClassification[${ticketClassificationCounter}].Reason02ListItemId" value="${reason02ListItemId}" />  
                </td>
                <td>
					<input type="hidden" name="TicketClassification[${ticketClassificationCounter}].UserId" value="${currentUserId}" />
					${currentUsername}
                </td>
            </tr>`

            classificationTable.append(row);

            $('#btnTicketClassificationClose').trigger('click');

            let autoSaveTable = $('#autoSaveTable').val();
            console.log(autoSaveTable);

            if (autoSaveTable !== undefined) {
               
                let ticketClassificationAutoSaveArray = [];
                let ticketClassificationAutoSave = {};
                ticketClassificationAutoSave.ManifestationTypeId = manifestationTypeId;
                ticketClassificationAutoSave.ManifestationTypeName = manifestationTypeName;
                ticketClassificationAutoSave.ServiceUnitId = serviceUnitId;
                ticketClassificationAutoSave.ServiceUnitName = serviceUnitName;
                ticketClassificationAutoSave.ServiceId = serviceId;
                ticketClassificationAutoSave.ServiceName = serviceName;
                ticketClassificationAutoSave.Reason01Id = reason01Id;
                ticketClassificationAutoSave.Reason01ListItemName = reason01ListItemName;
                ticketClassificationAutoSave.Reason01ListItemId = reason01ListItemId;
                ticketClassificationAutoSave.Reason02Id = reason02Id;
                ticketClassificationAutoSave.Reason02ListItemName = reason02ListItemName;
                ticketClassificationAutoSave.reason02ListItemId = reason02ListItemId;

                if (autoSaveTable !== '') {
                    //console.log(autoSaveTable);
                    ticketClassificationAutoSaveArray = JSON.parse(autoSaveTable);
                }

                ticketClassificationAutoSaveArray.push(ticketClassificationAutoSave);
                const ticketClassificationAutoSaveArrayStr = JSON.stringify(ticketClassificationAutoSaveArray);
                $('#autoSaveTable').val(ticketClassificationAutoSaveArrayStr);
            }

        }
    }
}