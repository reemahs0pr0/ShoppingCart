window.onload = function () {
    let wish_list = document.getElementsByClassName("wish_icon");

    for (let i = 0; i < wish_list.length; i++) {
        wish_list[i].addEventListener("click", onWish);
    }
}

function onWish(event) {
    let logState = document.getElementById("logState");
    let elem = event.currentTarget;
    let productIdWish = elem.getAttribute("id");
    let productId = productIdWish.replace('wish', ''); // trimming the wish part of Id
    
    // console.log(productId);

    if (logState.getAttribute("state") == "yes") {
        if (elem.getAttribute("src") === "/img/hearts black.png") {
            onAddWish(productId);
            elem.setAttribute("src", "/img/hearts color.png");
        }
        else {
            onRemoveWish(productId);
            elem.setAttribute("src", "/img/hearts black.png");
        }
    }
    else
    {
        pleaseLogIn();
    }
    

}

function pleaseLogIn() {
    if (confirm("Please log in to add to your wish list")) {
        window.location.href = "/Login/Index";
    }
}

function onAddWish(productId) {
    //let productIdWish = elem.getAttribute("id");
    //let productId = productIdWish.replace('wish', ''); // trimming the wish part of Id

    //send AJAX request to add record to database
    let xhr = new XMLHttpRequest();

    //send to action method to receive AJAX call
    xhr.open("POST", "/Wish/AddToWishList");
    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");
    xhr.onreadystatechange = function () {
        if (this.readyState === XMLHttpRequest.DONE) {
            if (this.status == 200) {
                let data = JSON.parse(this.responseText);
                console.log("Successful operation: " + data.success);
            }
        }
    };
    xhr.send(JSON.stringify({
        Id: productId,
    }));

}

function onRemoveWish(productId) {
    //let productIdWish = elem.getAttribute("id");
    //let productId = productIdWish.replace('wish', ''); // trimming the wish part of Id

    //send AJAX request to remove record to database
    let xhr = new XMLHttpRequest();

    //send to action method to receive AJAX call
    xhr.open("POST", "/Wish/RemoveFromWishList");
    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");
    xhr.onreadystatechange = function () {
        if (this.readyState === XMLHttpRequest.DONE) {
            if (this.status == 200) {
                let data = JSON.parse(this.responseText);
                console.log("Successful operation: " + data.success);
            }
        }
    };
    xhr.send(JSON.stringify({
        Id: productId,
    }));
}
