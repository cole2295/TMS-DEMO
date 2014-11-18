var posting = false;                          //是否提交中
var MAXCOUNT = 50;                  //每箱最大单量限制
var IsRePrintChanged = false;         //补打印是否有修改
var IsPageRePrint = false;              //是否是补打印
var IsScalesConnected = false;       //是否连接了电子称
$(function () {
    $('.sortingCenterSelect').change(function () {
        reloadData();
        $("span#InboundCount").html(0);
        $("#txtWeight").removeAttr("disabled");
        $("#trSortingCenterSelect select").val(-1);
        $('.sortingCenterSelect').each(function () {
            var o = $(this).parent().next().find("select");
            if (typeof ($(this).attr("checked")) == "undefined"
                        || $(this).attr("checked") == ""
                        || !$(this).attr("checked")) {
                o.attr("disabled", "disabled");
            } else {
                o.removeAttr("disabled");
            }
        });
    });

    $("#SortingCenterList").attr("disabled", "disabled");
    $("#DistributorList").attr("disabled", "disabled");

    $("#selCityAndStation_Station").live("change", function () {
        reloadData();
        $("span#InboundCount").html(0);
        getCount();
    });
    $("#DistributorList").change(function () {
        reloadData();
        $("span#InboundCount").html(0);
        getCount();
    });
    $("#SortingCenterList").change(function () {
        reloadData();
        $("span#InboundCount").html(0);
        getCount();
    });

    $("input:radio[name='rdoInboundBox']").change(function () {
        $(this).removeAttr("disabled");
        if ($(this).attr("id") == "radioBoxNo") {
            $("#txtFormCode").val("");
            $("#txtFormCode").attr("disabled", "disabled");
            $("#txtBoxNo").removeAttr("disabled");
            $("#txtBoxNo").focus();
        }
        else {
            $("#txtBoxNo").val("");
            $("#txtBoxNo").attr("disabled", "disabled");
            $("#txtFormCode").removeAttr("disabled");
            $("#txtFormCode").focus();
            //如果补打印没有做任何操作，则变为装箱状态
            if ($("#lblBoxNo").html() == "") {
                IsPageRePrint = false;
                IsRePrintChanged = false;
                $("#txtWeight").val("");
                $("#tblBillInfo tbody").empty();
            }
        }
//        if ($(this).attr("checked") == "checked") {
//            $(this).next().val("");
//            $(this).next().focus();
//        }
    });

    $("#txtFormCode").keydown(function (e) {
        if (e.which != 13) {
            return;
        }
        scanFormCode();
    });

    $("#txtBoxNo").keydown(function (e) {
        if (e.which != 13) {
            return;
        }
        scanBoxNo();
    });
    window.setInterval(initWeight, 500);
});

function deleteBill() {
    var ckbList = $("#tblBillInfo tbody tr :checkbox:checked");
    if (ckbList.length < 1) {
        alert("请选择一项！");
        return;
    }
    ckbList.each(function () {
        $(this).parent().parent().remove();
    });
    if (IsPageRePrint) {
        IsRePrintChanged = true;
        $("#txtWeight").removeAttr("disabled");
        $("#txtWeight").val("");
    }
    rebuildIndex();
}

function resetPage() {
    $("#rdoStation").attr("checked", "checked");
    $("#SortingCenterList").attr("disabled", "disabled");
    $("#DistributorList").attr("disabled", "disabled");
    $("#selCityAndStation_City").removeAttr("disabled");
    $("#selCityAndStation_Station").removeAttr("disabled");
    $("#selCityAndStation_City").val(-1);
    $("#selCityAndStation_City").change();
    $("#radioWaybillNo").attr("checked", "checked");
    $("#radioBoxNo").removeAttr("checked");
    $("#radioWaybillNo").change();
    $("span#InboundCount").html(0);
    reloadData();
    $("#txtWeight").removeAttr("disabled");
    IsPageRePrint = false;
    posting = false;
}

function initWeight() {
    $.ajax({
        url: "http://localhost:41941/WeightService/Getweight?jsoncallback=?",
        dataType: 'jsonp',
        beforeSend: function () { },
        success: function (data) {
            if ((!IsScalesConnected && (data == undefined || data == ""  || isNaN(data.Weight) || data.Weight == 0))
                || $("#txtWeight").attr("disabled") == "disabled") {
                return;
            }
            IsScalesConnected = true;
            $("#txtWeight").val(data.Weight);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            if (IsScalesConnected) {
                $("#txtWeight").val("");
            }
            IsScalesConnected = false;
        }
    });
};

function getCount() {
    var selectedType = $(".sortingCenterSelect:checked").val();
    var stationValue = getStationValue(selectedType);
    if (stationValue == "" || stationValue == -1) {
        $("span#InboundCount").html(0);
        return;
    }
    var params = {};
    params.hidData = $("#hdInboundInitData").val();
    params.selectedType = selectedType;
    params.selectStationValue = stationValue;
    $.ajax({
        type: "POST",
        url: "GetCount",
        data: params ,
        dataType: 'json',
        success: function (result) {
            if (result == null) { return; }
            if (result.IsSuccess) {
                $("span#InboundCount").html(result.ExtendedObj.Count);
            }
            else {
                alert(result.Message);
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert('操作超时，服务器无响应' + textStatus + errorThrown);
        }
    });
}

function reloadData() {
    $("#tblBillInfo tbody").empty();
    $("#lblBoxNo").html("");
    $("#lblCount").html("");
    $("#txtWeight").val("");
    $("#txtFormCode").val("");
    $("#txtBoxNo").val("");
    IsRePrintChanged = false;
    if ($("#radioWaybillNo").attr("checked") == "checked") {
        IsPageRePrint = false;
    }
}

//扫描运单号
function scanFormCode() {
    var formCode = $.trim($("#txtFormCode").val());
    if (formCode == "") {
        clearFormCode();
        return;
    }
    var formType = $("#selFormType option:selected").val();
    if (formType == "0" && (isNaN(formCode) || formCode.length > 14)) {
        alert("运单号不正确!");
        clearFormCode();
        return;
    }
    var trs = $("#tblBillInfo tbody tr");
    if (trs.length > MAXCOUNT) {
        alert("每箱单量不能超过" + MAXCOUNT + "单!");
        clearFormCode();
        return;
    }
    if (posting) {
        alert('您的操作太快了，页面正在提交');
        clearFormCode();
        return;
    }
    var isFirst = true;
    if (trs.length > 0) {
        isFirst = false;
    }
    //判断运单号重复
    var isRepeate = false;
    if (!isFirst) {
        trs.each(function () {
            var code = $.trim($(this).find("td:eq(2)").html());
            if (formType == "1") {
                code = $.trim($(this).find("td:eq(5)").html());
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
        ErrorNotice();
        clearFormCode();
        return;
    }
    var selectedType = $(".sortingCenterSelect:checked").val();
    var stationValue = getStationValue(selectedType);
    if (!isFirst) {
        if (stationValue == "" || stationValue == -1) {
            alert('请选择站点');
            return;
        }
    }
    var boxNo = $("#lblBoxNo").html();
    posting = true;
    var params = {};
    params.hidData = $("#hdInboundInitData").val();
    params.selectedType = selectedType;
    params.selectStationValue = stationValue;
    params.code = formCode;
    params.boxNo = boxNo;
    params.isFirst = isFirst;
    params.formType = formType;
    $.ajax({
        type: "POST",
        url: "ScanFormCode",
        data: params,
        dataType: 'json',
        success: function (result) {
            if (result == null) {
                alert("服务器无响应");
                return;
            }
            if (result.IsSuccess) {
                if (result.ExtendedObj.IsReprint) {
                    $("#radioBoxNo").attr("checked", "checked");
                    $("#radioBoxNo").change();
                    IsPageRePrint = true;
                    $("#txtFormCode").val("");
                    $("#tblBillInfo tbody").empty();
                    for (var i = 0; i < result.ExtendedObj.List.length; i++) {
                        var html = $("#clientTemplate").tmpl(result.ExtendedObj.List[i]);
                        $("#tblBillInfo tbody").append(html);
                    }
                    $("#txtWeight").val(result.ExtendedObj.Box.Weight);
                    $("#txtWeight").attr("disabled", "disabled");
                    IsRePrintChanged = false;
                    $("#lblBoxNo").html(result.ExtendedObj.Box.BoxNo);
                    SucceessNotice();
                    selectBoxToStation(result.ExtendedObj.Box);
                }
                else {
                    var findObj = $("#tblBillInfo #tr" + result.ExtendedObj.Bill.FormCode);
                    if (findObj.length > 0) {
                        $("#tblBillInfo tbody tr").removeClass("cur");
                        findObj.addClass("cur");
                    }
                    else {
                        var html = $("#clientTemplate").tmpl(result.ExtendedObj.Bill);
                        $("#tblBillInfo tbody").prepend(html);
                        $("#tblBillInfo tbody tr").removeClass("cur");
                        $("#tblBillInfo tbody tr:eq(0)").addClass("cur");
                        SucceessNotice();
                        if (result.Message != "" && result.Message != undefined) {
                            alert(result.Message);
                        }
                    }
                    clearFormCode();
                    if (result.ExtendedObj.Count == -1) {
                        getCount();
                    } else {
                        $("span#InboundCount").html(result.ExtendedObj.Count);
                    }
                    $("#lblBoxNo").html(result.ExtendedObj.BoxNo);
                    $("#txtWeight").removeAttr("disabled");
                    if (IsPageRePrint) {
                        IsRePrintChanged = true;
                        $("#txtWeight").val("");
                    }
                }
                rebuildIndex();
            }
            else {
                ErrorNotice();
                alert(result.Message);                
                clearFormCode();
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert('操作超时，服务器无响应' + textStatus + errorThrown);
        },
        complete: function (XHR, TS) {
            posting = false;
            XHR = null;
            $("#Loading").hide();
        }
    });
}

//扫描箱号
function scanBoxNo() {
    var boxNo = $.trim($("#txtBoxNo").val());
    if (boxNo == "") {
        clearBoxNo();
        return;
    }
    if (posting) {
        alert('您的操作太快了，页面正在提交');
        return;
    }
    var params = {};
    params.hidData = $("#hdInboundInitData").val();
    params.boxNo = boxNo;
    posting = true;
    $.ajax({
        type: "POST",
        url: "ScanBoxNo",
        data: params,
        dataType: 'json',
        success: function (result) {
            if (result == null) {
                alert("服务器无响应");
                return;
            }
            if (result.IsSuccess) {
                reloadData();
                $("#tblBillInfo tbody").empty();
                for (var i = 0; i < result.ExtendedObj.List.length; i++) {
                    var html = $("#clientTemplate").tmpl(result.ExtendedObj.List[i]);
                    $("#tblBillInfo tbody").append(html);
                }
                SucceessNotice();
                $("#txtWeight").val(result.ExtendedObj.Box.Weight);
                $("#lblBoxNo").html(boxNo);
                rebuildIndex();
                $("#txtWeight").attr("disabled", "disabled");
                IsPageRePrint = true;
                IsRePrintChanged = false;
                selectBoxToStation(result.ExtendedObj.Box);
            }
            else {
                alert(result.Message);
                clearBoxNo();
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert('操作超时，服务器无响应' + textStatus + errorThrown);
        },
        complete: function (XHR, TS) {
            posting = false;
            XHR = null;
            $("#Loading").hide();
        }
    });
}

function rebuildIndex() {
    var rows = $("#tblBillInfo tbody tr");
    $("#lblCount").html(rows.length);
    rows.each(function (i) {
        $(this).find("td:eq(1)").html(rows.length - i);
    });
}

function clearFormCode() {
    $("#txtFormCode").val("");
    $("#txtFormCode").focus();
}

function clearBoxNo() {
    $("#txtBoxNo").val("");
    $("#txtBoxNo").focus();
}

function printBox() {
    //如果是补打印，并且没有改变，则直接补打印，不修改数据
    if (IsPageRePrint && !IsRePrintChanged) {
        showPrintInfo();
        return;
    }
    var trs = $("#tblBillInfo tbody tr");
    var isAllDelete = false;
    if (IsPageRePrint && trs.length == 0) {
        isAllDelete = true;
    }
    //如果全部删除，则不判断重量
    var weight = $("#txtWeight").val();
    if (!isAllDelete) {
        var reg = /^[0-9]+(\.[0-9]{1,3})?$/;
        if (!reg.test(weight)) {
            alert("请输入合法重量！");
            $("#txtWeight").select();
            return;
        }
        if (weight <= 0) {
            alert("重量应该大于0！");
            $("#txtWeight").select();
            return;
        }
    }
    if (weight == "") {
        weight = 0;
    }
    if (!IsPageRePrint && trs.length < 1) {
        alert("暂无数据，无法打印！");
        return;
    }
    var selectedType = $(".sortingCenterSelect:checked").val();
    var stationValue = getStationValue(selectedType);
    if (stationValue == "" || stationValue == -1) {
        alert('请选择站点');
        return;
    }
    var formCodes = getFormCodes();
    if (posting) {
        alert('您的操作太快了，页面正在提交');
        return;
    }
    var boxNo = $("#lblBoxNo").html();
    posting = true;
    var params = {};
    params.hidData = $("#hdInboundInitData").val();
    params.boxNo = boxNo;
    params.weight = weight;
    params.formCodes = formCodes;
    params.selectStationValue = stationValue;
    params.selectedType = selectedType;
    params.isUpdate = IsPageRePrint;
    $.ajax({
        type: "POST",
        url: "Packing",
        data: params,
        dataType: 'json',
        success: function (result) {
            if (result == null) {
                alert("服务器无响应");
                return;
            }
            if (result.IsSuccess) {
                showPrintInfo();
                reloadData();
                if ($("#radioWaybillNo").attr("checked") == "checked") {
                    IsPageRePrint = false;
                }
            }
            else {
                alert(result.Message);
            }
            clearFormCode();
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert('操作超时，服务器无响应' + textStatus + errorThrown);
        },
        complete: function (XHR, TS) {
            posting = false;
            XHR = null;
            $("#Loading").hide();
        }
    });
}

function getStationValue(selectedType) {
    if (selectedType == 0) { return $("#selCityAndStation_Station option:selected").val(); }
    else if (selectedType == 2) { return $("#SortingCenterList option:selected").val(); }
    else if (selectedType == 3) { return $("#DistributorList option:selected").val(); }
    return "";
}

//显示打印信息
function showPrintInfo() {
    var boxNo = $("#lblBoxNo").html();
    if ($.trim(boxNo) == "") {
        ymPrompt.errorInfo({ message: "没有可打印的箱子!" });
        return;
    }
    openWindow('ShowPackingPrintInfo?boxNo=' + boxNo, 'PackingPrint', 800, 600);
}

function getFormCodes() {
    var formCodes = "";
    $("#tblBillInfo tbody tr").each(function () {
        formCodes += $.trim($(this).find("td:eq(2)").html()) + ",";
    });
    return formCodes.substr(0, formCodes.length - 1);
}

function selectBoxToStation(box) {
    if (box.InboundType == "0") {
        //站点
        $("#rdoStation").attr("checked", "checked");
        $("#SortingCenterList").val(-1);
        $("#DistributorList").val(-1);
        $("#SortingCenterList").attr("disabled", "disabled");
        $("#DistributorList").attr("disabled", "disabled");
        $("#selCityAndStation_City").removeAttr("disabled");
        $("#selCityAndStation_Station").removeAttr("disabled");
        $("#selCityAndStation_City").val(-1);
        var option = "<option value='-1'>--请选择--</option><option value='" + box.ArrivalID + "'>" + box.ArrivalCompanyName + "</option>";
        $("#selCityAndStation_Station").html(option);
        $("#selCityAndStation_Station").val(box.ArrivalID);
    } else if (box.InboundType == "2") {
        //分拣中心
        $("#rdoSortingCenter").attr("checked", "checked");
        $("#selCityAndStation_City").val(-1);
        $("#selCityAndStation_Station").html("<option value='-1'>请选择</option>");
        $("#selCityAndStation_Station").val(-1);
        $("#DistributorList").val(-1);
        $("#selCityAndStation_City").attr("disabled", "disabled");
        $("#selCityAndStation_Station").attr("disabled", "disabled");
        $("#DistributorList").attr("disabled", "disabled");
        $("#SortingCenterList").removeAttr("disabled");
        $("#SortingCenterList").val(box.EncryptValue);
    }
    else if (box.InboundType == "3") {
        //配送商
        $("#rdoDistribution").attr("checked", "checked");
        $("#selCityAndStation_City").val(-1);
        $("#selCityAndStation_Station").html("<option value='-1'>请选择</option>");
        $("#selCityAndStation_Station").val(-1);
        $("#SortingCenterList").val(-1);
        $("#selCityAndStation_City").attr("disabled", "disabled");
        $("#selCityAndStation_Station").attr("disabled", "disabled");
        $("#SortingCenterList").attr("disabled", "disabled");
        $("#DistributorList").removeAttr("disabled");
        $("#DistributorList").val(box.EncryptValue);
    }
    $("span#InboundCount").html(0);
    getCount();
}