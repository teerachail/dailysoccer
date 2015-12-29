module app.reward {
	'use strict';

	class RewardsController {

		//public model: any = null;

        static $inject = ['data'];
        constructor(public data) {
		}

		// public myMethod(): void {
		// }

	}

	angular
		.module('app.reward')
        .controller('app.reward.RewardsController', RewardsController);
}