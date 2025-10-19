$(function () {
    
});


var ticketClassification = {
    Events: {
        OnAddClassificationClicked: function () {
            $.get(`${origin_url}/TicketClassification/ScreenPopup`, function (data, success) {
                
                if (success === 'success') {
                    $('#ticketClassificationOpenId').html(data);
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
            $('#reason01DivId').hide();
            $('#reason02DivId').hide();

            if (selectedValue != "0") {
                $('#serviceUnitDivId').show();
            }
        },

        OnServiceUnitChanged: function () {
            var selectedValue = $('#ServiceUnitId').val();

            $('#ServiceId').val("0");
            $('#Reason01Id').val("0");
            $('#Reason02Id').val("0");

            $('#serviceDivId').hide();
            $('#reason01DivId').hide();
            $('#reason02DivId').hide();

            if (selectedValue != "0") {
                $('#serviceDivId').show();
            }
        },

        OnServiceChanged: function () {
            var selectedValue = $('#ServiceId').val();

            $('#Reason01Id').val("0");
            $('#Reason02Id').val("0");

            $('#reason01DivId').hide();
            $('#reason02DivId').hide();

            if (selectedValue != "0") {
                $('#reason01DivId').show();
            }
        },

        OnReason01Changed: function () {
            var selectedValue = $('#Reason01Id').val();

            $('#Reason02Id').val("0");

            $('#reason02DivId').hide();

            if (selectedValue != "0") {
                $('#reason02DivId').show();
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
            const manifestationTypeName = $('#ManifestationTypeId option:selected').text();

            const serviceUnitId = $('#ServiceUnitId').val();
            const serviceUnitName = $('#ServiceUnitId option:selected').text();

            const serviceId = $('#ServiceId').val();
            const serviceName = $('#ServiceId option:selected').text();

            const reason01Id = $('#Reason01Id').val();
            const reason01Name = $('#Reason01Id option:selected').text();

            const reason02Id = $('#Reason02Id').val();
            const reason02Name = $('#Reason02Id option:selected').text();

            console.log(manifestationTypeId, manifestationTypeName)
            if (manifestationTypeName === 'Opções') {
                $('#btnTicketClassificationClose').trigger('click');
                return;
            }

            serviceUnitName = serviceUnitName === "Opções" ? '' : serviceUnitName;
            serviceName = serviceName === "Opções" ? '' : serviceName;
            reason01Name = reason01Name === "Opções" ? '' : reason01Name;
            reason02Name = reason02Name === "Opções" ? '' : reason02Name;

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
                </td>
                <td>
                    <input type="hidden" name="TicketClassification[${ticketClassificationCounter}].Reason02Id" value="${reason02Id}" />                     
                    <input type="hidden" name="TicketClassification[${ticketClassificationCounter}].Reason02Name" value="${reason02Name}" /> ${reason02Name}
                </td>
            </tr>`

            classificationTable.append(row);

            $('#btnTicketClassificationClose').trigger('click');
        }
    }
}