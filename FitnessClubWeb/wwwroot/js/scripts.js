/*jshint esversion: 6 */
const uri = "/api/clients/";
let items = null;


function isAuthenticated() {
    var request = new XMLHttpRequest();
    var role = "";
    request.open("GET", "/api/Account/userRole", false);
    request.onload = function () {
        role = request.response;
    }
    request.send();

    if (role !== "" && role !== "udentified") {
        document.getElementById("LogIn").innerText = "Выйти";
    }
    else {
        document.getElementById("LogIn").innerText = "Войти";
    }
    return role;
}

function loadClients() {
    var i, x = "";
    var role;
    var request = new XMLHttpRequest();

    var role = isAuthenticated();

    request.open("GET", uri, false);
    request.onload = function () {
        items = JSON.parse(request.responseText);
        for (i in items) {
            x += "<hr>";
            x += "<div class='row'>";
            x += "<h4 class='col-5'>" + items[i].fio + "</h4>";
            x += "<button type='button' class='col-2 btn btn-sm btn-outline-secondary' data-toggle='modal' data-target='#clientInfoModal' onclick='loadClientInfo(" + items[i].id + ");'> Посмотреть </button>";
            if (role == '"admin"') {
                x += "<button type='button' class='col-2 btn btn-sm btn-outline-primary' data-toggle='modal' data-target='#clientEditModal' onclick='loadClientEditInfo(" + items[i].id + ");'> Изменить </button>";
                x += "<button type='button' class='col-2 btn btn-sm btn-outline-secondary' onclick='deleteClient(" + items[i].id + ");'>Удалить</button>";
                x += "</div><br />";
                x += "<div class='row'>";
                x += "<button type='button' class='col-4 btn btn-sm btn-outline-secondary' onclick='editClient(" + items[i].id + ");'>Применить изменения</button>";
                x += "<button id='markClient" + items[i].id + "' type='button' class='col-2 offset-1 btn btn-sm btn-success' onclick='markClient(" + items[i].id + ");'>Отметить приход</button>";
                x += "<button id='unmarkClient" + items[i].id + "' type='button' class='col-2 btn btn-sm btn-warning' onclick='unmarkClient(" + items[i].id + ");'>Отметить уход</button>";
            }
                x += "</div>";
        }
        document.getElementById("clientsDiv").innerHTML = x;
    };
    request.send();
}

function deleteClient(id) {
    var request = new XMLHttpRequest();
    var url = uri + id;
    request.open("DELETE", url, false);
    request.onload = function () {
        // Обработка кода ответа
        var msg = "";
        if (request.status === 401) {
            msg = "У вас не хватает прав для совершения действия";
        } else if (request.status === 200) {
            loadClients();
        } else {
            msg = "Неизвестная ошибка";
        }
        document.querySelector("#actionMsg").innerHTML = msg;
    }
    request.send();
}

function createClient() {
    var fio = document.getElementById("FIO").value;
    var birthDay = document.getElementById("Birthday").value;
    var startDate = document.getElementById("startDate").value;
    var endDate = document.getElementById("endDate").value;
    var price = document.getElementById("price").value;

    if (startDate > endDate) {
        alert("Дата начала действия абонемента не может превышать дату окончания действия!");
        return;
    }
    if (startDate == "" || endDate == "" || price == "") {
        alert("Не заполены поля регистрации абонемента!");
        return;
    }

    var request = new XMLHttpRequest();
    request.open("POST", uri);
    request.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    request.onload = function () {
        // Обработка кода ответа
        var msg = "";
        if (request.status === 401) {
            msg = "У вас не хватает прав для совершения действия";
        } else if (request.status === 201) {
            msg = "Клиент создан";
            cleanInput();
        } else {
            msg = "Неизвестная ошибка";
        }
        document.querySelector("#actionMsg").innerHTML = msg;
    };
    var json = JSON.stringify({ fio: fio, birthDay: birthDay, subscription: { timeOfActionStart: startDate, timeOfActionEnd: endDate, price: price } });
    request.send(json);
}

function cleanInput() {
    document.getElementById("FIO").value = "";
    document.getElementById("Birthday").value = "";
    document.getElementById("startDate").value = "";
    document.getElementById("endDate").value = "";
    document.getElementById("price").value = "";
}

function cleanSubscriptionInput() {
    document.getElementById("startDate").value = "";
    document.getElementById("endDate").value = "";
    document.getElementById("price").value = "";
}

function loadClientInfo(id) {
    var visitings = "";
    var request = new XMLHttpRequest();
    request.open("GET", uri + id, false);
    request.onload = function () {
        client = JSON.parse(request.responseText);
        document.getElementById("modal-fio").innerText = client.fio;
        document.getElementById("modal-birthDay").innerText = new Date(client.birthDay).toLocaleDateString();
        document.getElementById("modal-startDate").innerText = new Date(client.subscription.timeOfActionStart).toLocaleDateString();
        document.getElementById("modal-endDate").innerText = new Date(client.subscription.timeOfActionEnd).toLocaleDateString();
        document.getElementById("modal-price").innerText = client.subscription.price;
        for (i in client.visitings) {
            var start = "";
            var end = "";
            if (client.visitings[i].startTime != null) {
                start = new Date(client.visitings[i].startTime).toLocaleString();
            }
            if (client.visitings[i].finishTime != null) {
                end = new Date(client.visitings[i].finishTime).toLocaleString();
            }
            visitings += "<p>" + start + " - " + end + "</p>";
        }
        document.getElementById("visitingsInfo").innerHTML = visitings;
    }
    request.send();
}

function loadClientEditInfo(id) {
    var request = new XMLHttpRequest();
    request.open("GET", uri + id, false);
    request.onload = function () {
        client = JSON.parse(request.responseText);
        document.getElementById("edit-modal-fio").value = client.fio;
        document.getElementById("edit-modal-birthDay").value = new Date(client.birthDay).toISOString().substr(0, 10);
    }
    request.send();
}

function editClient(id) {
    var fio = document.getElementById("edit-modal-fio").value;
    var birthDay = document.getElementById("edit-modal-birthDay").value;

    var request = new XMLHttpRequest();
    request.open("PUT", uri + id);
    request.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    request.onload = function () {
        // Обработка кода ответа
        var msg = "";
        if (request.status === 401) {
            msg = "У вас не хватает прав для совершения действия";
            document.querySelector("#actionMsg").innerHTML = msg;
        } else if (request.status === 200) {
            msg = "Клиент обновлен";
            loadClients();
        } else {
            msg = "Неизвестная ошибка";
            document.querySelector("#actionMsg").innerHTML = msg;
        }
    };
    var json = JSON.stringify({ fio: fio, birthDay: birthDay });
    request.send(json);

}

function markClient(id) {
    var request = new XMLHttpRequest();
    request.open("PUT", uri + id + '/mark', false);
    request.onload = function () {
        // Обработка кода ответа
        var msg = "";
        if (request.status === 401) {
            msg = "У вас не хватает прав для совершения действия";
            document.querySelector("#actionMsg").innerHTML = msg;
        } else if (request.status === 200) {
            document.getElementById("markClient" + id).setAttribute("disabled", null);
            document.getElementById("unmarkClient" + id).removeAttribute("disabled");
        } else {
            msg = "Неизвестная ошибка";
            document.querySelector("#actionMsg").innerHTML = msg;
        }
    }
    request.send();
}

function unmarkClient(id) {
    var request = new XMLHttpRequest();
    request.open("PUT", uri + id + '/unmark', false);
    request.onload = function () {
        // Обработка кода ответа
        var msg = "";
        if (request.status === 401) {
            msg = "У вас не хватает прав для совершения действия";
            document.querySelector("#actionMsg").innerHTML = msg;
        } else if (request.status === 200) {
            document.getElementById("unmarkClient" + id).setAttribute("disabled", null);
            document.getElementById("markClient" + id).removeAttribute("disabled");
        } else {
            msg = "Неизвестная ошибка";
            document.querySelector("#actionMsg").innerHTML = msg;
        }
    }
    request.send();
}