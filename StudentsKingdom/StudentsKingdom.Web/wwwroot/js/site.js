$(document).ajaxSend(function (e, xhr, options) {
    if (options.type.toUpperCase() != "GET") {
        xhr.setRequestHeader("Content-type", "application/json");
        xhr.setRequestHeader("X-CSRF-TOKEN", $("[name='CSRF-TOKEN-MOONGLADE-FORM']").val());
    }
});

//тази тъпотия ми взе нервите :)

//XSRF Security for ajax posts