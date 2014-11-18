//titles    ：选项卡显示的文字       例如   选项卡一
//mehref    ：iframe的src指向
//ObjNum    ：具有的标示
//isShow    ：是否选择了连接就指向这个选项卡和显示对应的iframe
function RFD_tabs(titles, mehref, isShow) {
    this.titles = typeof (titles) === "undefined" ? "选项卡" : titles;
    this.mehref = typeof (mehref) === "undefined" ? "###" : mehref;
    this.isShow = typeof (isShow) === "undefined" ? false : isShow;
    this.ObjNum = parseInt(Math.random() * 100000000, 10);
}

RFD_tabs.TabShowOrder = 0;

//删除 选项卡 与 对应的Iframe
function delObjs(objNum) {
    var _objNum = "";
    //非 new 出来绑定的
    if (this === window) {
        _objNum = objNum;
        $("#li_" + _objNum).remove();
        $("#ifrm_" + _objNum).remove();
    } else {
        _objNum = $(this).attr("data-objNums");
        $("#li_" + _objNum).remove();
        $("#ifrm_" + _objNum).remove();
    }

    $("#meTabBG ul li a").removeClass("select");

    var tabShowOrder = 0;
    $("#meTabBG ul li a").each(function () {
        var curTabShowOrder = parseInt($(this).attr("data-TabShowOrder"));
        if (tabShowOrder < curTabShowOrder) {
            tabShowOrder = curTabShowOrder;
        }
    });

    var objNums = $("#meTabBG ul li a[data-TabShowOrder=" + tabShowOrder + "]").attr("data-objNums");
    $("#meTabBG ul li a[data-objNums=" + objNums + "]").addClass("select");
    $("#mebody iframe[data-objNums=" + objNums + "]").eq(0).show();
}
RFD_tabs.prototype.binddelObjs = function () {
    $("#li_" + this.ObjNum + " span").bind("click", function (event) {
        delObjs.call(this);
        //阻止事件冒泡
        event.stopPropagation();
    });
}

//点击时候发生
function clickObjs(objNum) {
    var _objNum = "";

    $("#meTabBG ul li a").removeClass("select");
    $("#mebody iframe").hide();

    //非 new 出来绑定的
    if (this === window) {
        _objNum = objNum;

    } else {
        $(this).attr("data-TabShowOrder", ++RFD_tabs.TabShowOrder);
        _objNum = $(this).attr("data-objNums");
        $("#li_" + _objNum + " a").addClass("select");
        $("#ifrm_" + _objNum).show();
    }
}
RFD_tabs.prototype.bindclickObjs = function () {
    $("#li_" + this.ObjNum + " a").bind("click", function () {
        clickObjs.call(this);
    });
}



//创建  自动   包括判断等
RFD_tabs.prototype.CreateForAuto = function () {
    if (this.isCreate()) {
        this.CreateTab();
        this.CreateIfrm();
        this.binddelObjs();
        this.bindclickObjs();
        if (this.isShow) {
            this.ShowMe();
        }
    } else {
        if (this.isShow) {
            this.ShowMe();
        }
    }
}

RFD_tabs.prototype.CloseTab = function (TabRandomNum) {
    $("#mebody iframe").each(function () {
        if ($(this).get(0).contentWindow.TabRandomNum == TabRandomNum) {
            var id = $(this).attr("id").split('_')[1];
            delObjs(id);
            return false;
        }
    });
}

//创建 tab
//li id = li_????
RFD_tabs.prototype.CreateTab = function () {
    var tabHTML = "";
    tabHTML += "<li";
    tabHTML += " id=\"li_" + this.ObjNum + "\"  >";
    tabHTML += "<a href=\"javascript:void(0);\"  data-href=\"" + this.mehref + "\"  data-objNums=\"" + this.ObjNum + "\" >";
    tabHTML += "<p data-objNums=\"" + this.ObjNum + "\" >" + this.titles + "</p>";
    tabHTML += "<span ";
    tabHTML += " data-objNums=\"" + this.ObjNum + "\" >";
    tabHTML += "</span>";
    tabHTML += "</a>";
    tabHTML += "</li>";

    $("#meTabBG ul").append(tabHTML);

}
//创建 iframe
//iframe id = ifrm_???   name= ifrm_???
RFD_tabs.prototype.CreateIfrm = function () {
    var IfrmHTML = "";
    IfrmHTML += "<iframe ";
    IfrmHTML += " id=\"ifrm_" + this.ObjNum + "\" ";
    IfrmHTML += " data-objNums=\"" + this.ObjNum + "\" ";
    IfrmHTML += " name=\"ifrm_" + this.ObjNum + "\" ";
    IfrmHTML += " height=\"100%\" width=\"100%\" frameborder=\"0\"   ";
    IfrmHTML += " class=\"com_disnone\" ";
    IfrmHTML += " src=\"" + this.mehref + "\"  >";
    IfrmHTML += "</iframe>";

    $("#mebody").append(IfrmHTML);
}


//判断之前是否已经创建过了，若创建过了，则指向
//true  ：可以创建
//false ：不能创建
RFD_tabs.prototype.isCreate = function () {
    //首先判断  选项卡里是否有重复
    var count = 0,
    RFD_tabs_mehref = this.mehref;
    $("#meTabBG ul li a").each(function () {
        if ($(this).attr("href") == RFD_tabs_mehref) {
            count += 1;
        }
    });
    //判断  iframe里是否有重复
    $("#mebody iframe").each(function () {
        if ($(this).attr("src") == RFD_tabs_mehref) {
            count += 1;
        }
    });
    if (count > 0) {
        return false;   //不可以创建
    } else {
        return true;
    }
}

//显示指定的  或则  默认点击的
RFD_tabs.prototype.ShowMe = function (objHref) {
    var objhrefs = typeof (objHref) === "undefined" ? this.mehref : objHref;    //需要指定的href
    //(所有的)给li加class  and  给iframe 隐藏掉  
    $("#meTabBG ul li a").removeClass("select");
    $("#mebody iframe").hide();
    $("#meTabBG ul li a").each(function () {
        //alert($(this).attr("href").replace("javascript:","") == objhrefs);
        if ($(this).attr("data-href") == objhrefs) {
            $(this).addClass("select");
            $(this).attr("data-TabShowOrder", ++RFD_tabs.TabShowOrder);
        }
    });
    $("#mebody iframe").each(function () {
        if ($(this).attr("src") == objhrefs) {
            $(this).show();
        }
    });
}


