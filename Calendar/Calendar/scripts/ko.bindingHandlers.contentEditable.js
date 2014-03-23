ko.bindingHandlers.htmlLazy = {
    update: function (element, valueAccessor) {
        var value = ko.unwrap(valueAccessor());

        //console.log(value);
        //console.log(element.isContentEditable);

        if (!element.isContentEditable) {
            element.innerHTML = value;
        }
    }
};
ko.bindingHandlers.contentEditable = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        var value = ko.unwrap(valueAccessor()),
            htmlLazy = allBindingsAccessor().htmlLazy;

        //console.log(element);
        //console.log(htmlLazy());
        //console.log(ko.isWriteableObservable(htmlLazy));

        $(element).on("input", function () {
            //console.log(this);
            //console.log(this.isContentEditable);
            //console.log("INPUT");
            if (this.isContentEditable && ko.isWriteableObservable(htmlLazy)) {
                //console.log("writable");
                htmlLazy(this.innerHTML);
            }
        });
    },
    update: function (element, valueAccessor) {
        var value = ko.unwrap(valueAccessor());

        element.contentEditable = value;

        if (!element.isContentEditable) {
            $(element).trigger("input");
        }
    }
};