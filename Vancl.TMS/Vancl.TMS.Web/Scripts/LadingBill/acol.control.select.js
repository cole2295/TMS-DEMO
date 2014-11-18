
/*
  <!--Edit By Acol 2012/01/02 -->
  QQ:125752647
*/
$(function () {
    $(".select").each(function () {
        var w = $(this).width();
        $(this).find(".dropdown-menu ul li").width(w - 10);
        $(this).find(".select").width(w);
        $(this).find(".btn-success").width(w);
        $(this).find(".btn-success_c").width(w - 10 - 4 - 26);
    });

    $(".select .btn-success").click(function () {
        $(".select").css("position", "");
        var prev_item = $(".select .btn-success_r span[class='hover']").parent().parent().parent();

        if ($(prev_item).attr("container") != $(this).parent().attr("container")) {
            prev_item.find(".btn-success_r span").removeClass("hover");
            prev_item.find(".dropdown-menu").hide();
        }
        $(this).parent().css("position", "relative");

        if ($(this).find(".btn-success_r span").hasClass("hover")) {
            $(this).find(".btn-success_r span").removeClass("hover");
            $(this).parent().find(".dropdown-menu").slideUp(200, function () {
                $(this).hide();
            });
        } else {
            $(this).find(".btn-success_r span").addClass("hover");
            $(this).parent().find(".dropdown-menu").slideDown(200);
        }

    });
    $(".dropdown-menu li").click(function () {
        $(this).parent().parent().parent().find(".btn-success_r span").removeClass("hover");
        var value = $(this).attr("val");
        
        var container = $(this).parent().parent().parent().attr("container");
        $("#" + container).val(value);
     
        $(this).parent().parent().parent().find(".btn-success_c span").html($(this).text());

        $(this).parent().parent().slideUp(200);

    });
    $(".select .dropdown-menu li").hover(function () {
        $(this).addClass("hover");
    }, function () {
        $(this).removeClass("hover");
    });
});