var undefinedStr = this.undefined;
var meColor = "#edf3ff";    //table 换行的颜色
var chkIsInit = false;      //是否创建了div,类fn_checkbox()的全局函数

var main = {
    showAJAX: function () {
        var meOffset = $("#ifrm").offset();
        var meHeight = $("#ifrm").height();
        var meWidth = $("#ifrm").width();
        $("#MAIN_AJAX").css({ "top": meOffset.top + "px", "left": meOffset.left + "px",
            "height": meHeight + "px", "width": meWidth + "px", "display": "block"
        });
    },
    hideAJAX: function () {
        window.top.$("#MAIN_AJAX").hide();
    },
    //绑定导航的入口
    bindNav: function () {
        this.initFirstNav();
        this.initSecondNav();
    },
    //一级导航初始化
    initFirstNav: function () {
        var meTitles = "", meIDS = "";
        $("#meNav ul li a").each(function () {
            $(this).bind("click", function () {
                meTitles = $(this).html();
                meIDS = $(this).attr("data-id");
                //show or hide
                $("#meNavSecond_Title").html(meTitles);
                $("#meNavSecond ul").each(function () {
                    //清除动画
                    if ($(this).is(":animated")) {
                        $(this).stop(true,true);
                    }
                    $(this).hide();
                });
                $("#ul_" + meIDS).show("normal");
                //css
                $("#meNav ul li a").removeClass("select");
                $(this).addClass("select");
            });
        })
    },
    //2级导航初始化
    initSecondNav: function () {
        var meTitles = "", meIDS = "", meHref = "";
        $("#meNavSecond ul li a").each(function () {
            $(this).bind("click", function () {
                meTitles = $(this).text().replace("·", "");
                meHref = $(this).attr("href").replace("javascript:", "").replace("#:", "");
                //new出来       带参或则不带参
                var tabs = new RFD_tabs(meTitles, meHref, true);
                tabs.CreateForAuto();
                //当前a的addClass
                $("#meNavSecond ul li a").removeClass("select");
                $(this).addClass("select");
            });
        });
    }
}
//首页要用到的
var defaults = {
    meX: 1,    //记录折叠     初始值为1   1：没有向左折叠      2:已经向左折叠了
    //首页高度控制
    fnPage: function () {
        var meWidth = $(window).width(),
        meHeight = $(window).height(),
        toIE = 20,              //距离浏览器宽度
        toLeft = 246,           //没有折叠的距离
        toTop = 103;

        //没有折叠
        if (this.meX == 1) {
            $("#meNav").css({ "width": meWidth - toLeft - toIE + "px" });
            $("#meTabBG").css({ "width": meWidth - toLeft - toIE + 3 + "px" });     //因为CSS里1级nav的右边margin了-2
            $("#mebody").css({ "width": meWidth - toLeft - toIE + 3 + "px" });
        } else {
            $("#meNav").css({ "width": meWidth - toLeft - toIE + "px" });
            $("#meTabBG").css({ "left": "20px", "width": meWidth - toIE * 1.8 + "px" });
            $("#mebody").css({ "left": "20px", "width": meWidth - toIE * 1.8 + "px" });
        }

        $("#meNavSecond").css({ "height": meHeight - toTop - toIE + 40 + "px" });
        $("#mebody").css({ "height": meHeight - toTop - toIE - 30 + 40 + "px" });        //30是tab的高度
    },
    //向左滚动  1和2
    foldLeft: function (obj) {
        var meWidth = $(window).width(),
        toIE = 20,              //距离浏览器宽度   因为左边右边都有20  所以这里是40  == toIE * 2
        toLeft = 246,           //没有折叠的距离
        toTop = 103;
        if (this.meX == 1) {    //向左折叠
            $("#meNavSecond").animate({ "left": "-227px" });    //左边的2级导航
            $("#meTabBG").animate({ "left": "20px", "width": meWidth - toIE * 1.8 + "px" });
            $("#mebody").animate({ "left": "20px", "width": meWidth - toIE * 1.8 + "px" });
            $(obj).animate({ "left": "0px" }, 'normal');
            $(obj).removeClass("fn_FoldtopLeft").addClass("fn_FoldtopRight");
            this.meX = 2;
        } else {
            $("#meNavSecond").animate({ "left": "20px" });    //左边的2级导航
            $("#meTabBG").animate({ "left": toLeft + "px", "width": meWidth - toLeft - toIE + 3 + "px" }); //因为CSS里1级nav的右边margin了-2
            $("#mebody").animate({ "left": toLeft + "px", "width": meWidth - toLeft - toIE + 3 + "px" });
            $(obj).animate({ "left": "235px" }, 'normal');
            $(obj).removeClass("fn_FoldtopRight").addClass("fn_FoldtopLeft");
            this.meX = 1;
        }
    },
    //弹出层
    //meHeight(非必传） meWidth(非必传） meSrc(必传）
    meLayout: function (json) {
        if (typeof json.meHeight != "undefined") {
            json.meHeight = json.meHeight.indexOf("%") > -1 ? $(window).height() / 100 * json.meHeight.replace(/%/g, "") : json.meHeight;
        }
        if (typeof json.meWidth != "undefined") {
            json.meWidth = json.meWidth.indexOf("%") > -1 ? $(window).width() / 100 * json.meWidth.replace(/%/g, "") : json.meWidth;
        }
        var winHeight = typeof json.meHeight == "undefined" ? $(window).height() / 2 : json.meHeight;
        var winWidth = typeof json.meWidth == "undefined" ? $(window).width() / 2 : json.meWidth;
        $("#ifrmContent").attr("src", json.meSrc);
        $(".contentBG").css({ "display": "block" });
        $(".contentLayout").css({ "height": winHeight + "px", "width": winWidth + "px",
            "margin-left": 0 - winWidth / 2 + "px", "margin-top": 0 - winHeight / 2 + "px", "display": "block"
        });
        $(".contentBody").css({ "height": winHeight - 30 + "px", "width": winWidth - 20 + "px",
            "margin-left": 0 - winWidth / 2 + 10 + "px", "margin-top": 0 - winHeight / 2 + 25 + "px", "display": "block"
        });
        $(".contentX").css({ "margin-left": 0 - winWidth / 2 + $(".contentLayout").width() - 47 + "px",
            "margin-top": 0 - winHeight / 2 + "px", "display": "block"
        });
    },
    //关闭
    meLayoutDoMe: function () {
        $(".contentBG,.contentLayout,.contentBody,.contentX").css({ "display": "none" });
        $("#ifrmContent").attr("src", "");
    },
    meLayout_FN: function (json) {
        window.top.defaults.meLayout(json);
    }
}

//其他页要用到的代码
var code = {
    //隐藏表单
    doMe: function (obj) {
        $(obj).parent().parent().find("table").toggle();
        $(obj).toggleClass('fn_po_show');
    },
    //meid  要的table id  支持多个值，值用“,”隔开
    tbColor: function (mejson) {
        if (arguments.length > 0) {
            if (typeof mejson.meid != "undefined") {
                if (mejson.meid.indexOf(",") > -1) {
                    var mearray = mejson.meid.split(",");
                    for (var i = 0; i < mearray.length; i++) {
                        $("#" + mearray[i] + " tbody tr:odd").css({ "background-color": meColor });
                    }
                } else {
                    $("#" + mejson.meid + " tbody tr:odd").css({ "background-color": meColor });
                }
            }
        } else {
            $(".tb_list tbody tr:odd").css({ "background-color": meColor });
        }
    },
    //提示信息
    //参数：m_id  m_top  m_right  m_height  m_width   m_content
    setInfo: function (jason) {
        var id = "";
        if (typeof (jason.m_id) == "undefined") {
            throw "jason is wrong";
            return;
        } else {
            id = jason.m_id;
        }
        var myWidth = typeof (jason.m_width) == "undefined" ? $("#" + id).css("width") : jason.m_width;
        //var myHeight = typeof (jason.m_height) == "undefined" ? $("#" + id).css("height") : jason.m_height;
        var myHeight = typeof (jason.m_height) == "undefined" ? "auto" : jason.m_height;

        var myRight = typeof (jason.m_right) == "undefined" ? "45%" : jason.m_right;
        var myTop = typeof (jason.m_top) == "undefined" ? $("#" + id).css("top") : jason.m_top;

        var myContent = typeof (jason.m_content) == "undefined" ? $("#" + id + " .content").html() : jason.m_content;
        $("#" + id).css({ "width": myWidth, "height": myHeight, "right": myRight, "top": myTop });
        $("#" + id).fadeIn("slow");
        $("#" + id + " .content").html(myContent)
    },
    //父页的提示层(要关闭)
    closeFatherInfo: function (msg) {
        window.top.code.setInfo({ "m_id": "infoMe",
            "m_content": msg
        });
        setTimeout("top.code.closeInfo('infoMe');", "4000");
    },
    //关闭提示   若没有ID  关闭所有Info
    //m_id
    closeInfo: function (myId) {
        if (arguments.length == "0") {
            $(".InfoTitle").fadeOut("slow");
        } else {
            $("#" + myId).fadeOut("slow");
        }
    },
    //子页面关闭AJAX效果的统一封装。
    hideAjax: function () {
        setTimeout(function () { window.top.$("#MAIN_AJAX").hide(); }, '1000');
    }
}
 