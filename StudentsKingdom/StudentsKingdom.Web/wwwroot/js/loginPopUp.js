$(document).ready(function () {

    var modalName = '#loginModal';

    $('.play-btn').click(function () {
        let url = $(modalName).data('url');
        $.get(url, function (data) {
            $(modalName).html(data);
            $(modalName).modal('show');
        });
    });
});