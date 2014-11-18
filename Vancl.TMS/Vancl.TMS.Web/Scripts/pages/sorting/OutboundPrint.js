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
    $("#FormExport").submit();
  //  window.location = "Export?batchNos=" + batchNo;
}

//显示交接单
function showReceipt() {
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