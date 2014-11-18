var posting = false;        //是否逐单提交中
var arrFormCode = [];   //空数组

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
    $("#batchInbound").click(function () {
        batchscanFormCode();
    });
    $("#txtFormCodeList").blur(function () {
        // ----------/\s+/ 匹配任何空白符，包括\n,\r,\f,\t,\v等（换行、回车、空格、tab等）
        if ($.trim($(this).val()) != "") {
            var reg = /^[A-Za-z0-9\s]+$/;
            arrFormCode = $(this).val().split(/\s+/);
            $("span#TotalInputCount").html(arrFormCode.length);
            return;
        }
        $("span#TotalInputCount").html(0);
        $("span#OperateErrorCount").html(0);
        $("span#OperateSucceedCount").html(0);
        arrFormCode = [];
    });
});

function reloadData() {
    $("#txtFormCodeList").val("");
    $("#tblErrorBillInfo tbody").empty();
    $("span#CurrentOpCount").html(0);
    $("span#TotalInputCount").html(0);
    $("span#OperateSucceedCount").html(0);
    $("span#OperateErrorCount").html(0);
}

function getStationValue(selectedType) {
    if (selectedType == 0) { return $("#selCityAndStation_Station option:selected").val(); }
    else if (selectedType == 2) { return $("#SortingCenterList option:selected").val(); }
    else if (selectedType == 3) { return $("#DistributorList option:selected").val(); }
    return "";
}

//取得入库数量
function getCount() {
    var selectedType = $(".sortingCenterSelect:checked").val();
    var stationValue = getStationValue(selectedType);
    if (stationValue == undefined
     || stationValue == ""
     || stationValue == -1) {
        $("span#CurrentOpCount").html(0);
        return;
    }
    var params = {};
    params.hidData = $("#hdInitData").val();
    params.selectedType = selectedType;
    params.selectStationValue = stationValue;
    $.ajax({
        url: "GetCount",
        data: params,
        dataType: 'json',
        success: function (result) {
            if (result == null || result == undefined) {
                ymPrompt.errorInfo({ message: '调用读取入库数量服务失败[返回result=null].' });
                return;
            }
            if (result.IsSuccess) {
                $("span#CurrentOpCount").html(result.InboundCount);
            }
            else {
                ymPrompt.errorInfo({ message: result.Message });
                return;
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            ymPrompt.errorInfo({ message: '调用读取入库数量服务失败' + textStatus + errorThrown });
        }
    });
}
function clearFormCode() {
    $("#txtFormCodeList").val("");
    $("#txtFormCodeList").focus();
}
//批量处理运单号
function batchscanFormCode() {
    if (arrFormCode.length <= 0) {
        ymPrompt.errorInfo({ message: '请输入需要入库的单号!' });
        clearFormCode();
        return;
    }
    if (posting) {
        ymPrompt.alert({ title: '提示', message: '您的操作太快了，页面正在提交，可能网速较慢，请稍后..' });
        clearFormCode();
        return;
    }
    var formType = $("#selFormType option:selected").val();
    var selectedType = $(".sortingCenterSelect:checked").val();
    var stationValue = getStationValue(selectedType);
    if (stationValue == "" || stationValue == -1) {
        ymPrompt.errorInfo({ title: '提示', message: '请选择站点' });
        clearFormCode();
        return;
    }
    posting = true;
    var params = {};
    params.hidData = $("#hdInitData").val();
    params.selectedType = selectedType;
    params.selectStationValue = stationValue;
    params.code = arrFormCode.join(";");
    params.formType = formType;
    ymPrompt.confirmInfo({
        title: '询问',
        message: '批量入库可能需要几分钟,请耐心等待......请确认？',
        handler: function (tp) {
            if (tp == 'ok') {
                $.ajax({
                    type: "POST",
                    url: "BatchScanFormCode",
                    data: params,
                    dataType: 'json',
                    beforeSend: function (xhr) {
                        $("#Loading").show();
                    },
                    success: function (result) {
                        posting = false;
                        if (result == null || result == undefined) {
                            ymPrompt.errorInfo({ message: '调用入库服务失败[返回result=null].' });
                            return;
                        }
                        if (result.IsSuccess) {
                            $("#tblErrorBillInfo tbody").empty();
                            $("#txtFormCodeList").val("");
                            $("span#CurrentOpCount").html(result.InboundCount);
                            $("span#OperateSucceedCount").html(result.SucceedCount);
                            $("span#OperateErrorCount").html(result.FailedCount);
                            if (result.ListErrorBill != null && result.ListErrorBill != undefined) {
                                for (var i = 0; i < result.ListErrorBill.length; i++) {
                                    var html = $("#tmpErrorList").tmpl(result.ListErrorBill[i]);
                                    $("#tblErrorBillInfo tbody").append(html);
                                }
                            };
                            if (result.Message != "" && result.Message != undefined) {
                                ymPrompt.succeedInfo({ message: result.Message });
                            }
                        }
                        else {
                            ymPrompt.errorInfo({ message: result.Message });
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
                }); //end ajax
            } //end if
        } // end hander
    });      //end ymPrompt.confirmInfo

}
