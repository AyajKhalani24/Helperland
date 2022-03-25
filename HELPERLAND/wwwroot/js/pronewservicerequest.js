const dashboard = document.querySelector("#newservicerequest");
dashboard.classList.add("current");
const body = document.querySelector("body");
const pets = document.querySelector("#haspets");

pets.checked = window.location.search.includes("HasPets=True");
pets.addEventListener("click", () => {
    if (pets.checked) {
        window.location.href = "/Provider/NewServiceRequest?HasPets=True";
    } else {
        window.location.href = "/Provider/NewServiceRequest?HasPets=False";
    }
});



const serviceDetailsModalHtml = document.querySelector("#serviceDetails");
const serviceDetailsModalBody = serviceDetailsModalHtml.querySelector(".modal-body");
const serviceDetailsModal = bootstrap.Modal.getOrCreateInstance(serviceDetailsModalHtml);
const extras = ["Inside Cabinate", "Inside Fridge", "Inside Oven", "Laundry Wash & Dry", "Interior Windows"];

const openDetailsModal = async (
    serviceId,
    CustermerName,
    serviceStartDate,
    rawServiceStartDate,
    serviceStartTime,
    serviceEndTime,
    payment,
    postalcode,
    city,
    addressline1,
    addressline2,
    phoneNumber,
    RecordVersion
) => {
    try {
        if (serviceId) {


            body.classList.add("loading");
            const res = await fetch(`/Customer/GetServiceDetails?serviceId=${serviceId}`, { method: "GET" });
            const data = await res.json();

            const mapboxres = await fetch(
                `https://api.mapbox.com/geocoding/v5/mapbox.places/${postalcode}.json?country=de&limit=1&types=postcode&access_token=pk.eyJ1IjoiYXlhamtoYWxhbmkyNCIsImEiOiJjbDBtZGZ0MWQxM3BoM2puc3piOXNyaHBtIn0.0afaSmJViCsYnrEJYrkQEg`
            );
            const mapboxdata = await mapboxres.json();

            body.classList.remove("loading");
            if (data.err) {
                serviceDetailsModal.hide();
            } else {
                let extraStr = "";
                let extraservicename = "";
                if (data.extras.length > 0) {
                    data.extras.forEach((e, i) => {
                        extraStr += i == data.extras.length - 1 ? extras[e] : extras[e] + ", ";
                    });
                } else extraStr = "No Extra Service !";
                serviceDetailsModalBody.innerHTML = `
				<div class="col-12 col-md-6"}">
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
                    <div><strong>Customer Name: </strong> ${CustermerName}</div>
                    <div><strong>Service Address: </strong> ${data.serviceStreetName} ${data.serviceHouseNumber}, ${data.postalCode} ${data.city}</div>
						<div><strong>Phone Number: </strong> ${data.phoneNumber}</div>
						<div><strong>Email: </strong>${data.email}</div>
                        </div>
					<hr />
					<div class="otherStuff mb-2">
						<div><strong>Comments: </strong> ${data.comments ? data.comments : ""}</div>
						<div><img src="/images/${data.hasPets ? "included.png" : "not-included.png"}"> I Have ${data.hasPets ? "" : "not"} Pets at Home</div>
					</div>
                    <div>
                    <p class="acceptbutton" onclick='acceptrequest("${serviceId}","${RecordVersion}")'
                            style="cursor: pointer; text-align:center;">
                                    Accept</p>
                    </div>
				</div>
                <div class="col-12 col-md-6 mb-3 mapcontainer">
                <iframe class="w-100 h-100" src="https://maps.google.com/maps?q=${mapboxdata.features[0].geometry.coordinates[1]},${mapboxdata.features[0].geometry.coordinates[0]
                    }&z=16&output=embed" style="border:0px;"></iframe>
               </div>
                `

                serviceDetailsModal.show();
            }
        }
    } catch (error) {
        body.classList.remove("loading");
        console.log(error.message)
        serviceDetailsModal.hide();
    }
}
const dt = new DataTable("#ustable", {
    dom: "Brtlip",
    // responsive: true,
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
    buttons: ["excel"],
    columnDefs: [{ orderable: false, targets: 1 }, { orderable: false, targets: 2 }, { orderable: false, targets: 4 }]
});

async function acceptrequest(serviceId, RecordVersion) {
    try {
        if (serviceId) {
            body.classList.add("loading");
            const res = await fetch(`/Provider/AcceptService?serviceId=${serviceId}&RecordVersion=${RecordVersion}`, { method: "GET" });
            const data = await res.json();

            if (data.responce == 1) {
                serviceDetailsModal.hide();
                errorspan.innerHTML = `<div class="alert alert-success alert-dismissible fade show" role="alert">
                <strong>Success! </strong>This service has been assign to you.
                <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                </div>`;
                setTimeout(() => {
                    window.location.reload();
                }, 3000);
            }
            else if (data.responce == 2) {
                serviceDetailsModal.hide();

                errorspan.innerHTML = `<div class="alert alert-danger alert-dismissible fade show" role="alert">
                <strong>Failed! </strong>You have another service at given date and time so you can't accept this service.
                <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                </div>`;
            }
            else if (data.responce == 3) {
                serviceDetailsModal.hide();

                errorspan.innerHTML = `<div class="alert alert-danger alert-dismissible fade show" role="alert">
                <strong>Failed! </strong>This service is not available now.
                <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                </div>`;
            }
        }
    }
    catch {
        errorspan.innerHTML = `<div class="alert alert-danger alert-dismissible fade show" role="alert">
                <strong>Failed! </strong>Sorry for inconvenince this service hasn't been assign to you.
                <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                </div>`;
    }
}