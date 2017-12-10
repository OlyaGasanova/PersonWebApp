var isDescriptionCollapsed = false;
var localStorageSupported = false;
var defaultApiAddress = "http://localhost:11001/api"

jQuery(document).ready(function($) {
	localStorageSupported = typeof(Storage) !== undefined;
	$("#intro-title-container").click(function() {
		$("#intro-description-container").animate({
			height: 'toggle'
		}, 300, function () {
			isDescriptionCollapsed = !isDescriptionCollapsed;
			if (localStorageSupported) {
				setLocalStorage("isDescriptionCollapsed", isDescriptionCollapsed);
			}
		});
	});
	isDescriptionCollapsed = getLocalStorage("isDescriptionCollapsed");
	if (isDescriptionCollapsed) {
		$("#intro-description-container").hide();
	}
});

function setLocalStorage (name, value) {
	window.localStorage.setItem(name, value);
}

function getLocalStorage(name) {
	var item = window.localStorage.getItem(name);
	if (item) {
		return JSON.parse(item);
	}
}

function checkConnection(connectionType) {
    $("#" + connectionType + "ConnectionMessage").text('');
    $("#" + connectionType + "ConnectionIndicator").addClass("loading");
    sendAjaxRequest({
		url: defaultApiAddress + "/CheckConnections/" + connectionType + "Connection",
		success: function (response) {
		    $("#" + connectionType + "ConnectionIndicator").removeClass("loading");
		    var responseObj = JSON.parse(response);
			if (responseObj.Code === 200) {
				$("#" + connectionType + "ConnectionIndicator").addClass("on");
			} else {
				$("#" + connectionType + "ConnectionIndicator").removeClass("on");
				console.error(responseObj.Message);
            }
            $("#" + connectionType + "ConnectionMessage").text(responseObj.Message);
		},
		error: function (error) {
			$("#" + connectionType + "ConnectionIndicator").removeClass("loading");
            $("#" + connectionType + "ConnectionIndicator").removeClass("on");
            $("#" + connectionType + "ConnectionMessage").text(error.statusText);
		}
	})
}


function sendAjaxRequest (requestConfig) {
	$.ajax({
		type: "GET",
		dataType: "json",
		url: requestConfig.url,
		success: requestConfig.success,
		error: requestConfig.error
	});
}



