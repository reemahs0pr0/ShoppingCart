window.onload = function () {
    let form = document.getElementById("form");
    form.onsubmit = function () {
        let uname_elem = document.getElementById("username");
        let pwd_elem = document.getElementById("password");
        let cfmpwd_elem = document.getElementById("confirmpassword");
        let name_elem = document.getElementById("name");

        let username = uname_elem.value.trim();
        let password = pwd_elem.value.trim();
        let confirmpassword = cfmpwd_elem.value.trim();
        let name = name_elem.value.trim();

        if (username.length === 0 || password.length === 0 || confirmpassword.length === 0 || name.length === 0) {
            alert("Please fill up all fields.");
            return false;
        }

        if (confirmpassword != password) {
            alert("Passwords do not match!");
            return false;
        }

        return true;
    }
}