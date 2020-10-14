window.onload = function () {
    //payment confirmation (including after discount, if any)
    if (document.getElementById("total").innerHTML != "") {
        alert("You have paid $" + document.getElementById("total").innerHTML + "(10% off included) successfully!");
    }

    //logout confirmation
    let elem = document.getElementById("logout");
    elem.onclick = function () {
        if (confirm("Confirm logout?")) {
            return true;
        }
        return false;
    }
}