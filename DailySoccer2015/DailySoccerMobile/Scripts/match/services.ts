module app.match {
	'use strict';

	export interface IMyService {
		method(): void;
	}

	export class MyService implements IMyService {

		static $inject = ['$resource'];
		constructor(private $resource: angular.resource.IResourceService) {
			// TODO: initialize service
			
		}

		public method(): void {
			// TODO: Implement or remove a method
		}

	}

	angular
		.module('app.match')
		.service('app.match.MyService', MyService);
}