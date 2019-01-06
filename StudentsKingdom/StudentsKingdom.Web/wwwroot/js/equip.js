$("#equip").click(function () {

    let id = $("#item").find(":selected").attr("id");

    if (id > 0) {
        $.ajax({
            url: "/Game/Dorm/Equip?data=" + id,
            method: "POST",
            data: { id: id },
        }).done(function (result) {
            if (result != "") {
                if (result == "Consumable") {
                    alert("Yummy");
                }
                $("#" + result).attr("class", "equipped-item");

                let capacity = $("#capacity").text().split("/");
                capacity[0]--;
                let newCapacity = capacity[0] + "/" + capacity[1];
                $("#capacity").text(newCapacity);

                $("#item").find(":selected").remove();

                
            }
            else {
                alert("You already equipped this type of item!");
            }
            
        });
    }
   

    return false;
});