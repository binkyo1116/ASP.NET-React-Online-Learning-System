var urlFunction = "https://modelapi.letsusedata.com/api/";
var urlCompiler = "https://compilerapi.letsusedata.com/api/";

var currentUrl = window.location.href;

if (currentUrl.includes('localhost')) {
    urlFunction = "https://localhost:7061/api/"; //Core
    //urlFunction = "https://localhost:44374/api/"; //RESTModelFunctions
    urlCompiler = "https://localhost:44396/api/"; // Rest Compiler

    //urlFunction = "https://localhost:44374/api/";
    //urlCompiler = "https://localhost:44396/api/";
}

const data = {
    ActivityId: GetFromQueryString("materialId"),
    Type: "Material"
};

fetchFunction("Activity", data).then(d => {
    window.location = d.Description;
});

async function fetchDirected(url, name, data) {

    const fullUrl = url + name;

    data.StudentHash = localStorage.getItem("Hash");
    data.Hash = localStorage.getItem("Hash");
    data.hash = localStorage.getItem("Hash");

    const parameter = {
        headers: { "content-type": "application/json; charset=UTF-8" },
        body: JSON.stringify(data),
        method: "post"
    };

    return fetch(fullUrl, parameter).then(res => res.json());
}

async function fetchFunction(name, data) {

    return fetchDirected(urlFunction, name, data);
}

function GetFromQueryString(name) {
    const urlParams = new URLSearchParams(window.location.search);
    return urlParams.get(name);
}
