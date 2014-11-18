//标题:面单打印页面专用
//作者:张本冬
//备注:依赖jquery

var CANPRINT = false;
var CANWEIGH = false;
var WaybillNo = "";
var MAXSHOWCOUNT = 10; //列表最大显示数量

var ajaxGetWeightHandle = null;
//保存称重标识，true:当前电子称上有物体，false:当前电子称上无物体
var WeightEmtpyFlag = false;
//从本地wcf获取重量
function getWeight() {
    //alert(CANWEIGH)
    if (!CANWEIGH) {
        return;
    }
    //  delay = parseInt(delay);
    //默认频率为100毫秒
    var delay = 500;
    if (ajaxGetWeightHandle != null) {
        window.clearTimeout(ajaxGetWeightHandle);
    }
    $.ajax({
        url: "http://localhost:41941/WeightService/Getweight?jsoncallback=?",
        dataType: 'jsonp',
        beforeSend: function () { },
        success: function (data) {
            //当前无物体
            if (!WeightEmtpyFlag && (data == undefined || data == "" || isNaN(data.Weight) || data.Weight == 0)) {
                $("#txtBillWeight").attr("disabled", false);
                return;
            }
            $("#txtBillWeight").attr("disabled", true);
            WeightEmtpyFlag = data.Weight > 0;
            if (!WeightEmtpyFlag) $("#txtBillWeight").val("");
            else $("#txtBillWeight").val(data.Weight);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            $("#txtBillWeight").attr("disabled", false);
        }
    });
    ajaxGetWeightHandle = window.setTimeout(getWeight, delay);
}

$(function () {
    initData();
    $("#txtFormCode").focus();
    //alert($("#Merchant option").length);
    if ($("#Merchant option").length == 1) {
        $('#Merchant').attr("disabled", "disabled");
    }
    if ($("#Merchant option").length <= 3) {
        $("#selFormType")[0].selectedIndex = 1;
    }
});

//初始化事件
function initData() {
    //设置单号扫描框
    $("#txtFormCode").focus(function () {
        //获取焦点后全选
        var obj = $(this).get(0);
        if (obj.createTextRange) { // IE       
            var rng = obj.createTextRange();
            var sel = rng.duplicate();
            sel.select();
        } else if (typeof obj.selectionStart == 'number') { // Firefox 
            obj.selectionStart;
            obj.setSelectionRange(0, 1000);
            obj.focus();
        }
    }).blur(function () {
        //   $(this).focus();
    }).keyup(function (event) {
        //回车事件
        if (event.keyCode == '13') {
            $(this).focus();
            var v = $(this).val();
            if ($.trim(v) == "") {
                showInfo("Warning", "请输入单号再提交！");
                return;
            }
            if (parseFloat($("#txtBillWeight").val()) > 50.0) {
                if (!confirm("重量大于50kg，是否继续录入？")) {
                    $("#txtFormCode").val("");
                    return;
                }

            }
            lockCodeInput();
            SimpleInbound();

        }
    }).mouseover(function () {
        $(this).focus();
    });

    //商家
    $("#Merchant").change(function () {
        var jMerchant = $("#Merchant option:selected");
        $("#lbMerchantName").text(jMerchant.text());
        $("#lbMerchantPrint").text(jMerchant.attr("IsSkipPrintBill") == "True" ? "否" : "是");
        $("#lbMerchantNeedWeight").text(jMerchant.attr("IsNeedWeight") == "True" ? "是" : "否");
        $("#lbMerchantCheckWeight").text(jMerchant.attr("IsCheckWeight") == "True" ?
            "是（" + jMerchant.attr("CheckWeight") + "）"
            : "无");
        $("#txtFormCode").focus();
    }).change();

    $.ajax({
        url: "http://localhost:41941/PrintService/CanPrint?jsoncallback=?",
        dataType: 'jsonp', beforeSend: function () { }, complete: function () { },
        success: function (data) { CANPRINT = data; }
    });
    $.ajax({
        url: "http://localhost:41941/WeightService/CanWeigh?jsoncallback=?",
        dataType: 'jsonp', beforeSend: function () { }, complete: function () { },
        success: function (data) {
            CANWEIGH = data;
            getWeight();
        }
    });
    $.ajax({
        url: "http://localhost:41941/CfgService/Version?jsoncallback=?",
        dataType: 'jsonp', beforeSend: function () { }, complete: function () { },
        success: function (data) { $("#CilentServiceInfo").text("当前打印称重服务版本:" + data); }
    });

}



function clearNoNum(obj) {
    //先把非数字的都替换掉，除了数字和.
    obj.value = obj.value.replace(/[^\d.]/g, "");
    //必须保证第一个为数字而不是.
    obj.value = obj.value.replace(/^\./g, "");
    //保证只有出现一个.而没有多个.
    obj.value = obj.value.replace(/\.{2,}/g, ".");
    //保证.只出现一次，而不能出现两次以上
    obj.value = obj.value.replace(".", "$#$").replace(/\./g, "").replace("$#$", ".");
    //  ^[0-9]+(.[0-9]{2})?$
}

function lockCodeInput() {
    $("#txtFormCode").attr("readonly", "readonly").attr("disabled", "disabled").css("background", "#efefef");
}
function unlockCodeInput() {
    $("#txtFormCode").removeAttr("readonly").removeAttr("disabled").css("background", "").focus().select();
}

function ReScanWeight() {
    var FormCode = $.trim($("#tableinfo tbody tr:eq(0) td:eq(0)").text());
    var Weight = $("#txtBillWeight").val();
    if (Weight > 50.0) {
        if (!confirm("重量大于50kg，是否重新称重?")) {
            return;
        }
    }
    $.ajax({
        url: "/Sorting/InboundV2/ReWeight",
        type: "POST",
        dataType: "json",
        contentType: "application/json",
        data: "{FormCode:'" + FormCode + "', Weight:'" + Weight + "'}",
        success: function (data) {
            if (data.result)
                alert("重新称重成功！");
            else {
                alert(data.Message);
            }
        }
    });


}

function RePrint() {
    alert("抱歉尚未实现该功能！");
}


function SimpleInbound() {
    var pageModel = {
        ScanType: $("#selFormType").val(),
        FormCode: $.trim($("#txtFormCode").val()),
        MerchantId: $("#Merchant").val(),
        BillWeight: $("#txtBillWeight").val(),
        TemplateId: $("Template").val()
    };
    var url = "/Sorting/InboundV2/Inbound";
    $.ajax({
        url: url,
        type: 'post',
        dataType: 'json',
        data: pageModel,
        success: function (data) {
            var html = $("#tmpSimpleList").tmpl(data.Model);
            $("#tableinfo tbody").prepend(html);
            $("#tableinfo tbody tr").removeClass("curError").removeClass("curCorrect");

            if (data.result) {
                playSound("succ");
                $("#tableinfo tbody tr:eq(0)").addClass("curCorrect");
            } else {
                playSound("error");
                alert(data.Model.Message);
                $("#tableinfo tbody tr:eq(0)").addClass("curError");
            }


            $("#tableinfo tbody tr:gt(0)").each(function () {
                if ($.trim($(this).children("td").eq(2).text()) == "入库失败")
                    $(this).addClass("passError");
            });

            if (MAXSHOWCOUNT < $("#tableinfo tbody tr").length) {
                $("#tableinfo tbody tr").last().remove();
            }

            $("#TotalCount").html("" + data.TotalCount);
            $("#CurrentCount").html("" + data.CurrentCount);
            var links = (data.IsNeedWeight ? "<a href='javascript:ReScanWeight()'>重新称重</a>" : "") + "&nbsp &nbsp &nbsp" + (data.IsSkipPrint ? "" : "<a href='javascript:RePrint()'>重新打印</a>");
            var details = "<tr><td>" + data.CurrentCount + "</td><td>" + data.CustomerWeight + "</td><td>" + links + "</td></tr>";
            $("#tabledetail tbody tr").remove();
            $("#tabledetail tbody").prepend(details);
            $("#txtFormCode").val("");
            unlockCodeInput();
            var trCount = $("#tableinfo tbody tr").length;
            if (trCount > 1) {
                $("#tableinfo tr :eq(" + parseInt(trCount - 1) + ")").each(function () {

                });
            }
        }
    });
}