function openSlidePanel(url, title) {
    var element = document.getElementById('gameSlidePanel');

    var panel = bootstrap.Offcanvas.getInstance(element) || new bootstrap.Offcanvas(element);

    $('#gameSlidePanelLabel').text(title);
    $('#slidePanelBody').html('<div class="text-center mt-5"><div class="spinner-border text-primary" role="status"></div></div>');

    panel.show();

    $.get(url, function (data) {
        $('#slidePanelBody').html(data);
    });
}

$(document).on('submit', '#gameSlidePanel form', function (e) {
    e.preventDefault();

    var form = $(this);
    var url = form.attr('action');
    var formData = new FormData(this); 

    $.ajax({
        type: 'POST',
        url: url,
        data: formData,
        contentType: false,
        processData: false,
        success: function (res) {
            if (res.success) {
                var panelEl = document.getElementById('gameSlidePanel');
                var panel = bootstrap.Offcanvas.getInstance(panelEl);
                if (panel) panel.hide();

                location.reload();
            } else {
                $('#slidePanelBody').html(res);
            }
        },
        error: function (err) {
            console.log(err);
            alert("An error occurred while saving.");
        }
    });
});

function deleteItem(url) {
    $.post(url, function (res) {
        if (res.success) {
            alert(res.message);
            location.reload();
        } else {
            alert("Error: " + res.message);
        }
    }).fail(function () {
        alert("Server error occurred.");
    });
}