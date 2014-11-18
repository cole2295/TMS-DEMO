var MAXCOUNT = 50; //装箱最大运单数
var posting = false;        //是否逐单提交中
var MAXSHOWCOUNT = 10; //列表最大显示数量
var isScanBox = false;
var flag = true;
$(function () {
    //单选change事件
    $(".sortingCenterSelect").change(function () {
        reloadData();
        var flag1 = true;
        var i = 0;
        $('.sortingCenterSelect').each(function () {
            i++;
            var o = $(this).parent().next().find("Select");
            if (typeof ($(this).attr("checked")) == "undefined"
                        || $(this).attr("checked") == ""
                        || !$(this).attr("checked")) {
                o.attr("disabled", "disabled");
            } else {
                if (i == 1) flag1 = false;
                o.removeAttr("disabled");
            }
            if (i == 1 && !flag1) { $("#btnReturnBound").attr("disabled", "disabled"); }
            else {
                flag1 ? $("#btnReturnBound").removeAttr("disabled") : "";
            }
        });
    }).change();
    $("#SortingCenterList").attr("disabled", "disabled");
    $("#DistributorList").attr("disabled", "disabled");
    $("#Merchant").live("change", function () {
        reloadData();
    });
    $("#DistributorList").change(function () {
        reloadData();
    });
    $("#SortingCenterList").change(function () {
        reloadData();
    });
    $(".w").change(function () {
        if ($("#ckcheckWeight").attr("checked") && flag) {
            $(".w1").children().attr("disabled", false);
            flag = false;
            $("#cknocheckWeight").removeAttr("checked");
        }
        if ($("#cknocheckWeight").attr("checked")) {
            flag = true;
            $(".w1").children().attr("disabled", "disabled");
            $("#ckcheckWeight").removeAttr("checked");
        }
    }).change();
    $(".BillInBox").change(function () {
        if ($(this).attr("checked")) {
            $("#txtBoxWeight").removeAttr("disabled");
            $("#btnDeleteBill").removeAttr("disabled");
            $("#btnInBoxPrint").removeAttr("disabled");
        }
        else {
            $("#txtBoxWeight").attr("disabled", "disabled");
            $("#btnDeleteBill").attr("disabled", "disabled");
            $("#btnInBoxPrint").attr("disabled", "disabled");
        }
    }).change();
    //扫描订单号事件
    $("#txtFormCode").keydown(function (e) {
        if (e.which != 13) {
            return;
        }
        scanFormCode();
    });
    //扫描箱号事件
    $("#txtBoxNo").keydown(function (e) {
        if (e.which != 13) {
            return;
        }
        isScanBox = true;
        scanBoxNo();
    });
    //扫描订单称重事件
    $("#txtWeight").keydown(function (e) {
        if (e.which != 13) {
            return;
        }
        Weight(txtWeight, true);
    });
    //装箱称重
    $("#txtBoxWeight").keydown(function (e) {
        if (e.which != 13) {
            return;
        }
        Weight(txtBoxWeight, false);
    });
});

//退货出库按钮事件
function ReturnOutBound() {
    var ckbList = $("#tblBillInfo tbody tr :checkbox:checked");
    if (ckbList.length < 1) {
        alert("请选择一项！");
        return;
    }
    var formCodes = "";
    ckbList.each(function () {
        var code = $.trim($(this).parent().parent()[0].id);
        formCodes += code + ",";
    });
    var isBox = true;
    var BoxNoOrFormCodes = $("#lbBoxNo").html().trim();
    if (BoxNoOrFormCodes == "") {
        isBox = false;
        BoxNoOrFormCodes = $("#hdBillReturnNos").val();
    }
    var params = {};
    params.BoxNoOrFormCodes = BoxNoOrFormCodes;
    params.isBox = isBox;
    $.ajax({
        url: "/Sorting/Return/ReturnOutBound",
        data: params,
        dataType: 'json',
        type: 'POST',
        success: function (result) {
            if (result == null) {
                alert(result);
                return;
            }
            if (result.IsSuccess) {
                alert(result.Message);
            }
        }
    });
}
//删除按钮事件
function DeleteBill() {
    var ckbList = $("#tblBillInfo tbody tr :checkbox:checked");
    if (ckbList.length < 1) {
        alert("请选择一项！");
        return;
    }
    var formCodes = "";
    ckbList.each(function () {
        var code = $.trim($(this).parent().parent()[0].id);
        formCodes += code + ",";
    });
    var params = {};
    params.FormCodelists = formCodes;
    params.hidCodeLists = $("#hdBillReturnNos").html();
    $.ajax({
        url: "/Sorting/Return/DeleteBill",
        data: params,
        dataType: 'json',
        type: 'POST',
        success: function (result) {
            if (result == null) {
                alert(result);
                return;
            }
            if (result.IsSuccess) {
                if (result.ExtendedObj.hidBillNos != "") {
                    $("#hdBillReturnNos").val(result.ExtendedObj.hidBillNos);
                }
                if ($("#lbBoxNo").html().trim() == "") {
                    GetBillReturnDetailInfo(false);
                }
                else {
                    GetBillReturnDetailInfo(true);
                }
                if (isScanBox) {
                    $("#lbWaybillCount").html($("#lbWaybillCount").html() == 0 ? 0 : $("#lbWaybillCount").html() - ckbList.length);
                }
                else {
                    $("#InboundCount").html($("#InboundCount").html() == 0 ? 0 : $("#InboundCount").html() - ckbList.length);
                }
            }
            else {
                alert(result.Message);
            }
        }
    });
}
//重置页面事件
function ResetPage() {
    $("#tblBillInfo tbody").empty();
    $("#txtFormCode").val("");
    $("#lbBoxNo").html("");
    $("#lbWaybillCount").html("");
    $("#ckcheckWeight").removeAttr("checked");
    $("#cknocheckWeight").attr("checked", "checked");
    $("#txtWeight").val("");
    $("#txtBoxNo").val("");
    $("#txtBoxWeight").val("");
    $("#hdBillReturnNos").val("");
    GetFocus(txtFormCode);
}
function reloadData() {
    $("#tblBillInfo tbody").empty();
    $("#txtFormCode").val("");
    $("#lbWaybillCount").html(0);
}

//获取选择的站点信息
function getStationValue(selectedType) {
    if (selectedType == "rdoMerchant") { return $("#Merchant option:selected").val(); }
    else if (selectedType == "rdoSortingCenter") { return $("#SortingCenterList option:selected").val(); }
    else if (selectedType == "rdoDistribution") { return $("#DistributorList option:selected").val(); }
    return "";
}
//获取选择的站点名称
function getStationName(selectedType) {
    if (selectedType == "rdoMerchant") { return $("#Merchant option:selected").html(); }
    else if (selectedType == "rdoSortingCenter") { return $("#SortingCenterList option:selected").html(); }
    else if (selectedType == "rdoDistribution") { return $("#DistributorList option:selected").html(); }
    return "";
}

//扫描运单号
function scanFormCode() {
    var isFirst = true;
    var formCode = $.trim($("#txtFormCode").val());
    if (formCode == "") {
        GetFocus(txtFormCode);
        return;
    }
    var formType = $("#selFormType option:selected").val();
    if (formType == "0" && (isNaN(formCode) || formCode.length > 14)) {
        alert("运单号不正确!");
        GetFocus(txtFormCode);
        return;
    }

    var trs = $("#tblBillInfo tbody tr");
    if (trs.length > MAXCOUNT) {
        alert("每箱单量不能超过" + MAXCOUNT + "单!");
        GetFocus(txtFormCode);
        return;
    }
    if (trs.length > 0) {
        isFirst = false;
    }
    //判断运单号重复
    var isRepeate = false;
    if (!isFirst) {
        trs.each(function () {
            var code = $.trim($(this).find("td:eq(2)").html());
            if (formType == "1") {
                code = $.trim($(this).find("td:eq(3)").html());
            } else if (formType == "2") {
                code = $.trim($(this).find("td:eq(6)").html());
            }
            if (code == formCode) {
                isRepeate = true;
                return false;
            }
        });
    }
    if (isRepeate) {
        GetFocus(txtFormCode);
        return;
    }

    if ($(".sortingCenterSelect:checked").length == 0) {
        alert("扫描单号请选择商家");
        return false;
    }
    var selectedType = $(".sortingCenterSelect:checked")[0].id;
    var stationValue = getStationValue(selectedType);
    var stationName = getStationName(selectedType);
    if (!isFirst) {
        if (stationValue == "" || stationValue == -1) {
            alert('请选择站点');
            return;
        }
    }
    posting = true;
    var isNeedWeight = $("#ckcheckWeight").attr("checked") ? true : false;
    var isInBox = $("#ckInBox").attr("checked") ? true : false;
    var scanFormCodeModel = {
        hidData: $("#hdReturnInitData").val(),
        Code: formCode,
        BoxNo: $("#lbBoxNo").html(),
        selectedType: selectedType,
        selectStationName: stationName,
        selectStationValue: stationValue,
        FormType: formType,
        hidBillNos: $("#hdBillReturnNos").val(),
        isFirst: isFirst,
        isNeedWeight: isNeedWeight,
        isInBox: isInBox
    };

    $.ajax({
        url: "/Sorting/Return/ScanFormCode",
        data: scanFormCodeModel,
        dataType: 'json',
        type: 'POST',
        success: function (result) {
            if (result == null) {
                alert(result);
                return;
            }
            if (result.IsSuccess) {
                if (isInBox) {
                    if (isFirst) {
                        $("#tblBillInfo tbody").empty();
                        $("#lbBoxNo").html(result.ExtendedObj.BoxNo);
                    }
                }
                if (isNeedWeight) {
                    GetFocus(txtWeight);
                }
                else {
                    GetFocus(txtFormCode);
                    $("#InboundCount").html(result.ExtendedObj.count);
                    $("#lbWaybillCount").html(trs.length + 1);
                    $("#hdBillReturnNos").val(result.ExtendedObj.hidBill);
                    if (result.ExtendedObj.model != null) {
                        var html = $("#tmpSimpleList").tmpl(result.ExtendedObj.model);
                        $("#tblBillInfo tbody").append(html);

                    }
                }
            }
            else {
                alert(result.Message);
                GetFocus(txtFormCode);
            }
        }
    });
}
//扫描箱号操作
function scanBoxNo() {
    var boxNo = $.trim($("#txtBoxNo").val());
    if (boxNo == "") {
        GetFocus(txtBoxNo);
        return;
    }
    GetBillsByBoxNo(boxNo);
}

//称重操作(包括扫描运单称重和装箱称重)
function Weight(BillOrBox, isBill) {
    var weight = parseFloat($(BillOrBox).val());
    if (isNaN(weight) || weight <= 0) {
        alert("称重重量有误！");
        GetFocus(BillOrBox);
        return;
    }
    var trs = $("#tblBillInfo tbody tr");
    var selectedType = $(".sortingCenterSelect:checked")[0].id;
    var ScanModel = {
        selectedType: selectedType,
        selectStationValue: getStationValue(selectedType),
        selectStationName: getStationName(selectedType),
        Code: $.trim($("#txtFormCode").val()),
        boxNo: $("#lbBoxNo").html(),
        weight: weight,
        hidFormCodes: $("#hdBillReturnNos").val(),
        formType: $("#selFormType option:selected").val()
    };
    //扫描运单称重
    if (isBill) {
        $.ajax({
            url: "/Sorting/Return/BillWeight",
            data: ScanModel,
            dataType: 'json',
            type: 'POST',
            success: function (result) {
                if (result == null) {
                    alert("服务器无响应");
                    return;
                }
                if (result.IsSuccess) {
                    $("#hdBillReturnNos").val(result.ExtendedObj.hidBill);
                    if (result.ExtendedObj.Model != null) {
                        var html = $("#tmpSimpleList").tmpl(result.ExtendedObj.Model);
                        $("#tblBillInfo tbody").append(html);
                    }
                    $("#InboundCount").html(result.ExtendedObj.count);
                    $("#lbWaybillCount").html(trs.length + 1);
                    GetFocus(txtFormCode);
                }
                else {
                    alert(result.Message);
                }
                GetFocus(txtFormCode);
            }
        });
    }
    //装箱称重
    else {
        $.ajax({
            url: "/Sorting/Return/BoxWeight",
            data: ScanModel,
            dataType: 'json',
            type: 'POST',
            success: function (result) {
                if (result == null) {
                    alert("服务器无响应");
                    return;
                }
                if (result.IsSuccess) {
                    alert(result.Message);
                    GetFocus(txtFormCode);
                }
                else {
                    alert(result.Message);
                    GetFocus(txtBoxWeight);
                }
            }
        });
    }
}
//清空扫描运单文本框并获取焦点
function GetFocus(id) {
    $(id).val("");
    $(id).focus();
}

//勾选运单装箱则生成箱号
function GetBoxNo() {
    var boxNo = $("#lbBoxNo").html();
    var params = {};
    params.BoxNo = boxNo;
    $.ajax({
        url: "/Sorting/Return/GetBoxNo",
        data: params,
        dataType: 'json',
        type: 'POST',
        success: function (result) {
            if (result == null) {
                alert("服务器无响应");
                return;
            }
            if (result.IsSuccess) {
                $("#lbBoxNo").html(result.ExtendedObj.BoxNo);
            }
        }
    });
}

function cleartxt() {
    $("#txtFormCode").val("");
    $("#txtWeight").val("");
    $("#txtBoxWeight").val("");
    $("#txtBoxNo").val("");
}
//写入数据并查询数据
function GetBillReturnDetailInfo(isBox) {
    $("#tblBillInfo tbody").empty();
    var params = {};
    params.billNosOrBoxNo = $("#hdBillReturnNos").html();
    if (isBox) {
        params.billNosOrBoxNo = $("#lbBoxNo").html().trim() == "" ? $.trim($("#txtBoxNo").val()) : $("#lbBoxNo").html().trim();
    }
    params.isBox = isBox;
    $.ajax({
        url: "/Sorting/Return/GetScanBillOrBox",
        data: params,
        dataType: 'json',
        type: 'POST',
        success: function (result) {
            if (result == null) {
                alert("服务器无响应");
                return;
            }
            if (result.IsSuccess) {
                if (result.ExtendedObj.list != null) {
                    for (var i = 0; i < result.ExtendedObj.list.length; i++) {
                        var html = $("#tmpSimpleList").tmpl(result.ExtendedObj.list[i]);
                        $("#tblBillInfo tbody").append(html);
                    }
                    $("#lbWaybillCount").html(result.ExtendedObj.list.length);
                }
                else {
                    $("#lbWaybillCount").html(0);
                }
            }
            else {
                if (isBox) alert(result.Message);
                $("#lbBoxNo").html("");
                $("#lbWaybillCount").html("");
                GetFocus(txtBoxNo);
            }
        }
    });
}

//按照箱号查找箱号中的运单
function GetBillsByBoxNo(boxNo) {
    var params = {};
    params.boxNo = boxNo;
    $.ajax({
        url: "/Sorting/Return/GetBillsByBoxNo",
        data: params,
        dataType: 'json',
        type: 'POST',
        success: function (result) {
            if (result == null) {
                alert("服务器无响应");
                return;
            }
            if (result.IsSuccess) {
                $("#lbBoxNo").html(boxNo);
                if (result.ExtendedObj.list != null) {
                    $("#tblBillInfo tbody").empty();
                    for (var i = 0; i < result.ExtendedObj.list.length; i++) {
                        var html = $("#tmpSimpleList").tmpl(result.ExtendedObj.list[i]);
                        $("#tblBillInfo tbody").append(html);
                    }
                    $("#lbWaybillCount").html(result.ExtendedObj.list.length);
                }
                else {
                    $("#lbWaybillCount").html(0);
                }
            }
            else {
                alert(result.Message);
                $("#lbBoxNo").html("");
                $("#lbWaybillCount").html("");
                GetFocus(txtBoxNo);
            }
        }
    });
}
//装箱打印
function showReceipt() {
    var boxNo = $("#lbBoxNo").html();
    var weight = $("#txtBoxWeight").val().trim();
    if (weight == "" || weight == "0") {
        alert("未称重，不能装箱打印");
        GetFocus(txtBoxWeight);
        return;
    }
    if ($("#tblBillInfo tbody tr").length == 0) {
        alert("没有运单不能装箱打印！"); return;
    }
    var params = {};
    params.boxNo = boxNo;
    params.weight = weight;
    $.ajax({
        url: "/Sorting/Return/InBoxPrint",
        data: params,
        dataType: 'json',
        type: 'POST',
        success: function (result) {
            if (result == null) {
                alert(result);
                return;
            }
            if (result.IsSuccess) {
                openWindow('/Sorting/Return/BackPackingPrint?boxno=' + boxNo, '箱号明细', 800, 600);
            }
            else {

                alert(result.Message);
            }
        }
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


           






