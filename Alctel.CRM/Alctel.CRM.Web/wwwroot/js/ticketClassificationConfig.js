$(function () {
    ticketClassificationConfig.Init.select2ToTree();
});


var ticketClassificationConfig = {
    Init: {
        select2ToTree: function () {
            const selectsArray = ['ManifestationTypeId', 'ServiceUnitId', 'ProgramId', 'Reason01Id', 'Reason02Id'];

            Array.from(selectsArray).forEach(item => {

                $(`#${item}`).select2ToTree({
                    placeholder: "Opções...",
                });

            });

        },
    },

    Events: {

        OnManifestationTypeIndexChanged: function () {
            var selectedValue = $('#ManifestationTypeId').val();
           
            $.get(`${origin_url}/TicketClassification/TicketClassificationByManifestationIndex/?id=${selectedValue}`, function (data, success) {
                if (success === 'success') {
                    if (data != null) {
                        console.log(data);

                        $('#classificationListId').html(data);
                    }
                }
            });
        },

        OnManifestationTypeChanged: function () {
            var selectedValue = $('#ManifestationTypeId').val();

            $('#ProgramId').val("0");
            $('#ProgramName').val("");
            $('#ServiceName').val("");
            $('#Reason01Id').val("0");
            $('#Reason01Name').val("");
            $('#Reason02Id').val("0");
            $('#Reason02Name').val("");

            if (selectedValue !== '0') {
                const manifestationName = $('#ManifestationTypeId option:selected').text();
                console.log(selectedValue, manifestationName)
                $('#ManifestationTypeName').val(manifestationName);

                $('#programDivId').show();
            }
            else {
                $('#programDivId').hide();
                $('#reason01DivId').hide();
                $('#reason02DivId').hide();
                $('#serviceDivId').hide();
                $('#btnTicketClassification').prop('disabled', true); 
            }
        },

        OnProgramChanged: function () {
            var selectedValue = $('#ProgramId').val();

            $('#ServiceName').val("");
            $('#Reason01Id').val("0");
            $('#Reason01Name').val("");
            $('#Reason02Id').val("0");
            $('#Reason02Name').val("");

            if (selectedValue !== '0') {
                const programName = $('#ProgramId option:selected').text();
                $('#ProgramName').val(programName);

                $('#reason01DivId').show();
                $('#serviceDivId').show();
                $('#btnTicketClassification').prop('disabled', false); 
            }
            else {
                $('#reason01DivId').hide();
                $('#reason02DivId').hide();
                $('#serviceDivId').hide();
                $('#btnTicketClassification').prop('disabled', true); 
            }
        },

        OnReason01Changed: function () {
            var reasonListId = $('#Reason01Id').val();

            $('#Reason02Id').val("0");
            $('#Reason02Name').val("Opções");
            
            if (reasonListId !== '0') {
                const reason01Name = $('#Reason01Id option:selected').text();
                $('#Reason01Name').val(reason01Name);

                $('#Reason02ListIdParent').val(reasonListId);

                $('#reason02DivId').show();
                //$.get(`${origin_url}/TicketClassification/GetTicketClassificationReasonSonList/?id=${reasonListId}`, function (data, success) {

                //    if (success === 'success') {
                //        if (data !== null) {

                //            $('#Reason02Id').append(
                //                $('<option>', {
                //                    value: '',
                //                    text: 'Opções',
                //                })
                //            )

                //            if (data.length > 0) {
                //                $.each(data, function (index, value) {
                //                    $('#Reason02Id').append(
                //                        $('<option>', {
                //                            value: value.Id,
                //                            text: value.Name,
                //                        })
                //                    )
                //                })
                //            }

                //            $('#reason02DivId').show();
                //        }
                //    }
                //});


            }
            else {
                $('#reason02DivId').hide();
            }
        },

        OnReason02Changed: function () {
            var selectedValue = $('#Reason02Id').val();

            const reason02Name = $('#Reason02Id option:selected').text();
            $('#Reason02Name').val(reason02Name);


            //if (selectedValue !== '0') {
            //    $('#serviceDivId').show();
            //}
            //else {
            //    $('#serviceDivId').hide();
            //}
        },

        OnServiceCheckChanged: function (serviceid, id) {
            console.log('OnServiceCheckChanged', serviceid, id);

            const checked = $(`#${id}`).is(':checked');
            console.log(checked);

            $.get(`${origin_url}/TicketClassification/UpdateTicketClassification/?serviceId=${serviceid}&active=${checked}`, function (data, success) {

                if (success === 'success') {
                    if (data !== null) {

                        console.log(data);

                        if (data.isValid) {
                            $('#messageSuccess').show();
                            setTimeout(() => {
                                $('#messageSuccess').hide();
                            }, 5000);
                        }
                        else {
                            $('#messageError').text(data.value);
                            $('#messageError').show();
                            $(`#${id}`).prop("checked", false);
                            setTimeout(() => {
                                $('#messageError').hide();
                            }, 5000);
                        }
                    }
                }
            });
            
        },

    }
}