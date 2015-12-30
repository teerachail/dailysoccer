module app.reward {
    'use strict';

    class RewardsController {

        private couponSummary: GetCouponSummaryRespond;

        static $inject = ['data', 'app.reward.CouponSummaryService', 'app.shared.UserProfileService', 'app.reward.BuyCouponDataService'];
        constructor(public data, private couponSvc: CouponSummaryService, private userprofileSvc: app.shared.UserProfileService, private buySvc: app.reward.BuyCouponDataService) {
            couponSvc.GetCouponSummary(userprofileSvc.GetUserProfile().UserId).then(
                (respond: GetCouponSummaryRespond) => {
                    this.couponSummary = respond;
                },
                err=> {
                    // TODO: Inform error to user
                    console.log('ERROR: ' + err)
                });
            buySvc.InitialData(200, 500); // HACK: Setup coupon data
        }

    }

    angular
        .module('app.reward')
        .controller('app.reward.RewardsController', RewardsController);
}