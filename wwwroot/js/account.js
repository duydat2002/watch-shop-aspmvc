function validateProfile() {
  const firstname = document.querySelector("#firstname").value.trim();
  const lastname = document.querySelector("#lastname").value.trim();
  const email = document.querySelector("#email").value.trim();
  const birthdate = document.querySelector("#birthdate").value;
  const firstnameError = document.querySelector("#firstname_error");
  const lastnameError = document.querySelector("#lastname_error");
  const emailError = document.querySelector("#email_error");
  const birthdateError = document.querySelector("#birthdate_error");
  console.log("Profile");
  var check = true;

  const checkText = /[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?0-9]+/;
  const checkEmail = /^([\w\.\-]+)@([\w\-]+)((\.(\w){2,4})+)$/;

  if (firstname == "") {
    firstnameError.innerHTML = "Please enter your firstname.";
    check = false;
  } else if (checkText.test(firstname)) {
    firstnameError.innerHTML = "The firstname you provided is not valid.";
    check = false;
  } else {
    firstnameError.innerHTML = "";
  }

  if (lastname == "") {
    lastnameError.innerHTML = "Please enter your lastname.";
    check = false;
  } else if (checkText.test(lastname)) {
    lastnameError.innerHTML = "The lastname you provided is not valid.";
    check = false;
  } else {
    lastnameError.innerHTML = "";
  }

  if (email == "") {
    emailError.innerHTML = "Please enter your email.";
    check = false;
  } else if (!checkEmail.test(email)) {
    emailError.innerHTML = "The email you provided is not valid.";
    check = false;
  } else {
    emailError.innerHTML = "";
  }

  if (new Date().getFullYear() - new Date(birthdate).getFullYear() < 18) {
    birthdateError.innerHTML = "You must be at least 18 years old";
    check = false;
  } else {
    birthdateError.innerHTML = "";
  }

  return check;
}

function validatePassword() {
  const currentPassword = document
    .querySelector("input[name='password']")
    .value.trim();
  const newPassword = document
    .querySelector("input[name='newpassword']")
    .value.trim();
  const reNewPassword = document
    .querySelector("input[name='re_newpassword']")
    .value.trim();
  const currentPasswordError = document.querySelector(
    "#current_password_error"
  );
  const newPasswordError = document.querySelector("#newpassword_error");
  const reNewPasswordError = document.querySelector("#re_newpassword_error");
  console.log("Password");
  var check = true;

  if (currentPassword.length < 8) {
    currentPasswordError.innerHTML =
      "Your current password needs to be at least 8 characters long.";
    check = false;
  } else {
    currentPasswordError.innerHTML = "";
  }

  if (newPassword.length < 8) {
    newPasswordError.innerHTML =
      "Your new password needs to be at least 8 characters long.";
    check = false;
  } else {
    newPasswordError.innerHTML = "";
  }

  if (reNewPassword != newPassword) {
    reNewPasswordError.innerHTML = "Please enter the same value again.";
    check = false;
  } else {
    reNewPasswordError.innerHTML = "";
  }

  return check;
}

function validateContact() {
  const phoneNumber = document
    .querySelector("input[name='phonenumber']")
    .value.trim();
  const address = document.querySelector("input[name='address']").value.trim();
  const phoneNumberError = document.querySelector("#phonenumber_error");
  const addressError = document.querySelector("#address_error");
  var check = true;
  console.log("Address");
  const checkPhoneNumber = /^0[\d]{9}$/;

  if (phoneNumber == "") {
    phoneNumberError.innerHTML = "Please enter your phone number.";
    check = false;
  } else if (!checkPhoneNumber.test(phoneNumber)) {
    phoneNumberError.innerHTML =
      "The phone number you provided is not valid. <br/> Should look like '0XXXXXXXXX'";
    check = false;
  } else {
    phoneNumberError.innerHTML = "";
  }

  if (address == "") {
    addressError.innerHTML = "Please enter your address.";
    check = false;
  } else {
    addressError.innerHTML = "";
  }

  return check;
}

// Active tab
function activeTab() {
  const categoryTitles = document.querySelectorAll(
    ".categories__list .categories-item"
  );
  const tabItems = document.querySelectorAll(".tab-container .tab-item");

  categoryTitles.forEach((item, index) => {
    item.onclick = () => {
      document
        .querySelector(".categories__list .categories-item.active")
        .classList.remove("active");
      document
        .querySelector(".tab-container .tab-item.active")
        .classList.remove("active");

      item.classList.add("active");
      tabItems[index].classList.add("active");
    };
  });

  const params = new URLSearchParams(window.location.search);

  if (params.has("tab")) {
    const tab = params.get("tab");
    if (tab == "profile" || tab == "contact" || tab == "change-password") {
      document
        .querySelector(".categories__list .categories-item.active")
        .classList.remove("active");
      document
        .querySelector(".tab-container .tab-item.active")
        .classList.remove("active");

      document.querySelector(`#${tab}Title`).classList.add("active");
      document.querySelector(`#${tab}`).classList.add("active");
    }

    if (tab == "contact") {
      document.querySelector("input[name='phonenumber']").value =
        params.get("phonenumber");
      document.querySelector("input[name='address']").value =
        params.get("address");
      document.querySelector("input[name='ordernumber']").value =
        params.get("ordernumber");
    }
  }
}
activeTab();

// Render User
function renderUser(firstname, lastname, email, birthdate, sex) {
  const socialMale = document.querySelector("#social-male");
  const socialFemale = document.querySelector("#social-female");

  document.querySelector("#firstname").value = firstname;
  document.querySelector("#lastname").value = lastname;
  document.querySelector("#email").value = email;
  document.querySelector("#birthdate").value = birthdate;
  if (sex) socialMale.checked = true;
  else socialFemale.checked = true;
}

// Show/ Hide Password
function showPassword() {
  const inputContainers = document.querySelectorAll(".form-input-password");

  inputContainers.forEach((item) => {
    const input = item.querySelector(".form-control-password");
    const btnShow = item.querySelector(".form-password-btn");

    btnShow.addEventListener("click", () => {
      if (input.type == "password") {
        input.type = "text";
        btnShow.innerHTML = `<i class="fas fa-eye-slash"></i>`;
      } else {
        input.type = "password";
        btnShow.innerHTML = `<i class="fas fa-eye"></i>`;
      }
    });
  });
  // const inputPass = document.querySelector(".form-control-password");
  // const btnShowPass = document.querySelector(".form-password-btn");

  // btnShowPass?.addEventListener("click", () => {
  //     if (inputPass.type == "password") {
  //         inputPass.type = "text";
  //         btnShowPass.innerHTML = `<i class="fas fa-eye-slash"></i>`;
  //     } else {
  //         inputPass.type = "password";
  //         btnShowPass.innerHTML = `<i class="fas fa-eye"></i>`;
  //     }
  // })
}
showPassword();
