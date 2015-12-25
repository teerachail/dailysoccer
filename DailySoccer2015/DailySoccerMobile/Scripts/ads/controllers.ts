module app.ads {
	'use strict';

	class MyController {

		//public model: any = null;

		static $inject = ['$scope'];
		constructor(private $scope: ng.IScope) {
		}

		// public myMethod(): void {
		// }

	}

	angular
		.module('app.ads')
		.controller('app.ads.MyController', MyController);
}