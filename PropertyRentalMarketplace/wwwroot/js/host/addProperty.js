
async function getCountryData(countryCode) {
    try {
        const response = await fetch(
            `https://api.countrystatecity.in/v1/countries/${countryCode}`,
            {
                headers: {
                    "X-CSCAPI-KEY": "WndlbUtKQVNRbm5tdnMyd2NQWWlpUFgyTkJ3YkNMbkpJeFF2c1Q1aA=="
                }
            }
        );

        const country = await response.json();
        return country;

    } catch (error) {
        console.error("Error fetching country data:", error);
        throw error;
    }
}

async function getCountries() {
    try {
        const response = await fetch(
            `https://api.countrystatecity.in/v1/countries`,
            {
                headers: {
                    "X-CSCAPI-KEY": "WndlbUtKQVNRbm5tdnMyd2NQWWlpUFgyTkJ3YkNMbkpJeFF2c1Q1aA=="
                }
            }
        );

        const countries = await response.json();
        return countries;

    } catch (error) {
        console.error("Error fetching country data:", error);
        throw error;
    }
}

async function getStatesByCountry(countryCode) {
    try {
        const response = await fetch(
            `https://api.countrystatecity.in/v1/countries/${countryCode}/states`,
            {
                headers: {
                    "X-CSCAPI-KEY": "WndlbUtKQVNRbm5tdnMyd2NQWWlpUFgyTkJ3YkNMbkpJeFF2c1Q1aA=="
                }
            }
        );

        const states = await response.json();
        return states;

    } catch (error) {
        console.error("Error fetching country data:", error);
        throw error;
    }
}

async function getStateData(countryCode, statecode) {
    try {
        const response = await fetch(
            `https://api.countrystatecity.in/v1/countries/${countryCode}/states/${statecode}`,
            {
                headers: {
                    "X-CSCAPI-KEY": "WndlbUtKQVNRbm5tdnMyd2NQWWlpUFgyTkJ3YkNMbkpJeFF2c1Q1aA=="
                }
            }
        );

        const state = await response.json();
        return state;

    } catch (error) {
        console.error("Error fetching country data:", error);
        throw error;
    }
}

async function showCountries(countries) {

    const countrySelectElement = document.querySelector('.countries');
    const selectedCountryCode = countrySelectElement.dataset.countrycode

    countrySelectElement.innerHTML = '';

    // Add default option
    const defaultOption = document.createElement('option');
    defaultOption.value = '';
    defaultOption.textContent = '-- Select Country --';
    countrySelectElement.appendChild(defaultOption);

    // Add countries alphabetically
    countries.forEach(country => {
        const option = document.createElement('option');
        option.value = country.iso2; // Country code (e.g., "US")
        option.textContent = country.name; // Country name
        if (country.iso2 == selectedCountryCode) {
            option.selected = true;
        }
        countrySelectElement.appendChild(option);
    })
    if (countrySelectElement.value != "") {
        const states = await getStatesByCountry(countrySelectElement.value);
        showStates(states)
    }
}

async function showStates(states) {
    const stateSelectElement = document.querySelector('.states');
    const selectedStateCode = stateSelectElement.dataset.statecode

    stateSelectElement.innerHTML = '';

    // Add default option
    const defaultOption = document.createElement('option');
    defaultOption.value = '';
    defaultOption.textContent = '-- Select State --';
    stateSelectElement.appendChild(defaultOption);

    // Add countries alphabetically
    states.forEach(state => {
        const option = document.createElement('option');
        option.value = state.iso2; // Country code (e.g., "US")
        option.textContent = state.name; // Country name
        if (state.iso2 == selectedStateCode) {
            option.selected = true;
        }
        stateSelectElement.appendChild(option);
    })
}


async function handleCountries() {

    let countries = await getCountries()
    await showCountries(countries);
}

document.addEventListener('DOMContentLoaded', async function () {
    let countrySelectElement = document.querySelector('.countries')
    let stateSelectElement = document.querySelector('.states')


    let map;
    let marker;

    await handleCountries()

    let defaultCoords = [document.getElementById('Latitude').value, document.getElementById('Longitude').value];

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
    countrySelectElement.addEventListener('change', async function () {
        try {
            const countryCode = this.value;
            const country = await getCountryData(countryCode) 
            updateMarkerPosition(country.latitude, country.longitude, 5);
            const states = await getStatesByCountry(country.iso2); 
            showStates(states)
        }
        catch(ex) {
            error.log(ex)
        }
    });

    stateSelectElement.addEventListener('change', async function () {
        try {
            const stateCode = this.value;
            const state = await getStateData(countrySelectElement.value, stateCode)
            updateMarkerPosition(state.latitude, state.longitude, 8); 
        }
        catch (ex) {
            error.log(ex)
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
});
