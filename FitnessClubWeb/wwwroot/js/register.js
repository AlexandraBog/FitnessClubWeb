function Register() {
    // Считывание данных с формы
    var email = document.querySelector("#email").value;
    var password = document.querySelector("#password").value;
    var passwordConfirm = document.querySelector("#passwordConfirm").value;

    let request = new XMLHttpRequest();
    request.open("POST", "/api/account/register");
    request.setRequestHeader("Accepts", "application/json;charset=UTF-8");
    request.setRequestHeader("Content-Type", "application/json;charset=UTF-8");

    // Обработка ответа
    request.onload = function () {
        ParseResponse(this);
    };
    // Запрос на сервер
    request.send(JSON.stringify({
        email: email,
        password: password,
        passwordConfirm: passwordConfirm
    }));
}
// Разбор ответа
function ParseResponse(e) {

    // Очистка контейнера вывода сообщений
    document.querySelector("#msg").innerHTML = "";
    var formError = document.querySelector("#formError");
    while (formError.firstChild) {
        formError.removeChild(formError.firstChild);
    }
    // Обработка ответа от сервера
    let response = JSON.parse(e.responseText);
    document.querySelector("#msg").innerHTML = response.message;
    // Вывод сообщений об ошибках
    if (response.error.length > 0) {
        for (var i = 0; i < response.error.length; i++) {
            let ul = document.querySelector("#formError");
            let li = document.createElement("li");
            li.appendChild(document.createTextNode(response.error[i]));
            ul.appendChild(li);
        }
    }
    // Очистка полей паролей
    document.querySelector("#password").value = "";
    document.querySelector("#passwordConfirm").value = "";
}

function logIn() {
    var email, password = "";
    // Считывание данных с формы
    email = document.getElementById("Email").value;
    password = document.getElementById("Password").value;
    var request = new XMLHttpRequest();
    request.open("POST", "/api/account/login");
    request.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    request.onreadystatechange = function () {
        // Очистка контейнера вывода сообщений
        document.getElementById("msg").innerHTML = "";
        var mydiv = document.getElementById('formError');
        while (mydiv.firstChild) {
            mydiv.removeChild(mydiv.firstChild);
        }
        // Обработка ответа от сервера
        if (request.responseText !== "") {
            var msg = null;
            msg = JSON.parse(request.responseText);
            document.getElementById("msg").innerHTML = msg.message;
            // Вывод сообщений об ошибках
            if (typeof msg.error !== "undefined" && msg.error.length >
                0) {
                for (var i = 0; i < msg.error.length; i++) {
                    var ul = document.getElementById('formError');
                    var li = document.createElement("li");
                    li.appendChild(document.createTextNode(msg.error[i]));
                    ul.appendChild(li);
                }
            }
            document.getElementById("Password").value = "";
        }
    };
    // Запрос на сервер
    request.send(JSON.stringify({ email: email, password: password }));
}
function logOff() {
    var request = new XMLHttpRequest();
    request.open("POST", "api/account/logoff");
    request.onload = function () {
        var msg = JSON.parse(this.responseText);
        document.getElementById("msg").innerHTML = "";
        var mydiv = document.getElementById('formError');
        while (mydiv.firstChild) {
            mydiv.removeChild(mydiv.firstChild);
        }
        document.getElementById("msg").innerHTML = msg.message;
    };
    request.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    request.send();
}

document.addEventListener("DOMContentLoaded", function (event) {
    getCurrentUser();
});

function getCurrentUser() {
    let request = new XMLHttpRequest();
    request.open("POST", "/api/Account/isAuthenticated", true);
    request.onload = function () {
        let myObj = "";
        myObj = request.responseText !== "" ?
            JSON.parse(request.responseText) : {};
        document.getElementById("msg").innerHTML = myObj.message;
    };
    request.send();
}