/// <reference path="toastr.js" />
/// <reference path="_ko.kendo.js" />
/// <reference path="../kendo/js/kendo.pager.js" />
/// <reference path="../kendo/js/kendo.all.js" />




var bespoke = bespoke || {};
bespoke.utils = {};

bespoke.utils.ServerPager = function (options) {
    options = options || {};
    var element = options.element,
        count = options.count || 0,
        sizes = options.sizes || [10, 20, 50],
        defaultSize = options.defaultSize || 20,
        changed = options.changed || function () {
            console.log("no change event");
        },
        self2 = this,
        rows = _.range(count),
        pagerDataSource = new kendo.data.DataSource({
            data: rows,
            pageSize: defaultSize
        });
    if (options.hidden) {
        return self2;
    }

    var pager = element.kendoPager({
        dataSource: pagerDataSource,
        pageSizes: sizes
    }).data("kendoPager");
    pager.page(1);
    pager.bind("change", function () {
        if (changed) {
            changed(pager.page(), pager.pageSize());
        }
    });

    self2.update = function (count2) {
        rows = [];
        for (var j = 0; j < count2 ; j++) {
            rows[j] = j;
        }
        setTimeout(function () {
            pagerDataSource.data(rows);
        }, 500);
    };
    self2.destroy = function () {
        pager.destroy();
        element.empty();
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

    var dropdownlist = $(element).find("select").data("kendoDropDownList");
    dropdownlist.bind("change", function () {
        try {
            changed(1, parseInt(this.value()));
        } catch (e) {

        }
    });



    return self2;

};