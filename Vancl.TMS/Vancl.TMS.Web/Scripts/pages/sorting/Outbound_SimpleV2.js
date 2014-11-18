var posting = false;        //是否逐单提交中
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
        getCount();
    });
    $("#DistributorList").change(function () {
        reloadData();
        getCount();
    });
    $("#SortingCenterList").change(function () {
        reloadData();
        getCount();
    });

    $("#txtFormCode").keydown(function (e) {
        if (e.which != 13) {
            return;
        } else {
            var formType = $("#selFormType option:selected").val();
            //扫描单号出库
            if (formType == "0") {
                $("#tblBillInfo").show();
                $("#tblBoxInfo").hide();
                scanFormCode();
            }
            //扫描箱号出库
            else {
                $("#tblBillInfo").hide();
                $("#tblBoxInfo").show();
                outBoundByBox();
            }
        }
    });

    //流程自定义
    CustomizeFlow();
});

function CustomizeFlow() {
    if ($("#hdFunOutBoundStation").val() != "True") {
        $("#thStation").hide();
        $("#tdStation").hide();
    }
    if ($("#hdFunOutBoundDeliverCenter").val() != "True") {
        $("#thSortingCenter").hide();
        $("#tdSortingCenter").hide();
    }
    if ($("#hdFunOutBoundCompany").val() != "True") {
        $("#thDistribution").hide();
        $("#tdDistribution").hide();
    }
}

function reloadData() {
    $("#tblBillInfo tbody").empty();
    $("#txtFormCode").val("");
    //$("span#CurrentOpCount").html(0);
    //$("span#BatchNo").html("");
    CurrentOpCount = 0;
}

function getStationValue(selectedType) {
    if (selectedType == 0) { return $("#selCityAndStation_Station option:selected").val(); }
    else if (selectedType == 2) { return $("#SortingCenterList option:selected").val(); }
    else if (selectedType == 3) { return $("#DistributorList option:selected").val(); }
    return "";
}

//清空运单号
function clearFormCode() {
    $("#txtFormCode").val("");
    $("#txtFormCode").focus();
}

//获取出库总量和出库到当前目的地数量
function getCount() {
    var selectedType = $(".sortingCenterSelect:checked").val();
    var stationValue = getStationValue(selectedType);
    if (stationValue == "" || stationValue == -1) {
        //alert(stationValue);
        $("span#CurrentDisCount").html(0);
        return;
    }
    var params = {};
    params.selectStationValue = stationValue;
    $.ajax({
        url: "GetCount",
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
                $("span#CurrentOpCount").html(result.CurOptCount);
                $("span#CurrentDisCount").html(result.CurArrivalCount);
            }
            else {

            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            posting = false;
            $("#Loading").hide();
            ymPrompt.errorInfo({ message: '操作超时，服务器无响应' + textStatus + errorThrown });
        },
        complete: function (XHR, TS) {
            XHR = null;
            $("#Loading").hide();
        }
    });
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
    params.BatchNo = "0";
    params.CurrentDisCount = $("span#CurrentDisCount").html();
    $.ajax({
        url: "ScanFormCodeV2",
        type: 'post',
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
                $("span#CurrentOpCount").html(result.CurOptCount);
                $("span#CurrentDisCount").html(result.CurArrivalCount);
                //$("span#BatchNo").html(result.BatchNo);
                if (MAXSHOWCOUNT < $("#tblBillInfo tbody tr").length) {
                    $("#tblBillInfo tbody tr").last().remove();
                }
                clearFormCode();
                //SucceessNotice();
                playSound("success");
                if (result.Message != "" && result.Message != undefined) {
                    alert(result.Message);
                }
            }
            else {
                //输入单号的格式不正确
                if (result.FormCode != "" && result.FormCode != null) {
                    alert(result.Message);
                    var html = $("#tmpSimpleList").tmpl(result);
                    $("#tblBillInfo tbody").prepend(html);
                    $("#tblBillInfo tbody tr").removeClass("cur");
                    $("#tblBillInfo tbody tr:eq(0)").addClass("cur");
                    $("#tblBillInfo tbody tr:eq(0)").css({ "color": "red" });
                    $("span#CurrentOpCount").html(result.CurOptCount);
                    $("span#CurrentDisCount").html(result.CurArrivalCount);
                    //$("span#BatchNo").html(result.BatchNo);
                    if (MAXSHOWCOUNT < $("#tblBillInfo tbody tr").length) {
                        $("#tblBillInfo tbody tr").last().remove();
                    }
                    clearFormCode();
                    //ErrorNotice();
                    playSound("error");
                } else {
                    //ErrorNotice();
                    playSound("error");
                    alert(result.Message);
                    clearFormCode();
                }
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

//按箱出库

function outBoundByBox() {
    var boxNo = $.trim($("#txtFormCode").val());
    if (boxNo == "") {
        clearFormCode();
        return;
    }
    var trs = $("#tblBoxInfo tbody tr");
    if (posting) {
        alert("您的操作太快了，页面正在提交，可能网速较慢，请稍后..");
        clearFormCode();
        return;
    }
    var selectedType = $(".sortingCenterSelect:checked").val();
    var stationValue = getStationValue(selectedType);
    if (stationValue == "" || stationValue == -1 || stationValue == undefined) {
        alert("请选择站点");
        clearFormCode();
        return;
    }
    posting = true;
    var params = {};
    //params.hidData = $("#hdInitData").val();
    params.selectedType = selectedType;
    params.selectStationValue = stationValue;
    params.boxNo = boxNo;
    //params.BatchNo = "0";
    //params.CurrentDisCount = $("span#CurrentDisCount").html();
    $.ajax({
        url: "BoxOutBoundSimple",
        type:"Post",
        data: params,
        dataType: 'json',
        beforeSend: function (xhr) {
            $("#Loading").show();
        },
        success: function (result) {
                //执行成功
                if (result.IsSuccess) {
                    var html = $("#tmpBoxList").tmpl(result);
                    $("#tblBoxInfo tbody").prepend(html);
                    $("#tblBoxInfo tbody tr").removeClass("cur");
                    $("#tblBoxInfo tbody tr:eq(0)").addClass("cur");
                    $("span#CurrentOpCount").html(result.CurOptCount);
                    $("span#CurrentDisCount").html(result.CurArrivalCount);
                    //$("span#BatchNo").html(result.BatchNo);
                    if (MAXSHOWCOUNT < $("#tblBillInfo tbody tr").length) {
                        $("#tblBoxInfo tbody tr").last().remove();
                    }
                    playSound("success");
                    
                } else {
                    //执行失败但是箱号合法
                    if (result.BoxNo != "" && result.BoxNo != null) {
                        alert(result.Message);
                        var html = $("#tmpBoxList").tmpl(result);
                        $("#tblBoxInfo tbody").prepend(html);
                        $("#tblBoxInfo tbody tr").removeClass("cur");
                        $("#tblBoxInfo tbody tr:eq(0)").addClass("cur");
                        $("#tblBoxInfo tbody tr:eq(0)").css({ "color": "red" });
                        $("span#CurrentOpCount").html(result.CurOptCount);
                        $("span#CurrentDisCount").html(result.CurArrivalCount);
                        //$("span#BatchNo").html(result.BatchNo);
                        if (MAXSHOWCOUNT < $("#tblBillInfo tbody tr").length) {
                            $("#tblBillInfo tbody tr").last().remove();
                        }
                        clearFormCode();
                        playSound("error");
                    }
                    //执行失败并且箱号不合法
                    else {
                        alert(result.Message);
                        playSound("error");
                    }
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
};

