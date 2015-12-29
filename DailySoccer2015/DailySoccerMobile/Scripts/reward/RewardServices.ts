module app.reward {
    'use strict';

    export class BuyCouponDataService {

        public RemainingPoints: number;
        public CouponCost: number;
        public BuyingPower: number;
        public RequestBuyAmount: number;
        public IsBuyingCompleted: boolean;
        private isAlreadyRequireFacebookLogin: boolean;
        private isAlreadyRequirePhoneVerification: boolean;

        public InitialData(couponCost: number, remainingPoints: number): void {
            this.CouponCost = couponCost;
            this.RemainingPoints = remainingPoints;
            const NotAvailableCost = 0;
            var isCouponCostValid = couponCost > NotAvailableCost;
            this.BuyingPower = isCouponCostValid ? Math.floor(this.RemainingPoints / this.CouponCost) : NotAvailableCost;
            this.ResetAllRequests();
        }

        public CheckFirstTimeForRequestFacebookLogin(): boolean {
            var beforeValueChanged = this.isAlreadyRequireFacebookLogin;
            this.isAlreadyRequireFacebookLogin = false;
            return beforeValueChanged;
        }

        public CheckFirstTimeForRequestPhoneVerification(): boolean {
            var beforeValueChanged = this.isAlreadyRequirePhoneVerification;
            this.isAlreadyRequirePhoneVerification = false;
            return beforeValueChanged;
        }

        public ResetAllRequests(): void {
            this.isAlreadyRequireFacebookLogin = true;
            this.isAlreadyRequirePhoneVerification = true;
            this.IsBuyingCompleted = false;
            this.RequestBuyAmount = 0;
        }

        public SendPurchaseOrderCompleted(): void {
            if (!this.IsBuyingCompleted) {
                this.IsBuyingCompleted = true;
                this.RemainingPoints -= this.CouponCost * this.RequestBuyAmount;
            }
        }
    }

    interface IBuyCouponService {
        BuyCoupon(body: BuyCouponRequest): ng.IPromise<BuyCouponRespond>;
    }

    export class BuyCouponService implements IBuyCouponService {

        private updatePhoneSvc: ng.resource.IResourceClass<any>;

        static $inject = ['$resource'];
        constructor(private $resource: angular.resource.IResourceService) {
            var saveAction: ng.resource.IActionDescriptor = { method: 'PUT' };
            this.updatePhoneSvc = $resource('http://localhost:2394/api/coupons/buy');
        }

        public BuyCoupon(body: BuyCouponRequest): ng.IPromise<BuyCouponRespond> {
            return (<ng.resource.IResource<BuyCouponRespond>>this.updatePhoneSvc.save(body)).$promise;
        }
    }

    angular
        .module('app.reward')
        .service('app.reward.BuyCouponDataService', BuyCouponDataService)
        .service('app.reward.BuyCouponService', BuyCouponService);
}