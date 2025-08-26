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