const dashboard = document.querySelector("#usermanagement");
const errorspan = document.querySelector(".errorspan");
dashboard.classList.add("currentadmin");

const dt = new DataTable("#ustable", {
    dom: "Rtlp",
    responsive: true,
    // pagingType: "full_numbers",
    language: {
        paginate: {
            // first: "<img src='./images/firstpage.png' alt='first' />",
            previous: "<img src='/images/previous.png' alt='previous' />",
            next: '<img src="/images/previous.png" alt="next" style="transform: rotate(180deg)" />',
            // last: "<img src='./images/firstpage.png' alt='first' style='transform: rotate(180deg)' />",
        },
        // info: "Total Record: _MAX_",
        lengthMenu: "Show_MENU_Entries",
    },
    buttons: ["excel"],
    columnDefs: [{ orderable: false, targets: 6 }],
});


async function Activate(UserId, Isapprove) {
    try {
        console.log(UserId, Isapprove)
        if (UserId) {
            // body.classList.add("loading");
            const res = await fetch(`/Admin/Activate?UserId=${UserId}&Isapprove=${Isapprove}`, { method: "GET" });
            const data = await res.json();

            if (data) {
                errorspan.innerHTML = `<div class="alert alert-success alert-dismissible fade show" role="alert">
                <strong>Success! </strong>${data.msg}
                <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                </div>`;
                setTimeout(() => {
                    window.location.reload();
                }, 3000);
            }
            else {
                errorspan.innerHTML = `<div class="alert alert-danger alert-dismissible fade show" role="alert">
                <strong>Failed! </strong>Internal Server Error
                <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                </div>`;
            }
        }
    }
    catch {
        errorspan.innerHTML = `<div class="alert alert-danger alert-dismissible fade show" role="alert">
                <strong>Failed! </strong>Internal Server Error.
                <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                </div>`;
        // console.log(error)
    }
}


const UserRole = document.querySelector("#User-Role")
const adminusername = document.querySelector("#adminusername")
const adminmono = document.querySelector("#adminmono")
const adminzipcode = document.querySelector("#adminzipcode")
const adminsubmit = document.querySelector("#adminsubmit")
const adminreset = document.querySelector("#adminreset")

adminsubmit.addEventListener("click", (e) => {
    e.preventDefault();
    $.fn.dataTable.ext.search.push(function (settings, data, dataIndex) {
        console.log(data)

        let username = data[0];
        let usertype = data[1];
        let zipcode = data[2];
        let phonenumber = data[4];
        const isusername = adminusername.value ? username === adminusername.value : true;
        const isusertype = UserRole.value === "" ? true : usertype === UserRole.value;
        const iszipcode = adminzipcode.value === "" ? true : zipcode === adminzipcode.value;
        const ismono = adminmono.value === "" ? true : phonenumber.includes(adminmono.value);

        return isusername && isusertype && iszipcode && ismono;
    });
    dt.draw();
});
adminreset.addEventListener("click", (e) => {
    $.fn.dataTableExt.afnFiltering.length = 0;
    dt.draw();
});