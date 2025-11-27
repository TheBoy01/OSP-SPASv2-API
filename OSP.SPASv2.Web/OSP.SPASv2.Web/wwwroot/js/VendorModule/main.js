//$(function(){
//    $("#form-total").steps({
//        headerTag: "h2",
//        bodyTag: "section",
//        transitionEffect: "fade",
//        enableAllSteps: true,
//        stepsOrientation: "vertical",
//        autoFocus: true,
//        transitionEffectSpeed: 500,
//        titleTemplate : '<div class="title">#title#</div>',
//        labels: {
//            previous : 'Back',
//            next : 'Nexts',
//            finish : 'Submit',
//            current : ''
//        },
//    })
//});


//$(function () {
//    $("#CreateAddressModal").click(function () {
//        //var customerId = $(this).closest("tr").find("td").eq(0).html();
//        $.ajax({
//            type: "GET",
//            url: "/VendorMaintenance/_VendorCreateAddress",
//            //data: { "customerId": customerId },
//            success: function (response) {
//                $("#CreateAddressPartialModal").find(".modal-body").html(response);
//                $("#CreateAddressPartialModal").modal('show');
//            },
//            failure: function (response) {
//                alert(response.responseText);
//            },
//            error: function (response) {
//                alert(response.responseText);
//            }
//        });
//    });
//});

function AddVendorInfo() {

    alert("OK"); 
}

$(document).ready(function () {
    $("#AddVendorRequiredDocs").click(function () {

        $.ajax({
            url: "/VendorMaintenance/CreateVendorRequiredDoc",
            type: "GET",
            data: { selected_div: "Create" },
            //beforeSend: function () {
            //    $("#loader").show();
            //},
            success: function (response) {

                $("#Generic_Modal").find(".modal-content").html(response);
                $("#Generic_Modal").modal('show');
            },
            error: function (xhr, status, error) {
                alert(xhr.responseText);
            },
            complete: function (data) {
                //$("#loader").hide();
            }
        });

    });
});

$(document).ready(function () {
    $("#AddVendorDocs").click(function () {
        $.ajax({
            url: "/VendorMaintenance/CreateVendorDoc",
            type: "GET",
            data: { selected_div: "Create" },
            //beforeSend: function () {
            //    $("#loader").show();
            //},
            success: function (response) {

                $("#Generic_Modal").find(".modal-content").html(response);
                $("#Generic_Modal").modal('show');
            },
            error: function (xhr, status, error) {
                alert(xhr.responseText);
            },
            complete: function (data) {
                //$("#loader").hide();
            }
        })

    });
});

//AddVendorATC

$(document).ready(function () {
    $("#AddVendorATC").click(function () {
        $.ajax({
            url: "/VendorMaintenance/CreateVendorATC",
            type: "GET",
            data: { selected_div: "Create" },
            //beforeSend: function () {
            //    $("#loader").show();
            //},
            success: function (response) {

                $("#Generic_Modal").find(".modal-content").html(response);
                $("#Generic_Modal").modal('show');
            },
            error: function (xhr, status, error) {
                alert(xhr.responseText);
            },
            complete: function (data) {
                //$("#loader").hide();
            }
        })

    });
});

$(document).ready(function () {
    $("#AddVendorAddress").click(function () {
        $.ajax({
            url: "/VendorMaintenance/CreateVendorAddress",
            type: "GET",
            data: { selected_div: "Create" },
            //beforeSend: function () {
            //    $("#loader").show();
            //},
            success: function (response) {

                $("#Generic_Modal").find(".modal-content").html(response);
                $("#Generic_Modal").modal('show');
            },
            error: function (xhr, status, error) {
                alert(xhr.responseText);
            },
            complete: function (data) {
                //$("#loader").hide();
            }
        })

    });
});

//$(document).ready(function () {
//    $(".add-Address").click(function () {
//        var AddressType = $("#AddressType").val();
//        var AddressNo = $("#AddressNo").val();
//        var Street = $("#Street").val();
//        var Brgy = $("#Brgy").val();
//        var City = $("#City").val();
//        var District = $("#District").val();
//        var Province = $("#Province").val();
//        var ZipCode = $("#ZipCode").val();
//        //var Address_Default = $().val();
//        var table = document.getElementById("Address_DataTable");
//        var table_len = (table.rows.length) - 1;
//        var markup = "<tr><td><input type='checkbox' name='record'></td><td id='addresstype_row" + table_len + "'>"
//            + AddressType +
//            "</td><td>"
//            + AddressNo + "," + Street + "," + Brgy + "," + City + "," + District + "," + Province +
//            "</td><td>"
//            + ZipCode +
//            "</td><td>"
//            + GetCheckBoxVal("Address_Default") +
//            "</td><td>"
//            + "<a data-bs-toggle='modal' data-bs-target='#Address_Modal' onclick='edit_row(" + table_len + ")' ><i class='bi bi-pencil-square fa-lg'></i> </a>" +
//            //"</td><td>"
//            //+ ZipCode +
//            "</td></tr>";
//        $("#tbody").append(markup);
//        $('#Address_Modal').modal('toggle');
//    });
//});

//$(document).ready(function () {
//    $(".add-BankAcct").click(function () {
//        var BankCode = $("#txtBankCode").val();
//        var BankAcctType = $("#BankAcctType").val();
//        var BankAcctNo = $("#txtBankAcctNo").val();
//        var table = document.getElementById("Bank_DataTable");
//        var table_len = (table.rows.length) - 1;
//        var markup = "<tr><td><input type='checkbox' name='record'></td><td id='BankAcctType_row" + table_len + "'>"
//            + BankCode +
//            "</td><td>"
//            + BankAcctType +
//            "</td><td>"
//            + BankAcctNo +
//            "</td><td>"
//            + GetCheckBoxVal("Bank_Default") +
//            "</td><td>"
//            + "<a data-bs-toggle='modal' data-bs-target='#Bank_Modal' onclick='edit_row(" + table_len + ")' ><i class='bi bi-pencil-square fa-lg'></i> </a>" +
//            //"</td><td>"
//            //+ ZipCode +
//            "</td></tr>";
//        $("#Bank_Tbody").append(markup);
//        $('#Bank_Modal').modal('toggle');
//    });
//});

//function save_row(no) {
//    var name_val = document.getElementById("name_text" + no).value;
//    var country_val = document.getElementById("country_text" + no).value;
//    var age_val = document.getElementById("age_text" + no).value;

//    document.getElementById("name_row" + no).innerHTML = name_val;
//    document.getElementById("country_row" + no).innerHTML = country_val;
//    document.getElementById("age_row" + no).innerHTML = age_val;

//    document.getElementById("edit_button" + no).style.display = "block";
//    document.getElementById("save_button" + no).style.display = "none";
//}


// Find and remove selected table rows
//    $(".delete-row").click(function () {
//        $("table tbody").find('input[name="record"]').each(function () {
//            if ($(this).is(":checked")) {
//                $(this).parents("tr").remove();
//            }
//        });
//    });
//});

function delete_row(TableRow, Table, Module) {
    let text = "Are you sure to delete this on the list of " + Module + "?";

    //console.log(Table);
    if (confirm(text) == true) {
        //alert(Table);
        Table.deleteRow(TableRow.rowIndex);
    }
}

function edit_row(RowID, TableName) {
    //console.log(TableName);

    //var TableName = Table.getAttribute("id");

    //var name = document.getElementById("name_row" + no);
    //var country = document.getElementById("country_row" + no);
    //var age = document.getElementById("age_row" + no);

    //var name_data = name.innerHTML;
    //var country_data = country.innerHTML;
    //var age_data = age.innerHTML;

    //name.innerHTML = "<input type='text' id='name_text" + no + "' value='" + name_data + "'>";
    //country.innerHTML = "<input type='text' id='country_text" + no + "' value='" + country_data + "'>";
    //age.innerHTML = "<input type='text' id='age_text" + no + "' value='" + age_data + "'>";
    //var row = table.insertRow(table_len).outerHTML = "<tr id='row" + table_len + "'><td id='name_row" + table_len + "'>" + new_name + "</td><td id='country_row" + table_len + "'>" + new_country + "</td><td id='age_row" + table_len + "'>" + new_age + "</td><td><input type='button' id='edit_button" + table_len + "' value='Edit' class='edit' onclick='edit_row(" + table_len + ")'> <input type='button' id='save_button" + table_len + "' value='Save' class='save' onclick='save_row(" + table_len + ")'> <input type='button' value='Delete' class='delete' onclick='delete_row(" + table_len + ")'></td></tr>";

    //document.getElementById("AddressModal_Title").innerHTML = "Edit Address";
    //var AddressType_Data = document.getElementById("addresstype_row" + no).innerHTML;
    //$("#AddressType").val(AddressType_Data);


    switch (TableName) {
        case "Vendor Attached Doc":
            //alert(RowID.rowIndex);
            //rIndex = this.rowIndex;
            //console.log(rIndex);
            //GANITO YUNG PAG UPDATE -> //table.rows[1].cells[0].innerHTML = document.getElementById("Add_cmbVendorDoc").options[document.getElementById("Add_cmbVendorDoc").selectedIndex].text;
            var _TblVendorAttached = {}; //document.getElementById("Edit_cmbVendorDoc").value(); //document.getElementById("Edit_cmbVendorDoc").options[document.getElementById("Edit_cmbVendorDoc").selectedIndex].text;
            _TblVendorAttached.DocCode = RowID.cells[0].innerHTML//Table.rows[RowID.rowIndex].cells[0].innerHTML;
            //alert(_SelectedDocDesc);
            $.ajax({
                url: "/VendorMaintenance/EditVendorDoc",
                type: "POST",
                data: { TblVendorAttached: _TblVendorAttached, selected_div: "Edit", ItemID: RowID.rowIndex },
                //beforeSend: function () {
                //    $("#loader").show();
                //},
                success: function (response) {

                    $("#Generic_Modal").find(".modal-content").html(response);
                    $("#Generic_Modal").modal('show');
                },
                error: function (xhr, status, error) {
                    alert(xhr.responseText);
                },
                complete: function (data) {
                    //$("#loader").hide();
                }
            })
            break;

        case "Vendor Required Documents":

            console.log(RowID);

            var _TblVendorDocRequired = {};
            _TblVendorDocRequired.DocCode = RowID.cells[0].innerHTML;
            _TblVendorDocRequired.FIleName = RowID.cells[1].innerHTML;
            _TblVendorDocRequired.Validity = RowID.cells[4].innerHTML;

            //alert(_SelectedDocDesc);
            $.ajax({
                url: "/VendorMaintenance/EditVendorRequiredDoc",
                type: "POST",
                data: { TblVendorDocRequired: _TblVendorDocRequired, _selected_div: "Edit", _ItemID: RowID.rowIndex },
                //beforeSend: function () {
                //    $("#loader").show();
                //},
                success: function (response) {

                    $("#Generic_Modal").find(".modal-content").html(response);
                    $("#Generic_Modal").modal('show');
                },
                error: function (xhr, status, error) {
                    alert(xhr.responseText);
                },
                complete: function (data) {
                    //$("#loader").hide();
                }
            })
            break;

        case "Vendor ATC":



            break;
    }
}

$(document).ready(function () {
    $("#CreateVendor").submit(function (event) {
        event.preventDefault();

        var AddressList = [];
        var tr = $("#Address_DataTable tr");
        for (var i = 1; i < tr.length; i++) {
            var tds = $(tr[i]).find("td");
            if (tds.length > 0) {
                var FullAddress = tds[2].innerHTML.split(',');
                var IsDefault = false;
                if (tds[4].innerHTML == "YES") {
                    IsDefault = true;
                }
                //console.log(FullAddress);
                AddressList.push({
                    "AddressType": tds[1].innerHTML,
                    "FullAddress": tds[2].innerHTML,
                    "AddressNo": FullAddress[0],
                    "Street": FullAddress[1],
                    "Brgy": FullAddress[2],
                    "District": FullAddress[4],
                    "City": FullAddress[3],
                    "Province": FullAddress[5],
                    "ZipCode": tds[3].innerHTML,
                    "IsDefault": IsDefault,
                })
            }
        }
        //console.log(AddressList);

        //var array = JSON.parse("[" + string + "]");
        //var _TblVendor = JSON.stringify({ DisplayName: VendorName, VendorType: VendorType });
        //JSON.stringify(req),
        //var TblVendorAddress = { "TblVendorAddress": result }
        var _TblVendor = {};
        _TblVendor.DisplayName = $("#VendorName").val();
        _TblVendor.VendorType = $("#VendorType").val();
        _TblVendor.LastName = $("#LastName").val();
        _TblVendor.MiddleName = $("#MiddleName").val();
        _TblVendor.FirstName = $("#FirstName").val();

        $.ajax({
            url: "/VendorMaintenance/CreateVendor",
            type: "POST",
            data: { TblVendor: _TblVendor, TblVendorAddressList: AddressList },
            //data: '{TblVendor: ' + JSON.stringify(_TblVendor) + '}',
            //contentType: "application/json",
            //contentType: "application/json; charset=utf-8",
            //dataType: "json",
            success: function (response) {
                alert(response);
            },
            error: function (xhr, status, error) {
                // Handle the error response
            }
        });
    });
});


$('#VendorType').on('change', function (e) {

    var DispDiv = document.getElementById("DisplayNameDiv");
    var EmpDiv = document.getElementById("EmployeeDiv");
    //alert($("#VendorType").val());
    if ($("#VendorType").val() == "EMP") {
        DispDiv.style.display = "none";
        EmpDiv.style.display = "block";
    }
    else {
        DispDiv.style.display = "block";
        EmpDiv.style.display = "none";
    }
});

function GetCheckBoxVal(checkboxId) {
    var checkbox = document.getElementById(checkboxId);
    //alert(checkbox.checked);
    if (checkbox.checked) {
        return "YES";
    }
    else {
        return "NO";
    }
}

$(document).ready(function () {
    $("#txtBankDesc").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '/VendorMaintenance/BankAutoComplete',
                data: { "prefix": request.term },
                type: "POST",
                success: function (data) {
                    response($.map(data, function (item) {
                        return item;
                    }))
                },
                error: function (response) {
                    alert(response.responseText);
                },
                failure: function (response) {
                    alert(response.responseText);
                }
            });
        },
        select: function (e, i) {
            //$("#hfCustomer").val(i.item.val);
            document.getElementById('txtBankCode').value = i.item.val;
        },
        minLength: 1
    });
});


$('#VendorType').on('change', function (e) {



});