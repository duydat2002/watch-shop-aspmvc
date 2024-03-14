const cartList = document.querySelector("#cartpage__list");
const confirmDeleteModal = document.querySelector(".confirm-delete-cart-modal");
const confirmDeleteBtn = document.querySelector(
  ".confirm-delete-cart-modal .modal-confirm"
);
let cartDeleteId;

AjaxGet(`/api/cart/get-cart`, (responseText) => {
  cartList.innerHTML = responseText;

  addEvent();
  updateSumary();
});

function addEvent() {
  const items = document.querySelectorAll(".cartpage-item");

  items.forEach((item) => {
    const quantity = item.querySelector(".quantity__input");
    const quantityUp = item.querySelector(".quantity-up");
    const quantityDown = item.querySelector(".quantity-down");
    const deleteBtn = item.querySelector(".cartpage-item-delete");

    quantityUp.addEventListener("click", () => {
      quantity.stepUp();
      updateCart(item, quantity.value);
    });

    quantityDown.addEventListener("click", () => {
      quantity.stepDown();
      updateCart(item, quantity.value);
    });

    quantity.addEventListener("change", (e) => {
      if (e.target.value <= 0) quantity.value = 1;
      updateCart(item, quantity.value);
    });

    deleteBtn.addEventListener("click", () => {
      cartDeleteId = item.dataset.cartid;
      confirmDeleteModal.classList.remove("hidden");
    });
  });

  function updateCart(cartItem, quantity) {
    const cartId = cartItem.dataset.cartid;
    const price = cartItem.querySelector(".product__price-sale").dataset
      .pricesale;
    const priceTotal = cartItem.querySelector(".cartpage-item__total");

    AjaxPost(
      "/api/cart/update-cart",
      { CartId: cartId, Quantity: quantity },
      (responseText) => {
        const data = JSON.parse(responseText);
        if (data.success) {
          priceTotal.innerHTML = `${(price * quantity).toLocaleString(
            "en-US"
          )}₫`;
          priceTotal.dataset.totalprice = price * quantity;

          updateSumary();
        }
      }
    );
  }
}

confirmDeleteBtn.addEventListener("click", () => {
  AjaxPost(
    "/api/cart/delete-cart",
    { CartId: cartDeleteId },
    (responseText) => {
      const data = JSON.parse(responseText);
      if (data.success) {
        const deletedCartItem = cartList.querySelector(
          `.cartpage-item[data-cartid='${cartDeleteId}']`
        );
        confirmDeleteModal.classList.add("hidden");
        deletedCartItem.remove();
        updateSumary();
      }
    }
  );
});

function updateSumary() {
  const cartItems = document.querySelectorAll("#cartpage__list .cartpage-item");
  const totalCount = document.querySelector(".subtotal-products-count");
  const subTotal = document.querySelector(".subtotal-products-price");
  const totalPrice = document.querySelector(".total-price");

  let count = 0;
  let total = 0;
  cartItems.forEach((item) => {
    const quantity = parseInt(item.querySelector(".quantity__input").value);
    const price = parseFloat(
      item.querySelector(".cartpage-item__total").dataset.totalprice
    );
    count += quantity;
    total += price;
  });

  totalCount.innerHTML = `${count} Items`;
  subTotal.innerHTML = `${total.toLocaleString("en-US")}₫`;
  totalPrice.innerHTML = `${total.toLocaleString("en-US")}₫`;

  getCartHeader();
}

function modalPayment() {
  const paymentModal = document.querySelector(".payment-modal");
  const paymentClose = document.querySelector(".payment-close");

  paymentClose.addEventListener("click", () => {
    paymentModal.classList.remove("active");
  });

  window.addEventListener("click", (e) => {
    if (!paymentModal.contains(e.target)) {
      paymentModal.classList.remove("active");
    }
  });
}
modalPayment();

function modalAddress() {
  const AddressModal = document.querySelector("#address_modal");
  const AddressClose = document.querySelector(".missing-address-close");

  AddressClose.addEventListener("click", () => {
    AddressModal.classList.remove("active");
    console.log(123);
  });

  window.addEventListener("click", (e) => {
    if (!AddressModal.contains(e.target)) {
      AddressModal.classList.remove("active");
    }
  });
}
modalAddress();
