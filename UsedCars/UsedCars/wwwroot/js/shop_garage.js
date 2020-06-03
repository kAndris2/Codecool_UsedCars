function getShopByID(id) {
    var result = null;
    $.ajax({
        url: "/Garage/GetShopByID/" + id,
        type: 'get',
        dataType: 'json',
        async: false,
        success: function (data) {
            result = data;
        }
    });
    return result;
}

function getVehiclesByShopID(id) {
    var result = null;
    $.ajax({
        url: "/Garage/GetVehicles/" + id,
        type: 'get',
        dataType: 'json',
        async: false,
        success: function (data) {
            result = data;
        }
    });
    return result;
}

function getVehicleFirstPicture(id) {
    var result = null;
    $.ajax({
        url: "/Garage/GetVehicleFirstPicture/" + id,
        type: 'get',
        dataType: 'json',
        async: false,
        success: function (data) {
            result = data;
        }
    });
    return result;
}

function getValueAtIndex(index) {
    var str = window.location.href;
    return str.split("/")[index];
}

function backToProfile() {
    window.location = "/Shop_Profile/" + shopID;
}

function getCurrentShopID() {
    return shopID;
}

function showVehicle(vehicle) {
    const vehicles = document.getElementById("vehicles");
    //
    const main_div = document.createElement("div");
    main_div.classList.add("media");
    main_div.style.backgroundColor = "white";
    //-Vehicle img-->
    var owner_img = new Image();
    var ow_tmp_img = getVehicleFirstPicture(vehicle.id);
    if (ow_tmp_img != null) {
        owner_img.src = ow_tmp_img.route;
    }
    else {
        owner_img.src = '/pics/no_img.png';
    }
    owner_img.width = 200;
    owner_img.height = 140;
    owner_img.onclick = function () {
        window.location.href = '/Vehicle_Profile/' + vehicle.id;
    };
    main_div.appendChild(owner_img); //-->Vehicle img append to main div
    //-Card body-->
    const div = document.createElement("div");
    div.classList.add("media-body");
    //-Vehicle name-->
    const h5 = document.createElement("h5");
    h5.classList.add("mt-0");
    h5.textContent = `${vehicle.brand} ${vehicle.model} ${vehicle.cylinder_Capacity} ${vehicle.type_Designation.split(",").join(" ")}`;
    h5.onclick = function () {
        window.location.href = '/Vehicle_Profile/' + vehicle.id;
    }
    div.appendChild(h5); //-->Vehicle name append to card body
    //-Vehicle properties-->
    const p = document.createElement("p");
    p.textContent = `${vehicle.fuel}, ${vehicle.vintage}, ${vehicle.cylinder_Capacity} cm3, ${vehicle.performance} kW, ${vehicle.odometer} km`;
    div.appendChild(p); //-->Vehicle properties append to card body
    //-Vehicle description-->
    const span = document.createElement("span");
    span.style.display = "block";
    span.style.width = "650px";
    span.style.overflow = "hidden";
    span.style.whiteSpace = "nowrap";
    span.style.textOverflow = "ellipsis";
    span.textContent = vehicle.description;
    div.appendChild(span); //-->Vehicle description append to card body
    //-Vehicle ID-->
    const p2 = document.createElement("p");
    p2.textContent = `(ID: ${vehicle.id})`;
    div.appendChild(p2); //-->Vehicle id append to card body
    //
    main_div.appendChild(div); //-->Card body append to main div
    //-Vehicle price-->
    const h6 = document.createElement("h6");
    h6.textContent = `${vehicle.price} Ft`;
    main_div.appendChild(h6); //-->Vehicle price append to main div
    //
    vehicles.appendChild(main_div);
}

function listVehicles() {
    const vehicles = getVehiclesByShopID(shopID);
    vehicles.forEach((vehicle) => {
        showVehicle(vehicle);
    });
}

function createVehicle() {
    var vehicle =
    {
        Brand: document.getElementById("brand").value,
        Model: document.getElementById("model").value,
        Vintage: parseInt(document.getElementById("vintage").value),
        Type: document.getElementById("type").value,
        Price: parseInt(document.getElementById("price").value),
        Fuel: document.getElementById("fuel").value,
        Type_Designation: document.getElementById("type_designation").value,
        Cylinder_Capacity: parseInt(document.getElementById("cylinder").value),
        Performance: parseInt(document.getElementById("performance").value),
        Odometer: parseInt(document.getElementById("odometer").value),
        Description: document.getElementById("description").value,
        Validity: document.getElementById("validity").value == "Valid",
        Shop_ID: parseInt(document.getElementById("shopid").value)
    };
    var vehJson = JSON.stringify(vehicle);
    //
    const xhr = new XMLHttpRequest();
    xhr.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            showVehicle(JSON.parse(this.responseText));
        }
    };
    xhr.open('POST', '/Garage/CreateVehicle', true);
    xhr.setRequestHeader("Content-type", "application/json");
    xhr.send(vehJson);
}

function start() {
    const xhr = new XMLHttpRequest();
    xhr.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            let user = JSON.parse(this.responseText);
            //
            if (user.rank == 'Admin' || user.id == shop.owner_ID) {
                const select = document.getElementById("vintage");
                for (let i = 1970; i < new Date().getFullYear() + 1; i++) {
                    let option = document.createElement("option");
                    option.textContent = i;
                    option.value = i;
                    select.appendChild(option);
                }
            }
            else {
                document.getElementById("creator").style.display = "none";
            }
            listVehicles();
        }
    };
    xhr.open('GET', '/Garage/GetCurrentUser');
    xhr.send();
}

let shopID = getValueAtIndex(4);
const shop = getShopByID(shopID);

document.getElementById("owner").innerHTML = shop.name + "'s garage:";

start();