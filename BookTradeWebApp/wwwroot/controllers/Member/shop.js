let selectedBookId = null;

$(document).ready(function () {
    LoadData();
});

function LoadData() {
    $.ajax({
        url: '/Member/Shop/GetAll',
        type: 'GET',
        success: function (response) {
            if (response.statusCode === 200) {
                // Clear existing items
                const $grid = $('.isotope-grid');
                $grid.isotope('remove', $grid.children()).isotope('layout');

                const books = response.data;
                books.forEach(function (book) {
                    const formatPrice = new Intl.NumberFormat('vi-VN').format(book.discountPrice) + 'đ';
                    const $bookItem = $(`<div class="col-sm-6 col-md-4 col-lg-3 p-b-35 isotope-item">
                                        <!-- Block2 -->
                                        <div class="block2">
                                            <div class="block2-pic hov-img0">
                                                <img src="/${book.imageUrl}" alt="IMG-BOOK">

                                                <a onclick="DisplayBookDetails(this)" class="block2-btn flex-c-m stext-103 cl2 size-102 bg0 bor2 hov-btn1 p-lr-15 trans-04 js-show-modal1"
                                                data-title="${book.title}"
                                                data-author="${book.author}"
                                                data-shopname="${book.shop.name}"
                                                data-description="${book.description}"
                                                data-image="${book.imageUrl}"
                                                data-price="${formatPrice}"
                                                data-id="${book.id}">
                                                    Quick View
                                                </a>
                                            </div>

                                            <div class="block2-txt flex-w flex-t p-t-14">
                                                <div class="block2-txt-child1 flex-col-l ">
                                                    <a href="#" class="stext-104 cl4 hov-cl1 trans-04 js-name-b2 p-b-6">
                                                        ${book.title}
                                                    </a>

                                                    <span class="stext-105 cl3">
                                                        ${formatPrice}
                                                    </span>
                                                </div>

                                                <div class="block2-txt-child2 flex-r p-t-3">
                                                    <a href="#" class="btn-addwish-b2 dis-block pos-relative js-addwish-b2">
                                                        <img class="icon-heart1 dis-block trans-04" src="/images/icons/icon-heart-01.png" alt="ICON">
                                                        <img class="icon-heart2 dis-block trans-04 ab-t-l" src="/images/icons/icon-heart-02.png" alt="ICON">
                                                    </a>
                                                </div>
                                            </div>
                                        </div>
                                    </div>`);
                    $grid.append($bookItem).isotope('appended', $bookItem);
                });
                setTimeout(function () {
                    // Refresh layout after adding
                    $grid.isotope('layout');
                }, 500);
            }
        },
        error: function (err) {
            //Handle other errors (e.g., server errors)
            ShowToastNoti('error', 'An error occurred, please try again.');
        }
    });
}

function DisplayBookDetails(button) {
    selectedBookId = $(button).data('id');
    const title = $(button).data('title');
    const author = $(button).data('author');
    const shopName = $(button).data('shopname');
    const description = $(button).data('description');
    const image = $(button).data('image');
    const price = $(button).data('price');

    $('#title').text(title);
    $('#author').text(author);
    $('#shopname').text(shopName);
    $('#price').text(price);
    $('#description').text(description);
    $('#quantity').val(1); // Reset quantity to 1

    // Set image
    $('.slick3').html(`
        <div class="item-slick3" data-thumb="${image}">
            <div class="wrap-pic-w pos-relative">
                <img src="/${image}" alt="IMG-BOOK">
                <a href="/${image}" class="flex-c-m size-108 how-pos1 bor0 fs-16 cl10 bg0 hov-btn3 trans-04">
                    <i class="fa fa-expand"></i>
                </a>
            </div>
        </div>
    `);

    $('.js-modal1').addClass('show-modal1');
}

function AddToCart() {
    const bookId = selectedBookId;
    const quantity = $('#quantity').val();

    $.ajax({
        url: '/Member/Shop/AddToCart',
        type: 'POST',
        data: {
            bookId: parseInt(bookId),
            quantity: parseInt(quantity)
        },
        dataType: 'json',
        success: function (response) {
            ShowToastNoti('success', response.message);

            $('.js-modal1').removeClass('show-modal1');
        },
        error: function (err) {
            //Handle other errors (e.g., server errors)
            ShowToastNoti('error', 'An error occurred, please try again.');
        }
    });
}