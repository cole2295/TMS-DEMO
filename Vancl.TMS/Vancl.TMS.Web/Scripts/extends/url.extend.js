if (typeof jQuery.tms == "undefined") jQuery.tms = {};
if (typeof jQuery.tms.url == "undefined") 
jQuery.tms.url = {
    params: function () {
        var args = new Object();
        var query = location.search.substring(1); //获取查询串   
        var pairs = query.split("&"); //在逗号处断开   
        for (var i = 0; i < pairs.length; i++) {
            var pos = pairs[i].indexOf('='); //查找name=val   
            if (pos == -1) continue; //如果没有找到就跳过   
            var argname = pairs[i].substring(0, pos); //提取name   
            var val = pairs[i].substring(pos + 1); //提取val   
            args[argname] = unescape(val); //存为属性(解码) 
        }
        return args;
    },
    relative: function (path) {
        var sUrl = document.URL;
        sUrl = sUrl.replace(/^.*?\:\/\/[^\/]+/, "").replace(/[^\/]+$/, "");
        if (!path) { return sUrl; }
        if (!/\/$/.test(sUrl)) { sUrl += "/"; }
        if (/^\.\.\//.test(path)) {
            var regex = new RegExp("^\\.\\.\\/"), iCount = 0;
            while (regex.exec(path) != null) {
                path = path.replace(Re, "");
                iCount++;
            }
            for (var i = 0; i < iCount; i++) {
                sUrl = sUrl.replace(/[^\/]+\/$/, "");
            }
            if (sUrl == "") return "/";
            return sUrl + path;
        }
        path = path.replace(/^\.\//, "");
        return sUrl + path;
    },
    root: function () {
        var strFullPath = window.document.location.href;
        var strPath = window.document.location.pathname;
        var pos = strFullPath.indexOf(strPath);
        var prePath = strFullPath.substring(0, pos);
        //var postPath = strPath.substring(0, strPath.substr(1).indexOf('/') + 1);
        return prePath;
    }
};