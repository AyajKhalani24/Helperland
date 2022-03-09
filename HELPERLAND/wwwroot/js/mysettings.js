const body = document.querySelector("body");
const oldpassword = document.querySelector("#oldpassword");
const newpassword = document.querySelector("#newpassword");
const confirmpassword = document.querySelector("#confirmpassword");
const changepassword = document.querySelector("#changepasswordsubmit");
const errorspan = document.querySelector(".errorspan");
var editaddressmodal = new bootstrap.Modal(document.getElementById('editaddress'), {
    keyboard: false
})

changepassword.addEventListener('click', async (e) => {
    try {
        const formvalidater = $("#changepassword").validate();
        if (formvalidater.form()) {
            e.preventDefault();
            const data = {};
            data.OldPassword = oldpassword.value;
            data.NewPassword = newpassword.value;
            body.classList.add("loading");

            const res = await fetch("/Customer/changepassword", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(data)
            })

            const jsondata = await res.json();
            body.classList.remove("loading");
            if (jsondata) {
                errorspan.innerHTML = `<div class="alert alert-success alert-dismissible fade show" role="alert">
                <strong>Congratulations! </strong>Your password has been change successfully.
                <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                </div>`
            } else {
                errorspan.innerHTML = `<div class="alert alert-danger alert-dismissible fade show" role="alert">
                <strong>Sorry! </strong>your old password is not correct please try again.
                <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                </div>`
            }
        }
    }
    catch {
        errorspan.innerHTML = `<div class="alert alert-danger alert-dismissible fade show" role="alert">
        <strong>Sorry for inconvenience! </strong>your details has not been change.
        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
      </div>`
    }
})


const detailsfname = document.querySelector("#detailsfname");
const detailslname = document.querySelector("#detailslname");
const detailsemail = document.querySelector("#detailsemail");
const detailsmono = document.querySelector("#detailsmono");
const detailsdobdate = document.querySelector("#detailsdobdate");
const detailsdobmonth = document.querySelector("#detailsdobmonth");
const detailsdobyear = document.querySelector("#detailsdobyear");
const detailsprelan = document.querySelector("#detailsprelan");
const savedetail = document.querySelector("#changedetailssubmit");
const errorspan2 = document.querySelector(".errorspan2");

savedetail.addEventListener('click', async (e) => {
    try {
        const formvalidater = $("#savedetail").validate();
        if (formvalidater.form()) {
            e.preventDefault();
            const data = {};
            data.FirstName = detailsfname.value;
            data.LastName = detailslname.value;
            data.Email = detailsemail.value;
            data.PhoneNumber = detailsmono.value;
            data.DobDate = detailsdobdate.value;
            data.DobMonth = detailsdobmonth.value;
            data.DobYear = detailsdobyear.value;
            data.PreLanguage = detailsprelan.value;

            body.classList.add("loading");

            const res = await fetch("/Customer/changedetail", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(data)
            })

            const jsondata = await res.json();
            body.classList.remove("loading");
            if (jsondata) {
                errorspan2.innerHTML = `<div class="alert alert-success alert-dismissible fade show" role="alert">
                <strong>Congratulations! </strong>Your Details has been change successfully.
                <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                </div>`
            }
            else {
                throw new Error();
            }
        }
    }
    catch {
        errorspan2.innerHTML = `<div class="alert alert-danger alert-dismissible fade show" role="alert">
        <strong>Sorry for inconvenience! </strong>your details has not been change.
        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
      </div>`
    }
})

const saveaddress = document.querySelector("#saveaddress");
const StreetName = document.querySelector("#StreetName");
const hno = document.querySelector("#HouseNumber");
const poc = document.querySelector("#PostalCode");
const City = document.querySelector("#City");
const PhoneNumber = document.querySelector("#addressmono");
const addressdiv = document.querySelector(".addressses");
const pilladdress = document.querySelector("#pills-profile-tab");

saveaddress.addEventListener('click', async (e) => {
    e.preventDefault();
    const data = {};
    data.StreetName = StreetName.value;
    data.hno = hno.value;
    data.poc = poc.value;
    data.City = City.options[City.options.selectedIndex].value;
    data.PhoneNumber = PhoneNumber.value;
    body.classList.add("loading");
    const res = await fetch(`/Bookservice/AddNewAddress`,
        {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(data)

        })
    const jsondata = await res.json();
    body.classList.remove("loading");

    addressdiv.innerHTML += `
    <div class="serviceaddress">
    <p class="addressservice"><b>Address:</b> ${data.StreetName}, ${data.hno}, ${data.City} ,${data.poc}<br> <b>Phone
            Number:</b> ${data.PhoneNumber}</p>
            <img src="/images/edit-icon.png" alt="">
</div>
    `
})

pilladdress.addEventListener('click', async (e) => {
    e.preventDefault();

    body.classList.add("loading");

    const res = await fetch(`/Customer/GetAddresses`, { method: "GET" });
    const adds = await res.json();

    body.classList.remove("loading");

    if (adds.length > 0) {
        addressdiv.innerHTML = "";
        adds.forEach((a) => {
            addressdiv.innerHTML += `<div class="serviceaddress" id="address_${a.addressId}">
                <p class="addressservice"><b>Address:</b> ${a.addressLine1}, ${a.addressLine2}, ${a.city} ,${a.postalCode}<br> <b>Phone
                        Number:</b> ${a.phoneNumber}</p>
                        <img src="/images/edit-icon.png" alt="" class="mx-2" style="cursor: pointer;" onclick='editaddress("${a.addressLine1}", "${a.addressLine2}", "${a.city}" ,"${a.postalCode}","${a.phoneNumber}","${a.addressId}")'>
            </div>`;
            // <input type="radio" name="Address" data-addressid="${a.addressId}" id="add${a.addressId}" class="check form-check-input add1"></input>
        });
    } else {
        addressdiv.innerHTML = "There Are No Addresses.Please Add Atleast one Address!";
    }
})

const editaddressbutton = document.querySelector("#editaddressbutton");
const StreetName1 = document.querySelector("#StreetName1");
const hno1 = document.querySelector("#HouseNumber1");
const poc1 = document.querySelector("#PostalCode1");
const City1 = document.querySelector("#City1");
const PhoneNumber1 = document.querySelector("#addressmono1");
const errorspan3 = document.querySelector(".errorspan3");

function editaddress(ad1, ad2, ci, pos, mono, addid) {
    StreetName1.value = ad1;
    hno1.value = ad2;
    poc1.value = pos;
    City1.value = ci;
    PhoneNumber1.value = mono;
    editaddressbutton.setAttribute("addressid", addid);
    editaddressmodal.show();
}

editaddressbutton.addEventListener('click', async (e) => {
    e.preventDefault();
    body.classList.add("loading");
    const data = {};
    data.StreetName = StreetName1.value;
    data.hno = hno1.value;
    data.poc = poc1.value;
    data.City = City1.options[City1.options.selectedIndex].value;
    data.PhoneNumber = PhoneNumber1.value;
    data.AddressId = parseInt(editaddressbutton.getAttribute("addressid"))
    const res = await fetch(`/Customer/EditAddress`,
        {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(data)

        })
    const jsondata = await res.json();
    if (jsondata) {
        body.classList.remove("loading");
        const adddiv = document.querySelector("#address_" + data.AddressId);
        adddiv.querySelector(
            ".addressservice"
        ).innerHTML = `<b>Address:</b> ${data.hno}, ${data.StreetName}, ${data.City} , ${data.poc}<br> <b>Phone
                Number:</b> ${data.PhoneNumber}`;
        adddiv.querySelector("img").setAttribute(
            "onclick",
            `editaddress("${data.StreetName}", "${data.hno}", "${data.City}" ,"${data.poc}","${data.PhoneNumber}","${data.AddressId}")`
        );
        editaddressmodal.hide();
        setTimeout(() => {
            errorspan3.innerHTML = `<div class="alert alert-success alert-dismissible fade show" role="alert">
            <strong>Congratulations! </strong>Your Address has been change successfully.
            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
            </div>`
        }, 500);

    }
    else {
        editaddressmodal.hide();
        setTimeout(() => {
            errorspan3.innerHTML = `<div class="alert alert-danger alert-dismissible fade show" role="alert">
            <strong>Sorry for inconvenience! </strong>your Address has not been change.
            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
          </div>`
        }, 500);


    }
})