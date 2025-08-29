async function confirmAction(message = "¿Seguro que deseas continuar?") {
    return Swal.fire({
        title: 'Confirmación',
        text: message,
        icon: 'question',
        showCancelButton: true,
        confirmButtonText: 'Yes',
        cancelButtonText: 'No',
        reverseButtons: true
    }).then((result) => {
        if (!result.isConfirmed) {
            showToast("info", "Process canceled by user");
        }
        return result.isConfirmed; // true si confirma, false si cancela
    });
}

//Transacción exitosa(se llama directo, no necesita Yes / No)
async function successAction(message = "Process completed successfully") {
    return Swal.fire({
        title: 'Successful',
        text: message,
        icon: 'success',
        confirmButtonText: 'OK',
        reverseButtons: true
    });
}

//Mensaje de error
async function errorAction(message = "An unexpected error occurred") {
    return Swal.fire({
        title: 'Error',
        text: message,
        icon: 'error',
        confirmButtonText: 'OK',
        reverseButtons: true
    });
}