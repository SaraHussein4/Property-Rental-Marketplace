
document.addEventListener('DOMContentLoaded', async function () {
    // Load default tab (Active Listings)
    await loadTab('active');

    // Tab click event
    $('[data-tab]').click(function (e) {
        e.preventDefault();
        const tab = $(this).data('tab');
        loadTab(tab);
    });

    async function loadTab(tabName) {
        // Await the AJAX call
        const response = await $.ajax({
            url: '/Host/LoadTab',
            type: 'GET',
            data: { tab: tabName }
        });

        // Update UI
        $('#tabContent').html(response);
        $('[data-tab]').removeClass('active');
        $(`[data-tab="${tabName}"]`).addClass('active');
    }
    console.log(document.querySelectorAll('#bookingModal'))

});
    






//async function loadTab(tabName) {
//    try {
//        // Show loading state
//        $('#tabContent').html(`
//            <div class="text-center py-4">
//                <div class="spinner-border text-primary" role="status">
//                    <span class="visually-hidden">Loading...</span>
//                </div>
//            </div>
//        `);

//        // Await the AJAX call
//        const response = await $.ajax({
//            url: '/Host/LoadTab',
//            type: 'GET',
//            data: { tab: tabName }
//        });

//        // Update UI
//        $('#tabContent').html(response);
//        $('[data-tab]').removeClass('active');
//        $(`[data-tab="${tabName}"]`).addClass('active');

//    } catch (error) {
//        console.error('Error loading tab:', error);
//        $('#tabContent').html(`
//            <div class="alert alert-danger">
//                Failed to load content. Please try again.
//            </div>
//        `);
//    }
//}



