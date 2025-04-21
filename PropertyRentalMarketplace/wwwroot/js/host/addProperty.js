    //async function getCountries() {
    //    const response = await fetch('https://restcountries.com/v3.1/all')
    //    countries = await response.json(); 

    //    const selectElement = document.querySelector('.countries');

    //    // Clear existing options (if any)
    //    selectElement.innerHTML = '';

    //    // Add default option
    //    const defaultOption = document.createElement('option');   
    //    defaultOption.value = '';
    //    defaultOption.textContent = '-- Select Country --';
    //    selectElement.appendChild(defaultOption);

    //    // Add countries alphabetically
    //    countries
    //        .sort((a, b) => a.name.common.localeCompare(b.name.common))
    //        .forEach(country => {
    //            const option = document.createElement('option');
    //            option.value = country.cca2; // Country code (e.g., "US")
    //            option.textContent = country.name.common; // Country name
    //            selectElement.appendChild(option);
    //        });

    //    console.log('Countries loaded successfully');
    //}


document.addEventListener('DOMContentLoaded', async function () {


    let map;
    let marker;
    console.log(document.getElementById('Latitude').value, document.getElementById('Longitude').value)
    let defaultCoords = [document.getElementById('Latitude').value, document.getElementById('Longitude').value]; // Default: London coordinates
    let selectElement = document.querySelector('.countries')
    let countries = await getCountries()

    initMap();

    function initMap() {
        // Initialize map
        map = L.map('map').setView(defaultCoords, 5);

        // Add tile layer (OpenStreetMap)
        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a>'
        }).addTo(map);

        // Add initial marker
        marker = L.marker(defaultCoords, { draggable: true }).addTo(map);

        // Click event to move marker
        map.on('click', function (e) {
            const { lat, lng } = e.latlng;
            marker.setLatLng([lat, lng]);
            updateCoordinates(lat, lng);
        });

        // Marker drag event
        marker.on('dragend', function () {
            const { lat, lng } = marker.getLatLng();
            updateCoordinates(lat, lng);
        });
            
    }
    // Update marker position and map view
    function updateMarkerPosition(lat, lng, zoomlevel = 5) {
        marker.setLatLng([lat, lng]);
        map.setView([lat, lng], zoomlevel);
        updateCoordinates(lat, lng);
    }
    function updateCoordinates(lat, lng) {
        // Update your form fields or display
        document.getElementById('Latitude').value = lat;
        document.getElementById('Longitude').value = lng;
        console.log(`Coordinates: ${lat}, ${lng}`);
     }

    // Country selection change handler
    selectElement.addEventListener('change', function () {
        const countryCode = this.value;
        if (countryCode) {
            // Find the selected country in our countries array
            const selectedCountry = countries.find(c => c.code === countryCode);
            if (selectedCountry) {
                updateMarkerPosition(selectedCountry.latitude, selectedCountry.longitude, 5);
            }
        }
    });

    document.querySelector('.propertyPrimaryImage').addEventListener('change', function (e) {
        console.log("d;fka;")
        const preview = document.getElementById('primaryImagesPreview');
        preview.innerHTML = '';

        Array.from(e.target.files).forEach(file => {
            const reader = new FileReader();
            reader.onload = function (e) {
                const img = document.createElement('img');
                img.src = e.target.result;
                img.style.maxHeight = '150px';
                img.className = 'img-thumbnail m-1';
                preview.appendChild(img);
            }
            reader.readAsDataURL(file);
        });
    });

    document.querySelector('.propertyImages').addEventListener('change', function (e) {
        console.log("d;fka;")
        const preview = document.getElementById('imagesPreview');
        preview.innerHTML = '';

        Array.from(e.target.files).forEach(file => {
            const reader = new FileReader();
            reader.onload = function (e) {
                const img = document.createElement('img');
                img.src = e.target.result;
                img.style.maxHeight = '150px';
                img.className = 'img-thumbnail m-1';
                preview.appendChild(img);
            }
            reader.readAsDataURL(file);
        });
    });

    async function getCountries() {
        let response = await fetch('/host/getcountries');
        return await response.json();
    }

});


//document.addEventListener('DOMContentLoaded', function () {


//    let userSelections = {
//        propertyTypes: null,
//        amenities: null,
//        safeties: null,
//        PropertyTypeId: +document.querySelector(".propertyType").dataset.propertytypeid,
//        Bedrooms:  +document.querySelector(".Bedrooms").dataset.bedrooms,
//        Bathrooms: +document.querySelector(".Bathrooms").dataset.bathrooms,
//        GarageSlots: +document.querySelector(".GarageSlots").dataset.garageslots,
//        BetsAllowed: +document.querySelector(".BetsAllowed").dataset.betsallowed,
//        Amenities: JSON.parse(document.querySelector(".amenities").dataset.amenities)
//    }; 

//    // Start Property Type 
//    const propertyTypeItems = document.querySelectorAll('.propertyType .item');

//    propertyTypeItems.forEach(item => {

//        item.addEventListener('click', function () {
//            // Remove 'selected' class from all items
//            propertyTypeItems.forEach(i => i.classList.remove('selected'));

//            // Add 'selected' class to clicked item
//            this.classList.add('selected');

//            // Get the data-type value
//            userSelections.PropertyTypeId = +this.dataset.type;

//            console.log(userSelections);
//        });
//    });

//    // End Property Type

//    // Start Facilites
//    // Get all counter items
//    const counterItems = document.querySelectorAll('.facilites .item');

//    counterItems.forEach(item => {
//        const minusBtn = item.querySelector('.minus');
//        const plusBtn = item.querySelector('.plus');
//        const numberDisplay = item.querySelector('.number');
//        const itemType = item.querySelector('.text').textContent.trim(); // "Bedrooms" or "Bathrooms"
//        let count = 0;

//        // Plus button click handler
//        plusBtn.addEventListener('click', function () {
//            userSelections[itemType] += 1;
//            updateDisplay();
//        });

//        // Minus button click handler
//        minusBtn.addEventListener('click', function () {
//            if (userSelections[itemType] > 0) {
//                userSelections[itemType] -= 1;
//                updateDisplay();
//            }
//        });

//        // Update display and button states
//        function updateDisplay() {
//            numberDisplay.textContent = userSelections[itemType];
//            updateButtonStates();
//            console.log(userSelections);

//        }

//        // Disable minus at 0 (visual feedback)
//        function updateButtonStates() {
//            minusBtn.style.opacity = userSelections[itemType] === 0 ? '0.5' : '1';
//            minusBtn.style.cursor = userSelections[itemType] === 0 ? 'not-allowed' : 'pointer';
//        }

//        // Initialize
//        updateDisplay();
//    });
//    // End Facilites

//    // Start Amenities
//    const amenities = document.querySelector('.amenities');
//    console.log(amenities.dataset.amenities)

//    const amenitiesItems = document.querySelectorAll('.amenities .item');
//    console.log(amenitiesItems)
//    amenitiesItems.forEach(item => {

//        item.addEventListener('click', function () {

//            if (this.classList.contains("selected")) {
//                const index = userSelections.Amenities.indexOf(+this.dataset.amenityid);
//                if (index !== -1) {
//                    userSelections.Amenities.splice(index, 1); // Removes 1 element at the found index
//                }
//            }
//            else {
//                userSelections.Amenities.push(+this.dataset.amenityid)
//            }
            
//            // toggle 'selected' class to clicked item
//            this.classList.toggle('selected');

//            console.log(userSelections);

//        });
//    });
//    // End Amenities

//    // Start Submit Logic
//    const submitButton = document.querySelector(".submitButton");
//    submitButton.addEventListener('click', async function () {
//        console.log("a;dkf");

//        const response = await fetch('/host/addproperty', {
//            method: 'POST',
//            headers: {
//                'Content-Type': 'application/json',
//            },
//            body: JSON.stringify(userSelections) // Convert object to JSON
//        })
//        const result = await response.json();

//        if (result.success) {
//            alert('Data saved successfully!');
//        } else {
//            alert('Error: ' + result.message);
//        }
//    })
//    // End Submit Logic

//})