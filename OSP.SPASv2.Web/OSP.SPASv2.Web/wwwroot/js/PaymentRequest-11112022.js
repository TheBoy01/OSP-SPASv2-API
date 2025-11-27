
$(document).ready(function () {


    $("#Companytype").change(function () {
        //var id = $("#Companytype option:selected").text();
        var id = $('#Companytype').val();
        //alert('ID' + id); 
        event.preventDefault();
        $.ajax({
            url: "/PaymentRequest/Company_Bind/" + $("#Companytype").val(),
            data: { 'CompanyID': id },
            contentType: "application/json;charset=utf-8",
            datatype: "json",
            success: function (result) {
                var items = '<option>-- Select Branch --</option>';
                for (var i = 0; i < result.length; i++) {
                    items += "<option value='" + result[i].value + "'>" + result[i].text + "</option>";
                }
                $('#Code').html(items);
            }
        });
    });

    $("#PaymentMethod").click(function () {
    //$("#PaymentMethod").on('change', function () {
        var id = $('#PaymentMethod').val();
        if (id == "DIGITAL WALLET") {
            $("#PaymentNetwork").val('GCASH');
        }
    });

    $("#ItemPrice").focusout(function () {
        $("#ItemPrice").formatCurrency();
        $("#ItemPrice").formatCurrency('.currencyLabel');
    });

    //$('#ListofPR').dataTable({
    //    "bInfo": false
    //});


});
