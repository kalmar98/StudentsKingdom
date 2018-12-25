$(document).ready(function () {
    alert("Invalid Username or Password");
    let modalName = '#loginModal';
    let url = $(modalName).data('url');

    $.get(url, function (data) {
        $(modalName).html(data);
        $(modalName).modal('show');

    });

});