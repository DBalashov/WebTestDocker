function getResponse(baseUrl) {
    $(".waiting").removeClass("hidden");
    $(".alert").addClass("hidden");
    $(".data, .headers").html('');

    $.get(baseUrl + '?ids=' + $("input[type='text']").val())
        .done(function (r) {
            $(".waiting").addClass("hidden");
            if (r.success) {
                $(".alert-primary").removeClass("hidden").html("OK");

                var items = [];
                for (var i = 0; i < r.data.length; i++) {
                    items.push(
                        "<tr><td>" + r.data[i].name + "</td>" +
                        "<td>" + r.data[i].value + "</td></tr>");
                }

                $(".data").html("<table>" + items.join('') + "</table>");
            } else {
                $(".alert-danger").removeClass("hidden").html(r.error);
            }
            
            if(r.requestHeaders)
                $(".headers").html(convertHeaders(r.requestHeaders));
        })
        .catch(function (r) {
            $(".waiting").addClass("hidden");
            $(".alert-danger")
                .removeClass("hidden")
                .html("<div>Headers: " + r.getAllResponseHeaders() + "</div>" +
                    "<div>" + r.statusText + "</div>" +
                    "<div>" + r.state() + "</div>");
        });
}

function convertHeaders(r) {
    var items = [];
    items.push("<tr><th colspan='2'>Request headers</th></tr>")
    for (var i = 0; i < r.length; i++) {
        items.push("<tr><td style='width:20em'>" + r[i].name + "</td><td>" + r[i].value + "</td></tr>");
    }

    return "<table>" + items.join('') + "</table>";
}