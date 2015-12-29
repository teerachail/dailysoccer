module app.reward {
	'use strict';

    export class BuyCouponDataService {

        public RemainingPoints: number;
        public CouponCost: number;
        public BuyingPower: number;

        public InitialData(couponCost: number, remainingPoints: number): void {
            this.CouponCost = couponCost;
            this.RemainingPoints = remainingPoints;
            const NotAvailableCost = 0;
            var isCouponCostValid = couponCost > NotAvailableCost;
            this.BuyingPower = isCouponCostValid ? Math.floor(this.RemainingPoints / this.CouponCost) : NotAvailableCost;
        }

    }

	angular
		.module('app.reward')
        .service('app.reward.BuyCouponDataService', BuyCouponDataService);
}