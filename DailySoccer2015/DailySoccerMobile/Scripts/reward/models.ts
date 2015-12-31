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

    export class RewardGroupRespond {
        public IsAvailable: boolean;
        public RequiredPoints: number;
        public Rewards: Reward[] = [];
    }
    export class Reward {
        public id: string;
        public OrderedNo: number;
        public Price: number;
        public Amount: number;
        public ImgPath: string;
        public ThumbImgPath: string;
        public Description: string;
    }
    export class RewardWinner {
        public id: string;
        public Winners: string[] = [];
    }
}