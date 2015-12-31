module app.reward {
	'use strict';

    class WinnersController {

        static $inject = ['data', 'winnerData'];
        constructor(public data: RewardGroupRespond, public winnerData: RewardWinner[]) {
        }

        public AnyRewardWinner(rewardId: string): boolean {
            var result = this.GetWinnersNameByRewardId(rewardId).length > 0;
            return result;
        }

        public GetWinnersNameByRewardId(rewardId: string): string[] {
            var qry = this.winnerData.filter(it=> it.id == rewardId);
            if (qry.length <= 0) return [];
            return qry[0].Winners;
        }
    }

	angular
		.module('app.reward')
        .controller('app.reward.WinnersController', WinnersController);
}