module app.shared {
	'use strict';

    export interface IMockDataService {
        getAll(): ng.IPromise<any>;
        get(id): ng.IPromise<any>;
	}

	class MockTableDataServiceBase implements IMockDataService {

        private svc: ng.resource.IResourceClass<any>;

        static $inject = ['key', '$resource'];
        constructor(key: string, private $resource: angular.resource.IResourceService) {
            // TODO: initialize service
            this.svc = $resource("http://moman.azurewebsites.net/mgw/api/:key/:id", { "key": key, "id": "@id" });
		}

        public getAll(): ng.IPromise<any> {
            // TODO: Implement or remove a method
            return this.svc.query().$promise;
        }

        public get(id): ng.IPromise<any> {
            return (<ng.resource.IResource<any>>this.svc.get({ "id": id })).$promise;
        }

    }

    class MockUrlDataServiceBase implements IMockDataService {

        private svc: ng.resource.IResourceClass<any>;

        static $inject = ['url', '$resource'];
        constructor(url: string, private $resource: angular.resource.IResourceService) {
            // TODO: initialize service
            this.svc = $resource(url, { "id": "@id" });
        }

        public getAll(): ng.IPromise<any> {
            // TODO: Implement or remove a method
            return this.svc.query().$promise;
        }

        public get(id): ng.IPromise<any> {
            return (<ng.resource.IResource<any>>this.svc.get({ "id": id })).$promise;
        }

    }

    export class SampleDataService extends MockTableDataServiceBase {

        static $inject = ['$resource'];
        constructor($resource: angular.resource.IResourceService) {
            super("mas-appmob-xpertab1", $resource);
        }

    }

    export class SampleUrlDataService extends MockUrlDataServiceBase {

        static $inject = ['$resource'];
        constructor($resource: angular.resource.IResourceService) {
            super("https://avxdemo.blob.core.windows.net/dionic/sample.json", $resource);
        }

    }

	angular
		.module('app.shared')
        .service('app.shared.SampleDataService', SampleDataService)
        .service('app.shared.SampleUrlDataService', SampleUrlDataService)
        .service('app.shared.MockTableDataServiceBase', MockTableDataServiceBase);
}