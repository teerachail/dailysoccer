module app.shared {
    'use strict';

    class SideMenu {

        static $inject = ['couponSummary'];
        constructor(public couponSummary) {
        }

    }

    angular
        .module('app.shared')
        .controller('app.shared.SideMenu', SideMenu);

}