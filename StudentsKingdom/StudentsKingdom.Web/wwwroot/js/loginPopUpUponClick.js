$(document).ready(function () {

    let modalName = '#loginModal';

    $('#playBtn').click(function () {
        
        let url = $(modalName).data('url');

        $.get(url, function (data) {
            $(modalName).html(data);
            $(modalName).modal('show');
           
        });
        
    });
});