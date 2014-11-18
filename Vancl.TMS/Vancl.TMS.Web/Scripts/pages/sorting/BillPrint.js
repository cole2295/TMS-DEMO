//标题:面单打印页面专用
//作者:张本冬
//备注:依赖jquery

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
});

//初始化事件
function initData() {
    //设置单号扫描框
    $("#txtFormCode").focus(function () {
        //获取焦点后全选
        var obj = $(this).get(0);
        if (obj.createTextRange) { // IE       
            var rng = obj.createTextRange()
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
            scanWeight();
        }
    }).mouseover(function () {
        $(this).focus();
    });
    //补打
    $("#cbxRePrint").change(function () {
        $(this).parent().css("background", $(this).attr("checked") ? "red" : "");
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
    //获取本地服务状态（jsonp）
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
            //  if (data.Weight <= 0) return;
            //   $("#txtBillWeight").val(data.Weight);
            //            var myDate = new Date();
            //            var s = myDate.getSeconds();
            //            if (s > 50 || (s > 20 && s < 30)) data.Weight = 0;
            //            else data.Weight = s;

            //当前无物体
            if (!WeightEmtpyFlag && data.Weight == 0) {
                return;
            }
            WeightEmtpyFlag = data.Weight > 0;
            if (!WeightEmtpyFlag) $("#txtBillWeight").val("");
            else $("#txtBillWeight").val(data.Weight);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            //   $("#txtBillWeight").val("error");
        }
    });
    ajaxGetWeightHandle = window.setTimeout(getWeight, delay);
    //   CollectGarbage();
}

function submitForMerchant(obj) {
    var jMerchant = $("#Merchant option:selected");
    var ScanModel = {
        ScanType: "0", //运单号
        FormCode: obj.FormCode,
        BillWeight: $("#txtBillWeight").val(),
        MerchantId: obj.MerchantID,
        IsSkipPrintBill: obj.IsSkipPrintBill,
        IsNeedWeight: obj.IsNeedWeight,
        IsCheckWeight: obj.IsCheckWeight,
        CheckWeight: obj.CheckWeight
    };
    scanWeight(ScanModel);
}

//扫描重量
function scanWeight(ScanModel) {
    var jMerchant = $("#Merchant option:selected");
    if (typeof (ScanModel) == "undefined") {
        ScanModel = {
            ScanType: $("#selFormType").val(),
            FormCode: $.trim($("#txtFormCode").val()),
            BillWeight: $("#txtBillWeight").val(),
            MerchantId: $("#Merchant").val(),
            IsSkipPrintBill: jMerchant.attr("IsSkipPrintBill") == "True",
            IsNeedWeight: jMerchant.attr("IsNeedWeight") == "True",
            IsCheckWeight: jMerchant.attr("IsCheckWeight") == "True",
            CheckWeight: parseFloat(jMerchant.attr("CheckWeight"))
        };
    }
    $("#txtFormCode").val("").focus();
    if (ScanModel.FormCode == null || ScanModel.FormCode == "") {
        return showInfo("Error", "单号不能为空！")
    }
    //重量检查 必须称重 2012106
    if (ScanModel.IsNeedWeight) {
        //if (true) {
        var BillWeight = parseFloat($("#txtBillWeight").val());
        if (isNaN(BillWeight) || BillWeight <= 0) {
            return showInfo("Error", "称重重量不能为空！")
        }
        else if (BillWeight > 50) {
            if (!window.confirm("当前重量大于50kg,请确认重量是否异常\r\"确定\"则接收当前重量，\"取消\"则返回修改!")) {
                return;
            }
        }
        ScanModel.BillWeight = BillWeight;
    }
    $("#SelectMultiMerchant").hide();
    showInfo("Info", "正在提交称重数据");

    lockCodeInput();
    $.ajax({
        url: "/Sorting/BillPrint/ScanBill",
        data: ScanModel,
        dataType: 'json',
        type: 'POST',
        success: function (data) {
            //   clearData();
            var cssClass = "";
            if (data.ScanResult == 0) cssClass = "Error";
            if (data.ScanResult == 1) cssClass = "Success";
            if (data.ScanResult == 2) cssClass = "Warning";
            showInfo(cssClass, data.Message);

            //包含多个商家
            if (data.IsMultiMerchant) {
                var mers = data.DataBag;
                var tpl = $("#MultiMerchantListTemplate").tmpl(data.DataBag);
                $("#SelectMultiMerchant tbody").empty().append(tpl).show();
                $("#SelectMultiMerchant").show();
                unlockCodeInput();
                return;
            }

            $("#lbMerchantWeight").text(data.MerchantWeight);
            $("#lbCurrentPackageIndex").text(data.CurrentPackageIndex);
            $("#lbTotalPackageCount").text(data.TotalPackageCount);
            if (data.PackageInfo) {
                $("#tbPackageList tbody").empty().append($("#PackageListTemplate").tmpl(data.PackageInfo));
            }
            $(".lbSiteNo").text(data.SiteNo);
            $("#lbStationName").text(data.StationName);
            $("#lbCompanyName").text(data.CompanyName);
            $("#hidFormCode").val(data.FormCode);
            //运单信息
            $("#lbFormCode").text(data.FormCode);
            if (data.FormCode != null && data.FormCode) $("#aRePint").show();
            else $("#aRePint").hide();
            $("#lbCustomerOrder").text(data.CustomerOrder);
            $("#lbBillSource").text(data.BillSource);
            $("#lbBillType").text(data.BillType);
            $("#lbTotalWeight").text(data.TotalWeight);
            $("#lbBillStatus").text(data.BillStatus);
            //商家信息
            CurrentMerchant = data.DataBag;
            //  alert(data.DataBag.CheckWeight);
            if (data.DataBag != null) {
                $("#lbMerchantName").text(data.DataBag.MerchantName);
                $("#lbMerchantPrint").text(data.DataBag.IsSkipPrintBill ? "否" : "是");
                $("#lbMerchantNeedWeight").text(data.DataBag.IsNeedWeight ? "是" : "否");
                $("#lbMerchantCheckWeight").text(data.DataBag.IsCheckWeight ? "是（" + data.DataBag.CheckWeight + "）" : "无");
            }

            if (data.FormCode && data.Message.indexOf("该单与所选商家不符") == -1) {
                //配送商
                if (data.CompanyFlag == 3) $("#SendToCompany").show();
                else $("#SendToSite").show();
            }
            //打印
            if (data.ScanResult > 0 && data.DataBag != null && data.DataBag.IsSkipPrintBill == false) {
                print(data.FormCode);
            }
            else {
                unlockCodeInput();
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            showInfo("Error", "异步提交数据出错！" + textStatus + errorThrown)
        }
    });
}

function print(FormCode) {
    lockCodeInput();
    if (!CANPRINT) {
        showPrint("Error", "本地打印服务不可用，不能进行打印!");
        unlockCodeInput();
        return;
    }
    showPrint("Info", "正在打印...");

    var url = location.protocol + "//" + location.host + "/Sorting/BillPrint/PrintData/" + FormCode;
    $.ajax({
        url: "http://localhost:41941/PrintService/Print?jsoncallback=?",
        dataType: 'jsonp',
        data: { url: url },
        success: function (data) {
            var cssClass = "";
            if (data.IsSuccess) cssClass = "Success";
            else cssClass = "Error";
            showPrint(cssClass, (data.IsSuccess ? "" : "打印失败，") + data.Message);
            unlockCodeInput();
        },
        error: function () {
            unlockCodeInput();        
        }
    });
}

function rePrint(elem) {
    $("#txtFormCode").focus();
    showInfo("Info", "");
    var FormCode = $("#hidFormCode").val();
    print(FormCode);
}

function reWeigh(elem) {
    $("#txtFormCode").focus();
    showInfo("Info", "正在提交称重信息...");
    clearPrint();
    var FormCode = $("#hidFormCode").val();
    var jTr = $(elem).parents("tr:eq(0)");
    var PackageIndex = parseInt(jTr.attr("PackageIndex"));

//    var Weight = parseFloat(jTr.attr("Weight"));
    var Weight = $("#txtBillWeight").val();
    var MerchantId = CurrentMerchant.MerchantID;
    //    var jMerchant = $("#Merchant option:selected");
    var IsCheckWeight = CurrentMerchant.IsCheckWeight;

    Weight = parseFloat($("#txtBillWeight").val());
    if (isNaN(Weight) || Weight <= 0) {
        return showInfo("Error", "称重重量不能为空！")
    }

    var url = "/Sorting/BillPrint/ReWeigh";
    $.getJSON(url,
        { FormCode: FormCode, PackageIndex: PackageIndex, Weight: Weight, MerchantId: MerchantId, IsCheckWeight: IsCheckWeight },
        function (result) {
            var cssClass = "Error";
            if (result.Result) {
                if (result.Result == 1) cssClass = "Success";
                if (result.Result == 2) cssClass = "Warning";
                jTr.attr("Weight", result.Weight);
                jTr.find("td:eq(1)").text(result.Weight)
                $("#lbTotalWeight").text(result.TotalWeight)
            }
            showInfo(cssClass, result.Message);
        }
     );
}

function showInfo(cssClass, msg) {
    if (cssClass == "Error" || cssClass == "Warning") ErrorNotice();
    else SucceessNotice();

    $("#MessageLabel").show().removeAttr("class").addClass(cssClass).html(msg);
}
function showPrint(cssClass, msg) {
    $("#PrintLabel").show().removeAttr("class").addClass(cssClass).html(msg);
}
function clearPrint() {
    $("#PrintLabel").html("");
}

function clearData() {
    $("#lbMerchantWeight").text();
    $("#lbCurrentPackageIndex").text(0);
    $("#lbTotalPackageCount").text(0);
    $("#tbPackageList tbody").empty();
    $(".lbSiteNo").text("");
    $("#lbStationName").text("");
    $("#lbCompanyName").text("");
    $("#lbFormCode").text("");
    $("#lbCustomerOrder").text("");
    $("#lbBillSource").text("");
    $("#lbBillType").text("");
    $("#lbTotalWeight").text("");
    $("#lbBillStatus").text("");
    $("#MessageLabel,#PrintLabel").text("");
    $("#SendToSite,#SendToCompany").hide();
    $("#aRePint").hide();
    clearPrint();
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

///Create by shixuekai
///2013-08-21
function rePrintByIdex(obj) {
    var ss = $(obj).parent().siblings("td[name='PackageIndexTd']").eq(0).text();
    $("#txtFormCode").focus();
    showInfo("Info", "");
    var FormCode = $("#hidFormCode").val();
    print(FormCode+","+ss);
}