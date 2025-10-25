$(function () {
    slaAlertConfig.Init.select2ToTree();
});


var slaAlertConfig = {
    Init: {
        select2ToTree: function () {
            const selectsArray = ['ManifestationTypeId', 'ServiceId', 'ProgramId', 'Reason01Id', 'Reason02Id', 'CriticalityId'];

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

            console.log(selectedValue)

            if (selectedValue !== '0') {
                //$('#programDivId').show();

                $.get(`${origin_url}/TicketClassification/GetTicketClassificationServiceByManifestation/?id=${selectedValue}`, function (data, success) {
                    console.log(data)
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

                            $('#serviceDivId').show();
                        }
                    }
                });
            }
            else {
                //$('#programDivId').hide();
                $('#reason01DivId').hide();
                $('#reason02DivId').hide();
                $('#serviceDivId').hide();
                $('#criticalityDivId').hide();
                $('#btnSlaCreate').prop('disabled', true);
            }
        },

        OnServiceChanged: function () {
            var serviceId = $('#ServiceId').val();
            const manifestationid = $('#ManifestationTypeId').val();

            if (serviceId !== '0') {
                //$('#programDivId').show();
                $('#reason01DivId').show();
                $('#criticalityDivId').show();

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
            else {
                //$('#programDivId').hide();
                $('#reason01DivId').hide();
                $('#reason02DivId').hide();
                $('#criticalityDivId').hide();
                $('#btnSlaCreate').prop('disabled', true);
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

            var selectedValue = $('#Reason01Id').val();

            if (selectedValue !== '0') {
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
            else {
                $('#reason02DivId').hide();
            }
        },

        OnReason02Changed: function () {
            var selectedValue = $('#Reason02Id').val();

            //if (selectedValue !== '0') {
            //    $('#serviceDivId').show();
            //}
            //else {
            //    $('#serviceDivId').hide();
            //}
        },

        OnCriticalityChanged: function () {
            const selectedValue = $('#CriticalityId option:selected').text();

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
        }
    }
}