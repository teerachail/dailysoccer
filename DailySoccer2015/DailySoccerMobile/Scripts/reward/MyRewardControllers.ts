module app.reward {
	'use strict';

	class MyRewardsController {

		//public model: any = null;

        static $inject = ['data','oldData'];
        constructor(public data, public oldData) {
		}

		// public myMethod(): void {
		// }

	}

	angular
		.module('app.reward')
        .controller('app.reward.MyRewardsController', MyRewardsController);
}