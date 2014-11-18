$(document).ready(function () {
    $(":radio").each(function () {
        $(this).click(function () {
            if ($(this).attr("divid") == "1") {
                $("#ORDERAMOUNT").val("");
                $("#" + $(this).attr("name") + "_" + "2").hide();
                $("#" + $(this).attr("name") + "_" + $(this).attr("divid")).show();
            }
            if ($(this).attr("divid") == "2") {
                $("#ONCEAMOUNT").val("");
                $("#" + $(this).attr("name") + "_" + "1").hide();
                $("#" + $(this).attr("name") + "_" + $(this).attr("divid")).show();
            }
        });
    });

    $("#txtWAREHOUSE").click(function () {
        var mechantid = $("#waybillSource_List_hide").val();
        if (mechantid == "") {
            alert("请先选择商家");
            return false;
        }
        if (mechantid == "undefined") {
            return false;
        }
        TMS.Page.WareHouse($(this), '#WAREHOUSEID', mechantid);
    });

    $("#PREDICTTIME").focus(function () {
        WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss' });
    });

    $("#RECEIVEEMAIL").blur(function () {
        var RECEIVEEMAIL = $("#RECEIVEEMAIL").val();
        var reg = /^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$/;

        $.each(RECEIVEEMAIL.split(";"), function (key, val) {
            if (!reg.test(val)) {
                alert("邮箱：" + val + " 验证失败，请重新录入！");
            
                return;
            }
        });
    });

    $("#MILEAGE").blur(function () {
        var mileage = $("#MILEAGE").val();
        var re = /^\d+(?=\.{0,1}\d+$|$)/;
        if (!re.test(mileage) && mileage != "") {
            alert("预计公里数只能为数字");
            return;
        }

    });

    $("#PREDICTWEIGHT").blur(function () {
        var predweight = $("#PREDICTWEIGHT").val();
        var re = /^\d+(?=\.{0,1}\d+$|$)/;
        if (!re.test(predweight) && predweight != "") {
            alert("预计提货重量只能为数字");
            return;
        }
    });

    $("#PREDICTORDERQUANTITY").blur(function () {
        var predweight = $("#PREDICTORDERQUANTITY").val();
        var re = /^\d+(?=\.{0,1}\d+$|$)/;
        if (!re.test(predweight) && predweight != "") {
            alert("预计提货单量只能为数字");
            return;
        }
    });
});


//表单提交
function OnSubmit() {
    if (checkForSubmit()) {
        $("#taskform").submit();
    }
}

 

function checkForSubmit() {

    var predorderqun = $("#PREDICTORDERQUANTITY").val();
    var predweight = $("#PREDICTWEIGHT").val();

    var re = /^\d+(?=\.{0,1}\d+$|$)/;
    if (!re.test(predweight) && predweight != "") {
        alert("预计提货重量只能为数字");
        return false;
    }
    
    if (!re.test(predorderqun) && predorderqun != "") {
        alert("预计提货单量只能为数字");
        return false;
    }

    if (!re.test($("#ONCEAMOUNT").val()) && $("#ONCEAMOUNT").val()!="")
    {
        alert("提货价格只能为数字");
        $("#ONCEAMOUNT").focus();
        return false;
    }
    
    if (!re.test($("#ORDERAMOUNT").val()) && $("#ORDERAMOUNT").val() != "") {
        alert("提货价格只能为数字");
        $("#ORDERAMOUNT").focus();
        return false;
    }
    

    var pretime = $("#PREDICTTIME").val();
    if (pretime == "") {
        alert("计划提货时间不能为空");
        return false;
    }

    var mechantid = $("#waybillSource_List_hide").val();
    if (mechantid == "") {
        alert("商家不能为空");
        return false;
    }

    if ($("#WAREHOUSEID").val() == "" || $("#WAREHOUSEID").val() == "0") {
        alert("商家库房不能为空");
        return false;
    }

    if ($("#distributionDiv_List_hide").val() == "") {
        alert("提货公司不能为空");
        return false;
    }

    var mileage = $("#MILEAGE").val();
    if (!re.test(mileage) && mileage!="") {
        alert("预计公里数只能为数字");
        $("#MILEAGE").focus();
        return false;
    }

    var receiveemail = $("#RECEIVEEMAIL").val();
    if (receiveemail == "") {
        alert("邮箱不能为空");
        return false;
    } else {
        var reg = /^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$/;
        var emaiType = "1";
        $.each(receiveemail.split(";"), function (key, val) {
            if (!reg.test(val)) {
                alert("邮箱：" + val + " 验证失败，请重新录入！");
                $("#RECEIVEEMAIL").focus();
                emaiType = "2";
            }
        });
        if (emaiType == "2") {
            return false;
        }
    }

    if ($('input:radio[name="PICKPRICETYPE"]').length > 0) {
        if ($('input:radio[name="PICKPRICETYPE"]').is(":checked")) {
            var val = $('input:radio[name="PICKPRICETYPE"]:checked').val();
            if (val == "0") {
                if ($("#ONCEAMOUNT").val() == "") {
                    alert("请输入提货价格");
                    return false;
                }
            }
            if (val == "1") {
                if ($("#ORDERAMOUNT").val() == "") {
                    alert("请输入提货价格");
                    return false;
                }
            }

        } else {
            alert("请选择提货价格类型");
            return false;
        }
    }
    return true;
}


function onlyInt(obj) {
    obj.keyup(function () { //keyup事件处理 
        obj.val(obj.val().replace(/\D|^0/g, ''));
    }).bind("paste", function () { //CTR+V事件处理 
        obj.val(obj.val().replace(/\D|^0/g, ''));
    }).css("ime-mode", "disabled"); //CSS设置输入法不可用
}

function onlyDouble(obj) {
    obj.keyup(function () {
        obj.val(obj.val().replace(/[^0-9.]/g, ''));
    }).bind("paste", function () { //CTR+V事件处理  
        obj.val(obj.val().replace(/[^0-9.]/g, ''));
    }).css("ime-mode", "disabled"); //CSS设置输入法不可用  
}





function changeWeeks(obj) {
    if (obj == "0") {
        $("#chkMonday").attr("checked", true);
        $("#chkTuesday").attr("checked", true);
        $("#chkWednesday").attr("checked", true);
        $("#chkThursday").attr("checked", true);
        $("#chkFriday").attr("checked", true);
        $("#chkSaturday").attr("checked", true);
        $("#chkSunday").attr("checked", true);
    }
    if (obj == "1") {
        $("#chkMonday").attr("checked", true);
        $("#chkTuesday").attr("checked", true);
        $("#chkWednesday").attr("checked", true);
        $("#chkThursday").attr("checked", true);
        $("#chkFriday").attr("checked", true);
        $("#chkSaturday").attr("checked", false);
        $("#chkSunday").attr("checked", false);
    }
    if (obj == "2") {
        $("#chkMonday").attr("checked", false);
        $("#chkTuesday").attr("checked", false);
        $("#chkWednesday").attr("checked", false);
        $("#chkThursday").attr("checked", false);
        $("#chkFriday").attr("checked", false);
        $("#chkSaturday").attr("checked", true);
        $("#chkSunday").attr("checked", true);
    }
    if (obj == "3") {
        $("#chkMonday").attr("checked", false);
        //$("#chkMonday").attr("disabled", true);
        $("#chkTuesday").attr("checked", false);
        //$("#chkTuesday").attr("disabled", true);
        $("#chkWednesday").attr("checked", false);
        //$("#chkWednesday").attr("disabled", true);
        $("#chkThursday").attr("checked", false);
        //$("#chkThursday").attr("disabled", true);
        $("#chkFriday").attr("checked", false);
        //$("#chkFriday").attr("disabled", true);
        $("#chkSaturday").attr("checked", false);
        //$("#chkSaturday").attr("disabled", true);
        $("#chkSunday").attr("checked", false);
        //$("#chkSunday").attr("disabled", true);
    }
}

function checkForPlan() {
    var mechantid = $("#waybillSource_List_hide").val();
    if (mechantid == "") {
        alert("商家不能为空");
        return false;
    }

    if ($("#WAREHOUSEID").val() == "" || $("#WAREHOUSEID").val() == "0") {
        alert("商家库房不能为空");
        return false;
    }

    if ($("#distributionDiv_List_hide").val() == "") {
        alert("配送商不能为空");
        return false;
    }

    if ($("#SPECIFICTIME").val() == "") {
        alert("提货时间不能为空");
        return false;
    }


    if ($("#AMOUNT").val() == "" && $("#AMOUNT_1").val() == "") {
        alert("提货价格不能为空");
        return false;
    } else {
        var amout = $("#AMOUNT").val();
        var amout1 = $("#AMOUNT_1").val();
        var re = /^\d+(?=\.{0,1}\d+$|$)/;
        if (amout != "") {
            if (!re.test(amout)) {
                alert("提货价格非法");
                return false;
            }
        }
        if (amout1 != "") {
            if (!re.test(amout1)) {
                alert("提货价格非法");
                return false;
            }
        }
        
    }
    
    var receivemail = $("#RECEIVEMAIL").val();
    if (receivemail != "") {
        var reg = /^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$/;
        var mailCheckFlag = true;
        $.each(receivemail.split(";"), function(key, val) {
            if (!reg.test(val)) {
                alert("邮箱：" + val + " 验证失败，请重新录入！");
                $("#RECEIVEMAIL").focus();
                mailCheckFlag = false;
            }
        });
        if (mailCheckFlag == false) {
            return mailCheckFlag;
        }
    } else {
        alert("任务接收邮箱不能为空");
        return false;
    }

    return true;
}


function getWeeks() {
    var weeks = "";
    if ($("#chkMonday").is(":checked")) {
        weeks += $("#chkMonday").val() + ",";
    }
    if ($("#chkTuesday").is(":checked")) {
        weeks += $("#chkTuesday").val() + ",";
    }
    if ($("#chkWednesday").is(":checked")) {
        weeks += $("#chkWednesday").val() + ",";
    }
    if ($("#chkThursday").is(":checked")) {
        weeks += $("#chkThursday").val() + ",";
    }
    if ($("#chkFriday").is(":checked")) {
        weeks += $("#chkFriday").val() + ",";
    }
    if ($("#chkSaturday").is(":checked")) {
        weeks += $("#chkSaturday").val() + ",";
    }
    if ($("#chkSunday").is(":checked")) {
        weeks += $("#chkSunday").val() + ",";
    }
    weeks = weeks.substring(0, weeks.length - 1);
    return weeks;
}

function convertToInt(obj) {
    if (!isNaN(obj) || obj.val() == "") {
        return 0;
    } else {
        return obj.val();
    }
}