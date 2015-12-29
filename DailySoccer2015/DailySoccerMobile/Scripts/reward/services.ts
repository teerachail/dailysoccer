module app.reward {
	'use strict';

    export class BuyCouponDataService {

        public RemainingPoints: number;
        public CouponCost: number;
        public BuyingPower: number;
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
        }
    }

	angular
		.module('app.reward')
        .service('app.reward.BuyCouponDataService', BuyCouponDataService);
}