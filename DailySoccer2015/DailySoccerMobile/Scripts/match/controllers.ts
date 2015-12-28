module app.match {
	'use strict';

    class PredictionController {
        static $inject = ['data'];
        constructor(public data) {
        }
    }
   
	angular
		.module('app.match')
        .controller('app.match.PredictionController', PredictionController);

}