// coming-soon.js (same as previous but with professional console message)
document.addEventListener('DOMContentLoaded', function () {
    console.log('Professional Coming Soon Page initialized');

    // Set the launch date (update this to your actual launch date)
    const launchDate = new Date();
    launchDate.setDate(launchDate.getDate() + 30); // 30 days from now

    // Update countdown every second
    const countdown = setInterval(function () {
        const now = new Date().getTime();
        const distance = launchDate - now;

        // Time calculations
        const days = Math.floor(distance / (1000 * 60 * 60 * 24));
        const hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
        const minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
        const seconds = Math.floor((distance % (1000 * 60)) / 1000);

        // Display results
        document.getElementById('days').textContent = days.toString().padStart(2, '0');
        document.getElementById('hours').textContent = hours.toString().padStart(2, '0');
        document.getElementById('minutes').textContent = minutes.toString().padStart(2, '0');
        document.getElementById('seconds').textContent = seconds.toString().padStart(2, '0');

        // If countdown is finished
        if (distance < 0) {
            clearInterval(countdown);
            document.querySelector('.countdown').innerHTML = '<div class="launch-message">We Have Launched!</div>';
        }
    }, 1000);

    // Form submission
    const notifyForm = document.querySelector('.notify-form');
    if (notifyForm) {
        notifyForm.addEventListener('submit', function (e) {
            e.preventDefault();
            const emailInput = this.querySelector('input[type="email"]');
            if (emailInput.value) {
                // Here you would typically send the email to your server
                alert(`Thank you for your interest. We will notify you at ${emailInput.value} when we launch.`);
                emailInput.value = '';
            }
        });
    }

    // Add animation to elements on scroll
    const animateOnScroll = function () {
        const elements = document.querySelectorAll('.countdown-item, .notify-form, .social-links');
        elements.forEach(element => {
            const elementPosition = element.getBoundingClientRect().top;
            const screenPosition = window.innerHeight / 1.3;

            if (elementPosition < screenPosition) {
                element.style.opacity = '1';
                element.style.transform = 'translateY(0)';
            }
        });
    };

    // Set initial state for animation
    const animatedElements = document.querySelectorAll('.countdown-item, .notify-form, .social-links');
    animatedElements.forEach(element => {
        element.style.opacity = '0';
        element.style.transform = 'translateY(20px)';
        element.style.transition = 'all 0.6s ease-out';
    });

    window.addEventListener('scroll', animateOnScroll);
    animateOnScroll(); // Run once on load
});