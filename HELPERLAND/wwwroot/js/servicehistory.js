const servicehistory = document.querySelector("#servicehistory");
servicehistory.classList.add("current");
const body = document.querySelector("body")

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
    columnDefs: [{ orderable: false, targets: 4 }, { orderable: false, targets: 1 }, { orderable: false, targets: 2 }],

});


// const stars = document.querySelectorAll(".stars svg path")
// // const ratings = document.querySelector("#ratings")
// const cover = document.querySelector(".cover")
// for (let i = 0; i < stars.length; i++) {
//     stars[i].addEventListener("click", () => {
//         // ratings.innerHTML = i + 1;
//         cover.style.width = ((4 - i) * 20) + "%"
//     })
//     stars[i].addEventListener("mouseover", () => {
//         // ratings.innerHTML = i + 1;
//         cover.style.width = ((4 - i) * 20) + "%"
//     })
//     stars[i].addEventListener("mouseout", () => {
//         // ratings.innerHTML = i + 1;
//         cover.style.width = ((5 - parseInt(ratings.innerHTML)) * 20) + "%"
//     })
// }
// const stars1 = document.querySelectorAll(".stars1 svg path")
// const ratings1 = document.querySelector("#ratings1")
// const cover1 = document.querySelector(".cover1")
// for (let i = 0; i < stars1.length; i++) {
//     stars1[i].addEventListener("click", () => {
//         ratings1.innerHTML = i + 1;
//         cover1.style.width = ((4 - i) * 20) + "%"
//     })
//     stars1[i].addEventListener("mouseover", () => {
//         // ratings.innerHTML = i + 1;
//         cover1.style.width = ((4 - i) * 20) + "%"
//     })
//     stars1[i].addEventListener("mouseout", () => {
//         // ratings.innerHTML = i + 1;
//         cover1.style.width = ((5 - parseInt(ratings1.innerHTML)) * 20) + "%"
//     })
// }
// const stars2 = document.querySelectorAll(".stars2 svg path")
// const ratings2 = document.querySelector("#ratings2")
// const cover2 = document.querySelector(".cover2")
// for (let i = 0; i < stars2.length; i++) {
//     stars2[i].addEventListener("click", () => {
//         ratings2.innerHTML = i + 1;
//         cover2.style.width = ((4 - i) * 20) + "%"
//     })
//     stars2[i].addEventListener("mouseover", () => {
//         // ratings.innerHTML = i + 1;
//         cover2.style.width = ((4 - i) * 20) + "%"
//     })
//     stars2[i].addEventListener("mouseout", () => {
//         // ratings.innerHTML = i + 1;
//         cover2.style.width = ((5 - parseInt(ratings2.innerHTML)) * 20) + "%"
//     })
// }
// const stars3 = document.querySelectorAll(".stars3 svg path")
// const ratings3 = document.querySelector("#ratings3")
// const cover3 = document.querySelector(".cover3")
// for (let i = 0; i < stars3.length; i++) {
//     stars3[i].addEventListener("click", () => {
//         ratings3.innerHTML = i + 1;
//         cover3.style.width = ((4 - i) * 20) + "%"
//     })
//     stars3[i].addEventListener("mouseover", () => {
//         // ratings.innerHTML = i + 1;
//         cover3.style.width = ((4 - i) * 20) + "%"
//     })
//     stars3[i].addEventListener("mouseout", () => {
//         // ratings.innerHTML = i + 1;
//         cover3.style.width = ((5 - parseInt(ratings3.innerHTML)) * 20) + "%"
//     })
// }

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
					<div class="otherStuff mb-2">
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
				`;
                serviceDetailsModal.show();
            }
        }
    } catch (error) {
        body.classList.remove("loading");
        console.log(error.message)
        serviceDetailsModal.hide();
    }
}


const ratingsubmit = document.querySelector("#ratingsubmit");
const serviceProviderName = document.querySelector("#serviceProviderName");
const ratingmodalspimg = document.querySelector("#ratingmodalspimg");
const ratings = document.querySelector("#ratings");
const cover = document.querySelector(".feedback .cover");
const RatingModalhtml = document.querySelector("#RatingModal");
const ratingcomment = document.querySelector("#ratingcomment");
const RatingModal = bootstrap.Modal.getOrCreateInstance(RatingModalhtml);
const errorspan = document.querySelector(".errorspan");

async function ratespfunction(serviceId, serviceproid, serviceproname, serviceproavgrating, servicepropic) {
    serviceProviderName.innerHTML = serviceproname;
    ratingmodalspimg.src = "/images/" + servicepropic;
    ratings.innerHTML = serviceproavgrating;
    cover.style.width = (5 - parseFloat(serviceproavgrating)) * 20 + "%";

    body.classList.add("loading");
    const res = await fetch(`/Customer/RatingGivenOrNot?ServiceId=${serviceId}&ServiceProviderId=${serviceproid}`, { method: "GET" });
    const data = await res.json();
    body.classList.remove("loading");

    const distthrees = document.querySelectorAll(".distthree");
    if (data.success) {
        ratingcomment.removeAttribute("readonly");

        distthrees.forEach((d) => {
            const stars = d.querySelectorAll(".stars svg path");
            const cover = d.querySelector(".cover");
            const gratings = d.querySelector(".gratings");
            cover.style.width = "100%";
            stars.forEach((p, i) => {
                const $p = p;
                $($p).on("click", () => {
                    cover.style.width = 100 - (i + 1) * 20 + "%";
                    gratings.innerHTML = i + 1;
                });
                $($p).on("mouseover", () => {
                    cover.style.width = 100 - (i + 1) * 20 + "%";
                });
                $($p).on("mouseout", () => {
                    cover.style.width = 100 - parseInt(gratings.innerHTML) * 20 + "%";
                });
            })
        })
        const $ratingsubmit = ratingsubmit;
        $($ratingsubmit).on('click', async () => {
            const data = {
                FriendlyRating: parseInt(distthrees[1].querySelector(".gratings").innerHTML),
                OnTimeArrivalRating: parseInt(distthrees[0].querySelector(".gratings").innerHTML),
                QualityOfServiceRating: parseInt(distthrees[2].querySelector(".gratings").innerHTML),
                ServiceId: parseInt(serviceId),
                ServiceProviderId: parseInt(serviceproid),
            };
            if (ratingcomment.value) {
                data.Comments = ratingcomment.value;
            }
            body.classList.add("loading");
            const response = await fetch("/Customer/Rating", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(data),
            });

            const resJson = await response.json();
            body.classList.remove("loading");
            if (resJson.success) {
                RatingModal.hide();
                errorspan.innerHTML = `<div class="alert alert-success alert-dismissible fade show" role="alert">
                <strong>Success! </strong>Your rating has been submitted successfully.
                <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                </div>`;
                setTimeout(() => {
                    window.location.reload();
                }, 3000);
            }
            else {

            }
        })

    }
    else {
        const strs = ["onTimeArrival", "friendly", "qualityOfService"];
        ratingcomment.setAttribute("readonly", "true");
        ratingcomment.value = data.comments;
        distthrees.forEach((d, i) => {
            const stars = d.querySelectorAll(".stars svg path");
            const cover = d.querySelector(".cover");
            const gratings = d.querySelector(".gratings");
            cover.style.width = (5 - data[strs[i]]) * 20 + "%";
            gratings.innerHTML = data[strs[i]];

            stars.forEach((p) => {
                const $p = p;
                $($p).unbind("click");
                $($p).unbind("mouseover");
                $($p).unbind("mouseout");
            })
        })
    }
    RatingModal.show();
}


