$(document).ready(function () {
    ///Function to do AJAX search.
    function search() {
        var query_value = $('input#search').val();
        query_value = query_value.toLowerCase();
        $('b#search-string').text(query_value);
            $.ajax({
                type: "POST",
                url: "WebService.asmx/search",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ str: query_value }),
                datatype: "json",
                cache: false,
                success: function (result) {
                    for (var i = 0; i < result.d.length; i++) {
                        var html = "";
                        html += "<ul><li class=\"result\">";
                        html += "<h3>nameString</h3>";
                        html += "</li>";
                        html += "</ul>";
                        html = html.replace("nameString", result.d[i]);
                        result.d[i] = html;
                    }
                    $("ul#results").html(result.d);
                }
            });
    }
    ///Function to call search() function. Embedded with timeout to allow for user fast typing.
    ///Binded to keyup.
    $("input#search").bind("keyup", function (e) {
        // Set Timeout
        clearTimeout($.data(this, 'timer'));

        // Set Search String
        var search_string = $(this).val();

        // Do Search
        if (search_string == '') {
            $("ul#results").fadeOut();
            $('h4#results-text').fadeOut();
        } else {
            $("ul#results").fadeIn();
            $('h4#results-text').fadeIn();
            $(this).data('timer', setTimeout(search, 50));
        };
    });
});