$(document).ready(function () {
    unequip();
    
});


function unequip() {
    $('[data-trigger="click"]').click(function() {

        let data = $(this).text();

        if ($(this).parent().hasClass("not-equipped-item")) {
            return;
        }

        $.ajax({
            url: "/Game/Dorm/Unequip?data=" + data,
            method: "POST",
            data: { data: data },
        }).done(function (result) {
            
            if (result != "") {
                $("#" + data).attr("class", "not-equipped-item");

                let capacity = $("#capacity").text().split("/");
                capacity[0]++;
                let newCapacity = capacity[0] + "/" + capacity[1];
                $("#capacity").text(newCapacity);

                let json = JSON.parse(result);

                $('.inventory').append(function () {
                    
                    let option = '<option data-toggle="popover" id="' + json.Id +  '" data-trigger="hover" data-content class="font-moria-citadel option-item center" data-id="" data-original-title title>' + json.Name + '</option>'

                    return option;

                })

                $("#" + json.Id).attr("data-id", result).trigger('customEvent');

                
            }
        });

        
    });
}




