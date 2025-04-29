document.addEventListener('DOMContentLoaded', function () {
    // Get all card containers
    const cardContainers = document.querySelectorAll('.card-container');

    // Add click event to each card
    cardContainers.forEach(card => {
        let cardBode = card.querySelector(".body");
        cardBode.addEventListener('click', function (e) {
            const propertyId = card.dataset.propertyid;
            console.log(propertyId)
            window.location.href = `/User/Details/${propertyId}`;
        });
    });
});