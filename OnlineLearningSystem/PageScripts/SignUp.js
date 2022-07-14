function signUp(theForm)
{
   
    var email = document.getElementById("Email").value;
    var confirmEmail = document.getElementById("ConfirmEmail").value;

    if (email != confirmEmail) {
        alert("\nEmail did not match: Please try again...");
        return false;
    }

    var password = document.getElementById("Password").value;
    var confirmPassword = document.getElementById("ConfirmPassword").value;

    if (password != confirmPassword) {
        alert("\nPassword did not match: Please try again...");
        return false;
    }

    const data = {
        FullName: document.getElementById("FullName").value,
        Email: document.getElementById("Email").value,
        Username: document.getElementById("Username").value,
        Password: document.getElementById("Password").value
    };

    fetchFunction("SignUp", data).then(d => {
        if (d.Success) {

            document.getElementById("FullName").value = "";
            document.getElementById("Email").value = "";
            document.getElementById("ConfirmEmail").value = "";
            document.getElementById("Username").value = "";
            document.getElementById("Password").value = "";
            document.getElementById("ConfirmPassword").value = "";
            document.getElementById("lblMessage").style.color = 'green';
            document.getElementById("lblMessage").innerHTML = "Signed up successfully!";

            window.setTimeout(function () {
                window.location.href = "index.html";
            }, 3000);

        } else {
            document.getElementById("lblMessage").innerHTML = d.Message;
            document.getElementById("lblMessage").style.color = 'red';
        }
    });

    return false;
}

 