﻿module app.shared {
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

    export class CouponPointsService extends MockTableDataServiceBase {

        static $inject = ['$resource'];
        constructor($resource: angular.resource.IResourceService) {
            super("mas-appmob-couponPoints", $resource);
        }
    }

    export class MatchService extends MockTableDataServiceBase {

        static $inject = ['$resource'];
        constructor($resource: angular.resource.IResourceService) {
            super("mas-appmob-match", $resource);
        }
    }

    export class RewardsService extends MockTableDataServiceBase {

        static $inject = ['$resource'];
        constructor($resource: angular.resource.IResourceService) {
            super("mas-appmob-rewards", $resource);
        }
    }

    export class WinnersRewardService extends MockTableDataServiceBase {

        static $inject = ['$resource'];
        constructor($resource: angular.resource.IResourceService) {
            super("mas-appmob-winnersReward", $resource);
        }
    }

    export class WinnersListService extends MockTableDataServiceBase {

        static $inject = ['$resource'];
        constructor($resource: angular.resource.IResourceService) {
            super("mas-appmob-winnersList", $resource);
        }
    }

    export class MyRewardsService extends MockTableDataServiceBase {

        static $inject = ['$resource'];
        constructor($resource: angular.resource.IResourceService) {
            super("mas-appmob-myReward", $resource);
        }
    }

    export class MyOldRewardsService extends MockTableDataServiceBase {

        static $inject = ['$resource'];
        constructor($resource: angular.resource.IResourceService) {
            super("mas-appmob-myOldReward", $resource);
        }
    }

    export class YearlyHistoryService extends MockTableDataServiceBase {

        static $inject = ['$resource'];
        constructor($resource: angular.resource.IResourceService) {
            super("mas-appmob-yearHistory", $resource);
        }
    }

    export class MouthlyHistoryService extends MockTableDataServiceBase {

        static $inject = ['$resource'];
        constructor($resource: angular.resource.IResourceService) {
            super("mas-appmob-mouthHistory", $resource);
        }
    }

    export class DaylyHistoryService extends MockTableDataServiceBase {

        static $inject = ['$resource'];
        constructor($resource: angular.resource.IResourceService) {
            super("mas-appmob-dayHistory", $resource);
        }
    }

    export class FavoriteLeagueService extends MockTableDataServiceBase {

        static $inject = ['$resource'];
        constructor($resource: angular.resource.IResourceService) {
            super("mas-appmob-favoriteLeague", $resource);
        }
    }

	angular
        .module('app.shared')
        .service('app.shared.CouponPointsService', CouponPointsService)
        .service('app.shared.MatchService', MatchService)
        .service('app.shared.RewardsService', RewardsService)
        .service('app.shared.WinnersRewardService', WinnersRewardService)
        .service('app.shared.WinnersListService', WinnersListService)
        .service('app.shared.MyRewardsService', MyRewardsService)
        .service('app.shared.MyOldRewardsService', MyOldRewardsService)
        .service('app.shared.YearlyHistoryService', YearlyHistoryService)
        .service('app.shared.MouthlyHistoryService', MouthlyHistoryService)
        .service('app.shared.DaylyHistoryService', DaylyHistoryService)
        .service('app.shared.FavoriteLeagueService', FavoriteLeagueService)
        .service('app.shared.SampleDataService', SampleDataService)
        .service('app.shared.SampleUrlDataService', SampleUrlDataService)
        .service('app.shared.MockTableDataServiceBase', MockTableDataServiceBase);
}