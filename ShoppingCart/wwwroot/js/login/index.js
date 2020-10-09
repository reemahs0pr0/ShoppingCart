window.onload = function () {
    let form = document.getElementById("form");
    form.onsubmit = function () {
        let uname_elem = document.getElementById("username");
        let pwd_elem = document.getElementById("password");

        let username = uname_elem.value.trim();
        let password = pwd_elem.value.trim();

        if (username.length === 0 || password.length === 0) {
            alert("Please fill up all fields.");
            return false;
        }

        return true;
    }
}