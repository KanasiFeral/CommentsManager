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

// Loading comments on the page by partial submission
function Comments(data) {
    if (data.ignoreMessage === false) {
        $("#" + data.divId).css("display", "block");
        $("#" + data.divId).html("<p>" + data.message + "</p>");

        $("#" + data.btnId).attr("disabled", true);

        setTimeout(function () {
            $("#" + data.divId).css("display", "none");
            $("#" + data.btnId).removeAttr("disabled");
        }, 3000);
    }
    else {
        if (data.secondLvl === true) {

            var divIdInsert = data.divIdInsert;

            $.ajax({
                url: "/Home/SingleCommentLvl2",
                type: "POST",
                data: {
                    table: data.table,
                    postId: data.postId,
                    replyCommentId: data.replyCommentId,
                    userId: data.userId,
                    adminId: data.adminId,
                    date: data.date,
                    time: data.time,
                    commentText: data.commentText
                },
                dataType: "html",
                success: function (data) {
                    // Insert block with new comment before form comment (second branch)
                    $("." + divIdInsert).css("display", "block");
                    $("." + divIdInsert + " ol").last().before(data);

                    // Insert event-click on the new blocks
                    var replyButtons = $(".replySecondLvl");

                    // Add "on click" event for all buttons in second lvl of comments
                    Array.from(replyButtons).forEach(function (element) {
                        element.addEventListener('click', function () {
                            $(".formSecondlvlComments textarea").val($(this).attr("name") + ", ");
                            $(".formSecondlvlComments textarea").focus();
                        });
                    });

                    // Show "reply" and "all comments" buttons (first branch)
                    $("a[name=" + divIdInsert + "]").eq(0).show();
                    $("a[name=" + divIdInsert + "]").eq(1).show();

                    // Close all forms of adding a comment and clear all input fields (first branch) and clear all input fields (second branch)
                    $(".formComments").css("display", "none");
                    $(".formComments textarea").val("");
                    $(".formSecondlvlComments textarea").val("");

                },
                error: function () {
                    $("." + divIdInsert + " ol").last().before("<ol class='children'><li><img src=\"../images/adminError.png\" style=\"width: 200px; height: 200px; margin-top: 10%;\"/></li></ol><br>");
                }
            });
        }
        else {
            $.ajax({
                url: "/Home/Comments",
                type: "POST",
                data: {
                    table: data.table,
                    id: data.id,
                    page: data.page
                },
                dataType: "html",
                beforeSend: function () {
                    $('#PartialViewId').html("<img src=\"../../gif/Loading.gif\"/>");
                },
                success: function (data) {
                    $("#PartialViewId").html(data);
                },
                error: function () {
                    $("#PartialViewId").html("<li><img src=\"../images/adminError.png\" style=\"width: 200px; height: 200px; margin-top: 10%;\"/></li><br>");
                }
            });
            $("#commentTextId").val("");
        }
    }
}