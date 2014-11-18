var re = /^\d+(?=\.{0,1}\d+$|$)/;

$(document).ready(function () {
   

});

function checkForSubmit_1() {
    var predorderqun = $("#PREDICTORDERQUANTITY").val();
    var predweight = $("#PREDICTWEIGHT").val();
  
    if (!re.test(predweight) && predweight != "") {
        alert("重量只能为数字");
        $("#PREDICTWEIGHT").focus();
        return false;
    }
    if (predweight == "") {
        alert("重量不能为空");
        $("#PREDICTWEIGHT").focus();
        return false;
    }
    
    if (!re.test(predorderqun) && predorderqun != "") {
        alert("单量只能为数字");
        $("#PREDICTORDERQUANTITY").focus();
        return false;
    }
    
    if (predorderqun == "") {
        alert("单量不能为空");
        $("#PREDICTORDERQUANTITY").focus();
        return false;
    }

    var mileage = $("#MILEAGE").val();
    if (!re.test(mileage)) {
        alert("公里数只能为数字");
        $("#MILEAGE").focus();
        return false;
    }
    if (mileage == "") {
        alert("公里不能为空");
        $("#MILEAGE").focus();
        return false;
    }

    var pretime = $("#TASKTIME").val();
    if (pretime == "") {
        alert("提货时间不能为空");
        return false;
    }

    var PICKMAN = $("#PICKMAN").val();
    if (PICKMAN == "") {
        alert("提货人不能为空");
        return false;
    }

    return true;
}