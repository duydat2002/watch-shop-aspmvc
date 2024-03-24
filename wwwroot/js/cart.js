const cartList = document.querySelector("#cartpage__list");
const confirmDeleteModal = document.querySelector(".confirm-delete-cart-modal");
const confirmDeleteBtn = document.querySelector(
  ".confirm-delete-cart-modal .modal-confirm"
);
let cartDeleteId;
let userContacts;

function getCart() {
  AjaxGet(`/api/cart/get-cart`, (responseText) => {
    cartList.innerHTML = responseText;

    addEvent();
    updateTotal();
    updateSumary();
  });
}
getCart();

function addEvent() {
  const items = document.querySelectorAll(".cartpage-item");
  const selectAll = document.querySelector("#order-all");
  const checkboxList = document.querySelectorAll(
    ".cartpage__list input[name='orders']"
  );

  items.forEach((item) => {
    const checkbox = item.querySelector("input[name='orders']");
    const quantity = item.querySelector(".quantity__input");
    const quantityUp = item.querySelector(".quantity-up");
    const quantityDown = item.querySelector(".quantity-down");
    const deleteBtn = item.querySelector(".cartpage-item-delete");
    const quantityMin = parseInt(quantity.min);
    const quantityMax = parseInt(quantity.max);

    checkbox.addEventListener("change", () => {
      updateSumary();
    });

    quantityUp.addEventListener("click", () => {
      quantity.stepUp();
      updateCart(item, quantity.value);
    });

    quantityDown.addEventListener("click", () => {
      quantity.stepDown();
      updateCart(item, quantity.value);
    });

    quantity.addEventListener("change", (e) => {
      const value = parseInt(e.target.value);
      if (value < quantityMin) quantity.value = 1;
      else if (value > quantityMax) quantity.value = quantityMax;
    });

    quantity.addEventListener("blur", (e) => {
      const value = parseInt(e.target.value);
      if (value < quantityMin) quantity.value = 1;
      else if (value > quantityMax) quantity.value = quantityMax;
      updateCart(item, quantity.value);
    });

    deleteBtn.addEventListener("click", () => {
      cartDeleteId = item.dataset.cartid;
      confirmDeleteModal.classList.remove("hidden");
    });
  });

  selectAll.addEventListener("change", () => {
    checkboxList.forEach((c) => {
      c.checked = selectAll.checked;
    });
    updateSumary();
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

          updateTotal();
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
        updateTotal();
        updateSumary();
      }
    }
  );
});

function updateTotal() {
  const cartItems = document.querySelectorAll("#cartpage__list .cartpage-item");
  const cartTotal = document.querySelector(".action__total-cart");
  const orderAllLabel = document.querySelector(".label-order-all");

  let count = 0;
  let total = 0;
  cartItems.forEach((item) => {
    count += parseInt(item.querySelector(".quantity__input").value);
    total += parseFloat(
      item.querySelector(".cartpage-item__total").dataset.totalprice
    );
  });

  orderAllLabel.innerHTML = `Select All (${count})`;
  cartTotal.innerHTML = `Cart total: ${total.toLocaleString("en-US")}₫`;

  getCartHeader();
}

function updateSumary() {
  const cartCheckedItems = document.querySelectorAll(
    ".cartpage-item:has(input[name='orders']:checked)"
  );
  const totalCount = document.querySelector(".subtotal-products-count");
  const subTotal = document.querySelector(".subtotal-products-price");
  const totalPrice = document.querySelector(".total-price");

  let count = 0;
  let total = 0;
  cartCheckedItems.forEach((item) => {
    count += parseInt(item.querySelector(".quantity__input").value);
    total += parseFloat(
      item.querySelector(".cartpage-item__total").dataset.totalprice
    );
  });

  totalCount.innerHTML = `${count} Items`;
  subTotal.innerHTML = `${total.toLocaleString("en-US")}₫`;
  totalPrice.innerHTML = `${total.toLocaleString("en-US")}₫`;
}

let orderContact;

const orderSuccessModal = document.querySelector(".payment-modal");
const orderError = document.querySelector(".order-error");
const missingAddressModal = document.querySelector(".missing-address-modal");

function orderClick() {
  orderError.classList.add("hidden");
  const orders = document.querySelectorAll("input[name='orders']:checked");

  if (!userContacts || userContacts.length == 0) {
    missingAddressModal.classList.remove("hidden");
    return;
  }

  if (orders.length > 0 && orderContact) {
    const carts = [];
    orders.forEach((o) => carts.push(o.value));

    AjaxPost(
      "/api/cart/add-order",
      {
        UserId,
        Carts: carts.join(","),
        PhoneNumber: orderContact.phoneNumber,
        Address: orderContact.address,
      },
      (responseText) => {
        const data = JSON.parse(responseText);
        if (data.success) {
          orderSuccessModal.classList.remove("hidden");
          setTimeout(() => {
            orderSuccessModal.classList.add("hidden");
          }, 1500);

          getCart();
        } else {
          orderError.classList.remove("hidden");
        }
      }
    );
  }
}

// Address Modal
const addressList = document.querySelector(".address-modal__list");
const addressBody = document.querySelector(".address__body");

function getUserContact() {
  AjaxGet(
    `/api/account/get-user-contact-raw?UserId=${UserId}`,
    (responseText) => {
      const data = JSON.parse(responseText);
      userContacts = data.userContacts;
      let defaultContact;

      if (userContacts && userContacts.length > 0) {
        let htmls = "";

        userContacts?.forEach((c) => {
          htmls += `
              <div class="address-modal__item" data-phonenumber='${
                c.phoneNumber
              }' data-address='${c.address}'>
                <div class="address-modal__radio">
                  <input type="radio" class="address-radio" name="address" value="${
                    c.userContactId
                  }" ${c.isDefault ? "checked" : ""}>
                  <div class="inner"></div>
                </div>
                <div class="address-modal__desc">
                  <span class="phonenumber">${c.phoneNumber}</span>
                  <span class="address">${c.address}</span>
                </div>
              </div>
            `;

          if (c.isDefault) {
            defaultContact = c;
            orderContact = c;
          }
        });

        addressList.innerHTML = htmls;

        addressBody.innerHTML = `
          <div class="address__desc">
            <span>${defaultContact.phoneNumber}</span>
            <span class="address">${defaultContact.address}</span>
          </div>
          <div class="address__button">
            <button class="change-address-button button button-gb">Change</button>
          </div>
        `;

        const changeAddressModal = document.querySelector(
          ".change-address-modal"
        );
        const changeAddressButton = document.querySelector(
          ".change-address-button"
        );
        changeAddressButton?.addEventListener("click", () => {
          changeAddressModal.classList.remove("hidden");
        });
      } else {
        addressBody.innerHTML = `
          <span class="no-address">To order, please add a delivery address</span>
          <a href="/account?tab=contact" class="missing-address__link">Add a new address</a>
        `;
      }
    }
  );
}
getUserContact();

function changeAddress() {
  const selectedAddress = document.querySelector(
    ".address-modal__item:has(input[name='address']:checked)"
  );
  const changeAddressModal = document.querySelector(".change-address-modal");
  const phoneNumber = selectedAddress.dataset.phonenumber;
  const address = selectedAddress.dataset.address;

  orderContact = { phoneNumber, address };

  changeAddressModal.classList.add("hidden");
}
