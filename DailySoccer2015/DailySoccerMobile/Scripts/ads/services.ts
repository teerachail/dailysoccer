module app.ads {
	'use strict';

    interface IAdvertisementResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        GetAdvertisement(): T;
    }
    export class AdvertisementService {

        private svc: IAdvertisementResourceClass<any>;

        static $inject = ['appConfig', '$resource'];
        constructor(appConfig: IAppConfig, private $resource: angular.resource.IResourceService) {
            this.svc = <IAdvertisementResourceClass<any>>$resource(appConfig.AdvertisementUrl, {}, {
                GetAdvertisement: { method: 'GET' }
            });
        }

        public GetAdvertisement(): ng.IPromise<any> {
            return this.svc.GetAdvertisement().$promise;
        }
    }

	angular
		.module('app.ads')
        .service('app.ads.AdvertisementService', AdvertisementService);
}