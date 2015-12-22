module app.shared {
	'use strict';

	class SimpleDataController {

		//public model: any = null;

        static $inject = ['data', '$scope', 'app.shared.SampleDataService'];
        constructor(public data, private $scope: ng.IScope, public SampleDataService: IMockDataService) {
		}

		// public myMethod(): void {
		// }

	}

	angular
		.module('app.shared')
		.controller('app.shared.SimpleDataController', SimpleDataController);
}