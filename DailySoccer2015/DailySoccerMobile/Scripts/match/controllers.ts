module app.match {
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
		.module('app.match')
		.controller('app.match.MyController', MyController);
}