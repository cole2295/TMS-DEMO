//标题:面单打印页面专用
//作者:张本冬
//备注:依赖jquery

var CANPRINT = false;
var CANWEIGH = false;
var WaybillNo = "";

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
            clearData();
            checkFormCode();
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
        }
    });
    $.ajax({
        url: "http://localhost:41941/CfgService/Version?jsoncallback=?",
        dataType: 'jsonp', beforeSend: function () { }, complete: function () { },
        success: function (data) { $("#CilentServiceInfo").text("当前打印称重服务版本:" + data); }
    });

}


function print(Model,Count,Index) {
    lockCodeInput();
    if (!CANPRINT) {
        showPrint("Error", "本地打印服务不可用，不能进行打印!");
        unlockCodeInput();
        return;
    }
    showPrint("Info", "正在打印...");

    var url = location.protocol + "//" + location.host + "/Sorting/BillPrintV2/PrintDataNew/" +Index + "?FormCode=" + Model.FormCode+"&Count=";
    $.ajax({
        url: "http://localhost:41941/PrintService/Print?jsoncallback=?",
        dataType: 'jsonp',
        data: { url: url },
        success: function (data) {
            if (++Index <= Count) {
                print(Model, Count, Index);
            } else {
                
                if (data.IsSuccess) {
                    appendToTable(Model.FormCode, Model.DeliverCode, "打印成功", "");
                    playSound("succ");
                } else {
                    appendToTable(Model.FormCode, Model.DeliverCode, "打印失败", data.Message);
                    playSound("error");
                }

                unlockCodeInput();
                $("#txtFormCode").val("");
                $("#txtFormCode").focus();
                showPrint("Info", "");
            }
           
        },
        error: function () {
            playSound("error");
            unlockCodeInput();
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

function checkFormCode() {
   
   var ScanModel = {
        ScanType: $("#selFormType").val(),
        FormCode: $.trim($("#txtFormCode").val()),
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
                $("#txtFormCode").val("");
                $("#txtFormCode").focus();
                playSound("error");
                return;
            } else {
                print(data.Model,data.PackageCount,1);
            }
        }
    });
}

function appendToTable(FromCode, DeliverCode, Status, Message) {
    var head = "<tr><td colspan = \"6\">订单号</td><td colspan = \"6\">配送单号</td><td colspan = \"4\">打印状态</td><td colspan = \"10\">信息</td></tr>";
    var tr = "";
    if(Status !="打印失败")
        tr = "<tr><td colspan = \"6\">" + FromCode + "</td><td colspan=\"6\">" + DeliverCode + "</td><td colspan=\"4\">" + Status + "</td><td colspan=\"10\">" + Message + "</td></tr>";
    else
        tr = "<tr><td colspan = \"6\" style=\"color:red;font-size 24px\">" + FromCode + "</td><td colspan=\"6\" style=\"color:red;font-size 24px\">" + DeliverCode + "</td><td colspan=\"4\" style=\"color:red;font-size 24px\">" + Status + "</td><td colspan=\"10\" style=\"color:red;font-size 24px\">" + Message + "</td></tr>";
    $("#tableinfo").find("tr:first").remove();
    $("#tableinfo").prepend(tr);
    $("#tableinfo").prepend(head);
    $("#tableinfo").find("tr:gt(10)").remove();
}