module app.reward {
    'use strict';

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

    angular
        .module('app.reward')
        .controller('app.reward.MyRewardsController', MyRewardsController);
}