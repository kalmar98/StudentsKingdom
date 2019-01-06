$(document).ready(function () {
    $('.inventory').trigger('customEvent');
});

$('.inventory').on('customEvent', function () {
    
    setData();

    $('[data-toggle="popover"]').popover({
        html: true,
        placement: "auto",
        animation: true

    });


    function setData() {
        $('[data-toggle="popover"]').each(function (i, e) {

            let item = e.getAttribute("data-id");
            
            $.ajax({
                url: "/Game/Dorm/ItemInfo?data=" + item,
                method: "POST",
                data: { item: item },
            }).done(function (result) {
                $(e).attr("data-content", result);
            });
            
        });
        
        
    }

})


