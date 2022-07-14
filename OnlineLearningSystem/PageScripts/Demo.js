function setMaterial() {
	localStorage.setItem("Hash", "ca87e85f-91c8-4bcd-b695-33f8f2a6de77");
	localStorage.setItem("Id", 187);
	localStorage.setItem("courseInstanceId", 26);
	localStorage.setItem("courseObjectiveId", 1);
	localStorage.setItem("moduleId", 1);
	localStorage.setItem("moduleObjectiveId", 1);
	localStorage.setItem("isDemo", true);
	window.location.href = "MaterialPage.html";
}

function startDemo() {
	setMaterial();
}

startDemo();