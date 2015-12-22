module app.shared {
	'use strict';

    export interface IMockDataService {
        getAll(): ng.IPromise<any>;
        get(id): ng.IPromise<any>;
	}

	export class MockDataServiceBase implements IMockDataService {

        private svc: ng.resource.IResourceClass<any>;

        static $inject = ['tableName', '$resource'];
        constructor(tableName: string, private $resource: angular.resource.IResourceService) {
            // TODO: initialize service
            this.svc = $resource("http://moman.azurewebsites.net/mgw/api/:tableName/:id", { "tableName": tableName, "id": "@id" });
		}

        public getAll(): ng.IPromise<any> {
            // TODO: Implement or remove a method
            return this.svc.query().$promise;
        }

        public get(id): ng.IPromise<any> {
            return (<ng.resource.IResource<any>>this.svc.get({ "id": id })).$promise;
        }

	}

    export class SampleDataService extends MockDataServiceBase {

        static $inject = ['$resource'];
        constructor($resource: angular.resource.IResourceService) {
            super("demo1", $resource);
        }

    }

	angular
		.module('app.shared')
        .service('app.shared.MockDataService', MockDataServiceBase)
        .service('app.shared.SampleDataService', SampleDataService);
}