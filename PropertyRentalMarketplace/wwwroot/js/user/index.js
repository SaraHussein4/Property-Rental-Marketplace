document.addEventListener('DOMContentLoaded', function () {
    // Get all card containers
    const featuredcardContainers = document.querySelectorAll('.FeaturedProperty-section .card-container');

    // Add click event to each card
    featuredcardContainers.forEach(card => {
        let cardBode = card.querySelector(".body");
        cardBode.addEventListener('click', function (e) {
            const propertyId = card.dataset.propertyid;
            console.log(propertyId)
            window.location.href = `/User/Details/${propertyId}`;
        });
    });


    // Get all card containers
    const latestcardContainers = document.querySelectorAll('.LatestProperty-section .card-container');

    // Add click event to each card
    latestcardContainers.forEach(card => {
        card.addEventListener('click', function (e) {
            const propertyId = card.dataset.propertyid;
            console.log(propertyId)
            window.location.href = `/User/Details/${propertyId}`;
        });
    });


    // Get all card containers
    const topratedcardContainers = document.querySelectorAll('.TopReatedProperty-section .card-container');

    // Add click event to each card
    topratedcardContainers.forEach(card => {
        card.addEventListener('click', function (e) {
            const propertyId = card.dataset.propertyid;
            console.log(propertyId)
            window.location.href = `/User/Details/${propertyId}`;
        });
    });
});