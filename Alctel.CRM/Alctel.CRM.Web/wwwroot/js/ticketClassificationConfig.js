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
        OnManifestationTypeChanged: function () {
            var selectedValue = $('#ManifestationTypeId').val();

            if (selectedValue !== '0') {
                $('#programDivId').show();
            }
            else {
                $('#programDivId').hide();
                $('#reason01DivId').hide();
                $('#reason02DivId').hide();
                $('#serviceDivId').hide();
                $('#btnTicketClassification').prop('disabled', false); 
            }
        },

        OnProgramChanged: function () {
            var selectedValue = $('#ProgramId').val();

            if (selectedValue !== '0') {
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
            var selectedValue = $('#Reason01Id').val();

            if (selectedValue !== '0') {
                $('#reason02DivId').show();
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