module app.reward {
    'use strict';

    export class BuyCouponRequest {
        public UserId: string;
        public BuyAmount: number;
    }
    export class BuyCouponRespond {
        public IsSuccess: boolean;
        public ErrorMessage: string;
        public AnnounceableDate: Date;
    }

}