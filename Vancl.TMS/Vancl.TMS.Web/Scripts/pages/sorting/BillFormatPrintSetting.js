//面单套打设置页面

var Top_BillElem_Index = 1000;

String.prototype.format = function () {
    var args = arguments;
    return this.replace(/\{(\d+)\}/g,
        function (m, i) {
            return args[i];
        });
}

window.onload = function () {
    if (! +"\v1" && !document.querySelector) { // for IE6 IE7
        document.body.onresize = resize;
    } else {
        window.onresize = resize;
    }

    function resize() {
        var height = $(window).height();
        height = height - 40;
        $("#tbContent").height(height);
    };
    resize();
}

$(function () {
    $(".tabs").tabs();
    $("#PropertyList li").draggable({
        appendTo: "body",
        helper: "clone"
    });
    $("#txtTplHeight,#txtTplWidth").change(function () {
        var height = parseFloat($("#txtTplHeight").val());
        var width = parseFloat($("#txtTplWidth").val());
        $("#BillArea").css({ height: height + "cm", width: width + "cm" });
    });
    var draggableArg = { start: function () { $(this).addClass("highlight"); }, stop: function () { $(this).removeClass("highlight"); }, drag: function (event, ui) { BindProperty(ui.helper) } };
    var resizableArg = { resize: function (event, ui) { BindProperty(ui.element) } };
    $("#BillArea").droppable({
        activeClass: "ui-state-default",
        hoverClass: "hover",
        accept: ":not(.ui-sortable-helper)",
        drop: function (event, ui) {
            if (ui.draggable.hasClass("BillElem")) return;
            var defaultStyle = ui.draggable.attr("DefaultStyle");
            var defaultValue = ui.draggable.attr("DefaultValue");
            var ItemType = ui.draggable.attr("ItemType");
            if (!defaultValue) defaultValue = "";
            var valueFormat = ui.draggable.attr("ValueFormat");
            if (!valueFormat) valueFormat = "{0}";
            var value = valueFormat.format(defaultValue);
            var billElem = $("<div class='BillElem' tabindex='100' hidefocus='hidefocus'><table><tr><td>" + valueFormat + "</td></tr></div>")
            .find("td").text(value).end()
            .attr("ItemType", ItemType)
            .attr("style", defaultStyle)
            .attr("title", $.trim(ui.draggable.text()))
            //      .attr("ValueFormat", valueFormat)
            .css("zIndex", Top_BillElem_Index++)
            .css("left", ui.offset.left - $(this).offset().left)
            .css("top", ui.offset.top - $(this).offset().top)
            .appendTo(this)
            .draggable(draggableArg)
            .resizable(resizableArg);
            SelectBillElem(billElem);
        }
    });
    $(".BillElem")
    .find(">div").remove().end()//需要把拖动的ui元素的删除
    .draggable(draggableArg)
    .resizable(resizableArg)
    .live("mousedown", function () {
     //   $(this).css("zIndex", Top_BillElem_Index++);
        SelectBillElem(this);
    }).live("keydown", function (event) {
        var currentElem = $("#BillArea .CurrentElem");
        var e = event || window.event;
        var k = e.keyCode || e.which;
        var left = parseInt(currentElem.css("left").replace("px"));
        var top = parseInt(currentElem.css("top").replace("px"));
        switch (k) {
            case 37: //左
                currentElem.css("left", --left);
                e.keyCode = 0;
                break;
            case 38: //上
                currentElem.css("top", --top);
                e.keyCode = 0;
                break;
            case 39: //右
                currentElem.css("left", ++left);
                e.keyCode = 0;
                break;
            case 40: //下
                currentElem.css("top", ++top);
                e.keyCode = 0;
                break;
            case 46: //delete
                currentElem.remove();
                e.keyCode = 0;
                break;
        }
        BindProperty(currentElem);
        return e.keyCode;
    });

    $("#pnl-property *[PropertyType]").change(function () {
        var currentElem = $("#BillArea .CurrentElem");
        var PropertyType = $(this).attr("PropertyType");
        var PropertyName = $(this).attr("PropertyName");
        var PropertyValue = $(this).attr("PropertyValue");
        var value = $(this).val();
        if (PropertyType == "css") {
            currentElem.css(PropertyName, value);
        }
        else if (PropertyType == "value") {
            //       //     var valueFormat = currentElem.attr("ValueFormat");
            //        //    if (!valueFormat) valueFormat = "{0}";
            //            //     var value = valueFormat.format(value);
            //            currentElem.attr("ValueFormat", value)
            $("#BillArea .CurrentElem td").text(value);
        }
        else if (PropertyType == "border") {
            currentElem.css(PropertyName, $(this).attr("checked") ? "1px" : "0px");
        }
        else if (PropertyType == "valign") {
            currentElem.find("td").css(PropertyName, value);
        }
        else if (PropertyType == "attr") {
            currentElem.attr(PropertyName, value);
        }
        currentElem.focus();
    }).keydown(function (event) {
        var currentElem = $("#BillArea .CurrentElem");
        var e = event || window.event;
        var k = e.keyCode || e.which;
        if (k == 13) {
            $(this).change();
        }
    });



});

//选择元素点
function SelectBillElem(elem) {
    $(elem).focus();
    if ($(elem).hasClass("CurrentElem")) return;
    $("#BillArea .CurrentElem").removeClass("CurrentElem");
    $(elem).addClass("CurrentElem");
    BindProperty(elem);
}

function BindProperty(elem) {
    $("#pnl-property *[PropertyType]").each(function () {
        var currentElem = $("#BillArea .CurrentElem");
        var PropertyType = $(this).attr("PropertyType");
        var PropertyName = $(this).attr("PropertyName");
        var PropertyValue = $(this).attr("PropertyValue");

        if (PropertyType == "value") {
            var value = currentElem.find("td").text();
            $(this).val(value);
            //            if (!valueFormat) valueFormat = "{0}";
            //            var pos = valueFormat.indexOf("{0}");
            //            var t = $(elem).text();
            //            t = t.substring(pos, pos + t.length - valueFormat.length + 3)
            //            $(this).val(t);
        }
        else if (PropertyType == "css") $(this).val($(elem).css(PropertyName));
        else if (PropertyType == "border") {
            var bw = currentElem.css(PropertyName);
            if (bw && bw == "1px") $(this).attr("checked", "checked");
            else $(this).removeAttr("checked");
        }
        else if (PropertyType == "valign") {
            var valign = currentElem.find("td").css(PropertyName);
            $(this).val(valign);
        }
        else if (PropertyType == "attr") {
            var v = currentElem.attr(PropertyName);
            $(this).val(v);
        }
    });
}

function preview() {
    alert($("#tdBillArea").html());
    return;
    window.open("PrintSettingPreview",
    'PrintSettingPreview',
    'height=500,width=300,top=100,left=100,toolbar=no,menubar=no,scrollbars=auto, resizable=yes,location=no, status=no');
}

//新建模板
function saveTemplate() {
    var name = $.trim($("#txtTemplateName").val());
    if (name == "") {
        alert("请输入模板名称！");
        $("#txtTemplateName").focus();
        return;
    }
    var isNew = $("#sltTpls").val() == "";
    $("#hidIsNew").val(isNew ? "true" : "false");
    var height = parseFloat($("#txtTplHeight").val());
    var width = parseFloat($("#txtTplWidth").val());
    if (height == NaN || height < 0 || width == NaN || width < 0) {
        alert("模板大小格式不正确！"); return;
    }
    var content = $("#tdBillArea").html();
    $("#hidContent").val(content);
    $("form").ajaxSubmit(function (d) {
        if (!d.IsSuccess) {
            alert("保存失败!" + d.Message);
            return;
        }
        var id = d.DataBag.Id;
        location = baseUrl + id;
    });
}

function deleteTemplate() {
    if (window.confirm("你确定要删除该模板样式！")) {
        var tplId = $("#sltTpls").val();
        var url = baseUrl + "../DeleteFormatPrint/" + tplId
        $.get(url, function (rm) {
            if (rm.IsSuccess) {
                location = baseUrl;
            }
            else {
                alert(rm.Message);
            }
        })
    }
}

function selectTpl() {
    var data = $("#sltTpls :selected").attr("data");
    alert(data);
}

function setIndex(pos) {
    var top = Top_BillElem_Index;
    var bot = top;
    $("#BillArea .BillElem").each(function () {
        var index = parseInt($(this).css("zIndex"));
        if (index != NaN) {
            if (index > top) top = index;
            if (bot > index) bot = index;
        }
    });
    if (pos == "top") {
        $("#BillArea .CurrentElem").css("zIndex", top + 1).foucs();
        Top_BillElem_Index++;
    }
    else {
        $("#BillArea .CurrentElem").css("zIndex", bot - 1).foucs();
    }
}