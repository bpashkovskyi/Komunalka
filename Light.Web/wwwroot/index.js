const hostURL = "";
const criteriaArray = [];
criteriaArray["queues"] = "Черга";
criteriaArray["streets"] = "Вулиця";

let filters = {};
let markers = [];
let accidents = [];
let map = null;
async function initMap() {
    const { Map } = await google.maps.importLibrary("maps");

    map = new Map(document.getElementById("map"), {
        zoom: 15,
        center: { lat: 48.923113, lng: 24.7283538 },
        mapId: "4504f8b37365c3d0",
    });
}
async function deleteMarkers() {
    for (let i = 0; i < markers.length; i++) {
        markers[i].setMap(null);
    }
    markers = [];
}
async function drawMarkers() {
    var marker, i;

    const infoWindow = new google.maps.InfoWindow({
        content: "",
        disableAutoPan: true,
    });
    markers = new Array(accidents.length);

    for (i = 0; i < accidents.length; i++) {

        var icon = getIcon(accidents[i]);

        marker = new google.maps.Marker({
            map,
            position: { lat: accidents[i].lat, lng: accidents[i].lng },
            icon: icon,
        });

        google.maps.event.addListener(
            marker,
            "click",
            (function (marker, i) {
                return function () {
                    infoWindow.setContent(accidents[i].description);
                    infoWindow.open(map, marker);
                };
            })(marker, i)
        );

        markers[i] = marker;
    }

    $("#total").text('Кількість: ' + accidents.length);
}

function getIcon(accident) {
    return (
        "http://maps.google.com/mapfiles/ms/micons/" + accident.markerColor + ".png"
    );
}

async function hideLoader() {
    $("body").removeClass("no-scroll");
    $(".loader-wrap").fadeOut("");
}

async function getFiltersAndDrawFilterGrid() {
    let filtersURL = hostURL + "/filters";
    let filtersResponse = await fetch(filtersURL, {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
        },
    });
    filters = await filtersResponse.json();
    await hideLoader();
    Object.keys(filters).forEach((filter) => {
        let optionsString = ``;
        filters[filter].forEach((filteritem) => {
            optionsString +=
                `<option value='` + filteritem + `'>` + filteritem + `</option>`;
        });
        $("#" + filter + "Select").before(`<label for="${filter}Select">` + criteriaArray[filter] + `</label>`);
        $("#" + filter + "Select").html(
            `
        <select id="` +
            filter +
            `Select" data-filter="` +
            filter +
            `" multiple="multiple">
        ` +
            optionsString +
            `
        </select>`
        );
    });

    $("select[multiple]").multiselect({
        texts: {
            placeholder: 'Оберіть опцію',
            selectedOptions: ' обрано'
        }
    });
}
async function getAccidents(filtersObject) {
    let accidentsURL = hostURL + "/accidents";
    let accidentsResponse = await fetch(accidentsURL, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(filtersObject),
    });
    let accidents = await accidentsResponse.json();
    return accidents;
}

async function filterAccidents() {
    let filtersObject = {};
    $(".dynamic-select").each(async function (index) {
        let selectedOptions = [];
        $(this)
            .next()
            .find("input:checked")
            .each(function (index) {
                selectedOptions.push($(this).attr("value"));
            });
        if (selectedOptions.length != 0) {
            filtersObject[$(this).attr("data-filter")] = selectedOptions;
        }
    });
    $(".static-select").each(async function (index) {
        if ($(this).val() != "all") {

            if ($(this).attr("data-filter") == "casualties") {
                filtersObject[$(this).attr("data-filter")] = $(this).val();
            }
            else {
                filtersObject[$(this).attr("data-filter")] = ($(this).val() == "yes");
            }
        }
    });

    accidents = await getAccidents(filtersObject);
    await deleteMarkers();
    await drawMarkers();
}

$("#resetRequest").click(async (e) => {
    e.preventDefault();

    $("input[type='checkbox']:checked").prop("checked", false);
    $("select[multiple]").multiselect("reset");

    accidents = await getAccidents({});
    await deleteMarkers();
    await drawMarkers();
});

$('.dynamic-select').change(async (e) => {
    e.preventDefault();
    await filterAccidents();
});

$('.static-select').change(async (e) => {
    e.preventDefault();
    await filterAccidents();
});

await initMap();

await getFiltersAndDrawFilterGrid();

accidents = await getAccidents({});

await drawMarkers();
