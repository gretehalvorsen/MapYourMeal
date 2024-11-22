// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
// Write your JavaScript code.

//Ready function that will run when the DOM is ready.
$(function(){
    if ($('#createRestaurantMap').length) {
        initCreateMap();
    }
    if ($('#map').length) {
        initMap();
    }
});

async function initCreateMap(){
    var createRestaurantMap = L.map('createRestaurantMap');
    createRestaurantMap.setView([59.91, 10.75], 13);
    L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 19,
        attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
    }).addTo(createRestaurantMap);

    var restaurants = await fetchRestaurants();
    restaurants.forEach(restaurant => {
        L.marker([restaurant.latitude, restaurant.longitude])
            .addTo(createRestaurantMap)
            .bindPopup(
                `<div style="width: 100 px">
                    <h4>${restaurant.restaurantName}</h4>
                    <a href="Restaurant?restaurantId=${restaurant.restaurantId}" 
                    class="btn btn-secondary" style="display: flex; background-color: #2a5b80; color: #fff; justify-content: center;">Vis mer</a>
                </div>`);
    });

    var marker;
    createRestaurantMap.on('click', function(e) {
        var lat = e.latlng.lat;
        var lng = e.latlng.lng;
        if (marker) {
            createRestaurantMap.removeLayer(marker);
        }
        marker = L.marker([lat, lng]).addTo(createRestaurantMap);
        $('input[name="Longitude"]').val(lng);
        $('input[name="Latitude"]').val(lat);
    });
}

async function initMap(){
    var map = L.map('map');
    map.setView([59.91, 10.75], 13);
    L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 19,
        attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
    }).addTo(map);

    var restaurants = await fetchRestaurants();
    restaurants.forEach(restaurant => {
        L.marker([restaurant.latitude, restaurant.longitude])
            .addTo(map)
            .bindPopup(
                `<div style="width: 100 px">
                    <h4>${restaurant.restaurantName}</h4>
                    <a href="Restaurant?restaurantId=${restaurant.restaurantId}" 
                    class="btn btn-secondary" style="display: flex; background-color: #2a5b80; color: #fff; justify-content: center;">Vis mer</a>
                </div>`);
    });
}

async function fetchRestaurants() {
    try{
        const response = await fetch('/Restaurant/GetAllRestaurants');
        if (!response.ok) {
            throw new Error('HTTP error! status ${response.status}');
        }
        const data = await response.json();
        return data.result;
    }
    catch (error) {
        console.error('An error occurred while fetching the restaurants:', error);
        return []; 
    }
}