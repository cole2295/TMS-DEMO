
$(function () {
    //单选change事件
    $('.sortingCenterSelect').change(function () {
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
});

function getStationValue(selectedType) {
    if (selectedType == 0) { return $("#selCityAndStation_Station option:selected").val(); }
    else if (selectedType == 2) { return $("#SortingCenterList option:selected").val(); }
    else if (selectedType == 3) { return $("#DistributorList option:selected").val(); }
    return "";
}

function ValidateSearchCondition() {
    if ($("#txtInboundStartTime").val() == "") {
        ymPrompt.errorInfo({ message: '入库开始时间必须输入' });
        return false;
    }
    if ($("#txtInboundEndTime").val() == "") {
        ymPrompt.errorInfo({ message: '入库结束时间必须输入' });
        return false;
    }
    var selectedType = $(".sortingCenterSelect:checked").val();
    var stationValue = getStationValue(selectedType);
    if (stationValue == undefined
     || stationValue == ""
     || stationValue == -1) {
        ymPrompt.errorInfo({ message: '请选择出库目的地' });
        return false;
    }
    return true;
}
//出库OP
function SearchOutbound() {
    var selectedType = $(".sortingCenterSelect:checked").val();
    var stationValue = getStationValue(selectedType);
    if (stationValue == undefined
     || stationValue == ""
     || stationValue == -1) {
        ymPrompt.errorInfo({ message: '请选择出库目的地' });
        return;
    }
    var obj = TMS.Page.CheckDataTableChecked(false);
    if (!obj) {
        ymPrompt.errorInfo({ message: '请选择要出库的运单' });
        return;
    }
    if (obj.length == 0) {
        ymPrompt.errorInfo({ message: "请至少选择一项进行操作！" });
        return;
    }
    var FormCodes = "";
    obj.each(function () {
        FormCodes += $(this).attr("FormCode") + ",";
    });
    FormCodes = FormCodes.substr(0, FormCodes.length - 1);
    var params = {};
    params.selectedType = selectedType;
    params.selectStationValue = stationValue;
    params.arrCodes = FormCodes;
    ymPrompt.confirmInfo({
        title: '询问',
        message: '你确定要出库选择的 ' + obj.length + ' 项？',
        handler: function (tp) {
            if (tp == 'ok') {
                $.ajax({
                    type: "POST",
                    url: "SortCenter_Search_Outbound",
                    data: params,
                    dataType: 'json',
                    beforeSend: function (xhr) {
                        $("#Loading").show();
                    },
                    success: function (result) {
                        if (result == null || result == undefined) {
                            ymPrompt.errorInfo({ message: '调用出库接口失败[返回result=null].' });
                            return;
                        }
                        if (result.IsSuccess) {
                            if (result.Message != "" && result.Message != undefined) {
                                ymPrompt.succeedInfo({ message: result.Message });
                            }
                        }
                        else {
                            ymPrompt.errorInfo({ message: result.Message });
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        $("#Loading").hide();
                        ymPrompt.errorInfo({ message: '操作超时，服务器无响应' + textStatus + errorThrown });
                    },
                    complete: function (XHR, TS) {
                        XHR = null;
                        $("#Loading").hide();
                    }
                }); //end ajax
            } //end if
        } // end hander
    });
}