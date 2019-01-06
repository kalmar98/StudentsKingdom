$("#remove").click(function () {
    let id = $("#item").find(":selected").attr("id");

    if (id > 0) {
        $.ajax({
            url: "/Game/Dorm/Remove?data=" + id,
            method: "POST",
            data: { id: id },
        }).done(function (result) {
            if (result != "") {
                let capacity = $("#capacity").text().split("/");
                capacity[0]--;
                let newCapacity = capacity[0] + "/" + capacity[1];
                $("#capacity").text(newCapacity);

                $("#item").find(":selected").remove();
            }
            else {
                alert("You are trying to remove invalid item!");
            }

        });
    }


    return false;
});