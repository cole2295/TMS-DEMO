$(function () {
    getWeight();
    //扫描订单号事件
    KeyPressDown(txtBillNo);
    //扫描标签事件
    KeyPressDown(txtLabel);
    //扫描封箱贴事件
    KeyPressDown(txtBoxLabel);
});

//扫描数据
function scanCode(id) {
    var codeType = getCodeType(id);
    if (codeType == 2) {
    }
    else {
        scanFormCode(codeType);
    }
    
}

//扫描订单号
function scanFormCode(codeType) {
    var trs = $("#tblBillInfo tbody tr");

    var weightValid = true;
    var weight = $("#txtBillWeight").val();
    if (isNaN(weight) || weight == "") {
        weight = 0;
    }
    var FormCode = "";
    if (codeType == 0) {
        FormCode = $.trim($("#txtBillNo").val());
        if (FormCode == "" || (isNaN(FormCode) || FormCode.length > 14)) {
            alert("运单号不正确!");
            GetFocus(txtBillNo);
            return;
        }
    }
    else {
        FormCode = $.trim($("#txtLabel").val())
    }

    var params = {};
    params.FormCode = FormCode;
    params.weight = weight;
    params.weightValid = weightValid;
    params.codeType = codeType;
    $.ajax({
        url: "/Sorting/Return/ScanForm",
        data: params,
        dataType: 'json',
        type: 'POST',
        success: function (r) {
            if (r.IsSuccess) {
                if (r.ExtendedObj.BillReturnInfo != null) {
                    var html = $("#tmpSimpleList").tmpl(r.ExtendedObj.BillReturnInfo);
                    $("#tblBillInfo tbody").append(html);
                    $("#lblCount").html(trs.length + 1);
                }
            }
            else {
                if (r.ResultAlertType == 0) {
                    alert(r.Message);
                }
                if (r.ResultAlertType == 2) {
                    if (confirm(r.Message)) {
                        GetReturnInfo(0, r.ExtendedObj.Model);
                    }
                }
            }
        }
    });

}
//扫描封箱贴
function ScanningBoxLabel() {
    var params = {};
    params.ValidBox = true;
    params.boxLabelNo = $.trim($("#txtBoxLabel").val());
    jPrompt('请输入箱号:', '', '周转箱', function (boxNo) {
        if (boxNo) {
            params.boxNo = boxNo;
            $.ajax({
                url: "/Sorting/Return/ScanningBoxLabel",
                data: params,
                dataType: 'json',
                type: 'POST',
                success: function (r) {
                    if (r.IsSuccess) {

                    }
                } 
            });
        }
        else {
            alert("箱号不能为空！");
        }
    });
}

//扫描事件
function KeyPressDown(id) {
    $(id).keydown(function (e) {
        if (e.which != 13) {
            return;
        }
        scanCode(id);
    });
}

//获取扫描类型
function getCodeType(id) {
    if (id.id == "txtBillNo") return 0;
    if (id.id == "txtLabel") return 1;
    if (id.id == "txtBoxLabel") return 2;
}

//清空扫描运单文本框并获取焦点
function GetFocus(id) {
    $(id).val("");
    $(id).focus();
}
function GetReturnInfo(CodeType, model) {
    var trs = $("#tblBillInfo tbody tr");
    var params = {};
    params.model = model;
    params.codeType = CodeType;
    $.ajax({
        url: "/Sorting/Return/ReturnInbound",
        data: params,
        dataType: 'json',
        type: 'POST',
        success: function (r) {
            if (r.IsSuccess) {
                if (result.ExtendedObj.BillReturnInfo != null) {
                    var html = $("#tmpSimpleList").tmpl(result.ExtendedObj.BillReturnInfo);
                    $("#tblBillInfo tbody").append(html);
                    $("#lblCount").html(trs.length + 1);
                }
            }
            else {
                alert(r.Message);
            }
        }
    });
}
var ajaxGetWeightHandle = null;
//从本地wcf获取重量
function getWeight() {
    //if (!CANWEIGH) return;
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
            if (data.Weight <= 0) return;
            $("#txtBillWeight").val(data.Weight);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            //  $("#txtBillWeight").val("");
        }
    });
    ajaxGetWeightHandle = window.setTimeout(getWeight, delay);
    //   CollectGarbage();
}