//标题:面单打印页面专用
//作者:张本冬
//备注:依赖jquery

var WaybillNo = "";
var InboundList = [];
var InboundCount = 0;
var failedCount = 0;
var Index = 0;
var CANPRINT = false;
var CANWEIGH = false;
var IsInterrupt = false;
var MAXSHOWCOUNT = 10; //列表最大显示数量
var t = null;

var ajaxGetWeightHandle = null;
//保存称重标识，true:当前电子称上有物体，false:当前电子称上无物体
var WeightEmtpyFlag = false;
//从本地wcf获取重量
function getWeight() {
    // alert(CANWEIGH)
    if (!CANWEIGH) return;
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
            if (!WeightEmtpyFlag && data.Weight == 0) {
                $("#txtBillWeight").attr("disabled", false);
                return;
            }
            WeightEmtpyFlag = data.Weight > 0; ;
            $("#txtBillWeight").attr("disabled", true);
            if (!WeightEmtpyFlag) $("#txtBillWeight").val("");
            else $("#txtBillWeight").val(data.Weight);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        }
    });
    ajaxGetWeightHandle = window.setTimeout(getWeight, delay);
}

$(function () {
    
    initData();
    $("#btnStartInboundV2").click(function () {
   
        if ($(this).val() == "中断入库" || $(this).val() == "继续入库") {
            return ContinueInbound();
        } else {
            return BatchInbound();
        }
    });

});

function ContinueInbound() {
    if (!IsInterrupt) {
        $("#btnStartInboundV2").val("继续入库");
        IsInterrupt = true;
        

    } else {
        IsInterrupt = false;
        $("#btnStartInboundV2").val("中断入库");
        InboundList = [];
        var txt = $("#txtCodeInput").val().trim();
        InboundList = txt.split(/\s+/);
        if (InboundList.length == 0) {
            return;
        }
        $("#lbStatus").html("正在入库");
        Index = 0;
        lockCodeInput();
        BatchInboundByFormCode(InboundList[0]);

    }
}
function BatchInbound() {
    clearData();
    var txt = $("#txtCodeInput").val().trim();

    if (txt == "") {
        showPrint("Warning", "没有要入库的数据");
        return false;
    }
   
    InboundList = [];
    InboundList = txt.split(/\s+/);

      if (InboundList.length == 0) {
            showPrint("Warning", "没有要入库的数据");
            return false;
        }

    if (InboundList.length > 100) {
        showPrint("Warning", "单量超过100单！");
        return false;
    }

    $("#lbStatus").html("正在入库");
    $("#btnStartInboundV2").val("中断入库");
    $("#lbTotalCount").html(InboundList.length);
    lockCodeInput();
    BatchInboundByFormCode(InboundList[0]);
    return false;
}

function BatchInboundByFormCode(FormCode) {
    var pageModel = {
        ScanType: $("#selFormType").val(),
        FormCode: FormCode,
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
                InboundCount++;
                $("#tableinfo tbody tr:eq(0)").addClass("curCorrect");
            } else {
                $("#tableinfo tbody tr:eq(0)").addClass("curError");
                failedCount++;
                $("#lbPrintFailureCount").html("" + failedCount);
            }
            $("#tableinfo tbody tr:gt(0)").each(function () {
                if ($.trim($(this).children("td").eq(2).html()) == "入库失败")
                    $(this).addClass("passError");
            });

            if (MAXSHOWCOUNT < $("#tableinfo tbody tr").length) {
                $("#tableinfo tbody tr").last().remove();
            }


            $("#lbInboundCount").html("" + InboundCount);
            $("#lbInboundFailureCount").html("" + failedCount);
            
            var txt = $("#txtCodeInput").val();
            var list = txt.split("\n");
            var newTxt = txt.replace(list[0], "");
            newTxt = newTxt.trim("\n");
            $("#txtCodeInput").val(newTxt);
            Index++;
            t = setTimeout(function () {
                if (!IsInterrupt) {
                    if (Index <= InboundList.length - 1) {
                        BatchInboundByFormCode(InboundList[Index]);
                    } else {
                        clearTimeout(t);
                        unlockCodeInput();
                        $("#lbStatus").html("录入停止");
                        $("#btnStartInboundV2").val("批量入库");
                    }
                } else {
                    clearTimeout(t);
                    unlockCodeInput();
                    $("#lbStatus").html("录入中断");
                    return;

                }

            }, 1000);

        }
    });
}
function showPrint(cssClass, msg) {
    $("#PrintLabel").show().removeAttr("class").addClass(cssClass).html(msg);
}

function clearData() {

    $("#PrintLabel").html("");
    $("#MessageLabel").html("");
    InboundCount = 0;
    failedCount = 0;
    Index = 0;
    $("#lbTotalCount").html("0");
    $("#lbInboundCount").html("0");
    $("#lbInboundFailureCount").html("0");
    $("#tableinfo tbody tr").remove();

}

//初始化事件
function initData() {

    //商家
    $("#Merchant").change(function () {
        var jMerchant = $("#Merchant option:selected");
        $("#lbMerchantName").text(jMerchant.text());
        $("#lbMerchantPrint").text(jMerchant.attr("IsSkipPrintBill") == "True" ? "否" : "是");
        $("#lbMerchantNeedWeight").text(jMerchant.attr("IsNeedWeight") == "True" ? "是" : "否");
        $("#lbMerchantCheckWeight").text(jMerchant.attr("IsCheckWeight") == "True" ?
            "是（" + jMerchant.attr("CheckWeight") + "）"
            : "无");
       
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
   // $("#btnStartInbound").attr("disabled", true);
    $("#txtCodeInput").attr("readonly", "readonly").attr("disabled", "disabled").css("background", "#efefef");
}
function unlockCodeInput() {
   // $("#btnStartInbound").attr("disabled", false);
    $("#txtCodeInput").removeAttr("readonly").removeAttr("disabled").css("background", "").focus().select();
}

