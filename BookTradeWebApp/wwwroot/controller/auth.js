function Login() {
    let form = $("#loginForm");

    if (!form.valid()) {
        return;
    }

    let formData = new FormData(form[0]);
    formData.append("__RequestVerificationToken", $('input[name="__RequestVerificationToken"]').val());

    $.ajax({
        url: "/Auth/Login",
        type: "POST",
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            if (response.statusCode === 200) {
                const role = response.data;
                if (role === 'Admin') {
                    // Redirect to admin dashboard
                }
                else if (role === 'Seller') {
                    // Redirect to seller dashboard
                }
                else if (role === 'Member') {
                    ShowToastNoti('success', response.message);

                    setTimeout(function () {
                        window.location.href = '/book-exchange';
                    }, 1000);
                }
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