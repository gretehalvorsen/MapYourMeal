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

function initCreateMap(){
    var createRestaurantMap = L.map('createRestaurantMap');
    createRestaurantMap.setView([59.91, 10.75], 13);
    L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 19,
        attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
    }).addTo(createRestaurantMap);

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
    console.log(restaurants);
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
/*
function addUserPosition(){
    navigator.geolocation.watchPosition(positionSuccess, positionError);
    function positionSuccess(pos) {
        const lat = pos.coords.latitude;
        const lon = pos.coords.longitude;
        const accuracy = pos.coords.accuracy;
        var marker = L.marker([lat, lon]).addTo(map);
        marker.bindPopup("<b>This is your location!</b>").openPopup();
    }
    function positionError(err){
        if (err.code === 1){
            alert("Please allow geolocation access");
        }
        else {
            alert("Cannot get current location");
        }
    }
}*/

/*function loadRestaurantsFromAPI(){
    var requestOptions = {
        method: 'GET',
    };
    //This is searching by rectangle. rect:lon1,lat1,lon2,lat2
    const apiResult = fetch("https://api.geoapify.com/v2/places?categories=catering&filter=rect:10.673365854787836,59.95383348582628,10.821658384284472,59.88587849035447&limit=20&apiKey=49a8fcc8e98649019ce05155b75301be")
        .then(response => response.json())
        .then((result) => {
            console.log(result.features);
            for (const restaurant of result.features) {
                const lon = restaurant.properties.lon;
                const lat = restaurant.properties.lat;
                var marker = L.marker([lat, lon]).addTo(map);
                marker.bindPopup(
                    "<address><strong>Name: " + restaurant.properties.name + 
                    "</strong><br>Latitude: " + restaurant.properties.lat + 
                    "<br>Longitude: " + restaurant.properties.lon + 
                    "</address>");
            }
        })
        .catch((error) => {
            console.log('error', error);
        });
}*/