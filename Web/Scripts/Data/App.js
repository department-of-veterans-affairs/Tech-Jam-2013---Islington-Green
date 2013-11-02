define(
    'App',
    ['jquery','bootstrap','fastclick'],
    (function () {
        window.onerror = function() {
            return true;
        };

        return {
            init: function (Data, Form) {
                $(document).ready(function () {
                    Data.init(function (repo) {
                        Form.populate(repo);
                    });
                });
            }
        };
    })()
);
