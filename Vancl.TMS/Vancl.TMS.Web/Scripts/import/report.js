include("../Styles/superTables_compressed.css");
include("../Scripts/plugins/jquery.supertable.compressed.js");
include("../Scripts/plugins/jquery.supertable.js");
$(function() {
	common.init.ready({ type: 'S', isRendered: false });
	var grid = $.dom("gv", ".grid");
	$(".summaryField").css("width", "auto");
	grid.toSuperTable({
		width: ($(document).width() - 100) + "px",
		height: "300px",
		fixedCols: 1,
		count: $.dom("hidTotalCount", "input").val(),
		onfinish: function() {
			$(".summaryfield").css("width", "auto");
			var height = grid.find("tr").height();
			$.dom("content").css("height", this.count == 0 ? 2 * height : "auto");
			$.dom("totalinfo").css("display", this.count == 0 ? "none" : "block");
		}
	});
	$.doms("BtnQuery,BtnReport", "input").click(function() {
		return util.dateUtil.selectDate({
			date: 'txtSTime',
			errorText: '请选择报表日期！'
		}) && common.isSelectStation({ errorText: '请选择配送站！' });
	});
	//hemingyu 2011-09-09增加批量导出功能
	$.dom("btnExportBatch", "input").click(function() {
		if (!util.dateUtil.selectDate({
			date: 'txtSTime',
			errorText: '请选择报表日期！'
		})) return false;
		//$(document).progressDialog.showDialog("导出中，请稍后...");
	});
});
