module app.reward {
    'use strict';

    class RewardsController {

        static $inject = ['data', 'couponSummary', 'app.reward.BuyCouponDataService'];
        constructor(public data: RewardGroupRespond, private couponSummary: GetCouponSummaryRespond, private buySvc: app.reward.BuyCouponDataService) {
            buySvc.InitialData(data.RequiredPoints, couponSummary.RemainingPoints);
        }

    }

    angular
        .module('app.reward')
        .controller('app.reward.RewardsController', RewardsController);
}