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

        public SendPurchaseOrderCompleted(isSuccess: boolean): void {
            if (!this.IsBuyingCompleted) {
                this.IsBuyingCompleted = true;
                if (isSuccess) this.RemainingPoints -= this.CouponCost * this.RequestBuyAmount;
            }
        }
    }

    interface IBuyCouponResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        BuyCoupon(data: T): T;
    }
    export class BuyCouponService {

        private svc: IBuyCouponResourceClass<any>;

        static $inject = ['appConfig', '$resource'];
        constructor(appConfig: IAppConfig, private $resource: angular.resource.IResourceService) {
            this.svc = <IBuyCouponResourceClass<any>>$resource(appConfig.BuyCouponUrl, {}, {
                BuyCoupon: { method: 'POST' }
            });
        }

        public BuyCoupon(userId: string, buyAmount: number): ng.IPromise<BuyCouponRespond> {
            return this.svc.BuyCoupon(new BuyCouponRequest(userId, buyAmount)).$promise;
        }
    }

    interface ICouponSummaryResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        GetCouponSummary(data: T): T;
    }
    export class CouponSummaryService {

        private svc: ICouponSummaryResourceClass<any>;

        static $inject = ['appConfig', '$resource'];
        constructor(appConfig: IAppConfig, private $resource: angular.resource.IResourceService) {
            this.svc = <ICouponSummaryResourceClass<any>>$resource(appConfig.CouponSummaryUrl, { 'id': '@id' }, {
                GetCouponSummary: { method: 'GET' }
            });
        }

        public GetCouponSummary(userId: string): ng.IPromise<GetCouponSummaryRespond> {
            return this.svc.GetCouponSummary(new GetCouponSummaryRequest(userId)).$promise;
        }
    }

    angular
        .module('app.reward')
        .service('app.reward.BuyCouponDataService', BuyCouponDataService)
        .service('app.reward.BuyCouponService', BuyCouponService)
        .service('app.reward.CouponSummaryService', CouponSummaryService);
}