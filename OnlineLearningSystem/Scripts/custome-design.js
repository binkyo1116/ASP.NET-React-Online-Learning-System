

//===========================================================================
//=========================Interaction.aspx==================================
var text = document.getElementById("txtCode1");
var str = text.innerHTML,
    reg = /red|blue|green|orange/ig; //g is to replace all occurances

//fixing a bit
var toStr = String(reg);
var color = (toStr.replace('\/g', '|')).substring(1);

//split it baby
var colors = color.split("|");

if (colors.indexOf("blue") > -1) {
    str = str.replace(/public/g, '<span style="color:blue;">public</span>');
}

if (colors.indexOf("blue") > -1) {
    str = str.replace(/class/g, '<span style="color:blue;">class</span>');
}

if (colors.indexOf("blue") > -1) {
    str = str.replace(/static/g, '<span style="color:blue;">static</span>');
}

if (colors.indexOf("blue") > -1) {
    str = str.replace(/bool/g, '<span style="color:blue;">bool</span>');
}
if (colors.indexOf("blue") > -1) {
    str = str.replace(/bool/g, '<span style="color:blue;">partial</span>');
}
if (colors.indexOf("blue") > -1) {
    str = str.replace(/bool/g, '<span style="color:blue;">protected</span>');
}
if (colors.indexOf("blue") > -1) {
    str = str.replace(/bool/g, '<span style="color:blue;">void</span>');
}
if (colors.indexOf("blue") > -1) {
    str = str.replace(/bool/g, '<span style="color:blue;">namespace</span>');
}

document.getElementById("custome-codeblock").innerHTML = str;
