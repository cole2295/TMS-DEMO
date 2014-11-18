var data_ajax_SearchData = "data_ajax_SearchData";

function AjaxError(xhr, status, error) {
    alert("异步调用出错，错误信息:\n" + status + "\n" + error)
}
var TabRandomNum = "";
function CloseCurrentTab() {
    TabRandomNum = parseInt(Math.random() * 100000000, 10);
    if (top.CloseTab) {
        top.CloseTab(TabRandomNum);
    }
}

$(function () {
    $.ajaxSetup({
        cache: false
    });

    $(".CbxColumn .CheckAll").live("change", function () {
        var checked = $(this).attr("checked")
        var $cbx = $(this).parents("table:eq(0)").find(".CbxColumn :checkbox");
        if (checked) {
            $cbx.attr("checked", "checked")
        } else {
            $cbx.removeAttr("checked");
        }
    });


    $("form[data-ajax=true]").live("submit", function () {
        $(this).data(data_ajax_SearchData, $(this).serialize());
    });

});