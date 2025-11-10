const notificationBadge = $('#notificationBadge');
const notificationList = $('#notificationList');

const tokenMeta = document.querySelector('meta[name="jwt-token"]');
const token = tokenMeta ? tokenMeta.content : null;

const connection = new signalR.HubConnectionBuilder()
    //.withUrl("https://localhost:7289/hubs/notification?access_token=" + token)
    //.build();
    .withUrl("https://localhost:7289/hubs/notification", {
        accessTokenFactory: () => token
    })
    .withAutomaticReconnect()
    .build();

connection.on("ReceiveNotification", (notification) => {
    IncreaseNotificationBadgeCount();

    const createdAt = moment(notification.createdAt).format('HH:mm DD/MM/YYYY');
    const notificationHtml = `
        <a href="${notification.redirectUrl}" class="notification-item dropdown-item d-flex justify-content-center align-items-center" data-id="${notification.id}" data-isread="${notification.isRead}" onclick="UpdateIsRead(${notification.id})">
            <div class="dropdown-list-image mr-3">
                <img class="rounded-circle" style="width: 2.5rem; height: 2.5rem;" src="/${notification.imageUrl}" alt="IMG-NOTIFICATION">
            </div>
            <div class="py-1">
                <div class="${notification.isRead ? '' : 'font-weight-bold'}" style="text-wrap: wrap;">
                    ${notification.description}
                </div>
                <div class="text-secondary">
                    ${createdAt}
                </div>
            </div>
        </a>`;
    notificationList.append(notificationHtml);
});

connection.start()
    .catch(err => console.error(err));

function LoadNotifications() {
    $.ajax({
        url: '/Member/Notification/GetAll',
        type: 'GET',
        success: function (response) {
            if (response.statusCode === 200) {
                // Set notification badge count
                const badgeCount = response.data.badgeCount;
                LoadNotificationBadgeCount(badgeCount);

                const notifications = response.data.notifications;

                $('#notificationList a').remove();

                notifications.forEach(function (item) {
                    const createdAt = moment(item.createdAt).format('HH:mm DD/MM/YYYY');
                    const notificationHtml = `
                        <a href="${item.redirectUrl}" class="notification-item dropdown-item d-flex justify-content-center align-items-center" data-id="${item.id}" data-isread="${item.isRead}" onclick="UpdateIsRead(${item.id})">
                            <div class="dropdown-list-image mr-3">
                                <img class="rounded-circle" style="width: 2.5rem; height: 2.5rem;" src="/${item.imageUrl}" alt="IMG-NOTIFICATION">
                            </div>
                            <div class="py-1">
                                <div class="${item.isRead ? '' : 'font-weight-bold'}" style="text-wrap: wrap;">
                                    ${item.description}
                                </div>
                                <div class="text-secondary">
                                    ${createdAt}
                                </div>
                            </div>
                        </a>`;
                    notificationList.append(notificationHtml);
                });
            } else if (response.statusCode === 404) {
                ShowToastNoti('warning', response.message);
            }
        },
        error: function (err) {
            //Handle other errors (e.g., server errors)
            ShowToastNoti('error', 'An error occurred, please try again.');
        }
    });
}
LoadNotifications();

function LoadNotificationBadgeCount(badgeCount) {
    if (badgeCount > 9) {
        notificationBadge.attr('data-notify', '9+');
    } else {
        notificationBadge.attr('data-notify', badgeCount);
    }
}

function IncreaseNotificationBadgeCount() {
    const badgeCount = notificationBadge.attr('data-notify') + 1;
    LoadNotificationBadgeCount(badgeCount);
}

function DecreaseNotificationBadgeCount() {
    const badgeCount = notificationBadge.attr('data-notify') - 1;
    LoadNotificationBadgeCount(badgeCount);
}

function UpdateIsRead(id) {
    const notificationHtml = $(`.notification-item[data-id="${id}"]`);
    const isRead = notificationHtml.data('isread');
    if (isRead) {

    } else {
        // Update notification UI as read
        DecreaseNotificationBadgeCount();
        notificationHtml.find('div.font-weight-bold').removeClass('font-weight-bold');
        notificationHtml.attr('data-isread', true);

        // Send AJAX request to update notification as read
        $.ajax({
            url: '/Member/Notification/UpdateIsRead',
            type: 'PUT',
            data: {
                notificationId: id
            },
            dataType: 'json',
            success: function (response) {
                if (response.statusCode === 200) {
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
}