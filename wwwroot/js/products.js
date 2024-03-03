let params = new URLSearchParams(window.location.search);
const categoryName = document
  .querySelector("#categoryName")
  .getAttribute("data-category");
const productQuantity = document.querySelector(".products__list-qty .list-qty");
const productList = document.querySelector("#products__list");
const filterClearBtn = document.querySelector(".search-filter-clearall");
const filterButton = document.querySelector(".filter__price-btn");

const renderFilterAjax = () => {
  AjaxRequest(
    "GET",
    `/products/get-colors?categories=${categoryName}${
      params.has("search") ? "&" + params.get("search") : ""
    }`,
    (responseText) => {
      const colors = JSON.parse(responseText);
      const htmls = colors
        .map(
          (color) =>
            `<label class='filter__item'>
          <input class="filter__checkbox hidden" value="${color.colorName}"
          type="checkbox" name="colors">
          <div class="filter__content">
            <span class='color__box' style="background-color: ${color.colorValue};">
            <i class='fas fa-check filter-check-icon'></i>
            </span>
            <span class='color__name'>${color.colorName}</span>
            <span class='color__qty'>(${color.quantity})</span>
          </div>
        </label>`
        )
        .join(" ");

      document.querySelector(".filter__color .filter__desc").innerHTML = htmls;
    }
  );

  AjaxRequest(
    "GET",
    `/products/get-sizes?categories=${categoryName}${
      params.has("search") ? "&" + params.get("search") : ""
    }`,
    (responseText) => {
      const sizes = JSON.parse(responseText);
      const htmls = sizes
        .map(
          (size) =>
            `<label class='filter__item'>
              <input class="filter__checkbox hidden" value="${size.sizeName}"
              type="checkbox" name="sizes">
              <div class="filter__content">
                <span class='size__box'>
                <i class='fas fa-check filter-check-icon'></i>
                </span>
                <span class='size__name'>${size.sizeName}</span>
                <span class='size__qty'>(${size.quantity})</span>
              </div>
            </label>`
        )
        .join(" ");

      document.querySelector(".filter__size .filter__desc").innerHTML = htmls;
    }
  );
};
// renderFilterAjax();

document.querySelector("#searchForm").action = "";
filterClearBtn.addEventListener("click", () => {
  params = new URLSearchParams();
  filterProducts();
  clearAllFilter();
});
filterButton.addEventListener("click", () => {
  const colorFilters = document.querySelectorAll(
    'input[name="colors"]:checked'
  );
  const sizeFilters = document.querySelectorAll('input[name="sizes"]:checked');
  const priceFrom = document.querySelector('input[name="priceFrom"]').value;
  const priceTo = document.querySelector('input[name="priceTo"]').value;

  const colors = Array.from(colorFilters).map((color) => color.value);
  const sizes = Array.from(sizeFilters).map((size) => size.value);

  if (colors.length != 0) params.set("colors", colors.join(","));
  else params.delete("colors");

  if (sizes.length != 0) params.set("sizes", sizes.join(","));
  else params.delete("sizes");

  if (priceFrom != "" || priceTo != "")
    params.set(
      "price",
      `${priceFrom != "" ? priceFrom : "0"}-${priceTo != "" ? priceTo : "inf"}`
    );
  else params.delete("price");

  filterProducts();
});

const filterProducts = () => {
  AjaxRequest(
    "GET",
    `/products/filter?categories=${categoryName}${
      params.size == 0 ? "" : `&${params.toString()}`
    }`,
    (responseText) => {
      productList.innerHTML = responseText;
      productQuantity.innerHTML =
        document.querySelectorAll(".product-item").length;

      if (params.has("price") || params.has("colors") || params.has("sizes"))
        filterClearBtn.classList.add("active");
      else filterClearBtn.classList.remove("active");

      changeUrlParams(params.toString());
      renderActiveFilter();
    }
  );
};
filterProducts();

const clearAllFilter = () => {
  const checkedFilter = document.querySelectorAll(
    'input[name="colors"]:checked, input[name="sizes"]:checked'
  );
  checkedFilter.forEach((filter) => {
    filter.checked = false;
  });
};

const renderFilters = () => {
  const queryFilter = decodeURIComponent(params);
  const filters = queryFilter.split("&");

  clearAllFilter();

  filters.forEach((filter) => {
    if (filter.includes("colors")) {
      const colors = filter.slice(7).split(",");
      colors.forEach((color) => {
        const colorCheckbox = document.querySelector(
          `input[name="colors"][value="${color}"]`
        );
        if (colorCheckbox) colorCheckbox.checked = true;
      });
    }

    if (filter.includes("sizes")) {
      const sizes = filter.slice(6).split(",");
      sizes.forEach((size) => {
        const sizeCheckbox = document.querySelector(
          `input[name="sizes"][value="${size}"]`
        );
        if (sizeCheckbox) sizeCheckbox.checked = true;
      });
    }

    if (filter.includes("price")) {
      const priceFilter = filter.slice(6).split("-");

      const priceFrom = document.querySelector('input[name="priceFrom"]');
      const priceTo = document.querySelector('input[name="priceTo"]');

      priceFrom.value = priceFilter[0];
      if (priceFilter[1] != "inf") priceTo.value = priceFilter[1];
    }
  });
};
renderFilters();

const renderActiveFilter = () => {
  const activeFilterList = document.querySelector(".active-filter__list");

  var arrayFilterActive = [];

  if (params.has("price"))
    arrayFilterActive.push({ key: "Price", value: params.get("price") });
  if (params.has("colors")) {
    params
      .get("colors")
      .split(",")
      .forEach((item) => {
        arrayFilterActive.push({ key: "Color", value: item });
      });
  }
  if (params.has("sizes")) {
    params
      .get("sizes")
      .split(",")
      .forEach((item) => {
        arrayFilterActive.push({ key: "Size", value: item });
      });
  }

  const htmls = arrayFilterActive.map((item) => {
    return ` <div class="filter-item" data-key="${item.key}" data-value="${item.value}">
      <span class="filter-item__name">${item.key}: ${item.value}</span>
      <i class="fas fa-times filter-item__close"></i>
      </div>`;
  });
  activeFilterList.innerHTML = htmls.join(" ");
  removeActiveFilter();
};

const removeActiveFilter = () => {
  const activeFilters = document.querySelectorAll(".filter-item");

  activeFilters.forEach((item) => {
    const filterName = item.dataset.key.trim().toLowerCase();
    const filterValue = item.dataset.value.trim();
    const removeFilterBtn = item.querySelector(".filter-item__close");

    removeFilterBtn.addEventListener("click", () => {
      if (filterName == "price") {
        params.delete("price");
      } else if (filterName == "color") {
        const colorFilters = params.get("colors").split(",");
        colorFilters.splice(colorFilters.indexOf(filterValue), 1);

        if (colorFilters.length != 0)
          params.set("colors", colorFilters.join(","));
        else params.delete("colors");
      } else if (filterName == "size") {
        const sizeFilters = params.get("sizes").split(",");
        sizeFilters.splice(sizeFilters.indexOf(filterValue), 1);

        if (sizeFilters.length != 0) params.set("sizes", sizeFilters.join(","));
        else params.delete("sizes");
      }

      filterProducts();
      renderFilters();
    });
  });
};

const sortHandle = () => {
  const sortContainer = document.querySelector(".sort-select");
  const sortTitle = document.querySelector(".sort__title");
  const sortValue = document.querySelector(".sort__value");
  const sortList = document.querySelector(".sort__list");
  const sortItems = document.querySelectorAll(".sort__list .sort-item");

  sortTitle.addEventListener("click", () => {
    sortList.classList.toggle("active");
  });

  window.addEventListener("click", (e) => {
    if (!sortContainer.contains(e.target)) {
      sortList.classList.remove("active");
    }
  });

  sortItems.forEach((item) => {
    item.addEventListener("click", () => {
      params.set("sort", item.dataset.value);
      sortValue.innerHTML = item.innerHTML;
      changeUrlParams(params.toString());
      filterProducts();
    });
  });

  //Render Sort
  if (params.has("sort")) {
    sortItems.forEach((item) => {
      if (item.dataset.value == params.get("sort")) {
        sortValue.innerHTML = item.innerHTML;
      }
    });
  }
};
sortHandle();

const toggleFilterNav = () => {
  const filterNavOpen = document.querySelector(".search-filter__open");
  const filterNavClose = document.querySelector(".search-filter__close");
  const filterNav = document.querySelector(".search-filter");
  const overlay = document.querySelector("#overlay");
  const html = document.querySelector("html");

  filterNavOpen.addEventListener("click", () => {
    filterNav.classList.add("active");
    overlay.classList.add("active");
    html.classList.add("dis-scroll");

    renderFilters();
  });

  filterNavClose.addEventListener("click", () => {
    filterNav.classList.remove("active");
    overlay.classList.remove("active");
    html.classList.remove("dis-scroll");
  });
  overlay.addEventListener("click", () => {
    filterNav.classList.remove("active");
    overlay.classList.remove("active");
    html.classList.remove("dis-scroll");
  });
  window.addEventListener("resize", () => {
    const windownWidth = window.innerWidth;

    if (windownWidth >= 992 && filterNav.className.includes("active")) {
      filterNav.classList.remove("active");
      overlay.classList.remove("active");
      html.classList.remove("dis-scroll");
    }
  });
};
toggleFilterNav();
