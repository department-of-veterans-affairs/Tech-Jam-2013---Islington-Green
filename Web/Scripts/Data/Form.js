define(
    'Form',
    ['jquery', 'knockout-mapping', 'bootstrap-switch'],
    function ($) {
        var containers = $('.data-list');

    	var render = function(data) {
    	    if (data) {
    	        var model = ko.mapping.fromJS(data);
    	        containers.each(function(index, item) {
    	            ko.applyBindings(model, item);
    	        });
    	        $('.portal').show(500, "fade");
    	    }
    	};

        return {
            populate: function (repo) {
                render(repo.sets);
            }
        };
    }
);
