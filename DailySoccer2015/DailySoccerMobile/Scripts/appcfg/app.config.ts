module app {
    'use strict';

    export interface IAppConfig {
        ProfileUrl: string;
        VerifyPhoneUrl: string;
        BuyCouponUrl: string;
        CouponSummaryUrl: string;
        PredictUrl: string;
        RewardUrl: string;
        MatchesUrl: string;
        HistoryUrl: string;
        TeamUrl: string;
        LeagueUrl: string;
        AdvertisementUrl: string;
    }

    export class AppConfig implements IAppConfig {

        public ProfileUrl: string;
        public VerifyPhoneUrl: string;
        public BuyCouponUrl: string;
        public CouponSummaryUrl: string;
        public PredictUrl: string;
        public MatchesUrl: string;
        public RewardUrl: string;
        public HistoryUrl: string;
        public TeamUrl: string;
        public LeagueUrl: string;
        public AdvertisementUrl: string;

        static $inject = ['defaultUrl'];
        constructor(defaultUrl: string) {
            var apiUrl = defaultUrl + '/api';

            this.ProfileUrl = apiUrl + '/profiles/:id/:action';
            this.VerifyPhoneUrl = this.ProfileUrl;
            this.BuyCouponUrl = apiUrl + '/coupons/buy';
            this.CouponSummaryUrl = apiUrl + '/coupons/summary/:id';
            this.PredictUrl = apiUrl + '/predictions/:id';
            this.RewardUrl = apiUrl + '/rewards/:action/:id';
            this.MatchesUrl = apiUrl + '/matches/:day';
            this.HistoryUrl = apiUrl + '/history';
            this.TeamUrl = apiUrl + '/teams';
            this.LeagueUrl = apiUrl + '/leagues';
            this.AdvertisementUrl = apiUrl + '/advertisements';
        }

    }

    angular
        .module('app')
        .constant('defaultUrl', 'http://dailysoccer-joker.azurewebsites.net')
        .constant('MaximunPredictionCount', 5)
        .service('appConfig', AppConfig);
}