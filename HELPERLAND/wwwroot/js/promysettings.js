const body = document.querySelector("body");
const oldpassword = document.querySelector("#oldpassword");
const newpassword = document.querySelector("#newpassword");
const confirmpassword = document.querySelector("#confirmpassword");
const errorspan = document.querySelector(".errorspan");
const changepassword = document.querySelector("#changepasswordsubmit");

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
                <strong>Success! </strong>Your password has been change successfully.
                <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                </div>`
            } else {
                errorspan.innerHTML = `<div class="alert alert-danger alert-dismissible fade show" role="alert">
                <strong>Failed! </strong>your old password is not correct please try again.
                <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                </div>`
            }
        }
    }
    catch {
        errorspan.innerHTML = `<div class="alert alert-danger alert-dismissible fade show" role="alert">
        <strong>Sorry for inconvenience! </strong>your password has not been change due to some technical error.
        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
      </div>`
    }
})

const changedetails = document.querySelector("#changedetailssubmit");
const detailsfname = document.querySelector("#detailsfname");
const detailslname = document.querySelector("#detailslname");
const detailsemail = document.querySelector("#detailsemail");
const detailsmono = document.querySelector("#detailsmono");
const detailsdobdate = document.querySelector("#detailsdobdate");
const detailsdobmonth = document.querySelector("#detailsdobmonth");
const detailsdobyear = document.querySelector("#detailsdobyear");
const nationality = document.querySelector("#detailsdobnationality");
const streetname = document.querySelector("#detailsstreetname");
const housenumber = document.querySelector("#detailshousenumber");
const postalcode = document.querySelector("#detailspostalcode");
const city = document.querySelector("#detailscity");
const errorspan2 = document.querySelector(".errorspan2");


changedetails.addEventListener('click', async (e) => {
    try {
        const formvalidater = $("#savedetail").validate();
        if (formvalidater.form()) {
            e.preventDefault();
            const gender = document.querySelector("input[name='detailsViewmodel.Gender']:checked");
            const profilepicture = document.querySelector("input[name='detailsViewmodel.ProfilePicture']:checked");
            const data = {};
            data.FirstName = detailsfname.value;
            data.LastName = detailslname.value;
            data.Email = detailsemail.value;
            data.PhoneNumber = detailsmono.value;
            data.DobDate = parseInt(detailsdobdate.value);
            data.DobMonth = parseInt(detailsdobmonth.value);
            data.DobYear = parseInt(detailsdobyear.value);
            data.StreetName = streetname.value;
            data.HouseNumber = housenumber.value;
            data.PostalCode = postalcode.value;
            data.City = city.value;
            data.Nationality = parseInt(nationality.value);
            data.ProfilePicture = profilepicture.value;
            data.Gender = parseInt(gender.value);

            body.classList.add("loading");

            const res = await fetch("/Provider/changedetail", {
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
                <strong>Success! </strong>Your Details has been change successfully.
                <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                </div>`
            }
            else {
                errorspan2.innerHTML = `<div class="alert alert-success alert-dismissible fade show" role="alert">
                <strong>Success! </strong>.
                <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                </div>`
            }
        }
    }
    catch (error) {
        body.classList.remove("loading");
        errorspan2.innerHTML = `<div class="alert alert-danger alert-dismissible fade show" role="alert">
        <strong>Sorry for inconvenience! </strong>your details has not been change due to some technical error.
        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
      </div>`
        console.log(error);
    }
})