
var CANPRINT = false;
var CANWEIGH = false;
$(function () {
    $.ajax({
        url: "http://localhost:41941/CfgService/Version?jsoncallback=?",
        dataType: 'jsonp', beforeSend: function () { }, complete: function () { },
        success: function (data) {
            $("#CilentServiceInfo").text("当前打印称重服务版本:" + data);
            $("#btnStartPrint").removeAttr("disabled");
        }
    });
    getWeight();
    

});

var ajaxGetWeightHandle = null;
//保存称重标识，true:当前电子称上有物体，false:当前电子称上无物体
var WeightEmtpyFlag = false;
//从本地wcf获取重量
function getWeight() {
    // alert(CANWEIGH)
    if (!CANWEIGH) return;
    //  delay = parseInt(delay);
    if ($("#txtBillWeight").attr("disabled")) return;
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
            //当前无物体
            if (!WeightEmtpyFlag && data.Weight == 0) {
                return;
            }
            WeightEmtpyFlag = data.Weight > 0;
            if (!WeightEmtpyFlag) $("#txtBillWeight").val("");
            else $("#txtBillWeight").val(data.Weight);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) { }
    });
    ajaxGetWeightHandle = window.setTimeout(getWeight, delay);
}

var BatchPrinter = {
    isLockWeight: false,
    totalCount: 0,
    printCodeList: [],
    status: "stop", //run,pause,stop

    $lbStatus: $("#lbStatus"),
    $lbTotalCount: $("#lbTotalCount"),
    $lbPrintCount: $("#lbPrintCount"),
    $lbPrintFailureCount: $("#lbPrintFailureCount"),

    $sltFormType: $("#sltFormType"),
    $btnLockWeight: $("#btnLockWeight"),
    $txtBillWeight: $("#txtBillWeight"),
    $sltMerchant: $("#sltMerchant"),

    $btnStartPrint: $("#btnStartPrint"),
    $btnStopPrint: $("#btnStopPrint"),

    $chkPause: $("#chkPause"),

    $txtCodeInput: $("#txtCodeInput"),
    $sltColdeList: $("#sltColdeList"),

    //清空所有
    clearAll: function () {
        this.printCodeList = [];
        this.totalCount = 0;
        this.$lbTotalCount.text("0");
        this.$lbPrintCount.text("0");
        this.$lbPrintFailureCount.text("0");
        this.$sltColdeList.html("");
    },
    //输入单号
    inputCode: function () {
        this.clearAll();
        var txt = this.$txtCodeInput.val();
        var list = txt.split("\n");
        var option = "";
        for (var i = 0; i < list.length; i++) {
            var code = $.trim(list[i]);
            if (code == "") continue;
            this.totalCount++;
            option += "<option>" + code + "</option>";
            this.printCodeList.push(code);
        }
        this.$sltColdeList.html(option);
        this.$lbTotalCount.text(this.totalCount);
    },

    _printCheck: function () {
        if (this.$sltFormType.val() == "") {
            alert("请选择单号类型！");
            this.$sltFormType.focus();
            return false;
        }
        if (!this.isLockWeight) {
            alert("请打印前先锁定重量！");
            this.$btnLockWeight.focus();
            return false;
        }
        if (this.$sltMerchant.val() == "") {
            alert("请选择商家！");
            this.$sltMerchant.focus();
            return false;
        }
        if (this.printCodeList.length == 0) {
            alert("请输入单号！");
            this.$txtCodeInput.focus();
            return false;
        }
        if (this.printCodeList.length > 100) {
            alert("一次批量打印不能超过100单！");
            this.$txtCodeInput.focus();
            return false;
        }
        return true;
    },
    _disableInput: function () {
        this.$sltFormType.attr("disabled", "disabled");
        this.$btnLockWeight.attr("disabled", "disabled");
        this.$sltMerchant.attr("disabled", "disabled");
        //    this.$chkPause.attr("disabled", "disabled");
    },
    _enableInput: function () {
        this.$sltFormType.removeAttr("disabled")
        this.$btnLockWeight.removeAttr("disabled");
        this.$sltMerchant.removeAttr("disabled");
        //   this.$chkPause.removeAttr("disabled");
    },
    _printIndex: 0,
    _startPrint: function (index) {
        if (index != "undefined") this._printIndex = index;
        //    debugger;
        var FormType = this.$sltFormType.val();
        var Weight = this.getWeight();
        var MerchantId = this.getMerchantId();
        //打印完成
        if (this._printIndex >= this.printCodeList.length) {
            alert("所有面单已打印完成！");
            this.$btnStopPrint.click();
        }
        var FormCode = this.printCodeList[this._printIndex];
        var self = this;

        if (this.status == "run") {
            this.$sltColdeList.find("option[selected]").removeAttr("selected");
            this.$sltColdeList.find("option:eq(" + this._printIndex + ")").attr("selected", "selected");
            var tpl = $("#PrintItemTemplate").tmpl({ FormCode: FormCode });
            $("#PrintList table").prepend(tpl);
            $.ajax({
                type: "POST",
                beforeSend: function (xhr) { },
                data: { FormType: FormType, FormCode: FormCode, Weight: Weight, MerchantId: MerchantId },
                success: function (rm) {
                    if (rm.IsDealSuccess) {
                        var url = location.protocol + "//" + location.host + "/Sorting/BillPrint/PrintData/" + rm.WaybillNo;
                        $.ajax({
                            url: "http://localhost:41941/PrintService/Print?jsoncallback=?",
                            dataType: 'jsonp',
                            data: { url: url },
                            async: false,
                            success: function (data) {
                                //    debugger;
                                if (!data.IsSuccess) {
                                    rm.Message = data.Message;
                                }
                                self._showPrintResult(data.IsSuccess, rm.WaybillNo, rm.Message);
                                self._startPrint(self._printIndex + 1);
                            },
                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                rm.Message = errorThrown;
                                self._showPrintResult(false, rm.WaybillNo, rm.Message);
                                self._startPrint(self._printIndex + 1);
                            }
                        });
                    } else {
                        self._showPrintResult(false, rm.WaybillNo, rm.Message);
                        self._startPrint(self._printIndex + 1);
                    }
                },
                error: function () {
                    alert("error");
                }
            });
        }
    },
    _showPrintResult: function (isSuccess, waybillNo, message) {
        var self = this;
        var ss = "";
        if (isSuccess) {
            var count = parseInt(self.$lbPrintCount.text());
            self.$lbPrintCount.text(count + 1);
            ss = "打印成功";
        } else {
            var count = parseInt(self.$lbPrintFailureCount.text());
            self.$lbPrintFailureCount.text(count + 1);
            ss = "打印失败";
        }
        $("#PrintList table tr:eq(0) .pStatus").text(ss);
        $("#PrintList table tr:eq(0) .pWaybillNo").text(waybillNo);
        $("#PrintList table tr:eq(0) .pMessage").text(message);
    },
    setPrintStaus: function (status) {
        if (status != "run" && status != "pause" && status != "stop") {
            alert("未识别的状态！"); return;
        }
        this.status = status;
        if (status == "run") {
            this.$lbStatus.text("正在打印");
        } else if (status == "pause") {
            this.$lbStatus.text("暂停打印");
        } else if (status == "stop") {
            this.$lbStatus.text("停止打印");
        }
    },
    getWeight: function () {
        return parseFloat(this.$txtBillWeight.val());
    },
    getMerchantId: function () {
        return parseInt(this.$sltMerchant.val());
    },
    //开始打印
    startPrint: function () {
        if (!this._printCheck()) return;
        this._printIndex = 0;
        this.$btnStartPrint.hide();
        this.$btnStopPrint.show();
        this.$txtCodeInput.hide();
        this.$sltColdeList.show();
        this._disableInput();
        this.setPrintStaus("run");
        $("#PrintList table").empty();
        this._startPrint(0);
    },
    //停止打印
    stopPrint: function () {
        this.$btnStartPrint.show();
        this.$btnStopPrint.hide();
        this.$txtCodeInput.show();
        this.$sltColdeList.hide();
        this._enableInput();
        this.setPrintStaus("stop");
    },
    //锁定重量
    lockWeight: function () {
        var weight = parseFloat(this.$txtBillWeight.val());
        if (isNaN(weight) || weight <= 0) {
            alert("重量不合法！"); return;
        }
        this.$txtBillWeight.attr("disabled", "disabled");
        this.$btnLockWeight.val("取消锁定重量");
        this.isLockWeight = true;
    },
    //取消锁定重量
    unLockWeight: function () {
        this.$txtBillWeight.removeAttr("disabled");
        this.$btnLockWeight.val("锁定重量");
        this.isLockWeight = false;
    },

    //页面初始化
    init: function () {
        var self = this;
        this.$btnStartPrint.click(function () { self.startPrint(); });
        this.$btnStopPrint.click(function () { self.stopPrint(); });
        this.$txtCodeInput.keyup(function () { self.inputCode(); });
        this.$btnLockWeight.click(function () {
            if (self.isLockWeight) self.unLockWeight();
            else self.lockWeight();
        });
    }
};

BatchPrinter.init();

