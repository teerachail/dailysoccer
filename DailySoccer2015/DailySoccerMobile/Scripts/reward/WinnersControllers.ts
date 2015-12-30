module app.reward {
	'use strict';

	class WinnersController {

		//public model: any = null;

        static $inject = ['data', 'nameData'];
        constructor(public data, public nameData) {
		}

		// public myMethod(): void {
		// }

        public ShowNoWinner(Id: string): number {
            var count = -1;
            for (var i = 0; i < this.nameData.length || count <= -1; i++)
                if (this.nameData[i].RewardId == Id)
                    count++;
            return count;
        }

	}

	angular
		.module('app.reward')
        .controller('app.reward.WinnersController', WinnersController);
}