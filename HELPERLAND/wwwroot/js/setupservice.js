$(function () {
    var dtToday = new Date();

    var month = dtToday.getMonth() + 1;
    var day = dtToday.getDate();
    var year = dtToday.getFullYear();
    if (month < 10)
        month = '0' + month.toString();
    if (day < 10)
        day = '0' + day.toString();

    var maxDate = year + '-' + month + '-' + day;

    $('#scheduledate').attr('min', maxDate);
});
$("#scheduledate").datepicker({
    changeMonth: true,
    changeYear: true,
    showButtonPanel: true,
    minDate: new Date(),
    showAnim: "slideDown",
    dateFormat: "dd/mm/yy",
});
$("#scheduledate").datepicker("setDate", new Date());

const successModalHtml = document.querySelector("#successModal");
const successModal = new bootstrap.Modal(successModalHtml);
var body = document.querySelector("body")
var date = document.querySelector('#scheduledate');
var BasicDate = document.querySelector('#basicdate');
var time = document.querySelector('#scheduletime');
var BasicTime = document.querySelector('#basictime');
var hours = document.querySelector('#scheduleduration');
var BasicHours = document.querySelector('#basichours');
var PerCleaning = document.querySelector('.percleaning');
var PerCleaning1 = document.querySelector('.percleaning1');
const tab1Btn = document.querySelector("#pills-home-tab");
const tab2Btn = document.querySelector("#pills-profile-tab");
const tab3Btn = document.querySelector("#pills-contact-tab");
const tab4Btn = document.querySelector("#pills-payment-tab");
const tab1 = bootstrap.Tab.getOrCreateInstance(tab1Btn);
const tab2 = bootstrap.Tab.getOrCreateInstance(tab2Btn);
const tab3 = bootstrap.Tab.getOrCreateInstance(tab3Btn);
const tab4 = bootstrap.Tab.getOrCreateInstance(tab4Btn);
const extraServicesCheckBoxs = document.querySelectorAll(".extraservicecheck");
const selectedCheckBoxs = [];

const tabsBtns = [tab1Btn, tab2Btn, tab3Btn, tab4Btn];
const handleTabsClick = (tabNumber) => {
    for (let i = tabNumber; i < tabsBtns.length; i++) {
        tabsBtns[i].setAttribute("disabled", "disabled");
        tabsBtns[i].classList.remove("completed");
    }
};
tabsBtns.forEach((tab, index) => {
    tab.addEventListener("click", () => {
        handleTabsClick(index + 1);
    });
});

var totalServiceTime = document.querySelector("#totalservicetime");
var extraServiceContainer = document.querySelector(".extraServiceContainer");

date.onchange = (e) => {
    BasicDate.innerHTML = e.target.value;
}
time.addEventListener('input', (e) => {
    BasicTime.innerHTML = e.target.value;
})
hours.addEventListener('input', (e) => {
    if (hours.value.trim() === "") BasicHours.innerHTML = "";
    else {
        if (totalServiceTime.innerHTML != "0 Hrs.") {
            totalServiceTime.innerHTML = `${parseFloat(totalServiceTime.innerHTML.replace(" Hrs.")) - parseFloat(BasicHours.innerHTML.replace(" Hrs."))
                } Hrs.`;
        }
        BasicHours.innerHTML = hours.options[hours.options.selectedIndex].innerHTML;
        totalServiceTime.innerHTML = `${parseFloat(totalServiceTime.innerHTML.replace(" Hrs.")) +
            parseFloat(hours.options[hours.options.selectedIndex].value)
            } Hrs.`;
    }
    calculateTotalPrice();
})

const calculateTotalPrice = () => (PerCleaning1.innerHTML = "$" + (parseFloat(totalServiceTime.innerHTML.replace(" Hrs.")) * 18));

const generateExtraServices = () => {
    extraServiceContainer.innerHTML = "";
    if (selectedCheckBoxs.length > 0) {
        extraServiceContainer.innerHTML = `<div class="summaryHeader fw-bold">Extras</div>`;
        selectedCheckBoxs.forEach((selectedCheckBox) => {
            extraServiceContainer.innerHTML += `
			<div class="serviceIndividualTime d-flex align-items-center justify-content-between">
			<span>${selectedCheckBox.text}</span>
			<span>30 Min.</span>
			</div>
			`;
        });
    }
    if (window.innerWidth <= 767) summaryContainer.style.bottom = `${-summaryContainer.offsetHeight}px`;
};

extraServicesCheckBoxs.forEach((extraServicesCheckBox, index) => {
    extraServicesCheckBox.addEventListener("click", () => {
        if (selectedCheckBoxs.length <= 0) {
            selectedCheckBoxs.push({ index, text: extraServicesCheckBox.getAttribute("data-extra-service-name") });
            totalServiceTime.innerHTML = `${parseFloat(totalServiceTime.innerHTML.replace(" Hrs.")) + 0.5} Hrs.`;
        } else {
            if (!extraServicesCheckBox.checked) {
                const i = selectedCheckBoxs.findIndex((s) => s.index === index);
                if (i >= 0) {
                    selectedCheckBoxs.splice(i, 1);
                }
                totalServiceTime.innerHTML = `${parseFloat(totalServiceTime.innerHTML.replace(" Hrs.")) - 0.5} Hrs.`;
            } else {
                selectedCheckBoxs.push({ index, text: extraServicesCheckBox.getAttribute("data-extra-service-name") });
                totalServiceTime.innerHTML = `${parseFloat(totalServiceTime.innerHTML.replace(" Hrs.")) + 0.5} Hrs.`;
            }
        }
        generateExtraServices();
        calculateTotalPrice();
    });
});

const CheckAvailability = document.querySelector('.checkavail');
const Postalcode = document.querySelector('#Postalcode');
const zipcodeerror = document.querySelector('.zipcodeerror');

CheckAvailability.addEventListener('click', async (e) => {
    e.preventDefault();
    body.classList.add("loading");
    const res = await fetch(`/Bookservice/CheckAvailability?PostalCode=${Postalcode.value}`, { method: "GET" })
    const data = await res.json();
    body.classList.remove("loading");
    if (data) {
        tab1Btn.classList.add("completed");
        tab2.show();
        zipcodeerror.innerHTML = "";
    }
    else {
        zipcodeerror.innerHTML = "We are not providing service in this area. We’ll notify you if any helper would start working near your area.";
    }
})

const addressdiv = document.querySelector(".addressses");
const tab2btnsubmit = document.querySelector("#tab2btnsubmit");

tab2btnsubmit.addEventListener('click', async (e) => {
    e.preventDefault();

    body.classList.add("loading");

    const res = await fetch(`/Bookservice/GetAddresses?ZipCode=${Postalcode.value}`, { method: "GET" });
    const adds = await res.json();

    body.classList.remove("loading");

    if (adds.length > 0) {
        addressdiv.innerHTML = "";
        adds.forEach((a) => {
            addressdiv.innerHTML += `<div class="serviceaddress">
                <input type="radio" name="Address" data-addressid="${a.addressId}" id="add${a.addressId}" class="check form-check-input add1">
                <p class="addressservice"><b>Address:</b> ${a.addressLine1}, ${a.addressLine2}, ${a.city} ,${a.postalCode}<br> <b>Phone
                        Number:</b> ${a.phoneNumber}</p>
            </div>`;
        });
    } else {
        addressdiv.innerHTML = "There Are No Addresses.Please Add Some !";
    }
    tab2Btn.classList.add("completed");
    tab2Btn.removeAttribute("disabled");
    tab3.show();
})

const saveaddress = document.querySelector("#saveaddress");
const StreetName = document.querySelector("#StreetName");
const hno = document.querySelector("#HouseNumber");
const poc = document.querySelector("#PostalCode");
const City = document.querySelector("#City");
const PhoneNumber = document.querySelector("#addressmono");

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
    <input type="radio" name="Address" id="add${jsondata.addressId}" class="check form-check-input add1">
    <p class="addressservice"><b>Address:</b> ${data.StreetName}, ${data.hno}, ${data.City} ,${data.poc}<br> <b>Phone
            Number:</b> ${data.PhoneNumber}</p>
</div>
    `
})
const tab3btnsubmit = document.querySelector("#tab3btnsubmit");

tab3btnsubmit.addEventListener("click", () => {
    const addressCheckboxes = document.querySelectorAll("input[name='Address'");
    if (addressCheckboxes.length > 0) {
        let anyAddressChecked = false;
        for (let index = 0; index < addressCheckboxes.length; index++) {
            const element = addressCheckboxes[index];
            element.addEventListener("click", () => {
                document.querySelector("#tab3Error").innerHTML = "";
            });
            if (element.checked) {
                anyAddressChecked = true;
            }
        }
        if (anyAddressChecked) {
            tab3Btn.removeAttribute("disabled");
            tab3Btn.classList.add("completed");
            tab4.show();
        } else {
            document.querySelector("#tab3Error").innerHTML = "No Address is selected ! Please Select any one !";
        }
    }
});





document.querySelector("#completebooking").addEventListener("click", async () => {
    try {
        const data = {};
        data.Zipcode = Postalcode.value;
        data.servicedate = date.value;
        data.servicetime = parseFloat(time.value);
        data.servicehours = parseFloat(hours.value);
        if (selectedCheckBoxs.length > 0) {
            data.ExtraServices = [];
            selectedCheckBoxs.forEach((s) => data.ExtraServices.push(s.index));
        }
        if (document.querySelector("#bookservicecomments").value) {
            data.Comments = document.querySelector("#bookservicecomments").value;
        }
        data.HasPets = document.querySelector("#petscheck").checked;
        const addressCheckboxes = document.querySelectorAll("input[name='Address'");
        for (let i = 0; i < addressCheckboxes.length; i++) {
            if (addressCheckboxes[i].checked) {
                data.AddressId = parseInt(addressCheckboxes[i].getAttribute("data-addressid"));
                break;
            }
        }
        body.classList.add("loading");
        const res = await fetch("/BookService/Index", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(data),
        });
        const jsonData = await res.json();
        body.classList.remove("loading");
        if (jsonData.serviceId) {
            let str = "";
            if (selectedCheckBoxs.length > 0) {
                selectedCheckBoxs.forEach((c, i) => {
                    str += i != selectedCheckBoxs.length - 1 ? c.text + ", " : c.text;
                });
            } else {
                str = "No Extra Service Selected";
            }
            successModalHtml.querySelector(".modal-body").innerHTML = `
            <div>Service Id = ${jsonData.serviceId}</div>
            <div>Service Date = ${date.value} ${time.value}</div>
            <div>Total Payment = ${PerCleaning1.innerHTML}</div>
            `;
            successModal.show();
            document.querySelector(".bookServiceErr").innerHTML = "";
        } else {
            throw new Error();
        }
    } catch (err) {
        console.log(err)
        document.querySelector(".bookServiceErr").innerHTML = "Internal Server Error !";
    }
});