$(document).ready(function () {

    $("#questBtn").click(function () {

        let modalName = '#questModal';
        
        //$.get(url, function (data) {
        //    $(modalName).html(data);
        //    $(modalName).modal('show');

        //});

        var data = $("input[name='difficult']:checked").val();
        alert(data);
        $.ajax({
            url: "/Game/University/Quest?id=" + data,
            method: "POST",
            data: { data: data },


        }).done(function (result) {
            $(modalName).html(result);
            $(modalName).modal('show');
        });


        return false;
    });
});

