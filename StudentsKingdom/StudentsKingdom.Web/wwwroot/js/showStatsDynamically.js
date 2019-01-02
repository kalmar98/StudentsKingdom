$("#stats").mouseenter(function () {
    var setData = "";
    var element = $(this);
    var id = element.attr("data-id");
    $.ajax({
        url: "/Game/Dorm/StatsInfo?id=" + id,
        method: "POST",
        data: { id: id },
        success: function (data) {
            setData = data;
        }

    }).done(function (result) {
        $('#statsInfo').html(result);
    });



});