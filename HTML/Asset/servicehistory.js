document.querySelectorAll(".vertnavcont").forEach(ele => {
    ele.addEventListener('click', () => {
        document.querySelector('.current').classList.remove('current');
        ele.classList.add('current');
    });
})
const hamburger2=document.querySelector('.hamburger2');
const opennav=document.querySelector('.usnav2');
const closebutton=document.querySelector('.closebutton');

hamburger2.addEventListener('click',()=>{
    opennav.classList.add('open');
})
closebutton.addEventListener('click',()=>{
    opennav.classList.remove('open')
})

const dt = new DataTable("#ustable", {
    dom: "Brtlip",
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
    columnDefs: [{ orderable: false, targets: 4 }],
});