//declare global variable of amount list so all functions can access and make changes to it
var amountList = [];

window.onload = function () {
    //store all amount into an array
    amounts = document.getElementsByClassName("amount");
    for (let i = 0; i < amounts.length; i++) {
        amountList[i] = amounts[i].innerHTML;
    }

    //create list of all quantity input boxes
    let quantityList = document.getElementsByClassName("quantity");

    //check each quantity input box for onchange event
    for (let i = 0; i < quantityList.length; i++) {
        quantityList[i].addEventListener("change", updateQuantity);
    }

    //create list of all remove icons
    let removeList = document.getElementsByClassName("remove_icon");

    //check each icon for onclick event
    for (let i=0; i<removeList.length; i++)
    {
        removeList[i].addEventListener("click", confirmRemove);
    }

    //create onclick event for checkout
    let checkout = document.getElementById("checkout");
    checkout.addEventListener("click", checkTotal);
}

//function for update quantity
function updateQuantity(event) {
    //get new quantity
    let elem = event.currentTarget;
    let quantity = elem.value;

    //get the price for the item
    let productId = elem.getAttribute("quantity_id");
    let priceList = document.getElementsByClassName("price");
    let price = priceList[productId - 1].innerHTML;

    //calc new amount and update in array and html
    let amount = price * quantity;
    amountList[productId - 1] = amount;
    amounts[productId - 1].innerHTML = amount.toFixed(2);

    //function to sum amount of each item
    SumAmount();

    //send AJAX request to server to update quantity onto database
    let xhr = new XMLHttpRequest();

    //send to action method to receive AJAX call
    xhr.open("POST", "/Cart/UpdateQuantity");
    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");
    xhr.onreadystatechange = function () {
        if (this.readyState === XMLHttpRequest.DONE) {
            if (this.status == 200) {
                let data = JSON.parse(this.responseText);
                console.log("Successful operation: " + data.success);
            }
        }
    };
    //send product id to controller as identifier and quantity to update
    xhr.send(JSON.stringify({
        Id: productId,
        Quantity: quantity,
    }));
}

function SumAmount() {
    //sum amount into total
    let total = 0;
    for (let i = 0; i < amountList.length; i++) {
        total += parseFloat(amountList[i]);
    }

    //display empty cart message
    if (parseInt(total) == 0) {
        document.getElementById("empty").style.display = "block";
    }

    //display new total price on html
    document.getElementById("total_price").innerHTML = total.toFixed(2);
}

//function for popup confirmation box
function confirmRemove(event) {
    if (confirm("Confirm remove item from the cart?")) {
        onRemove();
    }
}

//function to remove item from cart
function onRemove()
{
    //get product id for removed item
    let elem = event.currentTarget;
    let productId = elem.getAttribute("product_id");

    //set amount for item to zero and sum amount
    amountList[productId - 1] = 0;
    SumAmount();

    //hide row of removed item
    let row = document.getElementById(productId.toString());
    row.style.display = "none";

    //send AJAX request to server to remove record from database
    let xhr = new XMLHttpRequest();

    //send to action method to receive AJAX call
    xhr.open("POST", "/Cart/RemoveItem");
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

//function to prevent checking out if cart is empty
function checkTotal(event) {
    let elem = event.currentTarget;
    let total = document.getElementById("total_price").innerHTML;
    if (total == 0.00) {
        alert("Your cart is empty!");
        elem.setAttribute("href", "#");
    }
}

/*
//function to add item to cart
function onAdd(event) {
    //get product id for removed item
    let elem = event.currentTarget;
    let productId = elem.getAttribute("product_id");

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
}*/