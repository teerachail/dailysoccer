module app.reward {
    'use strict';

    class BuyCouponController {

        static $inject = ['$ionicModal', '$scope', '$state', 'app.reward.BuyCouponDataService'];
        constructor(private $ionicModal, private $scope, private $state: angular.ui.IStateService, private buySvc: app.reward.BuyCouponDataService) {
        }

        public BuyCoupons(buyAmount: number): void {
            const MinimumBuyAmount = 1;
            var isRequestValid = buyAmount >= MinimumBuyAmount && buyAmount <= this.buySvc.BuyingPower;
            if (!isRequestValid) return;

            this.buySvc.RequestBuyAmount = buyAmount;
            this.$state.go("app.coupon.processing");
        }
    }

    angular
        .module('app.reward')
        .controller('app.reward.BuyCouponController', BuyCouponController);
}