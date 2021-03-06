// Loading comments on the page by partial submission
function Comments(data) {
    if (data.secondLvl === true) {

        var divIdInsert = data.divIdInsert;

        $.ajax({
            url: "/Home/SingleCommentLvl2",
            type: "POST",
            data: {
                replyCommentId: data.replyCommentId
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
                $("." + divIdInsert + " ol").last().before("<ol class='children'><li><img src=\"../images/error.png\" style=\"width: 200px; height: 200px; margin-top: 10%;\"/></li></ol><br>");
            }
        });
    }
    else {
        $.ajax({
            url: "/Home/Comments",
            type: "POST",
            data: {
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
                $("#PartialViewId").html("<li><img src=\"../images/error.png\" style=\"width: 200px; height: 200px; margin-top: 10%;\"/></li><br>");
            }
        });
        $("#commentTextId").val("");
    }
}