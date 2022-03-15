const dashboard = document.querySelector("#blockcustomer");
dashboard.classList.add("current");
const body = document.querySelector("body");

const dt = new DataTable("#ustable", {
    dom: "Rtlip",
    responsive: false,
    pagingType: "full_numbers",
    language: {
        paginate: {
            first: "<img src='/images/firstPage.png' alt='first' />",
            previous: "<img src='/images/previous.png' alt='previous' />",
            next: '<img src="/images/previous.png" alt="next" style="transform: rotate(180deg)" />',
            last: "<img src='/images/firstPage.png' alt='first' style='transform: rotate(180deg)' />",
        },
        info: "Total Record: _MAX_",
        lengthMenu: "Show_MENU_Entries",
        emptyTable: "No service History Found",
    },
});

async function blockOrUnblock(userId) {
    // console.log(userId)
    try {
        const checkbox = document.querySelector("#user-" + userId);
        console.log(checkbox)
        body.classList.add("loading")
        const res = await fetch("/Provider/BlockCustomer", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify({ UserId: userId, IsBlocked: checkbox.checked }),
        });
        if (res.redirected) window.location.href = res.url;
        const data = await res.json();
        body.classList.remove("loading");
        if (data.success) {
            const label = document.querySelector(`label[for='user-${userId}']`);
            if (data.isBlocked) {
                label.innerHTML = "Unblock";
                label.classList.add("blocked");
                label.classList.remove("unblocked");
            } else {
                label.innerHTML = "Block";
                label.classList.remove("blocked");
                label.classList.add("unblocked");
            }

        } else {

        }
    } catch (error) {
        console.log(error.message);
    }
};
