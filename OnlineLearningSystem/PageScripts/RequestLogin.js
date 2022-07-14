function requestLogin(theForm) {

    if (validate()) {
        document.getElementById("lblMessage").innerText = "";
        //-----------------------Disable Button and Show LoaderImage----------------------
        document.getElementById("login-loder-img").style.display = "block"
        document.getElementById("disabled-div").style.display = "block"
    //--------------------------------------------------------------------------------
        const data = {
            Name: document.getElementById("TextBoxName").value,
            Email: document.getElementById("TextBoxEmail").value,
            SchoolName: document.getElementById("TextBoxSchoolName").value,
            CourseName: document.getElementById("TextBoxCourseName").value
        };

        fetchFunction("StudentRequestLogin", data).then(d => {
            if (d.Success) {

                document.getElementById("TextBoxName").value = "";
                document.getElementById("TextBoxEmail").value = "";
                document.getElementById("TextBoxSchoolName").value = "";
                document.getElementById("TextBoxCourseName").value = "";
                document.getElementById("lblMessage").innerHTML = "";
                document.getElementById("lblSuccessMessage").innerHTML = "Your request has been successfully submitted";
            } else {
                document.getElementById("lblSuccessMessage").innerHTML = "";
                document.getElementById("lblMessage").innerHTML = d.Message;
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
//function validateMessage(message) {
//    const re = /^[a-zA-Z0-9_ .,:;''!""()-]*$/;
//    return re.test(message);
//}
function validateMessageLength(message, lng) {
    var r = true;
    if (message.length > lng) {
        r = false;
    }
    return r;
}
function validate() {
    var result = true;
    document.getElementById("lblSuccessMessage").innerText = "";

    if (document.getElementById("TextBoxSchoolName").value == "") {
        $("#schoolname_message span").text("This field can’t be empty");
        result = false;
        document.getElementById("TextBoxSchoolName").classList.add("invalid")
        document.getElementById("schoolname_message").style.display = ""
    }
    //else if (!validateMessage(document.getElementById("TextBoxSchoolName").value)) {
    //    document.getElementById("lblMessage").innerText = "Sorry! School Name field do not support any special character except '.', '_', '-', and ',' ";
    //    result = false;
    // } 
    else if (!validateMessageLength(document.getElementById("TextBoxSchoolName").value, 50)) {
        $("#schoolname_message span").text("Invalid School Name");
        result = false;
        document.getElementById("TextBoxSchoolName").classList.add("invalid")
        document.getElementById("schoolname_message").style.display = ""
    }
    else {
        document.getElementById("TextBoxSchoolName").classList.remove("invalid")
        document.getElementById("schoolname_message").style.display = "none"
    }

    if (document.getElementById("TextBoxCourseName").value == "") {
        $("#coursename_message span").text("This field can’t be empty");
        result = false;
        document.getElementById("TextBoxCourseName").classList.add("invalid")
        document.getElementById("coursename_message").style.display = ""
    }
     //else if (!validateMessage(document.getElementById("TextBoxCourseName").value)) {
     //    document.getElementById("lblMessage").innerText = "Sorry! Course Name field do not support any special character except '.', '_', '-', and ',' ";
     //    result = false;
     //}
    else if (!validateMessageLength(document.getElementById("TextBoxCourseName").value, 101)) {
        $("#coursename_message span").text("Invalid Course Name");
        result = false;
        document.getElementById("TextBoxCourseName").classList.add("invalid")
        document.getElementById("coursename_message").style.display = ""
    }
    else {
        document.getElementById("TextBoxCourseName").classList.remove("invalid")
        document.getElementById("coursename_message").style.display = "none"
    }

    if (document.getElementById("TextBoxName").value == "") {
        $("#fullname_message span").text("This field can’t be empty");
        result = false;
        document.getElementById("TextBoxName").classList.add("invalid")
        document.getElementById("fullname_message").style.display = ""
    }
    //else if (!validateMessage(document.getElementById("TextBoxName").value)) {
    //     document.getElementById("lblMessage").innerText = "Sorry! The Name field do not support any special character except '.', '_', '-', and ',' ";
    //    result = false;
    // } 
    else if (!validateMessageLength(document.getElementById("TextBoxName").value, 50)) {
        $("#fullname_message span").text("Invalid Full Name");
        result = false;
        document.getElementById("TextBoxName").classList.add("invalid")
        document.getElementById("fullname_message").style.display = ""
    }
    else {
        document.getElementById("TextBoxName").classList.remove("invalid")
        document.getElementById("fullname_message").style.display = "none"
    }

    if (document.getElementById("TextBoxEmail").value == "") {
        $("#email_message span").text("This field can’t be empty");
        result = false;
        document.getElementById("TextBoxEmail").classList.add("invalid")
        document.getElementById("email_message").style.display = ""
    }
    else if (!validateEmail(document.getElementById("TextBoxEmail").value)) {
        $("#email_message span").text("Invalid Email Address");
        result = false;
        document.getElementById("TextBoxEmail").classList.add("invalid")
        document.getElementById("email_message").style.display = ""
    }
    else if (!validateMessageLength(document.getElementById("TextBoxEmail").value, 50)) {
        $("#email_message span").text("Invalid Email Address");
        result = false;
        document.getElementById("TextBoxEmail").classList.add("invalid")
        document.getElementById("email_message").style.display = ""
    }
    else {
        document.getElementById("TextBoxEmail").classList.remove("invalid")
        document.getElementById("email_message").style.display = "none"
    }
 
    return result;
}

function LoadLoginPage() {
    window.location.href = "index.html";
}