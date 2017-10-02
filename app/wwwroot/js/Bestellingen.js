var bestellingenView = {
    init: function() {
        $("#AantalMaanden").change(function () {
            $.post(this.action, $(this).serialize(), function (data) {
                $("#bestellingen").html(data);
            });
        });
    }
}

$(function() {
    bestellingenView.init();
});

