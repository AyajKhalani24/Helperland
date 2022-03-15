const dashboard = document.querySelector("#myrating");
dashboard.classList.add("current");
const body = document.querySelector("body");

jQuery.extend(jQuery.fn.dataTableExt.oSort, {
    "serviceDate-pre": function (a) {
        let time = a
            .match(/<span>.*<\/span>/)[0]
            .replace(`<span>`, "")
            .replace("</span>", "");
        time = time.split(" - ")[0] + ":00";
        a = a
            .match(/<b>.*<\/b>/)[0]
            .replace("<b>", "")
            .replace("</b>", "");
        let d = a.split("/");
        let day = d[0].length === 1 ? `0${d[0]}` : d[0];
        let month = d[1].length === 1 ? `0${d[1]}` : d[1];
        let year = d[2].length === 1 ? `0${d[2]}` : d[2];
        a = `${month}/${day}/${year} ${time}`;
        return a.toString();
    },
    "serviceDate-asc": function (a, b) {
        const dateA = new Date(a);
        const dateB = new Date(b);
        return dateA < dateB;
    },
    "serviceDate-desc": function (a, b) {
        const dateA = new Date(a);
        const dateB = new Date(b);
        return dateB > dateA;
    },
});

const dt = new DataTable("#ustable", {
    dom: "rtlip",
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
        emptyTable: "No Ratings Found",
    },
    columnDefs: [
        { orderable: false, targets: 2 },
        { orderable: false, targets: 3 },
        { type: "serviceDate", targets: 1 },
    ],
});