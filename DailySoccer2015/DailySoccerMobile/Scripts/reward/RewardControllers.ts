module app.reward {
    'use strict';

    class RewardsController {

        static $inject = ['data', 'point', 'app.reward.BuyCouponDataService'];
        constructor(public data, public point, private buySvc: app.reward.BuyCouponDataService) {
            buySvc.InitialData(200, 500); // HACK: Setup coupon data
        }

    }

    angular
        .module('app.reward')
        .controller('app.reward.RewardsController', RewardsController);
}