var count = 0;

window.onload = function () {
    //create list of all 'Add To Cart' button
    let elemList = document.getElementsByClassName("add-to-cart");

    //check each button for onclick event
    for (let i = 0; i < elemList.length; i++) {
        elemList[i].addEventListener("click", onAddToCart);
    }

    //create list of all 'Remove from Wishlist' icon
    let remove_wish_list = document.getElementsByClassName("remove-from-wish");

    //check each icon for onclick event
    for (let i = 0; i < remove_wish_list.length; i++) {
        remove_wish_list[i].addEventListener("click", onRemoveWish);
    }

    let elem = document.getElementById("logout");
    elem.onclick = function () {
        if (confirm("Confirm logout?")) {
            return true;
        }
        return false;
    }
}

function onAddToCart(event) {
    //get product id for added item
    let elem = event.currentTarget;
    let productId = elem.getAttribute("cart_id");

    let button = document.getElementById("clickme")
    let cartValue = button.getAttribute("value");
    let cart = cartValue.split(" ");
    let count = parseInt(cart[1]) + 1;
    button.value = "Cart: " + count;

    //send AJAX request to server to remove record from database
    let xhr = new XMLHttpRequest();

    //send to action method to receive AJAX call
    xhr.open("POST", "/Cart/AddItem");
    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");
    xhr.onreadystatechange = function () {
        if (this.readyState === XMLHttpRequest.DONE) {
            if (this.status == 200) {
                let data = JSON.parse(this.responseText);
                console.log("Successful operation: " + data.success);
            }
        }
    };
    //send product id to controller as identifier
    xhr.send(JSON.stringify({
        Id: productId,
    }));
}

function onRemoveWish(event) {
    //get product id for removed item
    let elem = event.currentTarget;
    let productId = elem.getAttribute("remove_id");

    //hide row of removed item
    let row = document.getElementById(productId.toString());
    row.style.display = "none";
    count = parseInt(count) + 1;

    //create list of all 'Remove from Wishlist' icon
    let remove_wish_list = document.getElementsByClassName("remove-from-wish");

    //display empty wishlist message
    if (count == remove_wish_list.length) {
        document.getElementById("empty").style.display = "block";
    }

    //send AJAX request to remove record from database
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