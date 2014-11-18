//出库打印

$(function () {
    $('.sortingCenterSelect').change(function () {
        $('#trSortingCenterSelect select').each(function () {
            $(this).attr("disabled", "disabled");
        });
        $('#trSortingCenterSelect :checked').parents("th:eq(0)").next()
        .find("select").removeAttr("disabled");
    }).change();

    //  $("#BtnSearch").click();
});

//导出
function exportOutbound() {
    var selTimeType = $("#selTimeType").val();
    if (selTimeType == "0") {
        exportOutboundNoprint();
    } else {
        exportOutboundPrint();
    }
}

//导出--未打印
function exportOutboundNoprint() {
    var list = TMS.Page.GetDataTableChecked();
    if (list.length == 0) {
        alert("请至少选择一个需要导出的内容！"); return;
    }
    var searchArg = $("#hiddenData").val();
    //alert(searchArg);    
    var arrivedList = "";
    list.each(function () {
        arrivedList += $(this).val().split("|")[0] + ",";
    });
    $("#searchArg").val(searchArg);
    $("#arrivedList").val(arrivedList);
    //alert(searchArg); alert(arrivedList);
    $("#FormExportNoPrint").submit();
}
//导出--已打印
function exportOutboundPrint() {
    var list = TMS.Page.GetDataTableChecked();
    if (list.length == 0) {
        alert("请至少选择一个需要导出的批次！"); return;
    }
    var batchNo = "";
    list.each(function () {
        //     batchNo += $(this).val() + ",";
        batchNo += $(this).val().split("|")[1] + ",";
    });
    //    alert(batchNo);
    $("#hidBatchNos").val(batchNo);
    $("#FormExportPrint").submit();
    //  window.location = "Export?batchNos=" + batchNo;
}

//显示交接单
function showReceipt() {
    var selTimeType = $("#selTimeType").val();
    if (selTimeType == "0") {
        showRecepiptNoPrint();
    } else {
        showRecepiptPrint();
    }
}

//显示未打印交接单
function showRecepiptNoPrint() {
    var list = TMS.Page.GetDataTableChecked();
    if (list.length == 0) {
        alert("请至少选择一个项！"); return;
    }
    var searchArg = $("#hiddenData").val();
    //alert(searchArg);    
    var arrivedList = "";
    list.each(function () {
        arrivedList += $(this).val().split("|")[0]  + ",";
    });
    //alert(arrivedList);
    openWindow('PrintReceiptV2?searchArg=' + searchArg + '&arrivedList=' + arrivedList, 'ReceiptV2', 800, 600);
}

//显示已打印交接单
function showRecepiptPrint() {
    var list = TMS.Page.GetDataTableChecked();
    if (list.length == 0) {
        alert("请至少选择一个项！"); return;
    }
    var batchNo = "";
    list.each(function () {
        batchNo += "batchNo=" + $(this).val().split("|")[1] + "&";
    });
    openWindow('PrintReceipt?' + batchNo, 'Receipt', 800, 600);
}

function showBatchDetails(batchNo) {
    openWindow('PrintBatchDetails?batchNo=' + batchNo, 'BatchDetails', 800, 600);
}
//批量发邮件
function sendMail() {
    var list = TMS.Page.GetDataTableChecked();
    if (list.length == 0) {
        alert("请至少选择一个项！"); return;
    }
    var batchNoAndEcId = "";
    list.each(function () {
        batchNoAndEcId += $(this).val() + ",";
    });
    //   alert(batchNoAndEcId);
    $.post("SendEmail",
        { batchInfo: batchNoAndEcId },
         function (data) {
             if (data.IsSuccess) {
                 alert("邮件发送成功！");
             }
             else {
                 alert(data.Message);
             }
         });
}

//批量发送邮件V2
function sendMailV2() {
    var list = TMS.Page.GetDataTableChecked();
    if (list.length == 0) {
        alert("请至少选择一个项！"); return;
    }
    var selTimeType = $("#selTimeType").val();
    var searchArg = $("#hiddenData").val();
    var batchNoAndEcId = "";
    list.each(function () {
        batchNoAndEcId += $(this).val() + ",";
    });
    //alert(selTimeType + selTimeType + batchNoAndEcId);
    $.post("SendEmailV2", 
        { batchInfo: batchNoAndEcId, selTimeType: selTimeType, searchArg: searchArg },
        function(data) {
            if (data.IsSuccess) {
                alert("邮件发送成功！");
            } else {
                alert(data.Message);
            }
        });
}


