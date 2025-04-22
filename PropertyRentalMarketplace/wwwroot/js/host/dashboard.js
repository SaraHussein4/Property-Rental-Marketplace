
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
        // Handle modal opening
         $('#bookingModal').on('show.bs.modal', function (event) {

            // Clear previous validation
            $('#bookingForm').trigger('reset');

            const button = $(event.relatedTarget);
            const propertyId = button.data('property-id');
            const propertyName = button.data('property-name');
            const baseFee = button.data('property-fee');


            console.log(propertyId, propertyName, baseFee)


                const modal = $(this);
                modal.find('#propertyId').val(propertyId);
                modal.find('#propertyNameDisplay').val(propertyName);
                modal.find('#baseFeeDisplay').val(baseFee);
                modal.find('#finalFee').val(baseFee);

        });

        // Use base fee button
        $('#useBaseFee').click(function () {
            const baseFee = $('#baseFeeDisplay').val();
            $('#finalFee').val(baseFee);
        });

        // Date validation
        $('input[name="StartDate"]').change(function () {

            const startDate = new Date(this.value);
            $('input[name="EndDate"]').attr('min', this.value);

            // Auto-set end date to 1 month later if empty
            if (!$('input[name="EndDate"]').val()) {
                const endDate = new Date(startDate);
                endDate.setMonth(endDate.getMonth() + 1);
                $('input[name="EndDate"]').val(endDate.toISOString().split('T')[0]);
            }
        });



    // Handle form submission
    $('#bookingForm').submit(function (e) {
        e.preventDefault(); // Prevent default form submission

        // Get form data
        const formData = {
            PropertyId: $('#propertyId').val(),
            phoneNumber: $('input[name="TenantPhone"]').val(),
            StartDate: $('input[name="StartDate"]').val(),
            EndDate: $('input[name="EndDate"]').val(),
            FeePerMonth: $('#finalFee').val()
        };

        // Show loading state
        const submitBtn = $(this).find('button[type="submit"]');
        submitBtn.prop('disabled', true).html(
            '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Processing...'
        );

        // Send AJAX request
        $.ajax({
            url: '/Host/DealClosed', // Update with your action URL
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(formData),
            success: function (response) {
                // Handle success
                $('#bookingModal').modal('hide');

                // Optional: Refresh your bookings list
                if (typeof loadTab === 'function') {
                    loadTab('booked');
                }
            },
            error: function (xhr) {
                // Handle errors
                let errorMessage = 'An error occurred';
                if (xhr.responseJSON && xhr.responseJSON.errors) {
                    errorMessage = Object.values(xhr.responseJSON.errors).join('<br>');
                }
                toastr.error(errorMessage);
            },
            complete: function () {
                submitBtn.prop('disabled', false).text('Confirm Booking');
            }
        });
    });


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



