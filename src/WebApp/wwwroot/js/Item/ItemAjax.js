﻿import { GetMessageDomain } from '../site.js';

$(document).ready(function () {
    //$('#formCreateItem').submit(function (e) {
    //    e.preventDefault()
    //    if ($(this).valid()) {
    //        $.ajax({
    //            type: 'POST',
    //            url: '/Item/Create/',
    //            data: $(this).serialize(),
    //            success: function (response) {
    //                GetMessageDomain()
    //                $('#listItens').empty()
    //                $('#listItens').append(response)
    //                $('#CodigoItem').attr('value', $('#proxItem').val())
    //            },
    //            error: function () {
    //                $('#formCreateItem').validate();
    //                GetMessageDomain()
    //            }
    //        })
    //    }
    //})

    $("#AnoAta").change(function () {
        $('#CodigoItem').find('option').remove();
        $('#CodigoItem').trigger("chosen:updated");
        var year = $(this).find("option:selected").val();

        if (year != "") {
            $.ajax({
                type: 'GET',
                url: '/Item/AutoCompleteListCodeAta/',
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: { yearAta: year },
                success: function (response) {
                    $('#CodigoAta').find('option').remove();
                    $('<option>').val("").text("...").appendTo($('#CodigoAta'))
                    if (response.length > 0) {
                        $.each(response, function (i, d) {
                            $('<option>').val(d).text(d).appendTo($('#CodigoAta'))
                        })
                    }
                }
            })
        } else {
            $('#CodigoAta').find('option').remove();
            $('<option>').val("").text("...").appendTo($('#CodigoAta'))
        }
    })

    $('#CodigoAta').change(function () {
        AutoCompleteItem()
    })

    function AutoCompleteItem() {
        var pathname = window.location.pathname.split('/');
        if (pathname[2] == "Create") {
            $.ajax({
                type: 'GET',
                url: '/Item/AutoCompleteCodeItem/',
                data: { yearAta: $('#AnoAta').val(), codeAta: $('#CodigoAta').val() },
                success: function (response) {
                    $('#listItens').empty()
                    $('#listItens').append(response)
                    $('#CodigoItem').attr('value', $('#proxItem').val())
                },
                error: function () {
                    $('#listItens').empty()
                    $('#CodigoItem').attr('value', 1)
                }
            })
        } else {
            var yearAta = $('#AnoAta').val()
            var codeAta = $('#CodigoAta').val()

            $.ajax({
                type: 'GET',
                url: '/Item/AutoCompleteListCodeItem/',
                data: { yearAta, codeAta },
                success: function (response) {
                    $('#CodigoItem').find('option').remove();
                    if (response.length > 0) {
                        $.each(response, function (i, d) {
                            $('<option>').val(d.id).text(d.numeroItem + " - " + d.descricao).appendTo($('#CodigoItem'))
                        })
                    }
                    else {
                        $('#CodigoItem').find('option').remove();
                    }
                    $('#CodigoItem').trigger("chosen:updated");

                }
            })
            GetListDetentoraRegistered(yearAta, codeAta)
        }
    }

    function GetListDetentoraRegistered(yearAta, codeAta) {
        $.ajax({
            type: 'GET',
            url: '/Item/GetListDetentoraRegistered/',
            data: { yearAta, codeAta },
            success: function (response) {
                $('#listDetentoraRegistered').empty()
                $('#listDetentoraRegistered').append(response)
            }
        })
    }

    $("#formIncludeDetentora").submit(function (e) {
        e.preventDefault()
        $.ajax({
            type: 'POST',
            url: '/Item/IncludeDetentora/',
            async: true,
            data: $(this).serialize(),
            success: function (response) {
                GetMessageDomain()
                GetListDetentoraRegistered(response.yearAta, response.codeAta);
                AutoCompleteItem()
            },
            error: function () {
                GetMessageDomain()
            }
        })
    })
})