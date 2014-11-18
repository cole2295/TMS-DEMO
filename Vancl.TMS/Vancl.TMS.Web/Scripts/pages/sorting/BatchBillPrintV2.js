//标题:面单打印页面专用
//作者:张本冬
//备注:依赖jquery

var WaybillNo = "";
var PrintList = [];
var printCount = 0;
var failedCount = 0;
var Index = 0;
var CANPRINT = false;
var CANWEIGH = false;

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
    $("#btnStartPrint").click(function () {
        BatchPrint();
    });
});

function BatchPrint() {
    clearData();
    var txt = $("#txtCodeInput").val().trim();

    if (txt == "") {
        showPrint("Warning", "没有要打印的数据");
        return;
    }
    PrintList = [];
    PrintList = txt.split("\n");
    if (PrintList.length == 0) {
        showPrint("Warning", "没有要打印的数据");
        return;
    }
    
    if (PrintList.length > 100) {
        showPrint("Warning", "单量超过100单！");
        return;
    }

    $("#lbStatus").html("正在打印");
    $("#lbTotalCount").html(PrintList.length);
    
    lockCodeInput();
   
    checkFormCode(PrintList[0].trim());

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
        $("#txtFormCode").focus();
    }).change();
  
}


function print(Model) {
    if (!CANPRINT) {
        showPrint("Error", "本地打印服务不可用，不能进行打印!");
        unlockCodeInput();
        return;
    }
    var url = location.protocol + "//" + location.host + "/Sorting/BillPrintV2/PrintDataNew/" + $("#Template").val() + "?FormCode=" + Model.FormCode;
    $.ajax({
        url: "http://localhost:41941/PrintService/PrintNew?jsoncallback=?",
        dataType: 'jsonp',
        data: { url: url },
        success: function (data) {
            if (data.IsSuccess) {
                appendToTable(Model.FormCode, Model.DeliverCode, "打印成功", "");
            } else {
                appendToTable(Model.FormCode, Model.DeliverCode, "打印失败", data.Message);
                failedCount++;
                $("#lbPrintFailureCount").html("" + failedCount);
            }

            printCount++;
            $("#lbPrintCount").html("" + printCount);
            var txt = $("#txtCodeInput").val();
            var list = txt.split("\n");
            var newTxt = txt.replace(list[0], "");
            newTxt = newTxt.trim("\n");
            $("#txtCodeInput").val(newTxt);
            Index++;
            if (Index <= PrintList.length - 1) {
                checkFormCode(PrintList[Index]);
            } else {
                unlockCodeInput();
                $("#lbStatus").html("打印停止");
            }
        },
        error: function () {
            appendToTable(Model.FormCode, Model.DeliverCode, "打印失败", "打印服务请求失败");
            return;
        }
    });
}

function showInfo(cssClass, msg) {
    if (cssClass == "Error" || cssClass == "Warning") ErrorNotice();
    else SucceessNotice();

    $("#MessageLabel").show().removeAttr("class").addClass(cssClass).html(msg);
}
function showPrint(cssClass, msg) {
    $("#PrintLabel").show().removeAttr("class").addClass(cssClass).html(msg);
}


function clearData() {

    $("#PrintLabel").html("");
    $("#MessageLabel").html("");
    printCount = 0;
    failedCount = 0;
    Index = 0;
     $("#lbTotalCount").html("0");
     $("#lbPrintCount").html("0");
     $("#lbPrintFailureCount").html("0");
     $("#tableinfo").find("tr").remove();
     var head = "<tr><th width=\"120\" style=\"font-size:24px\">运单号</th><th width=\"120\" style=\"font-size:24px\" >配送单号</th><th width=\"120\" style=\"font-size:24px\">打印状态</th><th style=\"font-size:24px\">信息</th></tr>";
     $("#tableinfo").prepend(head);
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
    $("#btnStartPrint").attr("disabled", true);
    $("#txtCodeInput").attr("readonly", "readonly").attr("disabled", "disabled").css("background", "#efefef");
}
function unlockCodeInput() {
    $("#btnStartPrint").attr("disabled", false);
    $("#txtCodeInput").removeAttr("readonly").removeAttr("disabled").css("background", "").focus().select();
}

function checkFormCode(code) {
   
   var ScanModel = {
        ScanType: $("#selFormType").val(),
        FormCode: $.trim(code),
        MerchantId: $("#Merchant").val()
    };

    $.ajax({
        url: "/Sorting/BillPrintV2/ScanBill",
        data: ScanModel,
        dataType: 'json',
        type: 'POST',
        success: function (data) {
            if (!data.result) {
                appendToTable(data.Model.FormCode, data.Model.DeliverCode, "打印失败", data.Message);
                Index++;
                printCount++;
                $("#lbPrintCount").html("" + printCount);
                var txt = $("#txtCodeInput").val();
                var list = txt.split("\n");
                var newTxt = txt.replace(list[0], "");
                newTxt = newTxt.trim("\n");
                $("#txtCodeInput").val(newTxt);
                failedCount++;
                $("#lbPrintFailureCount").html("" + failedCount);

                if (Index <= PrintList.length - 1) {
                    checkFormCode(PrintList[Index]);
                } else {
                    unlockCodeInput();
                    $("#lbStatus").html("打印停止");
                }
            } else {
                print(data.Model);
            }
        }
    });
}

function appendToTable(FromCode, DeliverCode, Status, Message) {
    var head = "<tr><th width=\"120\">订单号</th><th width=\"120\">配送单号</th><th width=\"120\">打印状态</th><th width=\"200\">信息</th></tr>";
    var tr = "";
    if (Status != "打印失败")
        tr = "<tr><td width=\"120\">" + FromCode + "</td><td  width=\"120\">" + DeliverCode + "</td><td  width=\"120\">" + Status + "</td><td  width=\"200\">" + Message + "</td></tr>";
    else
        tr = "<tr><td width=\"120\" style=\"color:red;font-size 24px\">" + FromCode + "</td><td width=\"120\" style=\"color:red;font-size 24px\">" + DeliverCode + "</td><td width=\"120\" style=\"color:red;font-size 24px\">" + Status + "</td><td width=\"200\" style=\"color:red;font-size 24px\">" + Message + "</td></tr>";
    $("#tableinfo").find("tr:first").remove();
    $("#tableinfo").prepend(tr);
    $("#tableinfo").prepend(head);
    $("#tableinfo").find("tr:gt(10)").remove();
}