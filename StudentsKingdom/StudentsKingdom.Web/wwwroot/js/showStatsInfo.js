$("#stats").mouseenter(function () {
    
    let id = $(this).attr("data-id");
    //alert(id);
    $.ajax({
        url: "/Game/Dorm/StatsInfo?id=" + id,
        method: "POST",
        data: { id: id },
        

    }).done(function (result) {
        $('#statsInfo').html(result);
    });

});