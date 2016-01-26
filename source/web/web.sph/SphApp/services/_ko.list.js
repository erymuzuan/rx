define([], function(){


var intiFilter = function (element, options, search) {
          var path = options.path,
              tooltip = options.tooltip || "Type to filter current page or type and [ENTER] to apply _all in remote query",
              $element = $(element),
              $filterInput = $("<input data-toggle=\"tooltip\" title=\"" + tooltip + "\" type=\"search\" class=\"search-query input-medium form-control\" placeholder=\"Filter.. \">"),
              $serverLoadButton = $("<a href='/#' title='Carian server'><i class='add-on icon-search'></i><a>"),
              $form = $("<form class='form-search row'>" +
                  " <div class='input-group pull-right' style='width:300px'>" +
                  "<span class='input-group-addon'>" +
                  " <span class='glyphicon glyphicon-remove'></span>" +
                  "</span> " +
                  "</div>" +
                  " </form>");


          $form.find("span.input-group-addon").before($filterInput);
          $form.find("span.glyphicon-remove").after($serverLoadButton);
          $element.before($form);

          $form.submit(function (e) {
              e.preventDefault();
              var filter = $filterInput.val().toLowerCase(),
                  tcs = new $.Deferred();
              if (!filter) {
                  return tcs.promise();
              }
              if (typeof search=== "function") {
                  return search("_all:" + filter);
              }
              return tcs.promise();
          });



          var dofilter = function () {
              var $rows = $element.find(path),
                  filter = $filterInput.val().toLowerCase();
              $rows.each(function () {
                  var $tr = $(this),
                      text = $tr.text().toLowerCase().trim();
                  if (!text) {
                      $("input", $tr).each(function (i, v) { text += " " + $(v).val() });
                      text = text.toLowerCase().trim();
                  }
                  if (text.indexOf(filter) > -1) {
                      $tr.show();
                  } else {
                      $tr.hide();
                  }
              });


          },
          throttled = _.throttle(dofilter, 800);

          $filterInput.on("keyup", throttled).siblings("span.input-group-addon")
              .click(function () {
                  $filterInput.val("");
                  dofilter();
              });

          if ($filterInput.val()) {
              dofilter();
          }
          $filterInput.tooltip();

      };


ko.bindingHandlers.queryPaging = {
  init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
      var value = valueAccessor(),
          query = value.query,
          executedQuery = null,
          list = value.list,
          map = value.map,
          pagerHidden = value.pagerHidden || false,
          searchButton = value.searchButton,
          $element = $(element),
          context = require("services/datacontext"),
          logger = require("services/logger"),
          cultures = require(objectbuilders.cultures),
          $pagerPanel = $("<div></div>"),
          $spinner = $("<img src=\"/Images/spinner-md.gif\" alt=\"loading\" class=\"absolute-center\" />"),
          startLoad = function () {
              $spinner.show();
              $element.fadeTo("fast", 0.33);
          },
          endLoad = function () {
              $spinner.hide();
              $element.fadeTo("fast", 1);
          },
          pager = null,
          setItemsSource = function (items) {

              if (!pagerHidden) {
                  _(items).each(function (v) {
                      v.pager = {
                          page: pager.page(),
                          size: pager.pageSize()
                      };
                  });
              }

              if (map) {
                  items = _(items).map(map);
              }
              if (typeof list === "string") {
                  viewModel[list](items);
              }
              if (typeof list === "function") {
                  list(items);
              }
          },
          pageChanged = function (page, size) {
              startLoad();
              var url = query + "?page=" + (page || 1) + "&size=" + (size || 20);
              if(executedQuery) url +="&q=" + executedQuery
              $.getJSON(url)
                   .then(function (lo) {
                       setItemsSource(lo._results);
                       endLoad();
                   });
          },
          search = function (page, size) {
              var tcs1 = new $.Deferred();
              startLoad();
              var url = query + "?page=" + (page || 1) + "&size=" + (size || 20);
              if(query.indexOf("?") > -1)url = query + "&page=" + (page || 1) + "&size=" + (size || 20);
              if(executedQuery) url +="&q=" + executedQuery
              $.getJSON(url)
                  .then(function (lo) {
                      if (pager) {
                          pager.update(lo._count);
                      } else {
                          var pagerOptions = {
                              element: $pagerPanel,
                              count: lo._count,
                              changed: pageChanged,
                              hidden: pagerHidden
                          };
                          pager = new bespoke.utils.ServerPager(pagerOptions);

                      }

                      setTimeout(function () {
                          setItemsSource(lo._results);
                          tcs1.resolve(lo);
                          endLoad();
                      }, 500);

                  });
              return tcs1.promise();
          },
          filterAndSearch = function (text) {
              pager.destroy();
              pager = null;
              executedQuery = text;
              return search();
          },
          filter = ko.unwrap(value.filter) || {};

      //exposed the search function
      intiFilter(element, { path : filter.path || 'tbody>tr'}, filterAndSearch);

      $element.after($pagerPanel).after($spinner)
          .fadeTo("slow", 0.33);

      if (searchButton) {
          $(document).on("click", searchButton, function (e) {
              e.preventDefault();
              if (!$(this).parents("form")[0].checkValidity()) {
                  logger.error(cultures.messages.FORM_IS_NOT_VALID);
                  return;
              }
              search(ko.toJS(query), 1, pager.pageSize());
          });

      }


      search(ko.toJS(executedQuery));
      return {
          search: search,
          filterAndSearch: filterAndSearch
      };

  }
};

});
