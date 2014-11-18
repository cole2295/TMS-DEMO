$(document).ready(function () {
    // 在这里写你的代码...
    $("#FINISHTIME").focus(function () {
        WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss' });
    });

    var isCheck = false;
    var re = /^\d+(?=\.{0,1}\d+$|$)/;
    $("#ORDERAMOUNT").blur(function () {
        var amount = $(this).val();
        var oFloat = parseFloat($("#ORDERQUANTITY").val());
        var kFloat = $("#KPIAMOUNT").val();
        if (!re.test(amount)) {
            alert("提货价格只能为数字");
            $("#ORDERAMOUNT").focus();
            return false;
        }
        if (kFloat == "") {
            kFloat = "0";
        }
        var AmountSum = 0;
        if (kFloat.indexOf('-') > -1) {
            AmountSum = parseFloat(amount) * oFloat - parseFloat(kFloat.replace("-", ""));
        } else {
            AmountSum = parseFloat(amount) * oFloat + parseFloat(kFloat);
        }

        $("#PICKGOODSAMOUNT_span").text(AmountSum);
        $("#finalAmount").text(AmountSum);
    });

    $("#ONCEAMOUNT").blur(function () {
 
        var amount = $(this).val();
        var kFloat = $("#KPIAMOUNT").val();
        var amountSum = 0;
        if (!re.test(amount)) {
            alert("提货价格只能为数字");
            $("#ONCEAMOUNT").focus();
            return false;
        }
        if (kFloat == "") {
            kFloat = "0";
        }
        if (kFloat.indexOf('-') > -1) {
            amountSum = parseFloat(amount) - parseFloat(kFloat.replace("-", ""));
        } else {
            amountSum = parseFloat(amount) + parseFloat(kFloat);
        }


        $("#PICKGOODSAMOUNT_span").text(amountSum);
        $("#finalAmount").text(amountSum);
    });

    $("#ORDERQUANTITY").blur(function () {
       
        var amount = $("#ORDERAMOUNT").val();
        var kFloat = $("#KPIAMOUNT").val();
        var oFloat = parseFloat($("#ORDERQUANTITY").val());
        var amountSum = 0;
        if (!re.test(amount)) {
            alert("单量只能为数字");
            $("#ORDERQUANTITY").focus();
            return false;
        }
        if (kFloat == "") {
            kFloat = "0";
        }
        //单量计费
        if ($("input[name='PICKPRICETYPE'][type='radio'][checked]").val() == "1") {
            if (kFloat.indexOf('-') > -1) {
                amountSum = parseFloat(amount) * oFloat - parseFloat(kFloat.replace("-", ""));
            } else {
                amountSum = parseFloat(amount) * oFloat + parseFloat(kFloat);
            }


            $("#PICKGOODSAMOUNT_span").text(amountSum);
            $("#finalAmount").text(amountSum);
        }

    });


    $("#WEIGHT").blur(function () {
        var amount = $(this).val();
        if (!re.test(amount)) {
            alert("重量只能为数字");
            $("#WEIGHT").focus();
            return false;
        }
    });

    $("#MILEAGE").blur(function () {
        var amount = $(this).val();
        if (!re.test(amount)) {
            alert("公里只能为数字");
            $("#MILEAGE").focus();
            return false;
        }
    });




    $("#KPIAMOUNT").blur(function () {
        var kmount = $(this).val();
        if (kmount == "") {
            return false;
        }
        if (kmount.indexOf("-") > -1 && kmount.length == 1) {
            alert("考核费用不正确");
            $(this).focus();
            return false;
        }

        if ($(this).val().split('-').length > 2) {
            alert("减号只能存在一个");
            $(this).focus();
            return false;
        }
        if ($(this).val().indexOf('-') > 0) {
            alert("减号只能存在数字的最前面");
            $(this).focus();
            return false;
        }

        if ($(this).val().indexOf('.') == 0) {
            alert("小数点不能再第一位");
            $(this).focus();
            return false;
        }

        if ($(this).val().split('.').length > 2) {
            alert("只能允许一个小数点");
            $(this).focus();
            return false;
        }


        if ($(this).val().split('.').length != 1) {
            for (var i = 0; i < $(this).val().split('.').length; i++) {
                if ($(this).val().split('.')[1].length > 2) {
                    alert("只能允许精确到两位小数");
                    $(this).focus();
                    return false;
                }
            }
        }
        var regs = /^[0-9]*$/;

        if (!regs.test($(this).val().replace('.', '').replace('-', ''))) {
            alert("考核费用只能为数值");
            $(this).focus();
            return false;
        }

        var kpiAmount = 0;
        var pickAmount = parseFloat($("#PICKGOODSAMOUNT_span").text());

        var resultAmount = 0;
        if ($(this).val().indexOf('-') > -1) {
            kpiAmount = parseFloat($(this).val().replace('-', ''));
            resultAmount = pickAmount - kpiAmount;
        } else {
            kpiAmount = parseFloat($(this).val().replace('-', ''));
            resultAmount = pickAmount + kpiAmount;
        }
        $("#finalAmount").text(resultAmount);
        $("#PICKGOODSAMOUNT").val(resultAmount);

        isCheck = true;
    });
});
    