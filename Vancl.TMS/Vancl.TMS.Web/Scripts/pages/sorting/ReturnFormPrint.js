
//获取选择的站点信息
function getStationValue(selectedType) {
    if (selectedType == "rdoMerchant") { return $("#Merchant option:selected").val(); }
    else if (selectedType == "rdoSortingCenter") { return $("#SortingCenterList option:selected").val(); }
    else if (selectedType == "rdoDistribution") { return $("#DistributorList option:selected").val(); }
    return "";
}
//获取选择的站点名称
function getStationName(selectedType) {
    if (selectedType == "rdoMerchant") { return $("#Merchant option:selected").html(); }
    else if (selectedType == "rdoSortingCenter") { return $("#SortingCenterList option:selected").html(); }
    else if (selectedType == "rdoDistribution") { return $("#DistributorList option:selected").html(); }
    return "";
}

//显示交接单
function showReceipt() {
    var list = TMS.Page.GetDataTableChecked();
    if (list.length == 0) {
        alert("请至少选择一个项！"); return;
    }
    var BoxNo = "";
    list.each(function () {
        //BoxNo += "BoxNo=" + $(this).val() + "&";
        BoxNo = $(this).val();
        var winName = BoxNo + "交接表打印";
        openWindow('FormPrintDetail?BoxNo=' + BoxNo, winName, 800, 600);
    });      //openWindow('PrintReceipt?' + BoxNo, 'Receipt', 800, 600);
}
function showBatchDetails(BoxNo) {
    openWindow('ReturnBoxDetail?BoxNo=' + BoxNo, '箱号明细单', 800, 600);
}
$(function () {
    $("#btnPrint").click(function () {
        var BoxNo = $("#hdBoxNo").val();
        //$("#hdBoxNo").val();
        var params = {};
        params.boxNo = BoxNo;
        $.ajax({
            url: "/Sorting/Return/BackFormPrint",
            data: params,
            dataType: 'json',
            type: 'POST',
            success: function (result) {
                if (result == null) {
                    alert(result);
                    return;
                }
                if (result.IsSuccess) {
                    window.print();
                }
                else {
                    alert(result.Message);
                    return;
                }
            }
        });
    });
});