module app.match {
	'use strict';

    interface IMatchResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        GetMatchesByDate(data: T): T;
        GetPredictionsByDate(data: T): T;
    }
    interface IPredictResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        Predict(data: T): T;
    }
    interface IHistoryResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        GetHistoryMonthly(data: T): T;
        GetHistoryDaily(data: T): T;
    }

    export class MatchService {

        private svc: IMatchResourceClass<any>;
        private getMatchesByDate: ng.resource.IResourceClass<any>;
        private getPredictionsByDate: ng.resource.IResourceClass<any>;

        static $inject = ['appConfig', '$resource'];
        constructor(appConfig: IAppConfig, private $resource: angular.resource.IResourceService) {
            this.getMatchesByDate = $resource(appConfig.MatchesUrl, { "day": "@day"});
		}

        public GetMatchesByDate(day: number): ng.IPromise<LeagueInformation[]> {
            return this.getMatchesByDate.query({ day: day }).$promise;
        }

    }

    export class PredictionsService {

        private svc: IPredictResourceClass<any>;
        private getPredictionsByDate: ng.resource.IResourceClass<any>;

        static $inject = ['appConfig', '$resource'];
        constructor(appConfig: IAppConfig, private $resource: angular.resource.IResourceService) {
            this.getPredictionsByDate = $resource(appConfig.PredictUrl+'/:day', { "id": "@id", "day": "@day" });
            this.svc = <IPredictResourceClass<any>>$resource(appConfig.PredictUrl, { "id": "@id"}, {
                Predict: { method: 'PUT', isArray: true }
            });
        }

        public GetPredictionsByDate(userId: string, day: number): ng.IPromise<PredictionInformation[]> {
            return this.getPredictionsByDate.query({ id: userId, day: day }).$promise;
        }

        public Predict(userId: string, matchId: string, teamId: string, isCancel: boolean): ng.IPromise<PredictionInformation[]> {
            return this.svc.Predict(new PredictionRequest(userId, matchId, teamId, isCancel)).$promise;
        }
    }

    export class HistoryService {
        private historyMonthlyScv: IHistoryResourceClass<any>;
        private historyDailyScv: IHistoryResourceClass<any>;

        constructor(appConfig: IAppConfig, private $resource: angular.resource.IResourceService) {
            this.historyMonthlyScv = <IHistoryResourceClass<any>>$resource(appConfig.HistoryUrl + '/:id', {}, {
                GetHistoryMonthly: { method: 'GET', isArray: true }
            });
            this.historyDailyScv = <IHistoryResourceClass<any>>$resource(appConfig.HistoryUrl + '/:id/:year/:month', {}, {
                GetHistoryDaily: { method: 'GET', isArray: true }
            });
        }

        public GetHistoryMonthly(id: string): ng.IPromise<PredictionMonthlySummary[]> {
            return this.historyMonthlyScv.GetHistoryMonthly(new HistoryMonthlyRequest(id));
        }
        public GetHistoryDaily(id: string, year: number, month: number): ng.IPromise<PredictionDailySummary[]> {
            return this.historyDailyScv.GetHistoryDaily(new HistoryDailyRequest(id, year, month));
        }
    }

	angular
		.module('app.match')
        .service('app.match.MatchService', MatchService)
        .service('app.match.PredictionsService', PredictionsService)
        .service('app.match.HistoryService', HistoryService);;
}