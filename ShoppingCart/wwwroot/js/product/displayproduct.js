window.onload = function () {
    //create list of all 'Add To Cart' button
    let elemList = document.getElementsByClassName("add-to-cart");

    //check each button for onclick event
    for (let i = 0; i < elemList.length; i++) {
        elemList[i].addEventListener("click", onAddToCart);
    }
}

function onAddToCart(event) {
    //get product id for added item
    let elem = event.currentTarget;
    let productId = elem.getAttribute("id");

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