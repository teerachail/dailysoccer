module app.reward {
    'use strict';

    class RewardsController {

        static $inject = ['data', 'couponSummary', 'app.reward.BuyCouponDataService'];
        constructor(public data: RewardGroupRespond, private couponSummary: GetCouponSummaryRespond, private buySvc: app.reward.BuyCouponDataService) {
            buySvc.InitialData(data.RequiredPoints, couponSummary.RemainingPoints);
        }

    }

    class WinnersController {

        static $inject = ['winnerData'];
        constructor(public winnerData: RewardWinner[]) {
        }

    }

    class MyRewardsController {

        static $inject = ['data'];
        constructor(public data: MyReward[]) {
        }

        public GetPresentRewards(): MyReward[] {
            var qry = this.data.filter(it=> it.IsPresent);
            return qry;
        }

        public GetPastRewards(): MyReward[] {
            var qry = this.data.filter(it=> !it.IsPresent);
            return qry;
        }

        public AnyPresentRewards(): boolean {
            var qry = this.GetPresentRewards();
            return qry.length > 0;
        }

        public AnyPastRewards(): boolean {
            var qry = this.GetPastRewards();
            return qry.length > 0;
        }

    }

    class BuyCouponController {

        static $inject = ['$ionicModal', '$ionicPopup', '$scope', '$state', 'app.reward.BuyCouponDataService'];
        constructor(private $ionicModal, private $ionicPopup, private $scope, private $state: angular.ui.IStateService, private buySvc: app.reward.BuyCouponDataService) {
        }

        public BuyCoupons(buyAmount: number): void {
            const MinimumBuyAmount = 1;
            var isRequestValid = buyAmount >= MinimumBuyAmount && buyAmount <= this.buySvc.BuyingPower;
            if (!isRequestValid) {
                this.$ionicPopup.alert({
                    title: 'ข้อมูลในการสั่งซื้อไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง!',
                    okType: 'button-royal',
                    okText: 'ระบุใหม่'
                });
                return;
            }

            this.buySvc.ResetAllRequests();
            this.buySvc.RequestBuyAmount = buyAmount;
            this.$state.go("app.coupon.processing");
        }

    }

    class BuyCouponProcessingController {

        private buyCouponResult: BuyCouponRespond;

        static $inject = ['$scope', '$timeout', '$ionicModal', '$ionicPopup', '$state', 'app.reward.BuyCouponDataService', 'app.shared.UserProfileService', 'app.reward.BuyCouponService', 'userprofile'];
        constructor(private $scope, private $timeout: ng.ITimeoutService, private $ionicModal, private $ionicPopup, private $state: angular.ui.IStateService, private couponDataSvc: app.reward.BuyCouponDataService, private userprofileSvc: app.shared.UserProfileService, private buySvc: app.reward.BuyCouponService, private userprofile) {
            this.$ionicModal.fromTemplateUrl('templates/Facebook.html',
                {
                    scope: $scope,
                    animation: 'slide-in-up'
                }).then(modal=> { this.$scope.FacebookPopup = modal; });
            this.$ionicModal.fromTemplateUrl('templates/Tie.html',
                {
                    scope: $scope,
                    animation: 'slide-in-up'
                }).then(modal=> { this.$scope.TiePopup = modal; });
            this.$ionicModal.fromTemplateUrl('templates/BuyCouponComplete.html',
                {
                    scope: $scope,
                    animation: 'slide-in-up'
                }).then(modal=> { this.$scope.CompletedPopup = modal; });

            this.$scope.$on('modal.hidden', () => this.validateUserProfile());
            this.validateUserProfile();
        }

        private validateUserProfile(): void {

            if (this.couponDataSvc.IsBuyingCompleted) {
                this.gobackToBuyCouponPage();
                return;
            }

            var isReadyToBuy = this.checkPreparingPopups()
                && this.checkFacebookAuthentication()
                && this.checkPhoneVerification();
            if (!isReadyToBuy) return;

            this.buyCoupons();
        }
        private checkPreparingPopups(): boolean {
            var isSetupCompleted = this.$scope.FacebookPopup != null
                && this.$scope.TiePopup != null
                && this.$scope.CompletedPopup != null;
            if (!isSetupCompleted) {
                console.log('Preparing...');
                this.$timeout(() => this.validateUserProfile(), 333);
            }
            return isSetupCompleted;
        }
        private checkFacebookAuthentication(): boolean {
            var userprofile = this.userprofileSvc.GetUserProfile();
            if (!userprofile.IsLoggedFacebook) {
                if (this.couponDataSvc.CheckFirstTimeForRequestFacebookLogin()) this.$scope.FacebookPopup.show();
                else this.gobackToBuyCouponPage();
            }
            return userprofile.IsLoggedFacebook;
        }
        private checkPhoneVerification(): boolean {
            var isPhoneNumberVerified = this.userprofile.VerifiedPhoneDate != null;
            if (!isPhoneNumberVerified) {
                if (this.couponDataSvc.CheckFirstTimeForRequestPhoneVerification()) this.$state.go('app.verify.phone');
                else this.gobackToBuyCouponPage();
            }
            return isPhoneNumberVerified;
        }
        private buyCoupons(): void {
            var userprofile = this.userprofileSvc.GetUserProfile();
            this.buySvc.BuyCoupon(userprofile.UserId, this.couponDataSvc.RequestBuyAmount)
                .then((respond: BuyCouponRespond) => {
                    this.buyCouponResult = respond;
                    this.couponDataSvc.SendPurchaseOrderCompleted(respond.IsSuccess);
                    if (respond.IsSuccess) this.$scope.CompletedPopup.show();
                    else {
                        this.$ionicPopup.alert({
                            title: this.buyCouponResult.ErrorMessage,
                            okType: 'button-royal',
                            okText: 'ระบุใหม่'
                        }).then(() => this.validateUserProfile());
                    }
                })
                .catch(err=> {
                    // TODO: Inform error to user
                });
        }
        private gobackToBuyCouponPage(): void {
            this.$state.go('app.coupon.buy', { location: 'replace' });
        }

    }

    angular
        .module('app.reward')
        .controller('app.reward.RewardsController', RewardsController)
        .controller('app.reward.WinnersController', WinnersController)
        .controller('app.reward.MyRewardsController', MyRewardsController)
        .controller('app.reward.BuyCouponController', BuyCouponController)
        .controller('app.reward.BuyCouponProcessingController', BuyCouponProcessingController);
}