const dt = new DataTable("#ustable", {
    dom: "Rtlip",
    responsive: false,
    pagingType: "full_numbers",
    language: {
        paginate: {
            first: "<img src='/images/firstpage.png' alt='' />",
            previous: "<img src='/images/previous.png' alt='' />",
            next: '<img src="/images/previous.png" alt="" style="transform: rotate(180deg)" />',
            last: "<img src='/images/firstpage.png' alt='' style='transform: rotate(180deg)' />",
        },
        info: "Total Record: _MAX_",
        lengthMenu: "Show_MENU_Entries",
    },
    //  buttons: ["excel"],
    columnDefs: [{ orderable: false, targets: 4 }, { orderable: false, targets: 1 }, { orderable: false, targets: 2 }],
});

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

const dashboard = document.querySelector("#dashboard");
dashboard.classList.add("current");
const body = document.querySelector("body")

var CancelModal = new bootstrap.Modal(document.getElementById('CancelModal'), {
    keyboard: false
})
var CancelButton = document.querySelector("#cancelsubmit");
var errorreason = document.querySelector("#errormsgforreason");
var cancelcomment = document.querySelector("#cancelcomment");
var errorspan = document.querySelector("#errorspan")

const cancelmodal = (serviceId) => {
    serviceDetailsModal.hide();
    RescheduleModal.hide();
    CancelModal.show();
    CancelButton.setAttribute("data-serviceid", serviceId);
};

CancelButton.addEventListener("click", async () => {
    try {
        const serviceId = CancelButton.getAttribute("data-serviceid");
        if (serviceId && validation()) {
            body.classList.add("loading");
            const res = await fetch("/Customer/CancelService", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({ ServiceId: parseInt(serviceId), CancelReason: cancelcomment.value }),
            });
            const data = await res.json();
            body.classList.remove("loading");
            CancelModal.hide();
            if (data) {
                document.querySelector("#service-" + serviceId).remove();
                cancelcomment.value = "";
                errorreason.innerHTML = "";
                errorspan.innerHTML = `<div class="alert alert-success alert-dismissible fade show" role="alert">
                <strong>Information! </strong>Your Service has been cancelled successfully.
                <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                </div>`
            } else {

            }
        }
    } catch (error) {
        CancelModal.hide();
        console.log(error.message);
        body.classList.remove("loading");
    }
});

const validation = () => {
    if (!cancelcomment.value.trim()) {
        errorreason.innerHTML = "Enter Cancel Reason !";
        cancelcomment.classList.add("input-validation-error");
        return false;
    } else {
        errorreason.innerHTML = "";
        cancelcomment.classList.remove("input-validation-error");
        return true;
    }
};

var RescheduleModal = new bootstrap.Modal(document.getElementById('reschedulemodal'), {
    keyboard: false
})
const Reschedulesubmit = document.querySelector("#Reschedulesubmit");
const scheduletime = document.querySelector("#scheduletime");
const scheduledate = document.querySelector("#scheduledate");

const reschedulemodal = (serviceId, servicedate, servicetime) => {
    serviceDetailsModal.hide();
    CancelModal.hide();

    $("#scheduledate").datepicker("setDate", new Date(servicedate.toString()));

    scheduletime.value = parseInt(servicetime.split(".")[0]) + parseInt(servicetime.split(".")[1]) / 60
    RescheduleModal.show();
    Reschedulesubmit.setAttribute("data-serviceid", serviceId);
};



Reschedulesubmit.addEventListener('click', async (e) => {
    try {
        const serviceId = Reschedulesubmit.getAttribute("data-serviceid");
        if (serviceId) {
            body.classList.add("loading");
            const res = await fetch("/Customer/RescheduleService", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({ ServiceId: parseInt(serviceId), NewServiceDate: scheduledate.value, NewServiceStartTime: parseFloat(scheduletime.value) }),
            });
            const data = await res.json();
            body.classList.remove("loading");
            RescheduleModal.hide();
            if (data) {
                // document.querySelector("#service-" + serviceId).remove();
                errorspan.innerHTML = `<div class="alert alert-success alert-dismissible fade show" role="alert">
                <strong>Information! </strong>Your Service has been cancelled successfully.
                <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                </div>`
                setTimeout(() => {
                    window.location.reload();
                }, 2500);
            } else {

            }
        }
    } catch (error) {
        RescheduleModal.hide();
        console.log(error.message);
        body.classList.remove("loading");
    }
});


const serviceDetailsModalHtml = document.querySelector("#serviceDetails");
const serviceDetailsModalBody = serviceDetailsModalHtml.querySelector(".modal-body");
const serviceDetailsModal = bootstrap.Modal.getOrCreateInstance(serviceDetailsModalHtml);
const extras = ["Inside Cabinate", "Inside Fridge", "Inside Oven", "Laundry Wash & Dry", "Interior Windows"];

const openDetailsModal = async (
    serviceId,
    serviceProviderName,
    profilePic,
    avgRating,
    serviceStartDate,
    rawServiceStartDate,
    serviceStartTime,
    serviceEndTime,
    payment
) => {
    try {
        if (serviceId) {
            CancelModal.hide();
            RescheduleModal.hide();
            body.classList.add("loading");
            const res = await fetch(`/Customer/GetServiceDetails?serviceId=${serviceId}`, { method: "GET" });
            const data = await res.json();
            body.classList.remove("loading");
            if (data.err) {
                serviceDetailsModal.hide();
            } else {
                let extraStr = "";
                if (data.extras.length > 0) {
                    data.extras.forEach((e, i) => {
                        extraStr += i == data.extras.length - 1 ? extras[e - 1] : extras[e - 1] + ", ";
                    });
                } else extraStr = "No Extra Service !";
                serviceDetailsModalBody.innerHTML = `
				<div class="${serviceProviderName.trim() == "" ? "col-12" : "col-12 col-md-7"}">
					<div class="serviceDateRelatedDetails">
						<div class="fw-bold fs-5">${serviceStartDate} ${serviceStartTime} - ${serviceEndTime}</div>
						<div><strong>Duration:</strong> ${data.duration} Hrs.</div>
					</div>
					<hr />
					<div class="serviceRelatedDetails">
						<div><strong>Service Id:</strong> ${serviceId}</div>
						<div><strong>Extras:</strong> ${extraStr}</div>
						<div class="d-flex align-items-start justify-content-start"><strong>Net Amount:</strong><span
								class="paymentAmount ms-3">${payment} €</span></div>
					</div>
					<hr />
					<div class="serviceAddressRelatedDetails">
						<div><strong>Service Address: </strong> ${data.serviceStreetName} ${data.serviceHouseNumber}, ${data.postalCode} ${data.city}</div>
						<div><strong>Phone Number: </strong> ${data.phoneNumber}</div>
						<div><strong>Email: </strong>${data.email}</div>
					</div>
					<hr />
					<div class="otherStuff">
						<div><strong>Comments: </strong> ${data.comments ? data.comments : ""}</div>
						<div><img src="/images/${data.hasPets ? "included.png" : "not-included.png"}"> I Have ${data.hasPets ? "" : "not"} Pets at Home</div>
					</div>
				</div>
				${serviceProviderName.trim() != ""
                        ? `<div class="vr mx-3 p-0 col-1 d-none d-md-block"></div>
				<div class="serviceProviderRelatedDetails col-12 col-md-4">
					<strong class="d-block fs-4 lh-sm">Service Provider Details</strong>
					<div class="serviceProvider d-flex align-items-center justify-content-start">
						<img class="rounded-circle" src="/images/${profilePic}">
						<div>
							<div class="fw-bold fs-5">${serviceProviderName}</div>
							<div
								class="stars position-relative d-inline-flex align-items-center justify-content-center">
								<svg xmlns="http://www.w3.org/2000/svg" width="17" height="16">
									<path fill-rule="evenodd" fill="#ECB91C" d="m8.176 12.865 5.045 3.735-1.334-5.78 4.453-3.84-5.871-1.402L8.176.6 5.882
										5.578.11 6.98l4.355 3.84L3.13 16.6l5.046-3.735z" />
								</svg>
								<svg xmlns="http://www.w3.org/2000/svg" width="17" height="16">
									<path fill-rule="evenodd" fill="#ECB91C" d="m8.176 12.865 5.045 3.735-1.334-5.78 4.453-3.84-5.871-1.402L8.176.6 5.882
										5.578.11 6.98l4.355 3.84L3.13 16.6l5.046-3.735z" />
								</svg>
								<svg xmlns="http://www.w3.org/2000/svg" width="17" height="16">
									<path fill-rule="evenodd" fill="#ECB91C" d="m8.176 12.865 5.045 3.735-1.334-5.78 4.453-3.84-5.871-1.402L8.176.6 5.882
										5.578.11 6.98l4.355 3.84L3.13 16.6l5.046-3.735z" />
								</svg>
								<svg xmlns="http://www.w3.org/2000/svg" width="17" height="16">
									<path fill-rule="evenodd" fill="#ECB91C" d="m8.176 12.865 5.045 3.735-1.334-5.78 4.453-3.84-5.871-1.402L8.176.6 5.882
										5.578.11 6.98l4.355 3.84L3.13 16.6l5.046-3.735z" />
								</svg>
								<svg xmlns="http://www.w3.org/2000/svg" width="17" height="16">
									<path fill-rule="evenodd" fill="#ECB91C" d="m8.176 12.865 5.045 3.735-1.334-5.78 4.453-3.84-5.871-1.402L8.176.6 5.882
										5.578.11 6.98l4.355 3.84L3.13 16.6l5.046-3.735z" />
								</svg>
								<div class="cover position-absolute top-0 end-0 h-100" style="width:${(5 - parseInt(avgRating)) * 20}%"></div>
							</div>
							<span class="lh-sm ms-1 avgRating">${avgRating}</span>
						</div>
					</div>
					<div>${parseInt(data.totalCleaning)} cleaning</div>
				</div>`
                        : ""
                    }
				<div class="buttons mt-3">
					<button
					onclick='reschedulemodal("${serviceId}","${rawServiceStartDate}","${serviceStartTime}")'
						class="rate border-0 rounded-pill d-inline-flex align-items-center justify-content-center text-white"><img
							src="/images/reschedule-icon-small.png" class="me-1">
						Reschedule</button>
					<button
					onclick='cancelmodal("${serviceId}")'
						class="cancel border-0 rounded-pill d-inline-flex align-items-center justify-content-center text-white"><img
							src="/images/close-icon-small.png" class="me-1">
						Cancel</button>
				</div>
				`;
                serviceDetailsModal.show();
            }
        }
    } catch (error) {
        body.classList.remove("loading");
        console.log(error.message)
        serviceDetailsModal.hide();
    }
};