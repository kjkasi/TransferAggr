﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    $(function () {
        $('#InputFrom').autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "http://localhost:5001/api/PlaceApi/SearchPlace",
                    data: { SearchText: request.term },
                    dataType: "json",
                    type: "GET",
                    success: function (data) {
                        if (data.length == 0) {
                            $('#FromPlaceId').val("");
                            $('#TransferMesssage').show();
                            return false;
                        }
                        else {
                            response($.map(data, function (item) {
                                return {
                                    //{guid: '20924468', address: 'улица Косыгина, дом 23, строение 2', raion: '', housenumber: '23', unit: '', …}
                                    label: item.cityParsed + ", " + item.address,
                                    place_id: item.GUID,
                                    long: item.long,
                                    lat: item.lat
                                }
                            }));
                        }
                    },
                    error: function (x, y, z) {
                        alert('error');
                    }
                });
            },
            messages: {
                noResults: "", results: ""
            },
            select: function (event, ui) {
                $('#InputFrom').val(ui.item.label);
                $('#InputFromPlaceId').val(ui.item.place_id);
                $('#InputFromLongitude').val(ui.item.long);
                $('#InputFromLatitude').val(ui.item.lat);
                return false;
            }
        });

        $('#InputTo').autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "http://localhost:5001/api/PlaceApi/SearchPlace",
                    data: { SearchText: request.term },
                    dataType: "json",
                    type: "GET",
                    success: function (data) {
                        if (data.length == 0) {
                            $('#FromPlaceId').val("");
                            $('#TransferMesssage').show();
                            return false;
                        }
                        else {
                            response($.map(data, function (item) {
                                return {
                                    label: item.cityParsed + ", " + item.address,
                                    place_id: item.GUID,
                                    long: item.long,
                                    lat: item.lat
                                }
                            }));
                        }
                    },
                    error: function (x, y, z) {
                        alert('error');
                    }
                });
            },
            messages: {
                noResults: "", results: ""
            },
            select: function (event, ui) {
                $('#InputTo').val(ui.item.label);
                $('#InputToPlaceId').val(ui.item.place_id);
                $('#InputToLongitude').val(ui.item.long);
                $('#InputToLatitude').val(ui.item.lat);
                return false;
            }
        });
    });
});
