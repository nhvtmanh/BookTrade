$(document).ready(function () {
    LoadData();
    HandleSelectAllCheckbox();

    // Disable Place Order button
    $('#btnPlaceOrder').addClass('disabled-btn');
});

function LoadData() {
    $.ajax({
        url: '/Member/Cart/GetCart',
        type: 'GET',
        success: function (response) {
            if (response.statusCode === 200) {
                const cartItems = response.data.cartItems;
                $('#cartTable tr:not(.table_head)').remove();
                let total = 0;
                // Loop through cart items and append rows
                cartItems.forEach(function (item) {
                    const book = item.book;
                    const quantity = item.quantity;
                    const price = book.discountPrice;
                    const subTotal = price * quantity;
                    total += subTotal;
                    const rowHtml = `
                        <tr data-bookid="${book.id}" data-price="${price}">
                            <td class="px-2">
                                <div class="form-check">
								    <input class="form-check-input input-checkbox input-checkbox-item" type="checkbox" />
							    </div>
                            </td>
                            <td class="py-4">
                                <div class="d-flex align-items-center">
                                    <img src="/${book.imageUrl}" alt="${book.title}" style="width:80px; height:auto;">
                                    <span class="stext-110 cl2 px-2">${book.title}</span>
                                </div>
                            </td>
                            <td class="text-center">${price.toLocaleString()} VND</td>
                            <td>
                                <div class="wrap-num-product flex-w m-l-auto m-r-auto">
                                    <div class="btn-num-product-down cl8 hov-btn3 trans-04 flex-c-m"
                                         onclick="SetQuantityDown(${book.id})">
                                        <i class="fs-16 zmdi zmdi-minus"></i>
                                    </div>
                                    <input class="mtext-104 cl3 txt-center num-product" 
                                           type="number" 
                                           value="${quantity}" 
                                           min="1" />
                                    <div class="btn-num-product-up cl8 hov-btn3 trans-04 flex-c-m"
                                         onclick="SetQuantityUp(${book.id})">
                                        <i class="fs-16 zmdi zmdi-plus"></i>
                                    </div>
                                </div>
                            </td>
                            <td class="text-center">
                                <span class="stext-110 cl2 px-2 sub-total"
                                      style="color: #e74a3b">
                                      ${subTotal.toLocaleString()} VND
                                </span>
                            </td>
                        </tr>`;
                    $('#cartTable').append(rowHtml);
                });

                HandleInputCheckboxChange();
                HandleInputQuantityChange();
            }
        },
        error: function (err) {
            //Handle other errors (e.g., server errors)
            ShowToastNoti('error', 'An error occurred, please try again.');
        }
    });
}

function HandleInputCheckboxChange() {
    $('.input-checkbox-item').on('change', function () {
        HandleDisplayButtonDelete();
        HandleDisableButtonPlaceOrder();

        if ($(this).is(':checked')) {
            // If all checkboxes are checked, check the "Select All" checkbox
            const totalCartItems = $('#cartTable tr[data-bookid]').length;
            const checkedItems = $('#cartTable .input-checkbox-item:checked').length;
            if (checkedItems === totalCartItems) {
                $('#inputCheckAll').prop('checked', true);
            }

            UpdateTotal();
        } else {
            // If any checkbox is unchecked, uncheck the "Select All" checkbox
            if ($('#inputCheckAll').is(':checked')) {
                $('#inputCheckAll').prop('checked', false);
            }

            UpdateTotal();
        }
    });
}

function UpdateTotal() {
    let total = 0;

    const checkedItems = $('#cartTable .input-checkbox-item:checked');
    checkedItems.each(function () {
        const row = $(this).closest('tr');
        const price = parseFloat(row.data('price'));
        const quantity = parseInt(row.find('input.num-product').val());
        const subTotal = price * quantity;
        total += subTotal;
    });

    $('#cartTotal').text(`${total.toLocaleString()} VND`);
}

function HandleDisableButtonPlaceOrder() {
    const checkedItems = $('#cartTable .input-checkbox-item:checked').length;
    if (checkedItems > 0) {
        // Enable button
        $('#btnPlaceOrder').removeClass('disabled-btn');
        $('#btnPlaceOrder').prop('disabled', false);
    } else {
        // Disable button
        $('#btnPlaceOrder').addClass('disabled-btn');
        $('#btnPlaceOrder').prop('disabled', true);
    }
}

function PlaceOrder() {
    const address = $('#txtAddress').val().trim();
    if (address === '') {
        ShowToastNoti('warning', 'Please enter your shipping address');
        return;
    }

    const bookIds = [];
    const checkedItems = $('#cartTable .input-checkbox-item:checked');
    checkedItems.each(function () {
        const row = $(this).closest('tr');
        const bookId = parseInt(row.data('bookid'));
        bookIds.push(bookId);
    });
    if (bookIds.length === 0) {
        ShowToastNoti('warning', 'Please select at least one book to place an order');
        return;
    }

    // Check stock quantity
    $.ajax({
        url: '/Member/Order/CheckStockQuantity',
        type: 'POST',
        data: {
            bookIds: bookIds
        },
        dataType: 'json',
        success: function (response) {
            if (response.statusCode === 200) {
                if (response.data == true) {
                    const paymentMethod = $('#sltPayment').val();
                    if (paymentMethod === 'Online') {
                        $.ajax({
                            url: '/Member/Payment/CreatePaymentUrl',
                            type: 'POST',
                            data: {
                                bookIds: bookIds,
                                description: JSON.stringify({
                                    bookIds: bookIds,
                                    shippingAddress: address
                                })
                            },
                            dataType: 'json',
                            success: function (response) {
                                if (response.statusCode === 200) {
                                    const paymentUrl = response.data;
                                    setTimeout(function () {
                                        window.location.href = paymentUrl;
                                    }, 1000);
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
                    else if (paymentMethod === 'COD') {

                    }
                }
                else {
                    ShowToastNoti('warning', response.message);
                }
            }
            else if (response.statusCode === 404) {
                ShowToastNoti('warning', response.message);
            }
        },
        error: function (err) {
            //Handle other errors (e.g., server errors)
            ShowToastNoti('error', 'An error occurred, please try again.');
        }
    });
}

function SetQuantityDown(bookId) {
    // Update quantity of the book in the cart
    const row = $(`tr[data-bookid="${bookId}"]`);
    const input = row.find('input.num-product');
    let quantity = parseInt(input.val());
    if (quantity > 1) {
        quantity--;
        input.val(quantity);
    }

    // Update subtotal
    const spanSubTotal = row.find('span.sub-total');
    const price = parseFloat(row.data('price'));
    const subTotal = price * quantity;
    spanSubTotal.text(`${subTotal.toLocaleString()} VND`);

    UpdateTotal();
}

function SetQuantityUp(bookId) {
    // Update quantity of the book in the cart
    const row = $(`tr[data-bookid="${bookId}"]`);
    const input = row.find('input.num-product');
    let quantity = parseInt(input.val());
    quantity++;
    input.val(quantity);

    // Update subtotal
    const spanSubTotal = row.find('span.sub-total');
    const price = parseFloat(row.data('price'));
    const subTotal = price * quantity;
    spanSubTotal.text(`${subTotal.toLocaleString()} VND`);

    UpdateTotal();
}

function HandleInputQuantityChange() {
    $('input.num-product').on('input', function () {
        const row = $(this).closest('tr');
        const price = parseFloat(row.data('price'));
        const quantity = parseInt($(this).val());

        if (quantity >= 1) {
            const subTotal = price * quantity;
            const spanSubTotal = row.find('span.sub-total');
            spanSubTotal.text(`${subTotal.toLocaleString()} VND`);
            UpdateTotal();
        } else {
            $(this).val(1);
            const subTotal = price;
            const spanSubTotal = row.find('span.sub-total');
            spanSubTotal.text(`${subTotal.toLocaleString()} VND`);
            UpdateTotal();
        }
    });
}

function HandleSelectAllCheckbox() {
    $('#inputCheckAll').on('change', function () {
        if ($(this).is(':checked')) {
            $('.input-checkbox-item').prop('checked', true);

            if ($('#cartTable tr[data-bookid]').length > 0) {
                $('#btnDelete').fadeIn();
            }
        } else {
            $('.input-checkbox-item').prop('checked', false);
            $('#btnDelete').fadeOut();
        }

        UpdateTotal();
        HandleDisableButtonPlaceOrder();
    });
}

function HandleDisplayButtonDelete() {
    const checkedItems = $('#cartTable .input-checkbox-item:checked').length;
    if (checkedItems > 0) {
        $('#btnDelete').fadeIn();
    } else {
        $('#btnDelete').fadeOut();
    }
}

function ShowDeleteCartItemsAlert() {
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: "btn btn-danger px-3 mx-1",
            cancelButton: "btn btn-secondary px-3 mx-1"
        },
        buttonsStyling: false
    });
    swalWithBootstrapButtons.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: "Yes",
        cancelButtonText: "No",
        showLoaderOnConfirm: true,
        //preConfirm: function () {
        //    return new Promise(function (resolve, reject) {
        //    });
        //}
    }).then((result) => {
        if (result.isConfirmed) {
            DeleteCartItems();
        }
    });
}

function DeleteCartItems() {
    $('.input-checkbox-item:checked').each(function () {
        $(this).closest('tr').remove();
    })

    $('#btnDelete').fadeOut();

    if ($('#inputCheckAll').is(':checked')) {
        $('#inputCheckAll').prop('checked', false);
    }

    UpdateTotal();
}