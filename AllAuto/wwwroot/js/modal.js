function openModal(parameters) {
    const id = parameters.data;
    const url = parameters.url;
    const modal = $('#modal');
    
    if (id === undefined || url === undefined) {
        alert('Упссс.... что-то пошло не так')
        return;
    }
    
    $.ajax({
        type: 'GET',
        url : url,
        data: { "id": id },
        success: function (response) {
            modal.find('.modal-body').html(response);
            modal.modal('show')
        },
        failure: function () {
            modal.modal('hide')
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
};

function openModalLogin(parameters) {
    const url = parameters.url;
    const modal = $('#modalLogin');

    $.ajax({
        type: 'GET',
        url: url,
        success: function (response) {
            modal.find('.modal-body').html(response);
            modal.modal('show')
        },
        error: function (response) {
            Swal.fire({
                title: 'Информация',
                text: response.responseJSON.errorMessage,
                icon: 'error',
                confirmButtonText: 'Окей'
            })
        }
    });
};