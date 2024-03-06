function quantityControl() {
    const cartpageItems = document.querySelectorAll(".cartpage-item");
    
    cartpageItems.forEach((item) => {
        const quantityInput = item.querySelector(".cartpage-item .quantity__input");
        const quantityUp = item.querySelector(".cartpage-item .quantity-up");
        const quantityDown = item.querySelector(".cartpage-item .quantity-down");
        const productPrice = item.querySelector(".cartpage-item .product__price-sale");
        const cartpageTotal = item.querySelector(".cartpage-item .cartpage-item__total");
        
        //quantityInput.value = 1;
        cartpageTotal.innerHTML = `$${(parseFloat(productPrice.innerHTML.slice(1)) * quantityInput.value).toFixed(2)}`;

        quantityUp.addEventListener("click", () => {
            const quantityNumber = parseInt(quantityInput.value);
            quantityInput.value = quantityNumber + 1;
            changePrice(cartpageTotal, quantityInput, productPrice);
        })
        quantityDown.addEventListener("click", () => {
            const quantityNumber = parseInt(quantityInput.value);
            quantityInput.value = quantityNumber == 1 ? 1 : quantityNumber - 1;
            changePrice(cartpageTotal, quantityInput, productPrice);
        })

        quantityInput.addEventListener("keyup", () => {
            changePrice(cartpageTotal, quantityInput, productPrice);
        })

        quantityInput.addEventListener("blur", () => {
            if (quantityInput.value == "") {
                quantityInput.value = 1;
                changePrice(cartpageTotal, quantityInput, productPrice)
            }
        })
    })

    function changePrice(cartpageTotal, quantityInput, productPrice) {
        if (isNaN(quantityInput.value) || (quantityInput.value <= 0 && quantityInput.value != ""))
            quantityInput.value = 1;

        cartpageTotal.innerHTML = `$${parseFloat(quantityInput.value*productPrice.innerHTML.slice(1)).toFixed(2)}`;
        
        changeSumary();
    }

    function changeSumary() {
        const productCount = document.querySelector(".subtotal-products-count");
        const productPriceSubtotal = document.querySelector(".subtotal-products-price");
        const productShipping = document.querySelector(".subtotal-shipping-price");
        const productPriceTotal = document.querySelector(".total-price");

        var count = 0, priceSubtotal = 0, priceTotal = 0;

        cartpageItems.forEach((item) => {
            const quantityInput = item.querySelector(".cartpage-item .quantity__input");
            const cartpageTotal = item.querySelector(".cartpage-item .cartpage-item__total");

            count += parseInt(quantityInput.value);
            priceSubtotal += parseFloat(cartpageTotal.innerHTML.slice(1));
        })
        priceTotal = priceSubtotal - productShipping.innerHTML.slice(1)

        productCount.innerHTML = `${count} items`;
        productPriceSubtotal.innerHTML = `$${priceSubtotal.toFixed(2)}`;
        productPriceTotal.innerHTML = `$${priceTotal.toFixed(2)}`;
    }
    changeSumary();
}
quantityControl();

function modalPayment() {
    const paymentModal = document.querySelector(".payment-modal");
    const paymentClose = document.querySelector(".payment-close")

    paymentClose.addEventListener("click", () => {
        paymentModal.classList.remove("active");
    })

    window.addEventListener("click", (e) => {
        if (!paymentModal.contains(e.target)) {
            paymentModal.classList.remove("active");
        }
    })
}
modalPayment();

function modalAddress() {
    const AddressModal = document.querySelector("#address_modal");
    const AddressClose = document.querySelector(".missing-address-close")

    AddressClose.addEventListener("click", () => {
        AddressModal.classList.remove("active");
        console.log(123);
    })

    window.addEventListener("click", (e) => {
        if (!AddressModal.contains(e.target)) {
            AddressModal.classList.remove("active");
        }
    })
}
modalAddress();