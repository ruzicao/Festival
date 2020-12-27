$(document).ready(function () {

    var host = window.location.host;
    var token = null;
    var headers = {};
    var eventsEndpoint = "/api/events/";
    var placeEndpoint = "/api/places/";
    var formAction = "Create";
    var editingId;

    loadEvents();

    $("body").on("click", "#btnDelete", deleteEvent);
    $("body").on("click", "#btnEdit", editEvent);
    $("body").on("click", "#btn2", refreshForm);
    $("body").on("click", "#btnfilter", festivalYear);

    $("#registration").css("display", "none");

    $("#logout").css("display", "none");

    function loadEvents() {
        var requestUri = 'https://' + host + eventsEndpoint;
        $.getJSON(requestUri, setEvents);

        var dropdownUrl = 'https://' + host + placeEndpoint;
        $.getJSON(dropdownUrl, setDropdown);
    };


    function festivalYear() {
        var yearfrom = $("#yearfrom").val();
        var yearto = $("#yearto").val();
        var requestUri1 = 'https://' + host + '/api/eventyear?start=' + yearfrom.toString() + '&end=' + yearto.toString();
        $.getJSON(requestUri1, setEvents);
    }


    function setDropdown(data, status) {
        var city = $("#city");
        city.empty();

        if (status == "success") {
            for (i = 0; i < data.length; i++) {
                var option = "<option value=" + data[i].Id + ">" + data[i].Location + "</option>";
                city.append(option);
            }
        }
        else {
            alert("Error");
        }
    };


    function setEvents(data, status) {

        var $container = $("#dataEvent");
        $container.empty();

        if (status == "success") {
            var div = $("<div></div>");
            var h1 = $("<h1>Festivals</h1>");
            div.append(h1);
            var table = $("<table class='table table-bordered'></table>");

            if (token) {
                var header = $("<thead style=\"background-color:burlywood;\"><tr><td>Id</td><td>Name</td><td>Year</td><td>Place</td><td>Delete</td><td>Edit</td></tr></thead>");
            } else {
                var header = $("<thead style=\"background-color: burlywood;\"><tr><td>Id</td><td>Name</td><td>Year</td><td>Place</td></tr></thead>");
            }

            table.append(header);
            var tbody = $("<tbody></tbody>");
            for (i = 0; i < data.length; i++) {
                var row = "<tr>";
                var displayData = "<td>" + data[i].Id + "</td><td>" + data[i].Name + "</td><td>" + data[i].Year + "</td><td>" + data[i].Place.Location + "</td>";
                var stringId = data[i].Id.toString();
                var displayDelete = "<td><button id=btnDelete name=" + stringId + ">Delete</button></td>";
                var displayEdit = "<td><button id=btnEdit name=" + stringId + ">Edit</button></td>";
                if (token) {
                    row += displayData + displayDelete + displayEdit + "</tr>";
                } else {
                    row += displayData + "</tr>";
                }
                tbody.append(row);
            }
            table.append(tbody);
            div.append(table);

            if (token) {
 
                $("#formEventDiv").css("display", "block");
                $("#filterYear").css("display", "block");
            }

            $container.append(div);
        } else {
            var div = $("<div></div>");
            var h1 = $("<h1>Error</h1>");
            div.append(h1);
            $container.append(div);
        }
    }

    $("#btnlogout").click(function () {
        token = null;
        headers = {};

        $("#login").css("display", "block");
        $("#registration").css("display", "none");
        $("#logout").css("display", "none");
        $("#info").empty();
        $("#formEventDiv").css("display", "none");
        $("#btnreg").css("display", "block");
        $("#filterYear").css("display", "none");
        $("#stdiv").css("display", "none");
        loadEvents();
    });

    $("#btnreg").click(function () {

        $("#login").css("display", "none");
        $("#registration").css("display", "block");
        $("#logout").css("display", "none");
        $("#info").empty();
        $("#btnreg").css("display", "none");
        $("#stdiv").css("display", "none");
    });

    $("#registration").submit(function (e) {
        e.preventDefault();

        var email = $("#regEmail").val();
        var loz1 = $("#regLoz").val();
        var loz2 = $("#regLoz2").val();

        var sendData = {
            "Email": email,
            "Password": loz1,
            "ConfirmPassword": loz2
        };

        $.ajax({
            type: "POST",
            url: 'https://' + host + "/api/Account/Register",
            data: sendData

        }).done(function (data) {
            $("#info").append("Uspešna registracija. Možete se prijaviti na sistem.");
            alert("Uspešna registracija. Možete se prijaviti na sistem.");
            $("#login").css("display", "block");
            $("#registration").css("display", "none");

        }).fail(function (data) {
            alert("Error");
        });
    });

    $("#login").submit(function (e) {
        e.preventDefault();

        var email = $("#priEmail").val();
        var loz = $("#priLoz").val();

        var sendData = {
            "grant_type": "password",
            "username": email,
            "password": loz
        };

        $.ajax({
            "type": "POST",
            "url": 'https://' + host + "/Token",
            "data": sendData

        }).done(function (data) {
       
            $("#info").empty().append("Prijavljen korisnik: " + data.userName);
            token = data.access_token;
            $("#login").css("display", "none");
            $("#registration").css("display", "none");
            $("#logout").css("display", "block");
            $("#eventForm").css("display", "block");
            $("#filterYear").css("display", "block");
            $("#stdiv").css("display", "block");
            loadEvents();
            refreshLogin();
        }).fail(function (data) {
            alert(data + "Error");
        });
    });

    $("#eventForm").submit(function (e) {

        e.preventDefault();

        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }

        var eventName = $("#eventName").val();
        var eventPrice = $("#eventPrice").val();
        var eventYear = $("#eventYear").val();
        var eventCity = $("#city").val();

        var httpAction;
        var sendData;
        var url;


        if (formAction === "Create") {
            httpAction = "POST",
            url = 'https://' + host + eventsEndpoint,
            sendData = {
                "Name": eventName,
                "Price": eventPrice,
                "Year": eventYear,
                "PlaceId": eventCity
            };
        }
        else {
            httpAction = "PUT",
            url = 'https://' + host + eventsEndpoint + editingId.toString(),
            sendData = {
                "Id": editingId,
                "Name": eventName,
                "Price": eventPrice,
                "Year": eventYear,
                "PlaceId": eventCity
            };
        }

        $.ajax({
            url: url,
            type: httpAction,
            headers: headers,
            data: sendData
        })
            .done(function (data, status) {
                formAction = "Create";
                refreshTable();

            })
            .fail(function (data, status) {
                alert("Error");
            });
    });


    function deleteEvent() {

        var deleteID = this.name;

        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }

        $.ajax({
            url: 'https://' + host + eventsEndpoint + deleteID.toString(),
            type: "DELETE",
            headers: headers
        })
            .done(function (data, status) {
                refreshTable();
            })
            .fail(function (data, status) {
                alert("Error");
            });
    };

    function editEvent() {

        var editId = this.name;

        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }

        $.ajax({
            url: 'https://' + host + eventsEndpoint + editId.toString(),
            type: "GET",
            headers: headers
        })
            .done(function (data, status) {
                $("#eventName").val(data.Name);
                $("#eventPrice").val(data.Price);
                $("#eventYear").val(data.Year);
                $("#city").val(data.PlaceId);
                editingId = data.Id;
                formAction = "Update";
              
            })
            .fail(function (data, status) {
                formAction = "Create";
                alert("Error");
            });
    };


    function refreshTable() {

        $("#eventName").val('');
        $("#eventPrice").val('');
        $("#eventYear").val('');
        $("#city").val('');

        loadEvents();
    };

    function refreshForm() {

        $("#eventName").val('');
        $("#eventPrice").val('');
        $("#eventYear").val('');
        $("#city").val('');

        loadEvents();
    };

    function refreshLogin() {
        $("#priEmail").val('');
        $("#priLoz").val('');
    };

    function refreshRegistracija() {
        $("#regEmail").val('');
        $("#regLoz").val('');
        $("#regLoz2").val('');
    };



    $("#btnstat").click(function () {
        
        var requestUrl = 'https://' + host + '/api/statistics';
        $.getJSON(requestUrl, setStatistics);
    });

    function setStatistics(data, status) {

        var $container = $("#statdata");
        $container.empty();

        if (status == "success") {

            var div = $("<div></div>");
            var h1 = $("<h1>Statistics</h1>");
            div.append(h1);

            var table = $("<table class='table table-bordered'></table>");
            var header = $("<tr style=\"background-color:burlywood;\"><td>Id</td><td>Place</td><td>Sum</td></tr>");
            table.append(header);
            for (i = 0; i < data.length; i++) {

                var row = "<tr>";
                var displayData = "<td>" + data[i].Id + "</td><td>" + data[i].Location + "</td><td>" + data[i].SumPrice + "</td>";
                row += displayData + "</tr>";
                table.append(row);
            }
            div.append(table);
            $container.append(div);
        }
        else {
            var div = $("<div></div>");
            var h1 = $("<h1>Error</h1>");
            div.append(h1);
            $container.append(div);
        }
    };
});