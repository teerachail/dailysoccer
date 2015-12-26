module app.ads {
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
		.module('app.ads')
		.service('app.ads.MyService', MyService);
}