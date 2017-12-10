
function getNames() {
    $("#AllNames").text('');
    $("#ConnectionIndicator").addClass("loading");
    sendAjaxRequest({
        url: defaultApiAddress + "/Person/Names",
        success: function (response) {
            $("#ConnectionIndicator").removeClass("loading");

            var responseObj = JSON.parse(response);
            if (responseObj.Code === 200) {
                $("#ConnectionIndicator").addClass("on");
            } else {
                $("#ConnectionIndicator").removeClass("on");
                console.error(responseObj.Message);
            }
            AddTableWithNames(responseObj.Names);
        },
        error: function (error) {
            $("#ConnectionIndicator").removeClass("loading");
            $("#ConnectionIndicator").removeClass("on");
            $("#AllNames").text(error.statusText);
        }
    })
}

function getCurrency(Current) {
    $("#ConnectionIndicator").addClass("loading");
    sendAjaxRequest({
        url: defaultApiAddress + "/Person/Currency/" + Current,
        success: function (response) {
            $("#ConnectionIndicator").removeClass("loading");
            var responseObj = JSON.parse(response);
            if (responseObj.Code === 200) {
                $("#ConnectionIndicator").addClass("on");
            } else {
                $("#ConnectionIndicator").removeClass("on");
                console.error(responseObj.Message);
            }
            UpdateTable();
        },
        error: function (error) {
            $("#ConnectionIndicator").removeClass("loading");
            $("#ConnectionIndicator").removeClass("on");
            console.log(error.statusText);
        }
    })
}



function getSingleOperations(Name) {
    $("#ConnectionIndicator").addClass("loading");
    sendAjaxRequest({
        url: defaultApiAddress + "/Person/SingleOperations/" + Name,
        success: function (response) {
            $("#ConnectionIndicator").removeClass("loading");
            var responseObj = JSON.parse(response);
            if (responseObj.Code === 200) {
                $("#ConnectionIndicator").addClass("on");
            } else {
                $("#ConnectionIndicator").removeClass("on");
                console.error(responseObj.Message);
            }
            AddTableWithSingleOperations(responseObj);
        },
        error: function (error) {
            $("#ConnectionIndicator").removeClass("loading");
            $("#ConnectionIndicator").removeClass("on");
            console.log(error.statusText);
        }
    })
}


function getAllOperations() {
    $("#ConnectionIndicator").addClass("loading");
    sendAjaxRequest({
        url: defaultApiAddress + "/Person/AllOperations",
        success: function (response) {
            $("#ConnectionIndicator").removeClass("loading");
            var responseObj = JSON.parse(response);
            if (responseObj.Code === 200) {
                $("#ConnectionIndicator").addClass("on");
            } else {
                $("#ConnectionIndicator").removeClass("on");
                console.error(responseObj.Message);
            }
            AddTableWithAllOperations(responseObj);
        },
        error: function (error) {
            $("#ConnectionIndicator").removeClass("loading");
            $("#ConnectionIndicator").removeClass("on");
            console.log(error.statusText);
        }
    })
}
