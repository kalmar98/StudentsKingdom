$(document).ready(function () {

    setData();

    $("#item").popover({
        html: true,
        placement: "auto",
        animation: true

    });


    function setData() {
        let item = $("#item").attr("data-id");
        $.ajax({
            url: "/Game/Dorm/ItemInfo?data=" + item,
            method: "POST",
            data: { item: item },
        }).done(function (result) {
            $("#item").attr("data-content", result);
        });
    }
   
})


