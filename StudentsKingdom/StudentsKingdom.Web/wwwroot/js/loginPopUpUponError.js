$(document).ready(function () {
    let modalName = '#loginModal';
    let url = $(modalName).data('url');

    $.get(url, function (data) {
        $(modalName).html(data);
        $(modalName).modal('show');

    });

});