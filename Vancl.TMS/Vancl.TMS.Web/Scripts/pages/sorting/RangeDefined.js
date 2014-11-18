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
    $("#selCityAndStation_City").attr("disabled", "disabled");
    $("#selCityAndStation_Station").attr("disabled", "disabled");
    //$("#BillQuery_div").attr("disabled", "disabled");
});

function ValidateSearchCondition() {
    //分拣中心改为非必填
    //    var selectedSortingCentre = $("#SortingCenterList option:selected").val();
    //    if (selectedSortingCentre == -1) {
    //        
    //        ymPrompt.errorInfo({ message: '请选择分拣中心后查询!' });
    //        //alert("请选择分拣中心后查询!");
    //        return false;
    //    }
    //获取配送商Ids
    if ($("#rdoDistribution").attr("checked") == "checked") {

        var distributionIds = $("#distributionDiv_List_hide").attr("disid");
        if (distributionIds.length >= 0) {
            $("#distributionIds").attr("value", distributionIds);
        } else {
            $("#distributionIds").attr("value", '-1');
        }
        //alert(distributionIds);
    }
    return true;
}

//添加前的合法性验证

function ValidateAddCondition() {
    var selectedSortingCentre = $("#SortingCenterList option:selected").val();
    if (selectedSortingCentre == -1) {
        //ymPrompt.errorInfo({ message: '请选择分拣中心后查询!' });
        alert("请选择分拣中心后添加!");
        return false;
    }
    //获取配送商Ids
    if ($("#rdoDistribution").attr("checked") == "checked") {

        var distributionIds = $("#distributionDiv_List_hide").attr("disid");
        if (distributionIds.length >= 0) {
            $("#distributionIds").attr("value", distributionIds);
        } else {
            $("#distributionIds").attr("value", '-1');
        }
    }
    //如果分拣范围未选择
    //-1
    var chooseType = $("[name = 'sortingCenterSelect']:checked").val();
    var a = $("#SortingCenterListWithoutSelf").val();
    //alert(a);
    //null、-1
    var b = $("#selCityAndStation_Station").val();
    //alert(b);
    //空、-1
    //var c = $("#distributionIds").val();
    var c = $("#distributionDiv_List_hide").attr("disid");
    //alert(c);
    //    if ((a == "-1" && b == null && c == "") || (a == "-1" && b == null && c == "-1")) {
    //        alert("请至少选择一个需要定义的分拣范围！");
    //            //return false;
    // }
    
    if (chooseType == "1" && a == "-1") {
        alert("请至少选择一个需要定义的分拣范围！");
        return false;
    }
    if ((chooseType == "2" && b == null) || (chooseType == "2" && b == "-1")) {
        alert("请至少选择一个需要定义的分拣范围！");
        return false;
    }
    if ((chooseType == "3" && c == "") || (chooseType == "3" && c == "-1")) {
        alert("请至少选择一个需要定义的分拣范围！");
        return false;
    }
    return true;
}

//function AddRange() {

//}

//导出数据
function exportRangeDefine() {
    var list = TMS.Page.GetDataTableChecked();
    if (list.length == 0) {
        alert("请至少选择一条记录！"); return;
    }
    var rangeIds = "";
    list.each(function () {
        //     batchNo += $(this).val() + ",";
        rangeIds += $(this).attr("id") + ",";
    });
    $("#rangeIds").val(rangeIds);
    $("#FormExport").submit();
}