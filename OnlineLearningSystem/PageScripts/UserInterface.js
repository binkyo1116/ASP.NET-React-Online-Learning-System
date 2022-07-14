/*
User Interface functions
*/


/*
Input box
*/

// Set (and remove) active border
$("input.textbox").on('focus', function () {
    $(this).parent().addClass("active");
});
$("input.textbox").on('blur', function () {
    $(this).parent().removeClass("active");
});

// Handle pressing the 'eye' icon of a password input box
$(".pw-vis").click(function () {
    var elmContainer = this.parentElement, elmEyeDiv = this, elmInput = elmContainer.getElementsByTagName("input")[0];

    if (elmEyeDiv.classList.contains("hidden")) {
        elmEyeDiv.classList.remove("hidden");
        elmEyeDiv.classList.add("revealed");
        elmInput.type = "text";
    } else {
        elmEyeDiv.classList.remove("revealed");
        elmEyeDiv.classList.add("hidden");
        elmInput.type = "password";
    }
});

// Set an input box as invalid
function inputboxSetInvalid(jqelmInput, strErrMsg) {
    var elmInputContainer = jqelmInput.parent();
    var elmErrorMessage = elmInputContainer.next().children('span');

    elmErrorMessage.text(strErrMsg);
    elmInputContainer.addClass("invalid");
}

// Remove invalid flag from inputboxes on input
$("input.textbox").on('input', function () {
    this.parentElement.classList.remove('invalid');
});


/*
Message alerts
*/

// Show a message alert
function msgShow(strElmId, strMsgClass, strHtmlText) {
    var jqElm = $("#" + strElmId), htmlIcon;

    if (strMsgClass == "error") {
        htmlIcon = '<i class="fa-solid fa-circle-exclamation"></i>';
        jqElm.removeClass("success");
    }
    if (strMsgClass == "success") {
        htmlIcon = '<i class="fa-solid fa-circle-check"></i>';
        jqElm.removeClass("error");
    }

    jqElm.html(htmlIcon + '&nbsp;' + strHtmlText);
    jqElm.addClass(strMsgClass);

    jqElm.fadeIn();
}

// Hide a message manually (not using the x button)
function msgHide(strElmId) {
    $("#" + strElmId).fadeOut();
}
