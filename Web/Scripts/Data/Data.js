define(
    'Data',
    ['underscore', 'amplify'],
    function (_, amplify) {
        var load = function (set, callback) {
                amplify.request({
                    resourceId: set,
                    success: function (data) {
                        self.sets[set] = data;
                        callback();
                    }
                });
            },
            self = {
                sets: {},
                init: function (callback) {
                    var _this = this,
                        sets = ["Appointment", "Drugs", "Labs", "Allergy", "MedicalEvents", "VitalsAndReadings", "TreatmentPlan",
                                "TreamentFacilities", "MilitaryHealthHistory", "Immunizations", "HealthInsurance", "HealthCareProvider",
                                "FamilyHealthHistory"];
                    _.each(sets, function (set) {
                        load(set, function () {
                            var done = _.chain(sets).values().all(function(x) { 
                                    return !_.isEmpty(x);
                                }).value() && _.keys(_this.sets).length === sets.length;

                            if (done) {
                                callback(_this);
                            }
                        });
                    });
                }
            };

        return self;
    }
);
