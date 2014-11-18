var posting = false;        //是否逐单提交中
var MAXSHOWCOUNT = 10; //列表最大显示数量

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
        }
        scanFormCode();
    });
 });

function reloadData() {
    $("#tblBillInfo tbody").empty();
    $("#txtFormCode").val("");
    $("span#CurrentOpCount").html(0);
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
    if (posting) {
        alert("您的操作太快了，页面正在提交，可能网速较慢，请稍后..");
        clearFormCode();
        return;
    }
    var selectedType = $(".sortingCenterSelect:checked").val();
    var stationValue = getStationValue(selectedType);
    if (stationValue == "" || stationValue == -1) {
        //ymPrompt.errorInfo({ title: '提示', message: '请选择站点' });
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
    $.ajax({
        url: "ScanFormCode",
        data: params,
        dataType: 'json',
        beforeSend: function (xhr) {
            $("#Loading").show();
        },
        success: function (result) {
            if (result == null || result == undefined) {
                ymPrompt.errorInfo({ message: '调用入库服务失败[返回result=null].' });
                return;
            }
            if (result.IsSuccess) {
                var html = $("#tmpSimpleList").tmpl(result);
                $("#tblBillInfo tbody").prepend(html);
                $("#tblBillInfo tbody tr").removeClass("cur");
                $("#tblBillInfo tbody tr:eq(0)").addClass("cur");
                $("span#CurrentOpCount").html(result.InboundCount);
                if (MAXSHOWCOUNT < $("#tblBillInfo tbody tr").length) {
                    $("#tblBillInfo tbody tr").last().remove();
                }
                clearFormCode();
                //SucceessNotice();
                playSound("succ");
                if (result.Message != "" && result.Message != undefined) {
                    //ymPrompt.succeedInfo({ message: result.Message });
                    alert(result.Message);
                }
                if (result.PromptMessage != "" && result.PromptMessage != undefined) {
                    $("#ShowTimeConsuming").html("<span style='color:#000;'>" + result.PromptMessage + "</span>");
                }
            }
            else {
                // ErrorNotice();
                playSound("error");
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
