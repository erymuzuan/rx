/// <reference path="toastr.js" />
/// <reference path="kendo.web.min.js" />
/// <reference path="../kendo/js/kendo.pager.js" />




var bespoke = bespoke || {};
bespoke.utils = {};

bespoke.utils.ServerPager = function (options) {
    options = options || {};
    var element = options.element,
        count = options.count || 0,
        changed = options.changed || function () {
            console.log("no change event");
        };

    var self2 = this;
    var rows = _.range(count);

    var pagerDataSource = new kendo.data.DataSource({
        data: rows,
        pageSize: 20
    });
    var pager = element.kendoPager({
        dataSource: pagerDataSource,
        messages: {
            display: "{0} - {1} of {2} rekod",
            empty: "Tiada rekod",
            page: "Muka",
            of: "dari {0}",
            itemsPerPage: "rekod setiap mukasurat",
            first: "Pergi mukasurat pertama",
            previous: "Pergi ke mukasurat belakang",
            next: "Sergi mukasurat depan",
            last: "Pergi mukasurat terakhir",
            refresh: "Muat"
        },
        pageSizes: [10, 20, 50]
    }).data("kendoPager");
    pager.page(1);
    pager.bind('change', function () {
        if (changed) {
            changed(pager.page(), pager.pageSize());
        }
    });

    self2.update = function (count2) {
        rows = [];
        for (var j = 0; j < count2 ; j++) {
            rows[j] = j;
        }
        pagerDataSource.data(rows);
    };

    self2.pageSize = function (size) {
        if (size) {
            pager.pageSize(size);
        }
        return pager.pageSize();
    };
    self2.page = function (pg) {
        if (pg) {
            pager.page(pg);
        }
        return pager.page();
    };



    return self2;

};