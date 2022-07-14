/*
Update Profile
*/

var g_UserImgFileBase64String = "";


/*************************************************************************************************/
function bindLabel(fullName, email, photo) {
    document.getElementById("fullName").value = fullName;
    document.getElementById("eMail").value = email;
    document.getElementById("ProfileImageView").src = (photo == null || photo == "") ? "Content/images/photo.jpg" : photo;

    document.getElementById("fileUploadImage").addEventListener("input", readFile);
}


/*************************************************************************************************/
function displayStudentCredentials() {
    const elmLoader = document.getElementById("loader-spinner"), data = {};

    elmLoader.style.display = "block"; // Show Loader Image

    fetchFunction("ProfileInfo", data).then(d => {
        if (d.Photo == "") d.Photo = localStorage.getItem("StudentProfileImage"); // Not sure if this is ok..!

        bindLabel(d.FullName, d.Email, d.Photo);
        elmLoader.style.display = "none"; // Hide Spinner
    });
}


/*************************************************************************************************/
function ClearMessageNotes(bClearInvalidInputs = true) {
    msgHide("msg-imageupdate");
    msgHide("msg-pwupdate");

    if (bClearInvalidInputs) {
        $("#passwordCurrent").parent().removeClass("invalid");
        $("#passwordNew").parent().removeClass("invalid");
        $("#passwordConfirm").parent().removeClass("invalid");
    }
}


/*************************************************************************************************/
function submitImage() {
    const elmLoader = document.getElementById("loader-spinner");

    ClearMessageNotes();
    elmLoader.style.display = "block"; // Show Loader Image

    //js:   data:image/jpeg;base64,/9j/4AA
    //c#:   data:image;base64,/9j/4AA

    var substr = "base64,";
    if (g_UserImgFileBase64String != "") {
        var index = g_UserImgFileBase64String.indexOf(substr);

        if (index == -1) return;

        var imgBase64String = g_UserImgFileBase64String.substring(index + substr.length);
        if (imgBase64String != "") {
            const data = {
                Photo: imgBase64String,
                InfoType: "Photo"
            };

            fetchFunction("UpdateProfile", data).then(d => {
                if (d.Result == "OK") {
                    // Update the local storage data (C# way)
                    let newImgBase64 = "data:image;base64," + imgBase64String;
                    localStorage.setItem('StudentProfileImage', newImgBase64);
                    $('#user-profile-image').attr('src', newImgBase64);

                    msgShow("msg-imageupdate", "success", "Picture uploaded sucsssfully.");
                    elmLoader.style.display = "none"; // Hide Spinner
                }
            });
        }
    }
    else {
        msgShow("msg-imageupdate", "error", "Incorrect file");
        elmLoader.style.display = "none"; // Hide Spinner
    }
}


/*************************************************************************************************/
function readFile() {
    ClearMessageNotes();

    if (this.files && this.files[0]) {
        if (this.files[0].type == "image/png" || this.files[0].type == "image/jpeg") {
            var FR = new FileReader();

            FR.addEventListener("load", function (e) {
                document.getElementById("ProfileImageView").src = e.target.result;
                g_UserImgFileBase64String = e.target.result;

                submitImage();
            });

            FR.readAsDataURL(this.files[0]);
        }
        else {
            msgShow("msg-imageupdate", "error", "Incorrect file");
        }
    }
}


/*************************************************************************************************/
function checkPassword(newPassword, confirmPassword, currentPassword) {
    var result = true;

    if (currentPassword == '') {
        inputboxSetInvalid($("#passwordCurrent"), "This field can't be empty");
        result = false;
    }

    if (newPassword == '') {
        inputboxSetInvalid($("#passwordNew"), "This field can't be empty");
        result = false;
    }

    if (confirmPassword == '') {
        inputboxSetInvalid($("#passwordConfirm"), "This field can't be empty");
        result = false;
    }

    if (newPassword != confirmPassword) {
        inputboxSetInvalid($("#passwordConfirm"), "Passwords do not match");
        result = false;
    }

    return result;
}


/*************************************************************************************************/
function updatePassword() {
    const elmLoader = document.getElementById("loader-spinner");

    ClearMessageNotes();
    elmLoader.style.display = "block"; // Show Loader Image

    var currentPassword = document.getElementById("passwordCurrent").value;
    var newPassword = document.getElementById("passwordNew").value;
    var confirmPassword = document.getElementById("passwordConfirm").value;

    if (checkPassword(newPassword, confirmPassword, currentPassword)) {
        const data = {
            CurrentPassword: currentPassword,
            NewPassword: newPassword,
            InfoType: "Password"
        };

        fetchFunction("UpdateProfile", data).then(d => {
            if (d.Result == "OK") {
                document.getElementById("passwordCurrent").value = "";
                document.getElementById("passwordNew").value = "";
                document.getElementById("passwordConfirm").value = "";

                msgShow("msg-pwupdate", "success", "Your password was updated successfully");
            } else {
                inputboxSetInvalid($("#passwordCurrent"), "Incorrect Password");
            }
            elmLoader.style.display = "none"; // Hide Spinner
        });
    }
    else {
        elmLoader.style.display = "none"; // Hide Spinner
    }
}



/*
Script start
*/

// Handle pressing "change password" button
$('#profile-changepassword').on('click', function () {
    updatePassword();
});

// Clear error messages on input (without clearing ALL inputs, the active one will be cleared only)
$("input.textbox").on('input', function () {
    ClearMessageNotes(false);
});


displayStudentCredentials();
