const dashboard = document.querySelector("#servicehistory");
dashboard.classList.add("current");
const body = document.querySelector("body");

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
    columnDefs: [{ orderable: false, targets: 2 }]
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
    phoneNumber
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