﻿$(document).ready(function () {
    $(document).ajaxStop(function () {
        $('#ListAtaTable').DataTable();
    })

    $('.btnTab').click(function () {
        $('#result').empty()
        $('.AnoAtaSelect').find('option[value=""]').prop("selected", true);
        $('.CodigoAtaSelect').find('option').remove();
        $('<option>').val("").text("...").appendTo($('.CodigoAtaSelect'))
    })
})