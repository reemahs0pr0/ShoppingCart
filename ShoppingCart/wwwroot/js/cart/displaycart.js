//declare global variable of amount list so all functions can access and make changes to it
var amountList = [];
var noOfProducts = 6; //update total number of products here 

for (var i = 0; i < noOfProducts; i++) {
    amountList[i] = 0;
}

window.onload = function () {
    //store all amount into an array
    amounts = document.getElementsByClassName("amount");

    for (let i = 0; i < amounts.length; i++) {
        let elem = amounts[i];
        let productId = elem.getAttribute("amount_id");
        amountList[productId - 1] = amounts[i].innerHTML;
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

    //create onclick event for checkout
    let submitCoupon = document.getElementById("validBtn");
    submitCoupon.addEventListener("click", onSubmitCoupon);
}

//function for update quantity
function updateQuantity(event) {
    //get new quantity
    let elem = event.currentTarget;
    let quantity = elem.value;
    let price = 0;

    //get the price for the item
    let productId = elem.getAttribute("quantity_id");
    let priceList = document.getElementsByClassName("price");
    for (let i = 0; i < priceList.length; i++) {
        let elem = priceList[i];
        let id = elem.getAttribute("price_id");
        if (id == productId) {
            price = priceList[i].innerHTML;
        }
    }

    //calc new amount and update in array and html
    let amount = price * quantity;
    amountList[productId - 1] = amount;
    let amounts = document.getElementsByClassName("amount");
    for (let i = 0; i < amounts.length; i++) {
        let elem = amounts[i];
        let id = elem.getAttribute("amount_id");
        if (id == productId) {
            amounts[i].innerHTML = amount.toFixed(2);
        }
    }

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

//function to validate coupon
function onSubmitCoupon(event) {

    let msg = document.getElementById("msg");

    // get couponId entered by user
    let elemCouponId = document.getElementById("couponCode");
    let couponId = elemCouponId.value.trim();

    if (couponId.length !== 0) {
        // send AJAX request to server to validate coupon from database
        let xhr = new XMLHttpRequest();

        //send to action method to receive AJAX call
        xhr.open("POST", "/Coupon/ValidateCoupon");
        xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");
        xhr.onreadystatechange = function () {
            if (this.readyState === XMLHttpRequest.DONE) {
                if (this.status == 200) {
                    let data = JSON.parse(this.responseText);
                    console.log("Successful operation: " + data.message);

                    // result from validate coupon
                    msg.innerHTML = data.message;
                }
            }
        };
        // send couponId to controller as identifier
        let data = {
            Id: couponId
        };
        xhr.send(JSON.stringify(data));
    }
    else {
        // if empty coupon code field entered
        msg.innerHTML = "Coupon field is empty";
    }
}