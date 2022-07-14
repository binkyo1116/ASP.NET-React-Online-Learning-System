
function SendMessage() {
  

    var message = document.getElementById("TextBoxMessage").value;

    if (validate(message)) {
        document.getElementById("lblOutputError").innerText = "";
    //-----------------------Disable Button and Show LoaderImage----------------------
        document.getElementById("loader-spinner").style.display = "block"
        document.getElementById("disabled-div").style.display = "block"
    //--------------------------------------------------------------------------------

    const data = {
        Message: message
    };
        fetchFunction("Contact", data).then(d => {
        if (d.error != "") {
            document.getElementById("lblOutputError").innerHTML = d.error;
        }
            if (d.success != "") {
               //document.getElementById("TextBoxName").value = "";
               //document.getElementById("TextBoxEmail").value = "";
               document.getElementById("TextBoxMessage").value = "";

                document.getElementById("lblOutputSuccess").innerHTML = d.success;
        }
        //----------------------Hide Loader Image----------------------------------
            document.getElementById("loader-spinner").style.display = "none"
        document.getElementById("disabled-div").style.display = "none"
        //--------------------------------------------------------------------------
    });
    }
    return false;
}

function validateEmail(email) {
    const re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}
function validateMessage(message) {
    const re = /^[a-zA-Z0-9?.,:! \n]*$/;
    return re.test(message);
}
function validateMessageLength(message) {
    var r = true;
    if (message.length > 400) {
        r = false;
    }
    return r;
}
function validateName(name) {
    const re = /^[a-zA-Z0-9_ .,-]*$/;
    return re.test(name);
}
function validate(message) {
    var result = true;
    document.getElementById("lblOutputSuccess").innerText = "";
    //if (senderName == "") {
    //    document.getElementById("lblOutputError").innerText = "Sorry! The Name field cannot be left blank.";
    //    result = false;
    //}
    //else if (!validateName(senderName)) {
    //    document.getElementById("lblOutputError").innerText = "Sorry! Name field do not support any special character except '.', '_', '-', and ',' ";
    //    result = false;
    //} 
    //else if (senderEmail == "") {
    //    document.getElementById("lblOutputError").innerText = "Sorry! The Email field cannot be left blank.";
    //    result = false;
    //}
    //else if (!validateEmail(senderEmail)) {
    //    document.getElementById("lblOutputError").innerText = "Sorry! Your Email address is not valid. Please provide a valid email address.";
    //    result = false;
    //}
    if (message == "") {
        document.getElementById("lblOutputError").innerText = "Sorry! The Message field cannot be left blank";
        result = false;
    }
    //else if (!validateMessage(message)) {
    //    document.getElementById("lblOutputError").innerText = "Sorry! Message field do not support any special character except '.', '?', ':', '!' and ',' ";
    //    result = false;
    //} 
    else if (!validateMessageLength(message)) {
        document.getElementById("lblOutputError").innerText = "Sorry! The message do not support more than 400 character.Total Character of your message is " + message.length + ".";
        result = false;
    }
    return result;
}
