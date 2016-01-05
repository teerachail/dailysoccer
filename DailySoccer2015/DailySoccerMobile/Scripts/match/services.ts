module app.match {
	'use strict';

    export interface IMatchResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        GetMatchesByDate(data: T): T;
        GetPredictionsByDate(data: T): T;

    }
    interface IPredictResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        Predict(data: T): T;
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

	angular
		.module('app.match')
        .service('app.match.MatchService', MatchService)
        .service('app.match.PredictionsService', PredictionsService);
}