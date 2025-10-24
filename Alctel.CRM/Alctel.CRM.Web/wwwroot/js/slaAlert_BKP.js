$(function () {
    slaAlertConfig.Init.select2ToTree();
});


var slaAlertConfig = {
    Init: {
        select2ToTree: function () {
            const selectsArray = ['ManifestationTypeId', 'ServiceId', 'ProgramId', 'Reason01Id', 'Reason02Id'];

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
                $('#serviceDivId').show();
            }
            else {
                //$('#programDivId').hide();
                $('#reason01DivId').hide();
                $('#reason02DivId').hide();
                $('#serviceDivId').hide();
                $('#btnTicketClassification').prop('disabled', true);
            }
        },

        OnServiceChanged: function () {
            var selectedValue = $('#ServiceId').val();

            if (selectedValue !== '0') {
                //$('#programDivId').show();
                $('#reason01DivId').show();
            }
            else {
                //$('#programDivId').hide();
                $('#reason01DivId').hide();
                $('#reason02DivId').hide();
                $('#btnTicketClassification').prop('disabled', true);
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
            var selectedValue = $('#Reason01Id').val();

            if (selectedValue !== '0') {
                $.get(`${origin_url}/TicketClassification/GetTicketClassificationReasonSonList/?id=${selectedValue}`, function (data, success) {

                    if (success === 'success') {
                        if (data !== null) {

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
    }
}