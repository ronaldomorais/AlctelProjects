$(function () {
    slaAlertConfig.Init.select2ToTree();
});


var slaAlertConfig = {
    Init: {
        select2ToTree: function () {
            const selectsArray = ['ManifestationTypeId', 'ServiceId', 'ProgramId', 'Reason01ListItemId', 'Reason02ListItemId', 'CriticalityId'];

            Array.from(selectsArray).forEach(item => {

                $(`#${item}`).select2ToTree({
                    placeholder: "Opções...",
                });

            });

        },
    },

    Events: {
        OnManifestationTypeChanged: function () {
            var selectedValue = $('#ManifestationTypeId').val();

            //$('#programDivId').hide();
            $('#slaDivId').hide();
            $('#alarmDivId').hide();
            $('#serviceDivId').hide();
            $('#reason01DivId').hide();
            $('#reason02DivId').hide();
            $('#criticalityDivId').hide();
            $('#btnSlaCreate').prop('disabled', true);
            $('#CriticalityId').siblings('.select2-container').css('border', '1px solid #ccc');

            //$('#ManifestationTypeId').val("0");
            //$('#ServiceId').val("0");
            //$('#Reason01ListItemId').val("0");
            //$('#Reason02ListItemId').val("0");
            $('#CriticalityId').val("0").change();

            $('#ServiceId').empty();
            $('#Reason01ListItemId').empty();
            $('#Reason02ListItemId').empty();

            if (selectedValue !== '0') {
                //$('#programDivId').show();

                $.get(`${origin_url}/TicketClassification/GetTicketClassificationServiceByManifestation/?id=${selectedValue}`, function (data, success) {
                    
                    if (success === 'success') {
                        if (data !== null) {

                            $('#ServiceId').append(
                                $('<option>', {
                                    value: '0',
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

                            $('#serviceDivId').show();
                        }
                    }
                });
            }
        },

        OnServiceChanged: function () {
            var serviceId = $('#ServiceId').val();
            const manifestationid = $('#ManifestationTypeId').val();

            $('#slaDivId').hide();
            $('#alarmDivId').hide();
            $('#reason01DivId').hide();
            $('#reason02DivId').hide();
            $('#criticalityDivId').hide();
            $('#btnSlaCreate').prop('disabled', true);
            $('#CriticalityId').siblings('.select2-container').css('border', '1px solid #ccc');

            //$('#ServiceId').val("0");
            //$('#Reason01ListItemId').val("0");
            //$('#Reason02ListItemId').val("0");
            $('#CriticalityId').val("0").change();


            $('#Reason01ListItemId').empty();
            $('#Reason02ListItemId').empty();

            if (serviceId !== '0') {
                
                $.get(`${origin_url}/TicketClassification/GetTicketClassificationReasonListItems/?manifestationid=${manifestationid}&serviceid=${serviceId}`, function (data, success) {
                    if (success === 'success') {
                        if (data !== null && data.length > 0) {

                            $('#Reason01ListItemId').append(
                                $('<option>', {
                                    value: '0',
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

                            $('#Reason01Id').val(data[0].id);
                            $('#Reason01ListId').val(data[0].listId);

                            $('#reason01DivId').show();
                        }
                        else {
                            $('#criticalityDivId').show();
                        }
                    }
                });
            }
        },

        //OnProgramChanged: function () {
        //    var selectedValue = $('#ProgramId').val();

        //    if (selectedValue !== '0') {
        //        $('#reason01DivId').show();
        //        $('#serviceDivId').show();
        //        $('#btnTicketClassification').prop('disabled', false);
        //    }
        //    else {
        //        $('#reason01DivId').hide();
        //        $('#reason02DivId').hide();
        //        $('#serviceDivId').hide();
        //        $('#btnTicketClassification').prop('disabled', true);
        //    }
        //},

        OnReason01Changed: function () {
            var serviceId = $('#ServiceId').val();
            const manifestationid = $('#ManifestationTypeId').val();
            const reason01ListId = $('#Reason01ListId').val();
            const reason01Id = $('#Reason01Id').val();

            $('#slaDivId').hide();
            $('#alarmDivId').hide();
            $('#reason02DivId').hide();
            $('#criticalityDivId').hide();
            $('#btnSlaCreate').prop('disabled', true);
            $('#CriticalityId').siblings('.select2-container').css('border', '1px solid #ccc');

            //$('#ManifestationTypeId').val("0");
            //$('#ServiceId').val("0");
            //$('#Reason01ListItemId').val("0");
            //$('#Reason02ListItemId').val("0");
            $('#CriticalityId').val("0").change();

            $('#Reason02ListItemId').empty();

            if (reason01Id !== '0') {
                //$('#reason02DivId').show();

                $.get(`${origin_url}/TicketClassification/GetTicketClassificationReasonListItems/?manifestationid=${manifestationid}&serviceid=${serviceId}&parentId=${reason01Id}`, function (data, success) {

                    if (success === 'success') {

                        if (data !== null && data.length > 0) {

                            $('#Reason02ListItemId').append(
                                $('<option>', {
                                    value: '0',
                                    text: 'Opções',
                                })
                            )

                            if (data.length > 0) {
                                $.each(data, function (index, value) {
                                    $('#Reason02ListItemId').append(
                                        $('<option>', {
                                            value: value.listItemId,
                                            text: value.listItemName,
                                        })
                                    )
                                })
                            }

                            $('#Reason02Id').val(data[0].id);
                            $('#Reason02ListId').val(data[0].listId);

                            $('#reason02DivId').show();
                        }
                        else {
                            $('#criticalityDivId').show();
                        }
                    }
                });
            }
            else {
                $('#reason02DivId').hide();
            }
        },

        OnReason02Changed: function () {
            const reason02ListId = $('#Reason02ListId').val();

            $('#slaDivId').hide();
            $('#alarmDivId').hide();
            $('#btnSlaCreate').prop('disabled', true);
            $('#CriticalityId').siblings('.select2-container').css('border', '1px solid #ccc');

            //$('#ManifestationTypeId').val("0");
            //$('#ServiceId').val("0");
            //$('#Reason01ListItemId').val("0");
            //$('#Reason02ListItemId').val("0");
            $('#CriticalityId').val("0").change();

            if (reason02ListId !== 0) {
                $('#criticalityDivId').show();
            }
        },

        OnCriticalityChanged: function () {
            const selectedValue = $('#CriticalityId option:selected').text();

            $('#slaDivId').hide();
            $('#alarmDivId').hide();
            $('#btnSlaCreate').prop('disabled', true);
            $('#CriticalityId').siblings('.select2-container').css('border', '1px solid #ccc');

            if (selectedValue.toUpperCase() !== 'OPÇÕES') {
                switch (selectedValue.toUpperCase()) {
                    case "ALTA":
                        $('#CriticalityId').siblings('.select2-container').css('border', '3px solid red');
                        break;
                    case "MÉDIA":
                        $('#CriticalityId').siblings('.select2-container').css('border', '3px solid yellow');
                        break;
                    case "BAIXA":
                        $('#CriticalityId').siblings('.select2-container').css('border', '3px solid green');
                        break;
                }
                $('#btnSlaCreate').prop('disabled', false);
                $('#slaDivId').show();
                $('#alarmDivId').show();
            }
        }
    }
}