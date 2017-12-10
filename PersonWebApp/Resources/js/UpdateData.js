function AddTableWithNames(Names) {

    var table = "<table id='NameTable'><tr><th class='col1'>№</th><th>Person</th></tr>";

    for (var i = 0; i < Names.length; i++) {
        table += "<tr><td>" + i + "</td>" + "<td>" + Names[i] + "</td></tr>";
    }

    table += "</table>";
    $("#AllNames").html(table);

}




function AddTableWithSingleOperations(Obj) {

    var table = "<div>" + Obj.ChoosenName + "</div><table id='SingleOperationsTable'><tr><th>Date</th><th>Sum</th><th>Currency</th></tr>";

    for (var i = 0; i < Obj.Date.length; i++) {
        table += "<tr><td>" + Obj.Date[i] + "</td>" + "<td>" + Obj.Value[i] +
            "</td>" + "<td>" + Obj.CurrencyNames[i] + "</td></tr>";
    }

    table += "</table>";

    $("#SingleOperations").html(table);

}


function AddTableWithAllOperations(Obj) {

    var table = "<table id='AllOperationsTable' style='width:1000px'><tr><th>Name</th><th>Income</th><th>Expense</th><th>Total</th></tr>";

    for (var i = 0; i < Obj.Total.length; i++) {
        table += "<tr><td>" + Obj.Names[i] + "</td>" + "<td>" + Obj.Income[i] +
            "</td>" + "<td>" + Obj.Expense[i] + "</td><td>" + Obj.Total[i] + "</td></tr>";
    }

    table += "</table>";
    $("#AllOperations").html(table);

}