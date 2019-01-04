$("#stats").mouseenter(function () {

    let id = $(this).attr("data-id");
    $.ajax({
        url: "/Game/Dorm/StatsInfo?id=" + id,
        method: "POST",
        data: { id: id },
        

    }).done(function (result) {
        $('#statsInfo').html(result);
    });

});