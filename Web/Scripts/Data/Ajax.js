/*****************************************************************************************
 * Ajax.js
 */
/// <reference path="../lib/amplify-1.1.0.min.js" />

define(
    'Ajax',
    ['underscore', 'jquery', 'amplify'],
    function (_, $, amplify) {
        var ajaxSettings = $.ajaxSetup({
            contentType: 'application/json; charset=utf-8',
            global: false,
            async: true,
            dataType: 'json',
            timeout: 30000,
            beforeSend: function (xhr) {
                xhr.setRequestHeader('Accept', 'application/json;text/javascript;application/javascript;text/html;text/plain');
                return true;
            }
        });

        /*****************************************************************************************
         * AJAX requests
         */
        var baseUrl = '/Api',
            defaultCache = {
                type: "sessionStorage",
                expires: 0
            },
            defaultDecoder = function (data, status, xhr, success, error) {
                data ? success(data.d || data) : error(xhr, status, data);
            };
        _(["Appointment", "Drugs", "Labs", "Allergy", "MedicalEvents", "VitalsAndReadings", "TreatmentPlan",
        "TreamentFacilities", "MilitaryHealthHistory", "Immunizations", "HealthInsurance", "HealthCareProvider",
        "FamilyHealthHistory"]).each(function (set) {
            amplify.request.define(set, "ajax", {
                url: baseUrl + "/" + set,
                dataType: "json",
                type: "GET",
                cache: null, //defaultCache,
                decoder: defaultDecoder
            });
        });
    }
);
