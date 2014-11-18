﻿var posting = false;        //是否逐单提交中
var MAXSHOWCOUNT = 10; //列表最大显示数量
var CurrentOpCount = 0;   //当前数量

$(function () {
    //单选change事件
    $('.sortingCenterSelect').change(function () {
        reloadData();
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
    });
    $("#DistributorList").change(function () {
        reloadData();
    });
    $("#SortingCenterList").change(function () {
        reloadData();
    });

    $("#txtFormCode").keydown(function (e) {
        if (e.which != 13) {
            return;
        }
        scanFormCode();
    });
});

function reloadData() {
    $("#tblBillInfo tbody").empty();
    $("#txtFormCode").val("");
    $("span#CurrentOpCount").html(0);
    $("span#BatchNo").html("");
    CurrentOpCount = 0;
}

function getStationValue(selectedType) {
    if (selectedType == 0) { return $("#selCityAndStation_Station option:selected").val(); }
    else if (selectedType == 2) { return $("#SortingCenterList option:selected").val(); }
    else if (selectedType == 3) { return $("#DistributorList option:selected").val(); }
    return "";
}

function clearFormCode() {
    $("#txtFormCode").val("");
    $("#txtFormCode").focus();
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
        alert("运单号不正确");
        clearFormCode();
        return;
    }
    var trs = $("#tblBillInfo tbody tr");
    if (posting) {
        alert("您的操作太快了，页面正在提交，可能网速较慢，请稍后..");
        clearFormCode();
        return;
    }
    var selectedType = $(".sortingCenterSelect:checked").val();
    var stationValue = getStationValue(selectedType);
    if (stationValue == "" || stationValue == -1) {
        alert("请选择站点");
        clearFormCode();
        return;
    }
    posting = true;
    var params = {};
    params.hidData = $("#hdInitData").val();
    params.selectedType = selectedType;
    params.selectStationValue = stationValue;
    params.code = formCode;
    params.formType = formType;
    params.BatchNo = $("span#BatchNo").html();
    $.ajax({
        url: "ScanFormCode",
        data: params,
        dataType: 'json',
        beforeSend: function (xhr) {
            $("#Loading").show();
        },
        success: function (result) {
            if (result == null || result == undefined) {
                ymPrompt.errorInfo({ message: '调用出库服务失败[返回result=null].' });
                return;
            }
            if (result.IsSuccess) {
                var html = $("#tmpSimpleList").tmpl(result);
                $("#tblBillInfo tbody").prepend(html);
                $("#tblBillInfo tbody tr").removeClass("cur");
                $("#tblBillInfo tbody tr:eq(0)").addClass("cur");
                $("span#CurrentOpCount").html(++CurrentOpCount);
                $("span#BatchNo").html(result.BatchNo);
                if (MAXSHOWCOUNT < $("#tblBillInfo tbody tr").length) {
                    $("#tblBillInfo tbody tr").last().remove();
                }
                clearFormCode();
                SucceessNotice();
                if (result.Message != "" && result.Message != undefined) {
                    alert(result.Message);
                }
            }
            else {
                ErrorNotice();
                alert(result.Message);
                clearFormCode();
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            posting = false;
            $("#Loading").hide();
            ymPrompt.errorInfo({ message: '操作超时，服务器无响应' + textStatus + errorThrown });
        },
        complete: function (XHR, TS) {
            posting = false;
            XHR = null;
            $("#Loading").hide();
        }
    });
}
