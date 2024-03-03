console.log(ProductVariants);

let colors = [];
let sizes = [];
let checkedColor = null;
let checkedSize = null;
let disabledColors = [],
  defaultDisabledColors = [];
let disabledSizes = [],
  defaultDisabledSizes = [];

ProductVariants.forEach((v) => {
  const colorIndex = colors.findIndex((c) => c.ColorId == v.Color.ColorId);
  const sizeIndex = sizes.findIndex((s) => s.SizeId == v.Size.SizeId);

  if (colorIndex == -1) {
    colors.push({ ...v.Color, Quantity: v.Quantity });
  } else {
    colors[colorIndex].Quantity += v.Quantity;
  }

  if (sizeIndex == -1) {
    sizes.push({ ...v.Size, Quantity: v.Quantity });
  } else {
    sizes[sizeIndex].Quantity += v.Quantity;
  }
});

console.log(sizes, colors);
sizes.forEach((s) => {
  if (s.Quantity == 0) disabledSizes.push(s.SizeId);
});
colors.forEach((c) => {
  if (c.Quantity == 0) disabledColors.push(c.ColorId);
});
defaultDisabledColors = [...disabledColors];
defaultDisabledSizes = [...disabledSizes];

const renderVariant = () => {
  const sizeList = document.querySelector(".productpage__size .items");
  sizeList.innerHTML = sizes
    .map(
      (s) =>
        `<span data-id='${s.SizeId}' class='action__item size-item ${
          s.Quantity == 0 ? "disabled" : ""
        }'>${s.SizeName}</span>`
    )
    .join(" ");

  const colorList = document.querySelector(".productpage__color .items");
  colorList.innerHTML = colors
    .map(
      (c) =>
        `<div data-id='${c.ColorId}' class='action__item color-item ${
          c.Quantity == 0 ? "disabled" : ""
        }' style="background:${c.ColorValue};"></div>`
    )
    .join(" ");
};
renderVariant();

const addEventClick = () => {
  const sizeButtons = document.querySelectorAll(
    ".productpage__size .action__item"
  );
  const colorButtons = document.querySelectorAll(
    ".productpage__color .action__item"
  );

  sizeButtons.forEach((b) => {
    clickItem("size", b, colorButtons);
  });

  colorButtons.forEach((b) => {
    clickItem("color", b, sizeButtons);
  });

  //button = nút cần gắn sự kiện này , buttons = những nút variant khác, name = 'color' | 'size'
  function clickItem(name, button, buttons) {
    button.addEventListener("click", () => {
      const buttonId = button.dataset.id;
      if (!disabledSizes.includes(buttonId)) {
        if (checkedSize == buttonId) {
          button.classList.remove("active");
          checkedSize = null;
          resetDefaultDisabled();
        } else {
          button.classList.add("active");
          checkedSize = buttonId;
        }

        buttons.forEach((otherButton) => {
          let sizeId, colorId, disableds;
          const otherId = otherButton.dataset.id;

          if (name == "size") {
            sizeId = buttonId;
            colorId = otherId;
            disableds = disabledColors;
          } else if (name == "color") {
            colorId = buttonId;
            sizeId = otherId;
            disableds = disabledSizes;
          }

          const variant = ProductVariants.find(
            (v) => v.Size.SizeId == sizeId && v.Color.ColorId == colorId
          );

          if (!disableds.includes(otherId) && variant && variant.Quantity > 0) {
            otherButton.classList.remove("disabled");
          } else {
            otherButton.classList.add("disabled");
          }
        });
      } else {
        console.log("this button is disabled!");
      }
    });
  }

  const resetDefaultDisabled = () => {
    colorButtons.forEach((cb) => {
      if (defaultDisabledColors.includes(cb.dataset.id)) {
        cb.classList.add("disabled");
      } else {
        cb.classList.remove("disabled");
      }
    });
    sizeButtons.forEach((cb) => {
      if (defaultDisabledSizes.includes(cb.dataset.id)) {
        cb.classList.add("disabled");
      } else {
        cb.classList.remove("disabled");
      }
    });
  };
};
addEventClick();
