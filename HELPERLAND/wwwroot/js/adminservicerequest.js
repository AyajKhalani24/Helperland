const dashboard = document.querySelector("#servicerequest");
const errorspan = document.querySelector(".errorspan");
const body = document.querySelector("body")
dashboard.classList.add("currentadmin");

const dt = new DataTable("#ustable", {
    dom: "Rtlp",
    responsive: true,
    // pagingType: "full_numbers",
    language: {
        paginate: {
            // first: "<img src='./images/firstpage.png' alt='first' />",
            previous: "<img src='/images/previous.png' alt='' />",
            next: '<img src="/images/previous.png" alt="" style="transform: rotate(180deg)" />',
            // last: "<img src='./images/firstpage.png' alt='first' style='transform: rotate(180deg)' />",
        },
        // info: "Total Record: _MAX_",
        lengthMenu: "Show_MENU_Entries",
    },
    // buttons: ["excel"],
    columnDefs: [{ orderable: false, targets: 5 }],
});


$("#AdminRescheduleViewModel_RescheduleDate").datepicker({
    changeMonth: true,
    changeYear: true,
    showButtonPanel: true,
    minDate: new Date(),
    showAnim: "slideDown",
    dateFormat: "dd/mm/yy",
});
$("#fromdate").datepicker({
    changeMonth: true,
    changeYear: true,
    showButtonPanel: true,
    showAnim: "slideDown",
    dateFormat: "mm/dd/yy",
});
$("#todate").datepicker({
    changeMonth: true,
    changeYear: true,
    showButtonPanel: true,
    showAnim: "slideDown",
    dateFormat: "mm/dd/yy",
});

const EditOrRescheduleModalHtml = document.querySelector("#EditOrRescheduleModal");
const EditOrRescheduleModalBody = EditOrRescheduleModalHtml.querySelector(".modal-body");
const EditOrRescheduleModal = bootstrap.Modal.getOrCreateInstance(EditOrRescheduleModalHtml);
const RDate = EditOrRescheduleModalBody.querySelector("#AdminRescheduleViewModel_RescheduleDate");
const RTime = EditOrRescheduleModalBody.querySelector("#AdminRescheduleViewModel_RescheduleTime");
const RStreetName = EditOrRescheduleModalBody.querySelector("#AdminRescheduleViewModel_StreetName");
const RHouseNumber = EditOrRescheduleModalBody.querySelector("#AdminRescheduleViewModel_HouseNumber");
const RReason = EditOrRescheduleModalBody.querySelector("#AdminRescheduleViewModel_RescheduleReason");
const RSubmit = EditOrRescheduleModalBody.querySelector("button[type='submit']");
const RescheduleSubmit = EditOrRescheduleModalBody.querySelector(".Reschedule");
const PostalCode = document.querySelector("#AdminRescheduleViewModel_PostalCode");
const city = document.querySelector("#AdminRescheduleViewModel_City");

async function openEditOrRescheduleModal(serviceId, serviceDate, serviceTime, houseNumber, postalCode, cityName, streetName) {

    // console.log(serviceTime);
    streetName = decodeURIComponent(streetName);
    RDate.value = serviceDate;
    RTime.value = parseFloat(serviceTime.split(".")[0]) + parseFloat(serviceTime.split(".")[1]) / 60;
    RStreetName.value = streetName;
    RHouseNumber.value = houseNumber;
    city.value = cityName;
    PostalCode.value = postalCode;
    RSubmit.setAttribute("data-service-id", serviceId);
    setTimeout(() => {
        const RescheduleFormValidator = $("#RescheduleForm").validate();
        RescheduleFormValidator.element("#" + RDate.id);
        RescheduleFormValidator.element("#" + RTime.id);
        RescheduleFormValidator.element("#" + RStreetName.id);
        RescheduleFormValidator.element("#" + RHouseNumber.id);
        RescheduleFormValidator.element("#" + PostalCode.id);
        RescheduleFormValidator.element("#" + city.id);
    }, 200);
    EditOrRescheduleModal.show();
}

PostalCode.addEventListener("focusout", async () => {
    try {
        const res = await fetch(`/Home/GetCityByPostalCode?PostalCode=${PostalCode.value}`, { method: "GET" });
        if (res.redirected) {
            window.location.href = res.url;
        }
        const data = await res.json();
        if (data.cityName) {
            city.value = data.cityName;
        } else {
            city.value = "";
        }
        const RescheduleFormValidator = $("#RescheduleForm").validate();
        RescheduleFormValidator.element("#" + city.id);
    } catch (error) {
        console.log(error.message);
    }
});


RescheduleSubmit.addEventListener("click", async (e) => {
    try {
        const RescheduleFormValidator = $("#RescheduleForm").validate();
        if (RescheduleFormValidator.form()) {
            e.preventDefault();
            const serviceId = RSubmit.getAttribute("data-service-id");
            if (serviceId) {
                EditOrRescheduleModal.hide();
                body.classList.add("loading");
                const res = await fetch("/Admin/EditOrRescheduleService", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                    },
                    body: JSON.stringify({
                        ServiceId: parseInt(serviceId),
                        RescheduleDate: RDate.value,
                        RescheduleTime: parseFloat(RTime.value),
                        StreetName: RStreetName.value,
                        HouseNumber: RHouseNumber.value,
                        PostalCode: PostalCode.value,
                        City: city.value,
                        RescheduleReason: document.querySelector("#AdminRescheduleViewModel_RescheduleReason").value,
                    }),
                });
                if (res.redirected) window.location.href = res.url;
                body.classList.remove("loading");
                const data = await res.json();
                if (data.success) {
                    errorspan.innerHTML = `<div class="alert alert-success alert-dismissible fade show mx-4" role="alert">
                <strong>Success! </strong>${data.success}
                <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                </div>`;
                    setTimeout(() => {
                        window.location.reload();
                    }, 3000);
                }
                else {
                    errorspan.innerHTML = `<div class="alert alert-danger alert-dismissible fade show mx-4" role="alert">
                    <strong>Failed! </strong>${data.err}
                    <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                    </div>`;
                }
            }
        }
    } catch (error) {
        console.log(error.message);
        EditOrRescheduleModal.hide();
        errorspan.innerHTML = `<div class="alert alert-danger alert-dismissible fade show mx-4" role="alert">
                <strong>Failed! </strong>Internal Server error
                <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                </div>`;
    }
});

const adminserviceid = document.querySelector("#adminserviceid")
const customer = document.querySelector("#customer")
const ServiceProvider = document.querySelector("#ServiceProvider")
const adminstatus = document.querySelector("#adminstatus")
const fromdate = document.querySelector("#fromdate")
const todate = document.querySelector("#todate")
const adminsubmit = document.querySelector("#adminsubmit")
const adminreset = document.querySelector("#adminreset")

adminsubmit.addEventListener("click", (e) => {
    e.preventDefault();
    body.classList.add("loading");
    $.fn.dataTable.ext.search.push(function (settings, data, dataIndex) {
        let serviceId = data[0];
        let serviceDate = data[1]
            .match(/[0-9]{2}\/[0-9]{2}\/[0-9]{4}/)[0]
        let custDetails = data[2];
        let spDetails = data[3];
        let status = data[4];
        const isServiceId = adminserviceid.value ? serviceId === adminserviceid.value : true;
        const isCustomer = customer.value === "" ? true : custDetails.includes(customer.value);
        const isSp = ServiceProvider.value === "" ? true : spDetails.includes(ServiceProvider.value);
        const isStatus = adminstatus.value === "" ? true : status === adminstatus.value;
        const dateSplited = serviceDate.split("/");
        const srdate = new Date(parseInt(dateSplited[2]), parseInt(dateSplited[1]) - 1, parseInt(dateSplited[0]));
        const isDateGreater = fromdate.value ? srdate >= new Date(fromdate.value) : true;
        const isDateSmaller = todate.value ? srdate <= new Date(todate.value) : true;
        console.log(data);
        body.classList.remove("loading");
        return isServiceId && isCustomer && isSp && isStatus && isDateGreater && isDateSmaller;
    });
    dt.draw();
});
adminreset.addEventListener("click", (e) => {
    $.fn.dataTableExt.afnFiltering.length = 0;
    dt.draw();
});

const RefundModalHtml = document.querySelector("#RefundModal");
const RefundModalBody = RefundModalHtml.querySelector(".modal-body");
const RefundModal = bootstrap.Modal.getOrCreateInstance(RefundModalHtml);
const paidamount = document.querySelector("#paidamount")
const refundedamount = document.querySelector("#refundedamount")
const inbalanceamount = document.querySelector("#inbalanceamount")
const RefundButton = RefundModalBody.querySelector(".refund");
// const RefundReason = RefundModalBody.querySelector("#AdminRescheduleViewModel_RescheduleReason");
const RefundSubmit = RefundModalBody.querySelector("button[type='submit']");


async function openRefundModal(serviceId, refundAmount, totalAmount) {
    // console.log(serviceId, refundAmount, totalAmount);
    console.log(refundAmount)
    RefundSubmit.setAttribute("data-service-id", serviceId);
    paidamount.value = `${parseInt(totalAmount)} $`
    if (refundAmount == "") {
        refundedamount.value = `0 $`
        inbalanceamount.value = `${totalAmount} $`
    }
    else {
        refundedamount.value = `${refundAmount} $`
        var inbalance = totalAmount - refundAmount;
        inbalanceamount.value = `${parseInt(inbalance)} $`
    }
    RefundModal.show();
}
const percfixedamount = document.querySelector("#percfixedamount");
const optionperfix = document.querySelector("#optionperfix");
const calculatebutton = document.querySelector("#calculatebutton");
const calculatedvalue = document.querySelector("#calculatedvalue");
const refundamounterror = document.querySelector("#refundamounterror");


calculatebutton.addEventListener('click', (e) => {
    e.preventDefault();
    var amount;
    if (optionperfix.value == "2") {
        amount = percfixedamount.value;
    }
    else if (optionperfix.value == "1") {
        amount = Math.round((parseInt(inbalanceamount.value) / 100) * parseInt(percfixedamount.value));
    }
    calculatedvalue.innerHTML = `${amount}`
})

RefundButton.addEventListener('click', async (e) => {
    try {
        const RescheduleFormValidator = $("#RefundForm").validate();
        const serviceId = RefundSubmit.getAttribute("data-service-id");
        if (RescheduleFormValidator.form()) {
            e.preventDefault();
            if (parseInt(calculatedvalue.innerHTML) != "") {
                if (serviceId) {
                    RefundModal.hide();
                    body.classList.add("loading");
                    if (parseInt(calculatedvalue.innerHTML) <= parseInt(inbalanceamount.value)) {

                        const res = await fetch("/Admin/RefundPayment", {
                            method: "POST",
                            headers: {
                                "Content-Type": "application/json",
                            },
                            body: JSON.stringify({
                                ServiceId: parseInt(serviceId),
                                RefundAmount: parseFloat(calculatedvalue.innerHTML),
                            }),
                        });
                        if (res.redirected) window.location.href = res.url;
                        body.classList.remove("loading");
                        const data = await res.json();
                        if (data) {
                            body.classList.remove("loading");
                            RefundModal.hide();
                            errorspan.innerHTML = `<div class="alert alert-success alert-dismissible fade show mx-4" role="alert">
                <strong>Success! </strong>Refund initiated.
                <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                </div>`;
                            setTimeout(() => {
                                window.location.reload();
                            }, 3000);
                        }
                        else {
                            body.classList.remove("loading");
                            RefundModal.hide();
                            errorspan.innerHTML = `<div class="alert alert-danger alert-dismissible fade show mx-4" role="alert">
                <strong>Failed! </strong>Internal Server Error.
                <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                </div>`;
                        }
                    }
                    else {
                        body.classList.remove("loading");
                        RefundModal.hide();
                        errorspan.innerHTML = `<div class="alert alert-danger alert-dismissible fade show mx-4" role="alert">
                <strong>Failed! </strong>Refund amount is greater than in balance amount.
                <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                </div>`;
                    }
                }
            }
            else {
                refundamounterror.innerHTML = "Please Calculate Value First"
            }
        }
    }
    catch (err) {
        body.classList.remove("loading");
        console.log(err.message);
    }
})
