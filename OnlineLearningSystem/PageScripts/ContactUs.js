
function SendMessage() {
    const  senderName = document.getElementById("TextBoxName").value;
    const senderEmail = document.getElementById("TextBoxEmail").value;
    const message = document.getElementById("TextBoxMessage").value;

    if (validate(senderName, senderEmail, message)) {
        document.getElementById("lblMessage").innerText = "";
    //-----------------------Disable Button and Show LoaderImage----------------------
        document.getElementById("login-loder-img").style.display = "block"
        document.getElementById("disabled-div").style.display = "block"
    //--------------------------------------------------------------------------------

    const data = {
        SenderName: senderName,
        SenderEmail: senderEmail,
        Message: message
    };
        fetchFunction("ContactUs", data).then(d => {
        if (d.error != "") {
            document.getElementById("lblMessage").innerHTML = d.error;
        }
            if (d.success != "") {
               document.getElementById("TextBoxName").value = "";
               document.getElementById("TextBoxEmail").value = "";
               document.getElementById("TextBoxMessage").value = "";

            document.getElementById("lblSuccessMessage").innerHTML = d.success;
        }
        //----------------------Hide Loader Image----------------------------------
        document.getElementById("login-loder-img").style.display = "none"
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
function validate(senderName, senderEmail, message) {
    var result = true;
    document.getElementById("lblSuccessMessage").innerText = "";
    if (senderName == "") {
        $("#fullname_message span").text("This field can’t be empty");
        result = false;
        document.getElementById("TextBoxName").classList.add("invalid")
        document.getElementById("fullname_message").style.display = ""
    }
    else if (!validateName(senderName)) {
        $("#fullname_message span").text("Invalid Full Name");
        result = false;
        document.getElementById("TextBoxName").classList.add("invalid")
        document.getElementById("fullname_message").style.display = ""
    }
    else {
        document.getElementById("TextBoxName").classList.remove("invalid")
        document.getElementById("fullname_message").style.display = "none"
    }

    if (senderEmail == "") {
        $("#email_message span").text("This field can’t be empty");
        result = false;
        document.getElementById("TextBoxEmail").classList.add("invalid")
        document.getElementById("email_message").style.display = ""
    }
    else if (!validateEmail(senderEmail)) {
        $("#email_message span").text("Invalid Email Address");
        result = false;
        document.getElementById("TextBoxEmail").classList.add("invalid")
        document.getElementById("email_message").style.display = ""
    }
    else if (message == "") {
        document.getElementById("lblMessage").innerText = "Sorry! The Message field cannot be left blank";
        result = false;
    }
    else {
        document.getElementById("TextBoxEmail").classList.remove("invalid")
        document.getElementById("email_message").style.display = "none"
    }

    if (!validateMessage(message)) {
        $("#message_message span").text("Invalid Message");
        result = false;
        document.getElementById("TextBoxMessage").classList.add("invalid")
        document.getElementById("message_message").style.display = ""
    } else if (!validateMessageLength(message)) {
        $("#message_message span").text("Invalid Message");
        result = false;
        document.getElementById("TextBoxMessage").classList.add("invalid")
        document.getElementById("message_message").style.display = ""
    }
    else {
        document.getElementById("TextBoxMessage").classList.remove("invalid")
        document.getElementById("message_message").style.display = "none"
    }
    return result;
}
