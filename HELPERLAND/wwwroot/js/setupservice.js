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
    const res = await fetch(`/Bookservice/CheckAvailability?PostalCode=${Postalcode.value}`, { method: "GET" })
    const data = await res.json();
    if (data) {
        tab1Btn.classList.add("completed");
        tab2.show();
        zipcodeerror.innerHTML = "";
    }
    else {
        zipcodeerror.innerHTML = "We are not providing service in this area. We’ll notify you if any helper would start working near your area.";
    }
})

const tab2btnsubmit = document.querySelector("#tab2btnsubmit");
tab2btnsubmit.addEventListener('click', (e) => {
    e.preventDefault();
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
    data.City = City.value;
    data.PhoneNumber = PhoneNumber.value;
    const res = await fetch(`/Bookservice/AddNewAddress`,
        {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            }
        })

})