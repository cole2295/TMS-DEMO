$(function () {
    //页面初始化
    $.tms.page.init();
    //切换操作
    $("span.po_h1_fn").click(function () {
        $(this).toggleClass('fn_po_show').parent().next()
            .slideToggle("fast", function () {
                $(window).resize();
            }
        );
    });
})