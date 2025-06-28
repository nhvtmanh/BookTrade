$(document).ready(function () {
    LoadData();
});

function LoadData() {
    $.ajax({
        url: '/BookExchange/GetAllBookExchangePost',
        type: 'GET',
        success: function (response) {
            if (response.statusCode === 200) {
                // Clear existing items
                const $grid = $('.isotope-grid');
                $grid.isotope('remove', $grid.children()).isotope('layout');

                const posts = response.data;
                posts.forEach(function (post) {
                    const $postItem = $(`<div class="col-sm-6 col-md-4 col-lg-3 p-b-35 isotope-item">
                                            <!-- Block2 -->
                                            <div class="block2">
                                                <div class="block2-pic hov-img0">
                                                    <img src="${post.bookExchange.imageUrl}" alt="IMG-BOOK">

                                                    <a href="#" class="block2-btn flex-c-m stext-103 cl2 size-102 bg0 bor2 hov-btn1 p-lr-15 trans-04 js-show-modal1">
                                                        Quick View
                                                    </a>
                                                </div>

                                                <div class="block2-txt flex-w flex-t p-t-14">
                                                    <div class="block2-txt-child1 flex-col-l ">
                                                        <a href="#" class="stext-104 cl4 hov-cl1 trans-04 js-name-b2 p-b-6">
                                                            ${post.bookExchange.title}
                                                        </a>

                                                        <span class="stext-105 cl3">
                                                            ${post.bookExchange.author}
                                                        </span>
                                                    </div>

                                                    <div class="block2-txt-child2 flex-r p-t-3">
                                                        <a href="#" class="btn-addwish-b2 dis-block pos-relative js-addwish-b2">
                                                            <img class="icon-heart1 dis-block trans-04" src="images/icons/icon-heart-01.png" alt="ICON">
                                                            <img class="icon-heart2 dis-block trans-04 ab-t-l" src="images/icons/icon-heart-02.png" alt="ICON">
                                                        </a>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>`);
                    $grid.append($postItem).isotope('appended', $postItem);
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

function ShowPostModal() {
    $("#postModalContent").load('/BookExchange/P_Post', function () {
        $("#postModal").modal('show');
    });
}

function Submit() {
    let form = $("#postForm");

    if (!form.valid()) {
        return;
    }

    let formData = new FormData(form[0]);
    formData.append("__RequestVerificationToken", $('input[name="__RequestVerificationToken"]').val());

    $.ajax({
        url: "/BookExchange/Post",
        type: "POST",
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            if (response.statusCode === 201) {
                $("#postModal").modal('hide');
                ShowToastNoti('success', response.message);
                LoadData();
            }
            else if (response.statusCode === 400 || response.statusCode === 404) {
                ShowToastNoti('warning', response.message);
            }
        },
        error: function (err) {
            //Handle other errors (e.g., server errors)
            ShowToastNoti('error', 'An error occurred, please try again.');
        }
    });
}