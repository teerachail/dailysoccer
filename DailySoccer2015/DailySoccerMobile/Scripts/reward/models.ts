module app.reward {
    'use strict';

    export class BuyCouponRequest {
        constructor(public UserId: string, public BuyAmount: number) { }
    }

    export class BuyCouponRespond {
        public IsSuccess: boolean;
        public ErrorMessage: string;
        public AnnounceableDate: Date;
    }

    export class GetCouponSummaryRequest {
        constructor(public id: string) { }
    }
    export class GetCouponSummaryRespond {
        public RemainingPoints: number;
        public OrderedCoupons: number;
    }
}