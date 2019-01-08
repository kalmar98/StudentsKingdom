﻿$(document).ready(function () {

    $("#questBtn").click(function () {

        let modalName = '#questModal';
        var data = $("input[name='difficult']:checked").val();
        
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

