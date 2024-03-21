function getUserContact() {
  const contactList = document.querySelector("#contact_list");
  AjaxGet(`/api/account/get-user-contact?UserId=${UserId}`, (responseText) => {
    contactList.innerHTML = responseText;

    handleContactButtons();
  });
}
getUserContact();

function validateProfile() {
  const firstname = document.querySelector("#firstname").value.trim();
  const lastname = document.querySelector("#lastname").value.trim();
  const email = document.querySelector("#email").value.trim();
  const birthdate = document.querySelector("#birthdate").value;
  const firstnameError = document.querySelector("#firstname_error");
  const lastnameError = document.querySelector("#lastname_error");
  const emailError = document.querySelector("#email_error");
  const birthdateError = document.querySelector("#birthdate_error");
  const errorDom = document.querySelector(".profile__container .server-error");
  const successDom = document.querySelector(
    ".profile__container .server-success"
  );
  var check = true;

  errorDom.classList.add("hidden");
  successDom.classList.add("hidden");

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

  if (check) {
    const gender = document.querySelector("input[name='gender']:checked").value;

    AjaxPost(
      "/api/account/update-user-info",
      {
        UserId,
        FirstName: firstname,
        Lastname: lastname,
        Email: email,
        Birthdate: birthdate,
        Gender: gender == "male",
      },
      (responseText) => {
        const data = JSON.parse(responseText);

        if (data.success) {
          const accountName = document.querySelector(".account__name");
          accountName.innerHTML = firstname + " " + lastname;
          // location.reload();
          successDom.classList.remove("hidden");
        } else {
          errorDom.classList.remove("hidden");
        }
      }
    );
  }
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
  const errorDom = document.querySelector(
    ".change-password__container .server-error"
  );
  const successDom = document.querySelector(
    ".change-password__container .server-success"
  );
  var check = true;

  errorDom.classList.add("hidden");
  successDom.classList.add("hidden");

  if (currentPassword.length < 6) {
    currentPasswordError.innerHTML =
      "Your current password needs to be at least 6 characters long.";
    check = false;
  } else {
    currentPasswordError.innerHTML = "";
  }

  if (newPassword.length < 6) {
    newPasswordError.innerHTML =
      "Your new password needs to be at least 6 characters long.";
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

  if (check) {
    AjaxPost(
      "/api/account/update-user-password",
      { UserId, OldPassword: currentPassword, NewPassword: newPassword },
      (responseText) => {
        const data = JSON.parse(responseText);

        if (data.success) {
          successDom.classList.remove("hidden");
        } else {
          errorDom.innerHTML =
            data.message ?? "Something went wrong, try again in a few minutes.";
          errorDom.classList.remove("hidden");
        }
      }
    );
  }
}

var userContactId = null;
function setUserContactId(id) {
  userContactId = id;
}

function handleContactButtons() {
  const contactInput = document.querySelector(".address__container");
  const contactItems = document.querySelectorAll(
    ".address__container .contact-item"
  );
  const phoneNumberDom = document.querySelector("input[name='phonenumber']");
  const addressDom = document.querySelector("input[name='address']");

  contactItems.forEach((item) => {
    const contactId = item.dataset.contactid;
    const address = item.dataset.address;
    const phonenumber = item.dataset.phonenumber;
    const editBtn = item.querySelector(".contact-edit");
    const deleteBtn = item.querySelector(".contact-delete");
    const setDefaultBtn = item.querySelector(".contact-setdefault");
    const errorDom = document.querySelector(
      ".address__container .server-error"
    );

    editBtn.addEventListener("click", () => {
      userContactId = contactId;

      phoneNumberDom.value = phonenumber;
      addressDom.value = address;

      contactInput.scrollIntoView();
    });

    deleteBtn?.addEventListener("click", () => {
      userContactId = contactId;

      AjaxPost(
        "/api/account/delete-user-contact",
        { UserId, UserContactId: userContactId },
        (responseText) => {
          const data = JSON.parse(responseText);

          if (data.success) {
            getUserContact();
          } else {
            errorDom.classList.remove("hidden");
          }
        }
      );
    });

    setDefaultBtn.addEventListener("click", () => {
      userContactId = contactId;

      AjaxPost(
        "/api/account/set-default-user-contact",
        { UserId, UserContactId: userContactId },
        (responseText) => {
          const data = JSON.parse(responseText);

          if (data.success) {
            getUserContact();
          } else {
            errorDom.classList.remove("hidden");
          }
        }
      );
    });
  });
}

// type = 'create' || 'update'
function validateContact(type) {
  const phoneNumber = document
    .querySelector("input[name='phonenumber']")
    .value.trim();
  const address = document.querySelector("input[name='address']").value.trim();
  const phoneNumberError = document.querySelector("#phonenumber_error");
  const addressError = document.querySelector("#address_error");
  const errorDom = document.querySelector(".address__container .server-error");
  const successDom = document.querySelector(
    ".address__container .server-success"
  );
  var check = true;
  const checkPhoneNumber = /^0[\d]{9}$/;

  errorDom.classList.add("hidden");
  successDom.classList.add("hidden");

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

  if (check) {
    if (type == "create") {
      AjaxPost(
        "/api/account/add-user-contact",
        {
          UserId,
          Address: address,
          PhoneNumber: phoneNumber,
        },
        (responseText) => {
          const data = JSON.parse(responseText);

          if (data.success) {
            successDom.classList.remove("hidden");
            getUserContact();
          } else {
            errorDom.innerHTML =
              data.message ??
              "Something went wrong, try again in a few minutes.";
            errorDom.classList.remove("hidden");
          }
        }
      );
    } else if (type == "update") {
      AjaxPost(
        "/api/account/update-user-contact",
        {
          UserId,
          UserContactId: userContactId,
          Address: address,
          PhoneNumber: phoneNumber,
        },
        (responseText) => {
          const data = JSON.parse(responseText);

          if (data.success) {
            successDom.classList.remove("hidden");
            getUserContact();
          } else {
            errorDom.classList.remove("hidden");
          }
        }
      );
    }
  }
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
