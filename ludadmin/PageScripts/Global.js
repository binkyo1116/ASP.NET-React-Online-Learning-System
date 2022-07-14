var urlTeachers = "";
//var urlCompiler = "https://restcompilerfunctions.azurewebsites.net/api/";

var currentUrl = window.location.href;

if (currentUrl.includes('localhost')) {
    urlTeachers = "https://localhost:44317/api/";
    //urlCompiler = "https://localhost:44396/api/";
}
console.log(urlTeachers);


async function fetchDirected(url, name, data) {

    const fullUrl = url + name;

    const parameter = {
        headers: { "content-type": "application/json; charset=UTF-8" },
        body: JSON.stringify(data),
        method: "post"
    };

    return fetch(fullUrl, parameter).then(res => res.json());
}

async function fetchFunction(name, data) {

    return fetchDirected(urlTeachers, name, data);
}

function onEnterClick(e, button)
{
    if (e.keyCode == 13) // 27=esc
    {
        button.click();
    }
}

function checkUser()
{
/*
    const hash = localStorage.getItem("Hash")
    if (hash == null) {
        window.location.href = "Login.html";
    }
*/
}

function getHeader() {
    $(document).ready(function () {

        $('#includedContent').load("Logout.html");

    });
}

getHeader();
