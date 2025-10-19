
$(function () {
    
});


var select2treeTicketAssignmentHelper = {
    Init: {
        agentAssistantSelect: function () {            
            $("#agentAssistantOptionsId").select2ToTree({
                // treeData: {
                //     dataArr:mydata
                // },
                // maximumSelectionLength: 3,
                placeholder: "Opções...",
            });
        },
    }
}

var associateTicketToMe = {
    Methods: {
        LoadScreen: function (ticketid, usernameorigin) {
            console.log('usernameorigin', usernameorigin)

            $('#associateTicketOpenId').empty();
            $.get(`${origin_url}/TicketAssignment/TicketManagement/?id=${ticketid}`, function (data, success) {
                if (success === 'success') {
                    $('#associateTicketOpenId').html(data);

                    $.get(`${origin_url}/User/GetAgentsAssistantList`, function (data, success) {
                        if (success === 'success') {
                            //select2treeTicketAssignmentHelper.Init.agentAssistantSelect();

                            let agentAssistantSelect = $('#agentAssistantOptionsId');
                            agentAssistantSelect.empty();

                            agentAssistantSelect.append($('<option>', {
                                value: 0,
                                text: 'Opções'
                            }));


                            $.each(data, function (index, value) {
                                if (value.username !== '') {
                                    
                                    agentAssistantSelect.append($('<option>', {
                                        value: value.userId,
                                        text: value.username
                                    }));

                                }
                            });
                        }                        
                    });

                    $.get(`${origin_url}/TicketAssignment/GetQueueGTList`, function (data, success) {
                        if (success === 'success') {
                            //select2treeTicketAssignmentHelper.Init.agentAssistantSelect();

                            let queueGTSelect = $('#queueGTOptionsId');
                            queueGTSelect.empty();

                            queueGTSelect.append($('<option>', {
                                value: 0,
                                text: 'Opções'
                            }));

                            $.each(data, function (index, value) {
                                if (value.name !== '') {
                                    queueGTSelect.append($('<option>', {
                                        value: value.id,
                                        text: value.name
                                    }));

                                }
                            });
                            $('#usernameOrigin').val(usernameorigin);
                        }
                    });
                }
            });
        },

        TicketAssignment: function () {

            const userSelected = $('#agentAssistantOptionsId').val();
            const queueSelected = $('#queueGTOptionsId').val();
            const assignmentTypeId = $('#assignmentTypeId').val();
            
            const ticketId = $('#ticketId').val();
            const userOriginId = $('#userOriginId').val();
            const usernameOrigin = $('#usernameOrigin').val();
            const queueGTOrigin = $('#QueueGT').val();

            console.log(`userSelected: ${userSelected}`, `queueSelected: ${queueSelected}`, `ticketId: ${ticketId}`, `userOriginId: ${userOriginId}`, `usernameOrigin: ${usernameOrigin}`, `queueGTOrigin: ${queueGTOrigin}`, `assignmentTypeId: ${assignmentTypeId}`);

            $.post(`${origin_url}/TicketAssignment/ConfirmAssignment`, {
                UserOriginId: userOriginId,
                UsernameOrigin: usernameOrigin,
                UserDestId: userSelected,
                QueueGTId: queueSelected,
                QueueGTOrigin: queueGTOrigin,
                TicketId: ticketId,
                AssignmentTypeId: assignmentTypeId
            },
                function (data, success) {
                    if (success === 'success') {
                        console.log(data);
                        console.log(data);

                        if (data.isAssigment) {
                            $('#ticketAssignmentSuccessMessage').show();
                            $('#buttonDivId').hide();
                        }
                        else {
                            $('#ticketAssignmentErrorMessage').show();                            
                        }

                        setTimeout(() => {
                            $(`#btnAssociateTicketModelClose`).trigger('click');
                            location.reload(true);

                        }, 3000);

                    }
                });
        },        
    },

    Events: {
        OnAssignmentType: function () {
            var selectedValue = $('#assignmentTypeId').val(); 

            switch (selectedValue) {
                case 'Queue':
                    $('#queueDivId').show();
                    $('#userDivId').hide();
                    $('#buttonDivId').hide();
                    $('#agentAssistantOptionsId').val('0');
                    $('#queueGTOptionsId').val('0');
                    break;
                case 'User':
                    $('#queueDivId').hide();
                    $('#userDivId').show();
                    $('#buttonDivId').hide();
                    $('#agentAssistantOptionsId').val('0');
                    $('#queueGTOptionsId').val('0');
                    break;
                default:
                    $('#queueDivId').hide();
                    $('#userDivId').hide();
                    $('#buttonDivId').hide();
                    $('#agentAssistantOptionsId').val('0');
                    $('#queueGTOptionsId').val('0');
            }
        },
        OnUserChanged: function () {
            var selectedValue = $('#agentAssistantOptionsId').val();
            
            if (selectedValue !== '0') {
                $('#buttonDivId').show();
            }
            else {
                $('#buttonDivId').hide();
            }
        },
        OnQueueChanged: function () {
            var selectedValue = $('#queueGTOptionsId').val(); 

            if (selectedValue !== '0') {
                $('#buttonDivId').show();
            }
            else {
                $('#buttonDivId').hide();
            }
        }
    }
}