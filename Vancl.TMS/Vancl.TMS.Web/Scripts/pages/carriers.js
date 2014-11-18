$(function () {
    $(".button").click(function () {
        ymPrompt.win({ message: '/BaseInfo/Carrier/Create', width: 550, height: 480, title: '新增承运商',
            handler: function (tp, data) {
                //...
            }, maxBtn: false, minBtn: false, iframe: true
        });
    });
});