document.querySelectorAll(".adminnavvertcont").forEach(ele => {
    ele.addEventListener('click', () => {
        document.querySelector('.currentadmin').classList.remove('currentadmin');
        ele.classList.add('currentadmin');
    });
});

const openvertnav1=document.querySelector(".adminnavvertcontent1");
const navcontent1=document.querySelector(".admininsidevertnav1");
const closebutton1=document.querySelector(".ccm1")
openvertnav1.addEventListener('click', () => {
    openvertnav1.style.display="none";
    navcontent1.style.display="flex";
});
closebutton1.addEventListener('click', () => {
    openvertnav1.style.display="flex";
    navcontent1.style.display="none";
})

const openvertnav2=document.querySelector(".adminnavvertcontent2");
const navcontent2=document.querySelector(".admininsidevertnav2");
const closebutton2=document.querySelector(".ccm2")
openvertnav2.addEventListener('click', () => {
    openvertnav2.style.display="none";
    navcontent2.style.display="flex";
});
closebutton2.addEventListener('click', () => {
    openvertnav2.style.display="flex";
    navcontent2.style.display="none";
})

const openvertnav3=document.querySelector(".adminnavvertcontent3");
const navcontent3=document.querySelector(".admininsidevertnav3");
const closebutton3=document.querySelector(".ccm3")
openvertnav3.addEventListener('click', () => {
    openvertnav3.style.display="none";
    navcontent3.style.display="flex";
});
closebutton3.addEventListener('click', () => {
    openvertnav3.style.display="flex";
    navcontent3.style.display="none";
})



const dt = new DataTable("#ustable", {
    dom: "Rtlip",
    responsive: true,
    pagingType: "full_numbers",
    language: {
        paginate: {
            first: "<img src='./images/firstpage.png' alt='first' />",
            previous: "<img src='./images/previous.png' alt='previous' />",
            next: '<img src="./images/previous.png" alt="next" style="transform: rotate(180deg)" />',
            last: "<img src='./images/firstpage.png' alt='first' style='transform: rotate(180deg)' />",
        },
        info: "Total Record: _MAX_",
        lengthMenu: "Show_MENU_Entries",
    },
    buttons: ["excel"],
    columnDefs: [{ orderable: false, targets: 4 },{ orderable: false, targets: 1 },{ orderable: false, targets: 2 },{ orderable: false, targets: 7}],
});