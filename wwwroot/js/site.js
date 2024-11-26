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

//Green icon source: https://github.com/pointhi/leaflet-color-markers?tab=readme-ov-file
var greenIcon = L.icon({
    iconUrl: '/lib/leaflet/dist/images/marker-icon-green.png',
    shadowUrl: '/lib/leaflet/dist/images/marker-shadow.png',
    iconSize: [25, 41],
    iconAnchor: [12, 41],
    popupAnchor: [1, -34],
    shadowSize: [41, 41]
});

//Definition of method to fetch restaurants from the database using endpoint /Restaurant/GetAllRestaurants.
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

async function initCreateMap(){
    //Initializing the map and adding the map layer
    var createRestaurantMap = L.map('createRestaurantMap');
    createRestaurantMap.setView([59.91, 10.75], 13);
    L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 19,
        attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
    }).addTo(createRestaurantMap);

    //Fetching all the restaurants from the database and pinning them on the map
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

    //Enabling the user to pin a new restaurant.
    var marker;
    createRestaurantMap.on('click', function(e) {
        var lat = e.latlng.lat;
        var lng = e.latlng.lng;

        // Ensure the decimal separator is a period mark: .
        lat = lat.replace(',', '.');
        lng = lng.replace(',', '.');

        if (marker) {
            createRestaurantMap.removeLayer(marker);
        }
        marker = L.marker([lat, lng], {icon: greenIcon}).addTo(createRestaurantMap);
        $('input[name="Longitude"]').val(lng);
        $('input[name="Latitude"]').val(lat);
    });
}

async function initMap(){
    //Initializing the map and adding the map layer
    var map = L.map('map');
    map.setView([59.91, 10.75], 13);
    L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 19,
        attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
    }).addTo(map);

    //Fetching all the restaurants from the database and pinning them on the map
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

async function initRestaurantIdMap(shownRestaurant){
    //Initializing the map and adding the map layer
    var map = L.map('restaurantidmap');
    map.setView([59.91, 10.75], 13);
    L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 19,
        attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
    }).addTo(map);

    //Fetching all the restaurants from the database and pinning them on the map
    var restaurants = await fetchRestaurants();
    restaurants.forEach(restaurant => {
        var markerOptions = {};
        console.log(shownRestaurant);
        if (restaurant.restaurantId == shownRestaurant) {
            markerOptions.icon = greenIcon;
        }
        L.marker([restaurant.latitude, restaurant.longitude], markerOptions)
            .addTo(map)
            .bindPopup(
                `<div style="width: 100 px">
                    <h4>${restaurant.restaurantName}</h4>
                    <a href="Restaurant?restaurantId=${restaurant.restaurantId}" 
                    class="btn btn-secondary" style="display: flex; background-color: #2a5b80; color: #fff; justify-content: center;">Vis mer</a>
                </div>`);
    });
}