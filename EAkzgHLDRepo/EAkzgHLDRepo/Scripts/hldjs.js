function copyTextToClipboard(text) {
    var textArea = document.createElement("textarea");

    textArea.style.position = 'fixed';
    textArea.style.top = 0;
    textArea.style.left = 0;
    textArea.style.width = '2em';
    textArea.style.height = '2em';
    textArea.style.padding = 0;
    textArea.style.border = 'none';
    textArea.style.outline = 'none';
    textArea.style.boxShadow = 'none';
    textArea.style.background = 'transparent';
    textArea.value = text;
    document.body.appendChild(textArea);
    textArea.select();
    try {
        var successful = document.execCommand('copy');
        var msg = successful ? 'successful' : 'unsuccessful';
        console.log('Copying text command was ' + msg);
        tempAlert('Link skopiowany do schowka',2000);
    } catch (err) {
        console.log('Oops, unable to copy');
        tempAlert('Błąd kopiowania do schowka', 2000);
    }
    document.body.removeChild(textArea);
}
function tempAlert(msg, duration) {
    var el = document.createElement("div");
    // el.setAttribute("style", "position:absolute;top:40%;left:20%;background-color:white;");
    el.setAttribute("class", "popUp");
    el.innerHTML = msg;
    setTimeout(function () {
        el.parentNode.removeChild(el);
    }, duration);
    document.body.appendChild(el);
}
function menuFunction(x) {
    x.classList.toggle("change");
   
    if (x.classList.contains("change")) {

        document.getElementById('menu').style.display = 'block';
    } else {
        document.getElementById('menu').style.display = 'none';
    }
}