function CurrencyChanged(value) {

    getCurrency(value);

}

function UpdateTable() {
    if (currentCase == 1) {
        $("#SingleOperations").html("");
        getSingleOperations(lastClickedLi.textContent)
    }
    if (currentCase == 2) {
        $("#AllOperations").html("");
        getAllOperations();
    }
}


var currentCase = 0;

function CaseChanged(index) {

    switch (index) {
        case 0: {
            if (currentCase != index) {
                $("#AllOperations").html("");
                $("#SingleOperations").html("");
                if (currentCase != 1) getNames();
                currentCase = index;
            }
        }
        case 1: {
            if (currentCase != index) {
                if (currentCase != 0) getNames();
                currentCase = index;
                $("#AllOperations").html("");
                $("#SingleOperations").html("Choose person");
            }
        }
        case 2: {
            if (currentCase != index) {
                $("#SingleOperations").html("");
                $("#AllNames").html("");
                getAllOperations();
                currentCase = index;

            }
        }
    }
}


var lastClickedLi = null;

function onTableClic(event) {
    var target = event.target;
    if (!(target.tagName == "TD" && currentCase == 1)) return;
    lastClickedLi = target;
    getSingleOperations(lastClickedLi.textContent);
}


