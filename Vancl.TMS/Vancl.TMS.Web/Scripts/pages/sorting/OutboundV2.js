var posting = false;        //是否逐单提交中
var arrFormCode = [];   //空数组
var MAXSHOWCOUNT = 10; //列表最大显示数量

var Index = 0;
var IsInterrupt = false;
var OperateErrorCount = 0;
var OperateSucceedCount = 0;

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
    //    $("#txtFormCodeList").blur(function () {
    //        // ----------/\s+/ 匹配任何空白符，包括\n,\r,\f,\t,\v等（换行、回车、空格、tab等）
    //        if ($.trim($(this).val()) != "") {
    //            var reg = /^[A-Za-z0-9\s]+$/;
    //            arrFormCode = $(this).val().split(/\s+/);
    //            $("span#TotalInputCount").html(arrFormCode.length);
    //            return;
    //        }
    //        $("span#TotalInputCount").html(0);
    //        $("span#OperateErrorCount").html(0);
    //        $("span#OperateSucceedCount").html(0);
    //        arrFormCode = [];
    //    });
    $("#txtFormCodeList").keydown(function (e) {
        if (e.which != 13) {
            return;
        } else {
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
        }
    });
    //批量出库
    $("#btnStartOutbound").click(function () {
        if ($(this).val() == "中断出库" || $(this).val() == "继续出库") {
            return ContinueOutbound();
        } else {
            return BatchOutboundList();
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
    $("span#TotalInputCount").html(0);
    $("span#OperateSucceedCount").html(0);
    $("span#OperateErrorCount").html(0);
    Index = 0;
    OperateErrorCount = 0;
    OperateSucceedCount = 0;
    
} 
function clearFormCode() {
    $("#txtFormCodeList").val("");
    $("#txtFormCodeList").focus();
}

function getStationValue(selectedType) {
    if (selectedType == 0) { return $("#selCityAndStation_Station option:selected").val(); }
    else if (selectedType == 2) { return $("#SortingCenterList option:selected").val(); }
    else if (selectedType == 3) { return $("#DistributorList option:selected").val(); }
    return "";
}

//获取出库总量和出库到当前目的地数量
function getCount() {
    var selectedType = $(".sortingCenterSelect:checked").val();
    var stationValue = getStationValue(selectedType);
    if (stationValue == "" || stationValue == -1) {
        //alert("请选择站点");
        //clearFormCode();
        //$("span#BatchNo").val("");
        $("span#CurrentDisCount").val("0");
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

function ContinueOutbound() {
     
    if (!IsInterrupt) {
        $("#btnStartOutbound").val("继续出库");
        IsInterrupt = true;
    } else {
        IsInterrupt = false;
        $("#btnStartOutbound").val("中断出库");
        arrFormCode = [];
        var txt = $("#txtFormCodeList").val().trim();
        arrFormCode = txt.split(/\s+/);
        if (arrFormCode.length == 0) {
            return ;
        }
        $("#CurStatus").html("正在出库");
        Index = 0;
        lockCodeInput();
        var formType = $("#selFormType option:selected").val();
        if (formType == "0") {
            $("#tblBillInfo").show();
            $("#tblBoxInfo").hide();
            $("[name='UnitName']").html("单");
            BatchOutBoundByCode($.trim(arrFormCode[0]));
        } else {
            $("#tblBillInfo").hide();
            $("#tblBoxInfo").show();
            $("[name='UnitName']").html("箱");
            BatchOutBoundByBox($.trim(arrFormCode[0]));

        }
    }
}

//批量出库--逐单
function BatchOutboundList() {
    
    reloadData();
    var selectedType = $(".sortingCenterSelect:checked").val();
    var stationValue = getStationValue(selectedType);
    if (stationValue == undefined
     || stationValue == ""
     || stationValue == -1) {
        ymPrompt.errorInfo({ message: '请选择出库目的地' });
        return false;
    }
    var txt = $("#txtFormCodeList").val().trim();
    if (txt == "") {
        ymPrompt.errorInfo({ message: '没有要出库的数据' });
        return false;
    }
    arrFormCode = txt.split(/\s+/);
    if (arrFormCode.length == 0) {
        ymPrompt.errorInfo({ message: '没有要出库的数据' });
        return false;
    }
    if (arrFormCode.length > 100) {
        ymPrompt.errorInfo({ message: '单量超过100单！' });
        return false;
    }
    $("span#CurStatus").html("正在出库...");
    $("#btnStartOutbound").val("中断出库");
    $("span#TotalInputCount").html(arrFormCode.length);
    lockCodeInput();
    var formType = $("#selFormType option:selected").val();
    //批量逐单出库
    if (formType == "0") {
        $("#tblBillInfo").show();
        $("#tblBoxInfo").hide();
        $("[name='UnitName']").html("单");
        BatchOutBoundByCode($.trim(arrFormCode[0]));
    }
    //批量逐箱出库
    else {
        $("#tblBillInfo").hide();
        $("#tblBoxInfo").show();
        $("[name='UnitName']").html("箱");
        BatchOutBoundByBox($.trim(arrFormCode[0]));
    }
    return false;
}

function BatchOutBoundByCode(formCode) {
    var selectedType = $(".sortingCenterSelect:checked").val();
    var stationValue = getStationValue(selectedType);
    var params = {};
    params.selectedType = selectedType;
    params.selectStationValue = stationValue;
    params.code = formCode;
    var url = "/Sorting/OutboundV2/BatchOutboundByCode";
    //alert(formCode);
    $.ajax({
        url: url,
        type: 'post',
        dataType: 'json',
        data: params,
        success: function (result) {
            if (result == null || result == undefined) {
                ymPrompt.errorInfo({ message: '调用出库接口失败[返回result=null].' });
                return;
            }
            //执行成功
            if (result.IsSuccess) {
                //alert("succ");
                OperateSucceedCount += 1;
                $("span#OperateSucceedCount").html(OperateSucceedCount);
                var html = $("#tmpBatchList").tmpl(result);
                $("#tblBillInfo tbody").prepend(html);
                $("#tblBillInfo tbody tr").removeClass("cur");
                $("#tblBillInfo tbody tr:eq(0)").addClass("cur");
                $("span#CurrentOpCount").html(result.CurOptCount);
                $("span#CurrentDisCount").html(result.CurArrivalCount);
                if (MAXSHOWCOUNT < $("#tblBillInfo tbody tr").length) {
                    $("#tblBillInfo tbody tr").last().remove();
                }
            } else {
                //输入单号的格式不正确
                //alert("err");
                OperateErrorCount += 1;
                $("span#OperateErrorCount").html(OperateErrorCount);
                var html = $("#tmpBatchList").tmpl(result);
                $("#tblBillInfo tbody").prepend(html);
                $("#tblBillInfo tbody tr").removeClass("cur");
                $("#tblBillInfo tbody tr:eq(0)").addClass("cur");
                $("#tblBillInfo tbody tr:eq(0)").css({ "color": "red" });
                $("span#CurrentOpCount").html(result.CurOptCount);
                $("span#CurrentDisCount").html(result.CurArrivalCount);
                if (MAXSHOWCOUNT < $("#tblBillInfo tbody tr").length) {
                    $("#tblBillInfo tbody tr").last().remove();

                }
            }

            var txt = $("#txtFormCodeList").val();
            var list = txt.split("\n");
            var newTxt = txt.replace(list[0], "");
            newTxt = newTxt.trim("\n");
            $("#txtFormCodeList").val(newTxt);
            Index++;
            var t = setTimeout(function () {
                if (!IsInterrupt) {
                    if (Index <= arrFormCode.length - 1) {
                        BatchOutBoundByCode($.trim(arrFormCode[Index]));
                    } else {
                        clearTimeout(t);
                        unlockCodeInput();
                        $("#CurStatus").html("录入停止");
                        $("#btnStartOutbound").val("批量出库");
                    }
                } else {
                    clearTimeout(t);
                    unlockCodeInput();
                    $("#CurStatus").html("录入中断");
                    return;
                }

            }, 1000);
        }
    });
}

function BatchOutBoundByBox(formCode) {
    //alert(formCode);
    var selectedType = $(".sortingCenterSelect:checked").val();
    var stationValue = getStationValue(selectedType);
    var params = {};
    params.selectedType = selectedType;
    params.selectStationValue = stationValue;
    params.boxNo = formCode;
    var url = "/Sorting/OutboundV2/BoxOutBoundSimple";
    $.ajax({
        url: url,
        type: 'post',
        dataType: 'json',
        data: params,
        success: function (result) {
                if (result == null || result == undefined) {
                    ymPrompt.errorInfo({ message: '调用出库接口失败[返回result=null].' });
                    return;
                }
                //执行成功
                if (result.IsSuccess) {
                    //alert("success");
                    OperateSucceedCount += 1;
                    $("span#OperateSucceedCount").html(OperateSucceedCount);
                    var html = $("#tmpBoxList").tmpl(result);
                    $("#tblBillInfo tbody").prepend(html);
                    $("#tblBillInfo tbody tr").removeClass("cur");
                    $("#tblBillInfo tbody tr:eq(0)").addClass("cur");
                    $("span#CurrentOpCount").html(result.CurOptCount);
                    $("span#CurrentDisCount").html(result.CurArrivalCount);
                    //$("span#BatchNo").html(result.BatchNo);
                    if (MAXSHOWCOUNT < $("#tblBillInfo tbody tr").length) {
                        $("#tblBillInfo tbody tr").last().remove();
                    }
                } else {
                    //输入单号的格式不正确
                    //alert("false");
                    OperateErrorCount += 1;
                    $("span#OperateErrorCount").html(OperateErrorCount);
                    var html = $("#tmpBoxList").tmpl(result);
                    $("#tblBoxInfo tbody").prepend(html);
                    $("#tblBoxInfo tbody tr").removeClass("cur");
                    $("#tblBoxInfo tbody tr:eq(0)").addClass("cur");
                    $("#tblBoxInfo tbody tr:eq(0)").css({ "color": "red" });
                    $("span#CurrentOpCount").html(result.CurOptCount);
                    $("span#CurrentDisCount").html(result.CurArrivalCount);
                    if (MAXSHOWCOUNT < $("#tblBillInfo tbody tr").length) {
                        $("#tblBoxInfo tbody tr").last().remove();
                    }
                }
                var txt = $("#txtFormCodeList").val();
                var list = txt.split("\n");
                var newTxt = txt.replace(list[0], "");
                newTxt = newTxt.trim("\n");
                $("#txtFormCodeList").val(newTxt);
                Index++;
                var t = setTimeout(function () {
                    if (!IsInterrupt) {
                        if (Index <= arrFormCode.length - 1) {
                            BatchOutBoundByBox($.trim(arrFormCode[Index]));
                        } else {
                            clearTimeout(t);
                            unlockCodeInput();
                            $("#CurStatus").html("录入停止");
                            $("#btnStartOutbound").val("批量出库");
                        }
                    } else {
                        clearTimeout(t);
                        unlockCodeInput();
                        $("#CurStatus").html("录入中断");
                        return;
                    }

                }, 1000);
        }
    });
}


function lockCodeInput() {
    // $("#btnStartOutbound").attr("disabled", true);
    $("#txtFormCodeList").attr("readonly", "readonly").attr("disabled", "disabled").css("background", "#efefef");
}
function unlockCodeInput() {
    // $("#btnStartOutbound").attr("disabled", false);
    $("#txtFormCodeList").removeAttr("readonly").removeAttr("disabled").css("background", "").focus().select();
}

//***********************************************************//
//批量出库
function BatchOutbound() {
    var formType = $("#selFormType option:selected").val();
    //批量单号出库
    if (formType == "0") {
        $("#tblBillInfo").show();
        $("#tblBoxInfo").hide();
        $("[name='UnitName']").html("单");
        BatchOutBoundByCodes();
    }
    //批量箱号出库
    else {
        $("#tblBillInfo").hide();
        $("#tblBoxInfo").show();
        $("[name='UnitName']").html("箱");
        BatchOutBoundByBoxs();
    }
}
//批量单号出库
function BatchOutBoundByCodes() {
    var selectedType = $(".sortingCenterSelect:checked").val();
    var stationValue = getStationValue(selectedType);
    if (stationValue == undefined
     || stationValue == ""
     || stationValue == -1) {
        ymPrompt.errorInfo({ message: '请选择出库目的地' });
        return;
    }
    if (arrFormCode.length == 0) {
        ymPrompt.errorInfo({ message: "请至少输入一项进行出库操作！" });
        return;
    }
    var FormCodes = "";
    for (var i = 0; i < arrFormCode.length; i++) {
        FormCodes += arrFormCode[i] + ",";
    }
    FormCodes = FormCodes.substr(0, FormCodes.length - 1);
    var params = {};
    params.selectedType = selectedType;
    params.selectStationValue = stationValue;
    params.arrCodes = FormCodes;
    ymPrompt.confirmInfo({
        title: '询问',
        message: '你确定要出库选择的 ' + arrFormCode.length + ' 项？',
        handler: function (tp) {
            if (tp == 'ok') {
                $.ajax({
                    //type: "POST",
                    url: "BatchOutboundByCodes",
                    data: params,
                    dataType: 'json',
                    beforeSend: function (xhr) {
                        $("#Loading").show();
                    },
                    success: function (resultList) {

                        var OperateSucceedCount = 0;
                        var OperateErrorCount = 0;
                        $("span#CurStatus").html("正在出库...");
                        //遍历返回的结果集
                        for (var i = 0; i < resultList.length; i++) {
                            var result = resultList[i];
                            if (result == null || result == undefined) {
                                ymPrompt.errorInfo({ message: '调用出库接口失败[返回result=null].' });
                                return;
                            }
                            //执行成功
                            if (result.IsSuccess) {
                                OperateSucceedCount += 1;
                                var html = $("#tmpBatchList").tmpl(result);
                                $("#tblBillInfo tbody").prepend(html);
                                $("#tblBillInfo tbody tr").removeClass("cur");
                                $("#tblBillInfo tbody tr:eq(0)").addClass("cur");
                                $("span#CurrentOpCount").html(result.CurOptCount);
                                $("span#CurrentDisCount").html(result.CurArrivalCount);
                                if (MAXSHOWCOUNT < $("#tblBillInfo tbody tr").length) {
                                    $("#tblBillInfo tbody tr").last().remove();
                                }
                            } else {
                                //输入单号的格式不正确
                                OperateErrorCount += 1;
                                //alert(OperateErrorCount);
                                //result.FormCode != "" && result.FormCode != null
                                if (true) {
                                    var html = $("#tmpBatchList").tmpl(result);
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
                                } else {
                                    alert("5");
                                    //ErrorNotice();
                                    alert(result.Message);
                                    //clearFormCode();
                                }
                            }
                        }
                        clearFormCode();
                        $("span#CurStatus").html("已停止");
                        $("span#OperateSucceedCount").html(OperateSucceedCount);
                        $("span#OperateErrorCount").html(OperateErrorCount);
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

//批量箱号出库
function BatchOutBoundByBoxs() {
    var selectedType = $(".sortingCenterSelect:checked").val();
    var stationValue = getStationValue(selectedType);
    if (stationValue == undefined
     || stationValue == ""
     || stationValue == -1) {
        ymPrompt.errorInfo({ message: '请选择出库目的地' });
        return;
    }
    if (arrFormCode.length == 0) {
        ymPrompt.errorInfo({ message: "请至少输入一项进行出库操作！" });
        return;
    }
    var FormCodes = "";
    for (var i = 0; i < arrFormCode.length; i++) {
        FormCodes += arrFormCode[i] + ",";
    }
    FormCodes = FormCodes.substr(0, FormCodes.length - 1);
    var params = {};
    params.selectedType = selectedType;
    params.selectStationValue = stationValue;
    //箱号s
    params.arrBoxNos = FormCodes;
    ymPrompt.confirmInfo({
        title: '询问',
        message: '你确定要出库选择的 ' + arrFormCode.length + ' 项？',
        handler: function (tp) {
            if (tp == 'ok') {
                $.ajax({
                    //type: "POST",
                    url: "BoxOutBoundV2",
                    data: params,
                    dataType: 'json',
                    beforeSend: function (xhr) {
                        $("#Loading").show();
                    },
                    success: function (resultList) {

                        var OperateSucceedCount = 0;
                        var OperateErrorCount = 0;
                        $("span#CurStatus").html("正在出库...");
                        //遍历返回的结果集
                        for (var i = 0; i < resultList.length; i++) {
                            var result = resultList[i];
                            if (result == null || result == undefined) {
                                ymPrompt.errorInfo({ message: '调用出库接口失败[返回result=null].' });
                                return;
                            }
                            //执行成功
                            if (result.IsSuccess) {
                                //alert("success");
                                OperateSucceedCount += 1;
                                //alert(OperateSucceedCount);
                                var html = $("#tmpBoxList").tmpl(result);
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
                                if (result.Message != "" && result.Message != undefined) {
                                    alert(result.Message);
                                }
                                playSound("success");
                            } else {
                                //输入单号的格式不正确
                                //alert("false");
                                OperateErrorCount += 1;
                                //alert(OperateErrorCount);
                                //result.FormCode != "" && result.FormCode != null
                                if (true) {
                                    var html = $("#tmpBoxList").tmpl(result);
                                    $("#tblBoxInfo tbody").prepend(html);
                                    $("#tblBoxInfo tbody tr").removeClass("cur");
                                    $("#tblBoxInfo tbody tr:eq(0)").addClass("cur");
                                    $("#tblBoxInfo tbody tr:eq(0)").css({ "color":"red" });
                                    $("span#CurrentOpCount").html(result.CurOptCount);
                                    $("span#CurrentDisCount").html(result.CurArrivalCount);
                                    //$("span#BatchNo").html(result.BatchNo);
                                    if (MAXSHOWCOUNT < $("#tblBillInfo tbody tr").length) {
                                        $("#tblBoxInfo tbody tr").last().remove();
                                    }
                                    //clearFormCode();
                                    //ErrorNotice();
                                    playSound("error");
                                } else {
                                    alert("5");
                                    //ErrorNotice();
                                    alert(result.Message);
                                    //clearFormCode();
                                }
                            }
                        }
                        clearFormCode();
                        //alert("End");
                        $("span#CurStatus").html("已停止");
                        $("span#OperateSucceedCount").html(OperateSucceedCount);
                        $("span#OperateErrorCount").html(OperateErrorCount);
                        //alert("over");
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