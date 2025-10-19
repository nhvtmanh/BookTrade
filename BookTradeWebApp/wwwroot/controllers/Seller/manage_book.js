function ShowAddModal() {
    $("#bookModalContent").load('/Seller/P_Add', function () {
        $("#bookModal").modal('show');
    });
}

function Submit() {
    let form = $("#bookForm");

    if (!form.valid()) {
        return;
    }

    let formData = new FormData(form[0]);
    formData.append("__RequestVerificationToken", $('input[name="__RequestVerificationToken"]').val());

    $.ajax({
        url: "/Seller/Create",
        type: "POST",
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            if (response.statusCode === 201) {
                $("#bookModal").modal('hide');
                ShowToastNoti('success', response.message);
                //dataTable.ajax.reload();
            }
            else if (response.statusCode === 400) {
                ShowToastNoti('warning', response.message);
            }
        },
        error: function (err) {
            //Handle other errors (e.g., server errors)
            ShowToastNoti('error', 'An error occurred, please try again.');
        }
    });
}