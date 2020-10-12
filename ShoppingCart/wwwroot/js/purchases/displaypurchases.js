window.onload = function () {
    let elem = document.getElementById("logout");
    elem.onclick = function () {
        if (confirm("Confirm logout?")) {
            return true;
        }
        return false;
    }
}