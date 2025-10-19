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
                    ShowToastNoti('success', response.message);

                    setTimeout(function () {
                        window.location.href = '/admin/dashboard';
                    }, 1000);
                }
                else if (role === 'Seller') {
                    ShowToastNoti('success', response.message);

                    setTimeout(function () {
                        window.location.href = '/seller/dashboard';
                    }, 1000);
                }
                else if (role === 'Member') {
                    ShowToastNoti('success', response.message);

                    setTimeout(function () {
                        window.location.href = 'member/shop';
                    }, 1000);
                }
            }
            else if (response.statusCode === 400 || response.statusCode === 403) {
                ShowToastNoti('warning', response.message);
            }
        },
        error: function (err) {
            //Handle other errors (e.g., server errors)
            ShowToastNoti('error', 'An error occurred, please try again.');
        }
    });
}

function RegisterSeller() {
    let form = $("#registerSellerForm");

    if (!form.valid()) {
        return;
    }

    let formData = new FormData(form[0]);
    formData.append("__RequestVerificationToken", $('input[name="__RequestVerificationToken"]').val());

    $.ajax({
        url: "/Auth/RegisterSeller",
        type: "POST",
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            if (response.statusCode === 201) {
                ShowToastNoti('success', response.message);

                setTimeout(function () {
                    window.location.href = '/';
                }, 1000);
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