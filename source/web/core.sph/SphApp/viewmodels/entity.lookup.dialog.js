﻿/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schema/sph.domain.g.js" />


define(['services/datacontext', 'services/logger', 'plugins/dialog'],
    function (context, logger, dialog) {

        var searchText = ko.observable(),
            results = ko.observableArray(),
            options = ko.observable(),
            selected = ko.observable(),
            attached = function(view){
                var thead  = "<tr><th></th>";
                _(ko.unwrap(options().columns)).each(function(v){
                    thead += '<th>' + v + '<th>';
                });
                thead += '</tr>';
                $('#thead').html(thead);

                $(view).on('click', 'tr', function(e){
                    e.preventDefault();
                    $('i.fa-check').css("color","#dfdfdf");
                    $(this).find('i.fa-check').css("color","#4B4B4B");
                    var id = parseInt( $(this).data('id'));
                    var item = _(results()).find(function(v){
                        return id === v[ko.unwrap(options().entity) + 'Id'];
                    });
                    selected(item);
                });
                selected(null);
                setTimeout(function(){
                    $('#search-text').focus();

                },500);
            },
            okClick = function () {
                    dialog.close(this, "OK");
            },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            },
            searchAsync = function () {
                var tcs = new $.Deferred(),
                    query = {"query": {"query_string": {"default_field": "_all",
                        "query": ko.unwrap(searchText)
                    }}};

                context.searchAsync({entity: ko.unwrap(options().entity)}, query)
                    .done(function (lo) {
                        results(lo.itemCollection);

                        var tbody  = "";
                        _(lo.itemCollection).each(function(m){
                            tbody += '<tr style="cursor: pointer" data-id="'+ m[ko.unwrap(options().entity) + 'Id'] +'">';
                            tbody += '<td><i class="fa fa-check" style="color:#dfdfdf"></i></td>';
                            _(ko.unwrap(options().columns)).each(function(v){
                                tbody += '  <td>' + m[v] + '<td>\r';
                            });
                            tbody += '</tr>\r';
                        });
                        $('#tbody').html(tbody);
                        tcs.resolve(true);
                    });


                return tcs.promise();
            };

        var vm = {
            results: results,
            attached: attached,
            selected: selected,
            searchAsync: searchAsync,
            searchText: searchText,
            options: options,
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });
