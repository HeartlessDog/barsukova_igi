$(document).ready(function () {
    $.ajax({
        url: "Home/TbodyInsert",
        cache: false,
        success: function (html) {
            $("tbody").append(html);
        },
        failure: function (response) {
            alert("2");
        },
        error: function (response) {
            alert("3");
        }
    });
    $.ajax({
        url: "Home/FormInsert",
        cache: false,
        success: function (html) {
            $("form").html(html);
        },
        failure: function (response) {
            alert("2");
        },
        error: function (response) {
            $("form").html("gdsgsdgds");
        }
    });
});