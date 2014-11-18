$(function() {
	var provinces = $.dom("ddlProvinces", "drop");
	var cities = $.dom("ddlCities", "drop");
	var areas = $.dom("ddlAreas", "drop");
	//加载省
	util.listUtil.loadData(provinces, "../Handlers/GetProvinces.ashx");
	//由省加载市
	provinces.change(function() {
		var $this = $(this);
		util.listUtil.loadItemDataEx(
			                       $this,
		                           cities,
		                           "../Handlers/GetCityList.ashx",
		                           { triggerAsChildrens: true });
		//将选中省回传给隐藏变量
		util.listUtil.getSelectedText({
			control: $this,
			hidden: "hidProvince"
		});
	}).change();
	//由市加载区
	cities.change(function() {
		var $this = $(this);
		util.listUtil.loadItemDataEx(
								   $this,
		                           areas,
		                           "../Handlers/GetAreaList.ashx");
		//将选中市回传给隐藏变量
		util.listUtil.getSelectedText({
			control: $this,
			hidden: "hidCity"
		});
	}).change();
	//加载区
	areas.change(function() {
		//将选中区回传给隐藏变量
		util.listUtil.getSelectedText({
			control: areas,
			hidden: "hidArea"
		});
	});
});