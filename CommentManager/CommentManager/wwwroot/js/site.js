// Display a message with a text prompt and lock a button(protection against spam keystrokes) for a certain time
function AlertMessage(data) {

    var divId = "#" + data.divId;
    var btnId = "#" + data.btnId;

    $(divId).css("display", "block");
    $(divId).html("<p>" + data.message + "</p>");

    if (btnId !== "#undefined") {
        $(btnId).attr("disabled", true);
    }

    setTimeout(function () {
        $(divId).css("display", "none");
        if (btnId !== "#undefined") {
            $(btnId).removeAttr("disabled");
        }
    }, data.time);
}

