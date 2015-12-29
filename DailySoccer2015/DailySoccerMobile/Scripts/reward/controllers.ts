module app.reward {
    'use strict';

    class RewardsController {

        static $inject = ['data', 'app.reward.BuyCouponDataService'];
        constructor(public data, private buySvc: app.reward.BuyCouponDataService) {
            buySvc.InitialData(200, 500); // HACK: Setup coupon data
        }

    }

    class BuyCouponController {

        static $inject = ['$state', 'app.reward.BuyCouponDataService'];
        constructor(private $state: angular.ui.IStateService, private buySvc: app.reward.BuyCouponDataService) {
        }

        public BuyCoupons(buyAmount: number): void {
            const MinimumBuyAmount = 1;
            var isRequestValid = buyAmount >= MinimumBuyAmount && buyAmount <= this.buySvc.BuyingPower;
            if (!isRequestValid) return;

            this.$state.go("app.processing");
        }
    }

    class BuyCouponProcessingController {
        static $inject = ['$scope', '$ionicModal', '$state', 'app.reward.BuyCouponDataService'];
        constructor(private $scope, private $ionicModal, private $state: angular.ui.IStateService, private buySvc: app.reward.BuyCouponDataService) {
            this.$ionicModal.fromTemplateUrl('templates/Facebook.html',
                {
                    scope: $scope,
                    animation: 'slide-in-up'
                }).then(function (modal): void { $scope.FacebookPopup = modal; });
            this.$ionicModal.fromTemplateUrl('templates/Tie.html',
                {
                    scope: $scope,
                    animation: 'slide-in-up'
                }).then(function (modal): void { $scope.TiePopup = modal; });
            this.$ionicModal.fromTemplateUrl('templates/BuyCouponComplete.html',
                {
                    scope: $scope,
                    animation: 'slide-in-up'
                }).then(function (modal): void { $scope.CompletedPopup = modal; });
        }
    }

    angular
        .module('app.reward')
        .controller('app.reward.RewardsController', RewardsController)
        .controller('app.reward.BuyCouponController', BuyCouponController)
        .controller('app.reward.BuyCouponProcessingController', BuyCouponProcessingController);
}