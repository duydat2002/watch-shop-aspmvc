const addCart = () => {
  if (!UserId) {
    notUserModal.classList.remove("hidden");
  } else {
    const ProductId = document.querySelector("input[name='ProductId']").value;
    const Quantity = document.querySelector("input[name='Quantity']").value;

    AjaxPost(
      "/api/cart/add-to-cart",
      { ProductId, Quantity },
      (responseText) => {
        const data = JSON.parse(responseText);

        let modal;
        if (data.success)
          modal = document.querySelector(".add-to-cart-modal-success");
        else modal = document.querySelector(".add-to-cart-modal-fail");

        modal.classList.remove("hidden");

        setTimeout(() => {
          modal.classList.add("hidden");
        }, 1500);
      }
    );
  }
};

function productpageSwiper() {
  const productpageSwiper = new Swiper(".productpage__slider .swiper", {
    speed: 500,
    slidesPerView: 3,
    spaceBetween: 10,
    navigation: {
      nextEl: ".productpage__slider .swiper-next",
      prevEl: ".productpage__slider .swiper-prev",
    },
    breakpoints: {
      768: {
        slidesPerView: 2,
        spaceBetween: 10,
      },
      992: {
        slidesPerView: 3,
        spaceBetween: 10,
      },
      1200: {
        slidesPerView: 4,
        spaceBetween: 10,
      },
    },
  });
}
productpageSwiper();

function productpageAction() {
  const quantityInput = document.querySelector(
    ".productpage__quantity .quantity__input"
  );
  const quantityUp = document.querySelector(
    ".productpage__quantity .quantity-up"
  );
  const quantityDown = document.querySelector(
    ".productpage__quantity .quantity-down"
  );
  const quantityMin = parseInt(quantityInput.min);
  const quantityMax = parseInt(quantityInput.max);

  quantityUp.addEventListener("click", () => {
    quantityInput.stepUp();
  });

  quantityDown.addEventListener("click", () => {
    quantityInput.stepDown();
  });

  quantityInput.addEventListener("change", (e) => {
    const value = parseInt(e.target.value);
    if (value < quantityMin) quantityInput.value = 1;
    else if (value > quantityMax) quantityInput.value = quantityMax;
  });

  quantityInput.addEventListener("blur", (e) => {
    const value = parseInt(e.target.value);
    if (value < quantityMin) quantityInput.value = 1;
    else if (value > quantityMax) quantityInput.value = quantityMax;
  });

  const productpageImg = document.querySelector(".productpage__mainimg > img");
  const productpageItems = document.querySelectorAll(".productpage-item");
  productpageItems.forEach((item) => {
    item.addEventListener("click", () => {
      const productpageActive = document.querySelector(
        ".productpage-item.active"
      );
      const srcImg = item.querySelector("img").src;

      productpageActive?.classList.remove("active");
      item.classList.add("active");
      productpageImg.src = `${srcImg}`;
    });
  });
}
productpageAction();
