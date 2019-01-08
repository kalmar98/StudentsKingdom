$(document).ready(function () {

    $("#pvpBtn").click(function () {

        let modalName = '#pvpModal';
        
        $.ajax({
            url: "/Game/Tavern/Pvp",
            method: "POST",
            //data: { data: data },

        }).done(function (result) {
            $(modalName).html(result);
            $(modalName).modal('show');
        });


        return false;
    });
});