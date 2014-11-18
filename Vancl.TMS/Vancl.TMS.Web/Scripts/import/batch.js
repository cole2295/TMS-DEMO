$(function() {
	//解决updatePanel与Jquery的冲突
	util.fileUtil.importFile();
	Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function() {
		var items = $(".grid td :checkbox:not([disabled])");
		util.listUtil.selectedAll("chkAll", items);
		$.dom("btnSearch", "input").click(function() {
			var waybillNo = $.dom("txtWaybillNo", "input");
			if (!util.numUtil.checkInteger(waybillNo)) {
				return false;
			}
			var beginTime = $.dom("txtBeginTime", "input").val();
			var endTime = $.dom("txtEndTime", "input").val();
			if (!util.dateUtil.checkDate(beginTime, endTime, {
				isCheckInteval: false
			})) return false;
			var sort = $.dom("ddlSortationList", "drop");
			var house = $.dom("ddlWarehouseList", "drop");
			if (util.listUtil.getSelectedIndex(sort, "drop") > 0 &&
                    util.listUtil.getSelectedIndex(house, "drop") > 0) {
				jAlert("分拣中心和仓库只能选择一项！");
				return false;
			}
			return true;
		});
	});
});

function OpenModelDialog(url, dialogHeight, dialogWidth, isResizeable, args) {

	var ret = window.showModalDialog(url, args, 'dialogHeight:' + dialogHeight + ';dialogWidth:' + dialogWidth + ';center:yes;resizable:' + isResizeable + ';status:no;');

	return ret;
}